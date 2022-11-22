using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DebtManagement.BackgroundTasks.Resources;
using DebtManagement.BackgroundTasks.Services.Grpc;
using DebtManagement.BackgroundTasks.Services.Organizations;
using DebtManagement.BackgroundTasks.Services.OutContracts;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using DebtManagement.Infrastructure;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;

namespace DebtManagement.BackgroundTasks.Tasks
{
    public class ReceiptVoucherManagementTask : BackgroundService
    {
        private string LOGGER_TAG;
        #region Service dependencies
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ReceiptVoucherManagementTask> _logger;

        #endregion

        private int RetryTimes = 0;
        private int FailedTimes = 0;
        private bool IsDelayForFirstTime = true;
        private HashSet<string> VoucherCodesByDate;

        public ReceiptVoucherManagementTask(ILogger<ReceiptVoucherManagementTask> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.UtcNow.AddHours(7);
                var exitBehavior = GenerateExitBehavior.Retry;
                this.VoucherCodesByDate = new HashSet<string>();

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    #region Inject dependencies
                    var _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var _context = scope.ServiceProvider.GetRequiredService<DebtDbContext>();
                    var _outContractGrpcService = scope.ServiceProvider.GetRequiredService<IOutContractGrpcService>();
                    var _receiptVoucherRepository = scope.ServiceProvider.GetRequiredService<IReceiptVoucherRepository>();
                    var _receiptVoucherDetailRepository = scope.ServiceProvider.GetRequiredService<IReceiptVoucherDetailRepository>();
                    var _receiptVoucherQueries = scope.ServiceProvider.GetRequiredService<IReceiptVoucherQueries>();
                    var _marketAreaGrpcService = scope.ServiceProvider.GetRequiredService<IMarketAreaGrpcService>();
                    var _projectGrpcService = scope.ServiceProvider.GetRequiredService<IProjectGrpcService>();
                    var _telcoServiceGrpcService = scope.ServiceProvider.GetRequiredService<ITelcoServiceGrpcService>();
                    var _voucherTargetQueries = scope.ServiceProvider.GetRequiredService<IVoucherTargetQueries>();
                    var _receiptVoucherTaxRepository = scope.ServiceProvider.GetRequiredService<IReceiptVoucherTaxRepository>();
                    var _receiptVoucherPromotionRepo = scope.ServiceProvider.GetRequiredService<IPromotionForReceiptVoucherRepository>();
                    var _voucherTargetRepository = scope.ServiceProvider.GetRequiredService<IVoucherTargetRepository>();
                    var _configurationSystemParameterService = scope.ServiceProvider.GetRequiredService<IConfigurationSystemParameterGrpcService>();
                    var _telcoServicePackageGrpc = scope.ServiceProvider.GetRequiredService<ITelcoServicePackageGrpcService>();
                    var _receiptVoucherDebtHistoryRepository = scope.ServiceProvider.GetRequiredService<IReceiptVoucherDebtHistoryRepository>();
                    var _outContractService = scope.ServiceProvider.GetRequiredService<IOutContractService>();
                    var _organizationGrpcService = scope.ServiceProvider.GetRequiredService<IOrganizationUnitGrpcService>();
                    var _temporaryGeneratingVoucherRepository = scope.ServiceProvider.GetRequiredService<ITemporaryGeneratingVoucherRepository>();
                    var _notificationGrpcService = scope.ServiceProvider.GetRequiredService<INotificationGrpcService>();
                    var _exchangeRateGrpcService = scope.ServiceProvider.GetRequiredService<IExchangeRateGrpcService>();
                    var _generateHistoryRepository = scope.ServiceProvider.GetRequiredService<IVoucherAutoGenerateHistoryRepository>();
                    #endregion

                    if (IsDelayForFirstTime)
                    {
                        exitBehavior = GenerateExitBehavior.Delay;
                        goto Finish;
                    }

                    try
                    {
                        var transaction = await _context.BeginTransactionAsync();
                        if (transaction == null)
                        {
                            transaction = _context.GetCurrentTransaction();
                        }

                        #region Fetching category data handler
                        /// Lấy tất cả đối tượng thu tiền(khách hàng) có thể
                        var voucherTargetUIds = _voucherTargetQueries.GetAllIds();

                        /// Lấy tất cả các danh mục cần thiết
                        /// Lấy danh sách dự án
                        var getProjectsRsp = await _projectGrpcService.GetAll();
                        if (getProjectsRsp == null) goto Finish;
                        var projects = getProjectsRsp.ProjectDtos?.ToList();

                        /// Lấy danh sách vùng thị trường
                        var getMarketsRsp = await _marketAreaGrpcService.GetAll();
                        if (getMarketsRsp == null) goto Finish;
                        var marketAreas = getMarketsRsp.MarketAreaDtos?.ToList();

                        /// Lấy danh sách dịch vụ
                        var getServicesRsp = await _telcoServiceGrpcService.GetAll();
                        if (getServicesRsp == null) goto Finish;
                        var telcoServices = getServicesRsp.ServiceDtos?.ToList();

                        /// Lấy danh sách gói cước
                        var getSrvPackagesRsp = await _telcoServicePackageGrpc.GetAll();
                        if (getSrvPackagesRsp == null) goto Finish;
                        var telcoSrvPackages = getSrvPackagesRsp.PackageDtos?.ToList();

                        /// Lấy danh sách phòng ban tổ chức
                        var getOrganizationUnitsRsp = await _organizationGrpcService.GetAll();
                        if (getOrganizationUnitsRsp == null || getOrganizationUnitsRsp.Count() == 0) goto Finish;
                        var organizationUnits = getOrganizationUnitsRsp?.ToList();

                        /// Lấy danh sách tỉ giá hiện tại
                        var exchangeRates = await _exchangeRateGrpcService.GetExchangeRatesByNow();
                        /// Đồng bộ tỉ giá ngày hiện tại và lấy ra
                        if ((exchangeRates?.Count ?? 0) == 0)
                        {
                            await _exchangeRateGrpcService.SynchronizeExchangeRates();
                            exchangeRates = await _exchangeRateGrpcService.GetExchangeRatesByNow();
                        }

                        /// Lấy thông tin cấu hình của hệ thống
                        var systemConfiguration = await _configurationSystemParameterService.GetSystemConfigurations();

                        /// Lấy thông tin giảm trừ tạm ngưng
                        var channelSuspensionTimes = _outContractService.GetChannelSuspensionTimes();
                        var handledSuspensionTimes = new List<int>();

                        /// Lấy danh sách khuyến mại đang hoạt động
                        var availablePromotions = await _outContractGrpcService.GetAvailablePromotions();
                        #endregion

                        this.LOGGER_TAG = $"[Vouchers Generate {DateTime.UtcNow.AddHours(7):dd/MM/yyyy}]";
                        var timer = new Stopwatch();

                        #region Get domain categories
                        timer.Start();

                        /// Lấy ra danh sách các hợp đồng cần thanh toán trong kỳ
                        var needToPayContracts = _outContractService.GetNeedToPaymentContracts();
                        if (needToPayContracts == null || needToPayContracts.Count == 0)
                        {
                            exitBehavior = GenerateExitBehavior.Done;
                            goto Finish;
                        }

                        /// Thứ tự của phiếu trong ngày hiện tại
                        var orderByIssuedDate = 0;
                        /// Lấy thứ tự của phiếu trong ngày hiện tại
                        VoucherCodesByDate = _receiptVoucherQueries
                            .GetVoucherCodeFromIssuedDate(DateTime.Now.AddHours(7));
                        if (VoucherCodesByDate.Count > 0)
                        {
                            var latestVoucherCode = VoucherCodesByDate.ElementAt(0);
                            var voucherCodePaths = latestVoucherCode.Split(@"/");
                            if (int.TryParse(voucherCodePaths[voucherCodePaths.Length - 1], out var vchrIdx))
                            {
                                orderByIssuedDate = vchrIdx;
                            }
                        }

                        timer.Stop();
                        Console.WriteLine("Fetching prerequisite data time: " + timer.Elapsed.ToString(@"m\:ss"));

                        #endregion

                        timer.Reset();
                        timer.Start();

                        var receiptVouchers = new List<ReceiptVoucherInsertBulkModel>();
                        var receiptVoucherTaxes = new List<ReceiptVoucherTaxInsertBulkModel>();
                        var receiptVoucherPromotions = new List<PromotionForReceiptVoucher>();
                        var receiptVoucherDetails = new List<ReceiptVoucherDetailInsertBulkModel>();
                        var newVoucherTargets = new List<VoucherTarget>();
                        var debtHistories = new List<DebtHistoryInsertBulkModel>();
                        var tempGeneratingVouchers = new List<TemporaryGeneratingVoucher>();

                        for (int i = 0; i < needToPayContracts.Count; i++)
                        {
                            var contract = needToPayContracts.ElementAt(i);
                            var currentOrganizationUnit = organizationUnits
                                .FirstOrDefault(o => o.IdentityGuid.Equals(contract.OrganizationUnitId));

                            KeyValuePair<string, int?> voucherTargetKeyDic;
                            var groupedByTargetAndCurrencyVchrs = contract.ServicePackages
                                .GroupBy(g => new
                                {
                                    PaymentTargetId = g.PaymentTarget.IdentityGuid,
                                    g.CurrencyUnitCode,
                                    g.CurrencyUnitId
                                });

                            foreach (var groupedSrvPackages in groupedByTargetAndCurrencyVchrs)
                            {
                                if (voucherTargetUIds.ContainsKey(groupedSrvPackages.Key.PaymentTargetId))
                                {
                                    voucherTargetKeyDic = voucherTargetUIds.Single(c => c.Key == groupedSrvPackages.Key.PaymentTargetId);
                                }
                                else
                                {
                                    var curVchrTarget = groupedSrvPackages.First().PaymentTarget;
                                    var newVoucherTarget = new VoucherTarget()
                                    {
                                        IdentityGuid = curVchrTarget.IdentityGuid,
                                        TargetCode = curVchrTarget.ContractorCode,
                                        ApplicationUserIdentityGuid = curVchrTarget.ApplicationUserIdentityGuid,
                                        TargetFullName = curVchrTarget.ContractorFullName,
                                        TargetEmail = curVchrTarget.ContractorEmail,
                                        TargetFax = curVchrTarget.ContractorFax,
                                        TargetPhone = curVchrTarget.ContractorPhone,
                                        TargetIdNo = curVchrTarget.ContractorIdNo,
                                        TargetTaxIdNo = curVchrTarget.ContractorTaxIdNo,
                                        City = curVchrTarget.ContractorCity,
                                        CityId = curVchrTarget.ContractorCityId,
                                        District = curVchrTarget.ContractorDistrict,
                                        DistrictId = curVchrTarget.ContractorDistrictId,
                                        TargetAddress = curVchrTarget.ContractorAddress,
                                        CreatedBy = "Hệ thống",
                                        CreatedDate = DateTime.UtcNow.AddHours(7),
                                        IsActive = true,
                                        IsDeleted = false,
                                        IsEnterprise = curVchrTarget.IsEnterprise,
                                        IsPayer = curVchrTarget.IsBuyer,
                                        IsPartner = false,
                                        Culture = "vi-VN",
                                        CurrentDebt = 0
                                    };
                                    newVoucherTargets.Add(newVoucherTarget);
                                    voucherTargetKeyDic = new KeyValuePair<string, int?>(curVchrTarget.IdentityGuid, null);
                                    voucherTargetUIds.Add(voucherTargetKeyDic.Key, voucherTargetKeyDic.Value);
                                }

                                // var nextBillingDate = DateTime.UtcNow.AddMonths(contract.TimeLine.PaymentPeriod);

                                var voucherProject = projects
                                    .FirstOrDefault(p => contract.ProjectId.HasValue && p.Id == contract.ProjectId);

                                var voucherMarketArea = marketAreas
                                    .FirstOrDefault(m => m.Id == contract.MarketAreaId);

                                var newReceiptVoucher = new ReceiptVoucher()
                                {
                                    CurrencyUnitId = groupedSrvPackages.Key.CurrencyUnitId,
                                    CurrencyUnitCode = groupedSrvPackages.Key.CurrencyUnitCode,
                                    AccountingCode = contract.AccountingCustomerCode,
                                    CreatedUserFullName = "Công ty cổ phần HTC viễn thông quốc tế",
                                    IdentityGuid = Guid.NewGuid().ToString(),
                                    OutContractId = contract.Id,
                                    ContractCode = contract.ContractCode,
                                    TargetId = voucherTargetKeyDic.Value,
                                    CreatedDate = DateTime.UtcNow.AddHours(7),
                                    CreatedBy = "Hệ thống",
                                    IsActive = true,
                                    MarketAreaId = voucherMarketArea?.Id,
                                    MarketAreaName = voucherMarketArea?.MarketName,
                                    TypeId = ReceiptVoucherType.Billing.Id,
                                    Payment = new PaymentMethod(
                                        contract.Payment.Form,
                                        contract.Payment.Method,
                                        contract.Payment.Address),
                                    IssuedDate = DateTime.UtcNow.AddHours(7),
                                    IsEnterprise = contract.ContractTypeId == 2,
                                    PaymentPeriod = contract.TimeLine.PaymentPeriod,
                                    OrganizationUnitId = contract.OrganizationUnitId,
                                    OrganizationUnitName = contract.OrganizationUnitName,
                                    Source = "DebtManagement.BackgroundTasks.Tasks.ReceiptVoucherGenerationTask",
                                    IsAutomaticGenerate = true,
                                    IsLock = false,
                                    InvalidIssuedDate = false,
                                    IsBadDebt = false,
                                    OrganizationPath = currentOrganizationUnit?.TreePath,
                                    NumberOfDebtHistories = 1,
                                    NumberOfOpeningDebtHistories = 0,
                                    NumberDaysOverdue = 0,
                                    NumberBillingLimitDays =
                                    contract.NumberBillingLimitDays > 0
                                        ? contract.NumberBillingLimitDays
                                        : 30
                                };

                                newReceiptVoucher.SetCashierUser(
                                    contract.CashierUserId,
                                    contract.CashierUserName,
                                    contract.CashierFullName,
                                    true);

                                if (!newReceiptVoucher.CurrencyUnitCode
                                        .Equals(CurrencyUnit.VND.CurrencyUnitCode, StringComparison.OrdinalIgnoreCase)
                                    && exchangeRates?.Count > 0)
                                {
                                    var exchangeRate = exchangeRates
                                        .Find(e => e.CurrencyCode.Equals(newReceiptVoucher.CurrencyUnitCode, StringComparison.OrdinalIgnoreCase));
                                    newReceiptVoucher.ExchangeRate = exchangeRate.TransferValue;
                                    newReceiptVoucher.ExchangeRateApplyDate = exchangeRate.CreatedDate;
                                }
                                else
                                {
                                    newReceiptVoucher.ExchangeRate = 1;
                                    newReceiptVoucher.ExchangeRateApplyDate = DateTime.UtcNow.AddHours(7);
                                }

                                var joinedServiceCodes = new List<string>();
                                foreach (var receiptSrvPackage in groupedSrvPackages)
                                {
                                    var telcoService = telcoServices
                                        .FirstOrDefault(s => s.Id == receiptSrvPackage.ServiceId);
                                    var telcoSrvPackage = telcoSrvPackages
                                        .FirstOrDefault(p => p.Id == receiptSrvPackage.ServicePackageId);

                                    // Khởi tạo đối tượng phiếu thu
                                    var receiptVoucherLine = new ReceiptVoucherDetail()
                                    {
                                        IdentityGuid = Guid.NewGuid().ToString(),
                                        CId = receiptSrvPackage.CId,
                                        ProjectId = receiptSrvPackage.ProjectId,
                                        CurrencyUnitCode = newReceiptVoucher.CurrencyUnitCode,
                                        CurrencyUnitId = newReceiptVoucher.CurrencyUnitId,
                                        ServiceId = receiptSrvPackage.ServiceId,
                                        ServiceName = telcoService.ServiceName,
                                        ServicePackageId = telcoSrvPackage?.Id,
                                        ServicePackageName = telcoSrvPackage?.PackageName,
                                        OutContractServicePackageId = receiptSrvPackage.Id,
                                        PackagePrice = receiptSrvPackage.PackagePrice,
                                        PricingType = 1,
                                        IsActive = true,
                                        CreatedDate = DateTime.Now.AddHours(7),
                                        CreatedBy = "Hệ thống",
                                        DomesticBandwidth = $"{receiptSrvPackage.DomesticBandwidth}{receiptSrvPackage.DomesticBandwidthUom}",
                                        InternationalBandwidth = $"{receiptSrvPackage.InternationalBandwidth}{receiptSrvPackage.InternationalBandwidthUom}",
                                        HasStartAndEndPoint = receiptSrvPackage.HasStartAndEndPoint,
                                        HasDistinguishBandwidth = receiptSrvPackage.HasDistinguishBandwidth,
                                        IsAutomaticGenerate = true,
                                        IsJoinedPayment = false,
                                        IsShow = true
                                    };

                                    if (receiptVoucherLine.ProjectId.HasValue)
                                    {
                                        var project = projects.FirstOrDefault(c => c.Id == receiptVoucherLine.ProjectId);
                                        newReceiptVoucher.ProjectId = project?.Id;
                                        newReceiptVoucher.ProjectName = project?.ProjectName;
                                    }

                                    if (string.IsNullOrEmpty(receiptVoucherLine.InternationalBandwidth))
                                    {
                                        receiptVoucherLine.InternationalBandwidth = "N/A";
                                    }

                                    if (string.IsNullOrEmpty(receiptVoucherLine.DomesticBandwidth))
                                    {
                                        receiptVoucherLine.DomesticBandwidth = "N/A";
                                    }

                                    /// Áp dụng thuế của kênh đang thanh toán vào phiếu thu
                                    if (receiptSrvPackage.OutContractServicePackageTaxes.Count > 0)
                                    {
                                        foreach (var taxValue in receiptSrvPackage.OutContractServicePackageTaxes)
                                        {
                                            var voucherLineTaxCmd = new CreateReceiptVoucherLineTaxCommand()
                                            {
                                                IdentityGuid = Guid.NewGuid().ToString(),
                                                TaxCode = taxValue.TaxCategoryCode,
                                                TaxName = taxValue.TaxCategoryName,
                                                TaxValue = taxValue.TaxValue,
                                                IsAutomaticGenerate = true
                                            };

                                            receiptVoucherLine
                                                .AddApplyTax(voucherLineTaxCmd);

                                            tempGeneratingVouchers.Add(new TemporaryGeneratingVoucher()
                                            {
                                                ReceiptVoucherId = newReceiptVoucher.IdentityGuid,
                                                ReceiptVoucherDetailId = receiptVoucherLine.IdentityGuid,
                                                VoucherTargetId = voucherTargetKeyDic.Key,
                                                VoucherTaxId = voucherLineTaxCmd.IdentityGuid,
                                                DebtHistoryId = newReceiptVoucher.ActivatedDebt.IdentityGuid
                                            });
                                        }
                                    }

                                    if (tempGeneratingVouchers.All(c =>
                                         c.ReceiptVoucherId != newReceiptVoucher.IdentityGuid &&
                                         c.ReceiptVoucherDetailId != receiptVoucherLine.IdentityGuid &&
                                         c.VoucherTaxId != voucherTargetKeyDic.Key &&
                                         c.DebtHistoryId != newReceiptVoucher.ActivatedDebt.IdentityGuid))
                                    {
                                        tempGeneratingVouchers.Add(new TemporaryGeneratingVoucher()
                                        {
                                            ReceiptVoucherId = newReceiptVoucher.IdentityGuid,
                                            ReceiptVoucherDetailId = receiptVoucherLine.IdentityGuid,
                                            VoucherTargetId = voucherTargetKeyDic.Key,
                                            DebtHistoryId = newReceiptVoucher.ActivatedDebt.IdentityGuid
                                        });
                                    }

                                    if (receiptSrvPackage.TimeLine.NextBilling.HasValue)
                                    {
                                        // Xác định ngày bắt đầu và kết thúc kỳ cước theo hình thức thanh toán(trả trước/trả sau),
                                        // tính giá trị trước thuế của kênh
                                        // Với trả trước
                                        if (newReceiptVoucher.Payment.Form == PaymentMethodForm.Prepaid)
                                        {
                                            receiptVoucherLine.StartBillingDate = receiptSrvPackage.TimeLine.NextBilling;
                                            if (receiptVoucherLine.StartBillingDate.Value.Day == 1)
                                            {
                                                receiptVoucherLine.EndBillingDate = receiptSrvPackage.TimeLine.NextBilling.Value
                                                    .AddMonths(receiptSrvPackage.TimeLine.PaymentPeriod)
                                                    .AddDays(-1);

                                                receiptVoucherLine.UsingMonths = receiptSrvPackage.TimeLine.PaymentPeriod;
                                                receiptVoucherLine.SubTotalBeforeTax = CurrencyUnit.RoundByCurrency(
                                                    receiptVoucherLine.CurrencyUnitId,
                                                    receiptVoucherLine.PackagePrice * receiptVoucherLine.UsingMonths);
                                            }
                                            else
                                            {
                                                var lastDayOfBillingMonth = new DateTime(
                                                    receiptVoucherLine.StartBillingDate.Value.Year,
                                                    receiptVoucherLine.StartBillingDate.Value.Month,
                                                        DateTime.DaysInMonth(
                                                            receiptVoucherLine.StartBillingDate.Value.Year,
                                                            receiptVoucherLine.StartBillingDate.Value.Month
                                                        )
                                                    );
                                                receiptVoucherLine.EndBillingDate = lastDayOfBillingMonth;
                                                receiptVoucherLine.CalculateSubTotalByFixedPrice();
                                            }

                                            if (receiptVoucherLine.StartBillingDate.Value.Date < newReceiptVoucher.IssuedDate.Date)
                                            {
                                                newReceiptVoucher.IssuedDate = receiptVoucherLine.StartBillingDate.Value;
                                            }
                                        }
                                        // Với trả sau
                                        else
                                        {
                                            receiptVoucherLine.StartBillingDate = receiptSrvPackage.TimeLine.NextBilling.Value
                                                .AddMonths(0 - receiptSrvPackage.TimeLine.PaymentPeriod);
                                            receiptVoucherLine.EndBillingDate = receiptSrvPackage.TimeLine.NextBilling.Value.AddDays(-1);

                                            receiptVoucherLine.UsingMonths = receiptSrvPackage.TimeLine.PaymentPeriod;
                                            receiptVoucherLine.SubTotalBeforeTax = CurrencyUnit.RoundByCurrency(
                                                receiptVoucherLine.CurrencyUnitId,
                                                receiptVoucherLine.PackagePrice * receiptVoucherLine.UsingMonths);

                                            if (receiptVoucherLine.EndBillingDate.Value.Date < newReceiptVoucher.IssuedDate.Date)
                                            {
                                                newReceiptVoucher.IssuedDate = receiptVoucherLine.EndBillingDate.Value;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    #region promotion

                                    if (availablePromotions.Any() &&
                                        receiptSrvPackage.PromotionForContracts.Count > 0)
                                    {
                                        foreach (var item in receiptSrvPackage.PromotionForContracts
                                            .Where(p => p.IsApplied))
                                        {
                                            var currentPromotion = availablePromotions.FirstOrDefault(s =>
                                                s.Id == item.PromotionId &&
                                                s.PromotionType != 3 && // Loại trừ những khuyến mại tặng sp
                                                s.StartDate.Date <= currentTime.Date &&
                                                s.EndDate.Date >= currentTime.Date &&
                                                s.PromotionDetails.Any(d => d.Id == item.PromotionDetailId)
                                            );
                                            if (currentPromotion == null) continue;
                                            var currentPromotionDetail = currentPromotion.PromotionDetails.First(d => d.Id == item.PromotionDetailId);

                                            if (currentPromotionDetail.MinPaymentPeriod.HasValue
                                                    && currentPromotionDetail.MinPaymentPeriod > receiptSrvPackage.TimeLine.PaymentPeriod) continue;

                                            var promo = _mapper.Map<PromotionForReceiptVoucher>(item);
                                            promo.IdentityGuid = Guid.NewGuid().ToString();
                                            promo.CreatedDate = DateTime.Now;
                                            promo.PromotionValue = currentPromotionDetail.PromotionValue;
                                            promo.NumberMonthApplied = currentPromotionDetail.NumberOfMonthApplied;
                                            newReceiptVoucher.SetPromotionForReceiptVoucher(promo);
                                            receiptVoucherPromotions.Add(promo);

                                            tempGeneratingVouchers.Add(new TemporaryGeneratingVoucher()
                                            {
                                                ReceiptVoucherId = newReceiptVoucher.IdentityGuid,
                                                ReceiptVoucherDetailId = receiptVoucherLine.IdentityGuid,
                                                VoucherTargetId = voucherTargetKeyDic.Key,
                                                DebtHistoryId = newReceiptVoucher.ActivatedDebt.IdentityGuid,
                                                PromotionForVoucherId = promo.IdentityGuid
                                            });
                                        }
                                    }

                                    #endregion

                                    /// Thực hiện giảm trừ tạm ngưng nếu có
                                    if (channelSuspensionTimes.Any(c => c.OutContractServicePackageId == receiptSrvPackage.Id))
                                    {
                                        var currentSuspensionTimes = channelSuspensionTimes
                                            .Where(c => c.OutContractServicePackageId == receiptSrvPackage.Id);
                                        receiptVoucherLine.DiscountAmountSuspend = currentSuspensionTimes.Sum(s => s.DiscountAmount);
                                        receiptVoucherLine.DiscountDescription = string.Join("", currentSuspensionTimes.Select(s => $"- {s.Description}\n"));
                                        receiptVoucherLine.SPSuspensionTimeIds = string.Join(',', currentSuspensionTimes.Select(c => c.Id));
                                        handledSuspensionTimes.AddRange(currentSuspensionTimes.Select(c => c.Id));
                                    }

                                    receiptVoucherLine.TaxPercent = receiptVoucherLine.ReceiptVoucherLineTaxes.Sum(t => t.TaxValue);
                                    receiptVoucherLine.TaxAmount = receiptVoucherLine.SubTotalBeforeTax * (decimal)receiptVoucherLine.TaxPercent / 100;
                                    receiptVoucherLine.SubTotal = CurrencyUnit.RoundByCurrency(receiptVoucherLine.CurrencyUnitId,
                                        receiptVoucherLine.SubTotalBeforeTax + receiptVoucherLine.TaxAmount);
                                    receiptVoucherLine.GrandTotalBeforeTax = receiptVoucherLine.SubTotalBeforeTax;
                                    receiptVoucherLine.ReductionFee += receiptVoucherLine.DiscountAmountSuspend;

                                    if (receiptVoucherLine.SubTotal > receiptVoucherLine.ReductionFee)
                                    {
                                        receiptVoucherLine.GrandTotal = receiptVoucherLine.SubTotal - receiptVoucherLine.ReductionFee;
                                    }
                                    else
                                    {
                                        receiptVoucherLine.ReductionFee = receiptVoucherLine.SubTotal;
                                        receiptVoucherLine.GrandTotal = 0;
                                    }

                                    newReceiptVoucher.AddReceiptVoucherDetail(receiptVoucherLine);
                                    joinedServiceCodes.Add(telcoService.ServiceCode);
                                    receiptVoucherDetails.Add(_mapper.Map<ReceiptVoucherDetailInsertBulkModel>(receiptVoucherLine));

                                    receiptVoucherTaxes.AddRange(
                                        receiptVoucherLine.ReceiptVoucherLineTaxes.Select(s => _mapper.Map<ReceiptVoucherTaxInsertBulkModel>(s))
                                    );
                                }
                                newReceiptVoucher.Content = "Cước ";
                                var groupedByServices = newReceiptVoucher
                                    .ReceiptVoucherDetails.GroupBy(g => g.ServiceId);

                                var groupIndex = 0;
                                foreach (var group in groupedByServices)
                                {
                                    var minStartBillingDate = group.Min(c => c.StartBillingDate);
                                    var maxEndBillingDate = group.Max(c => c.EndBillingDate);
                                    newReceiptVoucher.Content
                                    += $"dịch vụ {group.FirstOrDefault()?.ServiceName}" +
                                        $" từ {minStartBillingDate?.ToString("dd/MM/yyyy")}" +
                                        $" đến {maxEndBillingDate?.ToString("dd/MM/yyyy")}";
                                    if (groupIndex < groupedByServices.Count() - 1)
                                    {
                                        newReceiptVoucher.Content += ", ";
                                    }
                                    groupIndex++;
                                }

                                newReceiptVoucher.Content += $" của hợp đồng số: {contract.ContractCode}";
                                var receiptMarketArea =
                                    marketAreas.FirstOrDefault(m => m.Id == newReceiptVoucher.MarketAreaId);

                                var receiptProject =
                                    projects.FirstOrDefault(m => m.Id == newReceiptVoucher.ProjectId);
                                var generationResponse = GenerateNewVoucherCode(_receiptVoucherQueries, orderByIssuedDate,
                                    voucherProject?.ProjectCode,
                                    voucherMarketArea?.MarketCode,
                                    newReceiptVoucher.IsEnterprise);
                                newReceiptVoucher.VoucherCode = generationResponse.Item1;
                                orderByIssuedDate = generationResponse.Item2;

                                //pass tax categoreis into calculate
                                newReceiptVoucher.CalculateTotal();

                                if (newReceiptVoucher.ReductionFreeTotal > 0 &&
                                    newReceiptVoucher.ReceiptVoucherDetails.Any(r => !string.IsNullOrEmpty(r.DiscountDescription)))
                                {
                                    newReceiptVoucher.ReductionReason = string.Join("",
                                        newReceiptVoucher.ReceiptVoucherDetails
                                        .Where(r => !string.IsNullOrEmpty(r.DiscountDescription))
                                        .Select(r => r.DiscountDescription)
                                    );
                                }

                                newReceiptVoucher.SetStatusId(ReceiptVoucherStatus.Pending.Id);
                                newReceiptVoucher.UpdateStatusOverdue();
                                newReceiptVoucher.UpdateStatusBadDebt(systemConfiguration.NumberDaysBadDebt ?? 60);

                                newReceiptVoucher.ActivatedDebt.ReceiptVoucherCode = newReceiptVoucher.VoucherCode;
                                newReceiptVoucher.ActivatedDebt.ReceiptVoucherContent = newReceiptVoucher.Content;

                                debtHistories.Add(_mapper.Map<DebtHistoryInsertBulkModel>(newReceiptVoucher.ActivatedDebt));

                                var bulkInsertVchrModel = _mapper.Map<ReceiptVoucherInsertBulkModel>(newReceiptVoucher);
                                receiptVouchers.Add(bulkInsertVchrModel);
                            }
                        }

                        timer.Stop();
                        Console.WriteLine("Resolving receipt voucher data time: " + timer.Elapsed.ToString(@"m\:ss"));

                        timer.Reset();
                        timer.Start();

                        int insertedVoucherTarget = 0;
                        if (newVoucherTargets.Any())
                        {
                            insertedVoucherTarget =
                                await _voucherTargetRepository.InsertBulk(newVoucherTargets);
                        }

                        int insertedReceiptVoucher = 0;
                        if (receiptVouchers.Any())
                        {
                            int lastVchrId = _receiptVoucherQueries.GetLastVoucherId();
                            insertedReceiptVoucher =
                                await _receiptVoucherRepository.InsertBulk(receiptVouchers);

                            if (insertedReceiptVoucher < receiptVouchers.Count)
                            {
                                var insertedVchrCodes = _receiptVoucherQueries.GetVoucherCodeFromStartingId(lastVchrId);
                                var notInsertedVouchers = receiptVouchers
                                    .Where(c => insertedVchrCodes.All(r => !r.Contains(c.VoucherCode)))
                                    .Select(c => c.VoucherCode)
                                    .ToList();

                                await _notificationGrpcService.SendSMS(new SendSmsRequestGrpc()
                                {
                                    PhoneNumbers = "0335905133",
                                    Message = $"{LOGGER_TAG}\n" +
                                        $"Total {needToPayContracts.Count} contracts\n" +
                                        $"FAILED {receiptVouchers.Count - insertedReceiptVoucher}/{receiptVouchers.Count} vouchers",
                                });

                                throw new DebtDomainException($"ReceiptVouchers: The number of inserted records is not equal to number of inputs. {insertedReceiptVoucher}/{receiptVouchers.Count}");
                            }
                        }

                        int insertedDebtHistory = 0;
                        if (debtHistories.Any())
                        {
                            insertedDebtHistory =
                                await _receiptVoucherDebtHistoryRepository.InsertBulk(debtHistories);

                            if (debtHistories.Count != insertedDebtHistory)
                            {
                                throw new DebtDomainException($"ReceiptVoucherDebtHistories: The number of inserted records is not equal to number of inputs. {insertedReceiptVoucher}/{receiptVouchers.Count}");
                            }
                        }

                        int insertedReceiptVoucherDetail = 0;
                        if (receiptVoucherDetails.Any())
                        {
                            insertedReceiptVoucherDetail = await
                                _receiptVoucherDetailRepository.InsertBulk(receiptVoucherDetails);

                            if (receiptVoucherDetails.Count != insertedReceiptVoucherDetail)
                            {
                                throw new DebtDomainException($"ReceiptVoucherDetails: The number of inserted records is not equal to number of inputs. {insertedReceiptVoucher}/{receiptVouchers.Count}");
                            }
                        }

                        int insertedReceiptVoucherTax = 0;
                        if (receiptVoucherTaxes.Any())
                        {
                            insertedReceiptVoucherTax =
                                await _receiptVoucherTaxRepository.InsertBulk(receiptVoucherTaxes);

                            if (receiptVoucherTaxes.Count != insertedReceiptVoucherTax)
                            {
                                throw new DebtDomainException($"ReceiptVoucherTaxes: The number of inserted records is not equal to number of inputs. {insertedReceiptVoucherTax}/{receiptVoucherTaxes.Count}");
                            }
                        }

                        int insertedVoucherPromotions = 0;
                        if (receiptVoucherPromotions.Any())
                        {
                            insertedVoucherPromotions =
                                await _receiptVoucherPromotionRepo.InsertBulk(receiptVoucherPromotions);

                            if (receiptVoucherPromotions.Count != insertedVoucherPromotions)
                            {
                                throw new DebtDomainException($"PromotionForReceiptVoucher: The number of inserted records is not equal to number of inputs. {insertedVoucherPromotions}/{receiptVoucherPromotions.Count}");
                            }
                        }

                        if (tempGeneratingVouchers.Any())
                        {
                            int insertedTemporaries = await _temporaryGeneratingVoucherRepository.InsertBulk(tempGeneratingVouchers);

                            if (tempGeneratingVouchers.Count != insertedTemporaries)
                            {
                                throw new DebtDomainException($"TemporaryGeneratingVoucher: The number of inserted records is not equal to number of inputs. {insertedReceiptVoucher}/{receiptVouchers.Count}");
                            }
                        }

                        if (handledSuspensionTimes.Any())
                        {
                            _outContractService.ActivateSuspensionHandled(string.Join(",", handledSuspensionTimes));
                        }

                        timer.Stop();

                        await _temporaryGeneratingVoucherRepository.JoinGenerated(_context, transaction);
                        await _voucherTargetRepository.UpdateCurrentDebtForAllTarget(_context, transaction);
                        _outContractService.UpdateNextBillingDateOfPayingContracts();
                        _outContractService.DeleteFromTemporaryPayingContracts();

                        Console.WriteLine("Insert data time:" + timer.Elapsed.ToString(@"m\:ss"));

                        var logMessage = $"Contracts: {needToPayContracts.Count}.\n" +
                                $"VoucherTargets: {insertedVoucherTarget}.\n" +
                                $"ReceiptVouchers: {insertedReceiptVoucher}.\n" +
                                $"ReceiptVoucherDetails: {insertedReceiptVoucherDetail}.\n";

                        _generateHistoryRepository.Create(new VoucherAutoGenerateHistory()
                        {
                            CreatedBy = "SYSTEM",
                            Message = logMessage,
                            Status = "SUCCESS",
                            TryTimes = RetryTimes,
                            Records = insertedReceiptVoucher
                        });

                        await _context.CommitTransactionAsync(transaction);

                        await _notificationGrpcService.SendSMS(new SendSmsRequestGrpc()
                        {
                            PhoneNumbers = "0335905133",
                            Message = $"{LOGGER_TAG}\n" +
                                $"SUCCESSFULLY\n" +
                                logMessage,
                        });

                        exitBehavior = GenerateExitBehavior.Done;
                        RetryTimes = 0;
                    }
                    catch (Exception e)
                    {
                        this._logger.LogError("DebtManagement.BackgroundTasks.Tasks.ReceiptVoucherGenerationTask {0}", e);
                        FailedTimes++;
                        /// Lưu lại lịch sử tự động tạo phiếu thu
                        await _generateHistoryRepository.CreateAndSave(new VoucherAutoGenerateHistory()
                        {
                            CreatedBy = "SYSTEM",
                            Message = e.Message,
                            Status = "FAILED",
                            TryTimes = RetryTimes
                        });

                        if (!(e is DebtDomainException) && FailedTimes <= 5)
                        {
                            await _notificationGrpcService.SendSMS(new SendSmsRequestGrpc()
                            {
                                PhoneNumbers = "0335905133",
                                Message = $"{LOGGER_TAG}\n" +
                                $"FAILED {e.Message}"
                            });
                        }
                        exitBehavior = GenerateExitBehavior.Retry;
                        goto Finish;
                    }
                }

            Finish:
                switch (exitBehavior)
                {
                    case GenerateExitBehavior.Delay:
                        this.IsDelayForFirstTime = false;
                        await Task.Delay(5 * 60 * 1000, stoppingToken);
                        break;
                    case GenerateExitBehavior.Done:
                        FailedTimes = 0;
                        var current = DateTime.UtcNow.AddHours(7);
                        var nextScheduleTime = current.AddDays(1).AddHours(2);
                        await Task.Delay(nextScheduleTime - current, stoppingToken);
                        break;
                    case GenerateExitBehavior.Retry:
                        RetryTimes++;
                        await Task.Delay(15 * 60 * 1000, stoppingToken);
                        break;
                    default:
                        await Task.Delay(1 * 60 * 60 * 1000, stoppingToken);
                        break;
                }

            }
        }

        private (string, int) GenerateNewVoucherCode(IReceiptVoucherQueries _receiptVoucherQueries, int orderByIssuedDate, string voucherProject, string voucherMarketArea, bool isEnterprise)
        {
            orderByIssuedDate++;
            var newVoucherCode = _receiptVoucherQueries.GetReceiptVoucherCode(DateTime.Now.AddHours(7),
                voucherProject,
                voucherMarketArea,
                isEnterprise,
                orderByIssuedDate);
            if (this.VoucherCodesByDate.Count > 0 &&
                this.VoucherCodesByDate.Contains(newVoucherCode))
            {
                return GenerateNewVoucherCode(_receiptVoucherQueries, orderByIssuedDate, voucherProject, voucherMarketArea, isEnterprise);
            }
            return (newVoucherCode, orderByIssuedDate);
        }
    }

    public enum GenerateExitBehavior
    {
        Delay,
        Retry,
        Done,
        Unscheduleed
    }
}