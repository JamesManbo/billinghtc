using ContractManagement.Domain.Commands.OutContractCommand;
using Global.Models.StateChangedResponse;
using MediatR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Domain.Commands.ProjectCommand;
using ContractManagement.Utility;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Commands.MarketAreaCommand;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using System.Text.RegularExpressions;
using ContractManagement.Domain.Models.Location;
using ContractManagement.Domain.Models.OutContracts;
using AutoMapper;
using ContractManagement.API.Grpc.Clients.Location;
using System.Diagnostics;
using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Application.IntegrationEvents.Events.ApplicationUserEvents;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.Domain.Models.Organizations;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Infrastructure.Repositories.ContractPckTaxRepository;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Infrastructure.Repositories.OutputChannelPointRepository;
using ContractManagement.Domain.Commands.ServiceCommand;
using ContractManagement.Infrastructure.Repositories.ServiceRepository;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Utilities;
using ContractManagement.Infrastructure.Repositories.PromotionRepository;

namespace ContractManagement.API.Application.Commands.OutContractCommandHandler
{
    public class ExcelWorksheetHandler
    {
        public ExcelWorksheet Sheet { get; set; }
        public int Row { get; set; }
        public ExcelWorksheetHandler(ExcelWorksheet worksheet, int row = 0)
        {
            this.Sheet = worksheet;
            this.Row = row;
        }
        public string Get(string column)
        {
            return Sheet.Cells[$"{column}{this.Row}"].Value?.ToString()?.Trim();
        }
        public string GetAscii(string column)
        {
            return Sheet.Cells[$"{column}{this.Row}"].Value?.ToString()?.Trim()?.ToAscii()?.ToUpper();
        }
        public decimal GetDecimal(string column, string currencyCode = "VND")
        {
            var cellValue = this.Get(column);
            if (string.IsNullOrWhiteSpace(cellValue)) return 0;
            var realNumber = (decimal?)this.GetRealNumberFromString(cellValue);
            return realNumber?.RoundByCurrency(currencyCode) ?? 0;
        }
        public DateTime? GetDateTime(string column)
        {
            var datetimeAsString = this.Get(column);
            if (string.IsNullOrWhiteSpace(datetimeAsString))
            {
                return default;
            }

            datetimeAsString = datetimeAsString.Replace(".", "/");

            if (Regex.IsMatch(datetimeAsString, @"^[0-9,\.]+$"))
            {
                try
                {
                    return DateTime.FromOADate(Convert.ToDouble(datetimeAsString));
                }
                catch (ArgumentException)
                {
                    return default;
                }
            }
            else if (DateTime.TryParseExact(datetimeAsString, "d/M/yyyy HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                out var result))
            {
                return result;
            }
            else if (DateTime.TryParseExact(datetimeAsString, "d/M/yyyy",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                out var result2))
            {
                return result2;
            }
            else if (DateTime.TryParseExact(datetimeAsString, "M/d/yyyy HH:mm:ss",
              System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
              out var result3))
            {
                return result3;
            }
            else if (DateTime.TryParseExact(datetimeAsString, "M/d/yyyy",
              System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
              out var result4))
            {
                return result4;
            }
            else if (DateTime.TryParseExact(datetimeAsString, "yyyy/M/d HH:mm:ss",
              System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
              out var result5))
            {
                return result5;
            }
            else if (DateTime.TryParseExact(datetimeAsString, "yyyy/M/d",
              System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
              out var result6))
            {
                return result6;
            }

            return default;
        }

        public int? GetInt(string column)
        {
            var intAsString = this.Get(column);
            if (string.IsNullOrWhiteSpace(intAsString)) return default;
            return (int?)this.GetNumberFromString(intAsString);
        }

        public float? GetFloat(string column)
        {
            var intAsString = this.Get(column);
            if (string.IsNullOrWhiteSpace(intAsString)) return default;
            return this.GetRealNumberFromString(intAsString);
        }

        public float GetRoom(string column)
        {
            var valueAsString = this.Get(column);
            if (string.IsNullOrWhiteSpace(valueAsString)) return default;

            var valueAscii = valueAsString.Trim().ToAscii();
            if (new Regex(@"(phong|can ho|can)\s+").IsMatch(valueAscii))
            {
                var internationalBw = valueAsString.Split('/')[1].Trim();
                if (!this.IsValidBandWidth(internationalBw)) return default;
                return this.GetNumberFromString(internationalBw) ?? 0;
            }

            return default;
        }

        public float GetDomesticBw(string column)
        {
            var valueAsString = this.Get(column);
            if (string.IsNullOrWhiteSpace(valueAsString)) return default;

            string domesticBandwidthValue;
            if (valueAsString.Contains("/"))
            {
                domesticBandwidthValue = string.Empty;
                foreach (var valuePart in valueAsString.Split('/'))
                {
                    if (Regex.IsMatch(valuePart, @"\d+", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    {
                        domesticBandwidthValue = valuePart;
                        break;
                    }
                }
            }
            else
            {
                domesticBandwidthValue = valueAsString;
            }

            if (!this.IsValidBandWidth(domesticBandwidthValue)) return default;
            return this.GetNumberFromString(domesticBandwidthValue) ?? 0;
        }
        private long? GetNumberFromString(string intAsString)
        {
            var numberPart = new Regex(@"\D+").Replace(intAsString, string.Empty);
            if (long.TryParse(numberPart, out var result))
            {
                return result;
            }
            return default;
        }
        private float? GetRealNumberFromString(string intAsString)
        {
            var numberMatch = new Regex(@"[\d\.\,]+").Match(intAsString);
            var number = numberMatch.Value.Replace(",", string.Empty);
            if (float.TryParse(number, out var result))
            {
                return result;
            }
            return default;
        }
        private bool IsValidBandWidth(string bandwidthString)
        {
            var isContainNumber = new Regex(@"\d+", RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(bandwidthString);
            var isNotIndex = new Regex(@"(?<=[^\,\.\w]|\d)\d+", RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(bandwidthString);
            return isContainNumber && isNotIndex;
        }
    }

    public class ImportOutContractCommandHandler : IRequestHandler<ImportOutContractCommand, ActionResponse<int>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IMarketAreaQueries _marketAreaQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly IContractorQueries _contractorQueries;
        private readonly IContractorRepository _contractorRepository;
        private readonly IServiceGroupsQueries _serviceGroupsQueries;
        private readonly IServicePackageQueries _servicePackageQueries;
        private readonly IServicesQueries _servicesQueries;
        private readonly ITaxCategoryQueries _taxCategoriesQueries;
        private readonly ILocationGrpcService _locationGrpcService;
        private readonly IContractPckTaxRepository _contractPckTaxRepository;
        private readonly IOutputChannelPointQueries _outputChannelPointQueries;
        private readonly IPromotionQueries _promotionQueries;

        private readonly IServicesRepository _serviceRepository;
        private readonly IOutContractRepository _outContractRepository;
        private readonly IOutputChannelPointRepository _outputChannelPointRepository;
        private readonly IOutContractQueries _outContractQueries;
        private readonly IContractSrvPckRepository _contractSrvPckRepository;
        private readonly IContractTotalByCurrencyRepository _contractTotalByCurrencyRepository;
        private readonly IPromotionForContractRepository _promotionForContractRepository;

        private readonly IContractIntegrationEventService _contractIntegrationEventService;
        private readonly IOrganizationUnitGrpcService _organizationUnitGrpcService;

        public ImportOutContractCommandHandler(
            IOutContractRepository outContractRepository,
            IOutContractQueries outContractQueries,
            IContractSrvPckRepository contractSrvPckRepository,
            IContractorRepository contractorRepository,
            IMediator mediator,
            IContractorQueries contractorQueries,
            IServicePackageQueries servicePackageQueries,
            IServicesQueries servicesQueries,
            ITaxCategoryQueries taxCategoriesQueries,
            IMapper mapper,
            ILocationGrpcService locationGrpcService,
            IMarketAreaQueries marketAreaQueries,
            IProjectQueries projectQueries,
            IContractIntegrationEventService contractIntegrationEventService,
            IOrganizationUnitGrpcService organizationUnitGrpcService,
            IContractPckTaxRepository contractPckTaxRepository,
            IOutputChannelPointQueries outputChannelPointQueries,
            IContractTotalByCurrencyRepository contractTotalByCurrencyRepository,
            IOutputChannelPointRepository outputChannelPointRepository,
            IServiceGroupsQueries serviceGroupsQueries,
            IServicesRepository serviceRepository,
            IPromotionQueries promotionQueries, IPromotionForContractRepository promotionForContractRepository)
        {
            this._contractorRepository = contractorRepository;
            this._outContractRepository = outContractRepository;
            this._outContractQueries = outContractQueries;
            this._contractSrvPckRepository = contractSrvPckRepository;
            this._mediator = mediator;
            this._contractorQueries = contractorQueries;
            this._servicePackageQueries = servicePackageQueries;
            this._servicesQueries = servicesQueries;
            this._taxCategoriesQueries = taxCategoriesQueries;
            this._mapper = mapper;
            this._locationGrpcService = locationGrpcService;
            this._marketAreaQueries = marketAreaQueries;
            this._projectQueries = projectQueries;
            this._contractIntegrationEventService = contractIntegrationEventService;
            this._organizationUnitGrpcService = organizationUnitGrpcService;
            this._contractPckTaxRepository = contractPckTaxRepository;
            this._outputChannelPointQueries = outputChannelPointQueries;
            this._contractTotalByCurrencyRepository = contractTotalByCurrencyRepository;
            this._outputChannelPointRepository = outputChannelPointRepository;
            this._serviceGroupsQueries = serviceGroupsQueries;
            this._serviceRepository = serviceRepository;
            this._promotionQueries = promotionQueries;
            this._promotionForContractRepository = promotionForContractRepository;
        }

        protected LocationDTO Vietnam { get; set; }
        protected List<MarketAreaDTO> MarketAreas { get; set; }
        protected List<ProjectDTO> Projects { get; set; }
        protected List<ContractorDTO> Contractors { get; set; }
        protected List<LocationDTO> VietnamCities { get; set; }
        protected List<LocationDTO> VietnamDistricts { get; set; }
        protected List<TaxCategoryDTO> TaxCategories { get; set; }
        protected List<OrganizationUnitDTO> OrganizationUnits { get; set; }
        public List<ServiceDTO> Services { get; set; }
        public List<ServicePackageDTO> ServicePackages { get; set; }
        public List<ServiceGroupDTO> ServiceGroups { get; set; }
        public List<PromotionDTO> Promotions { get; set; }
        public HashSet<string> ExistContractCodes { get; set; }
        public UnitOfMeasurement DefaultSpeedUnit = UnitOfMeasurement.Mbps;


        public async Task<ActionResponse<int>> Handle(ImportOutContractCommand request,
           CancellationToken cancellationToken)
        {
            try
            {
                var commandResponse = new ActionResponse<int>();
                using (var stream = new MemoryStream())
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    await request.FormFileOutContract.CopyToAsync(stream, cancellationToken);
                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var defaultVATTaxCmd = new CUOutContractServicePackageTaxCommand()
                    {
                        TaxCategoryId = TaxCategory.VAT.Id,
                        TaxCategoryName = TaxCategory.VAT.TaxName,
                        TaxValue = TaxCategory.VAT.TaxValue,
                        TaxCategoryCode = TaxCategory.VAT.TaxCode
                    };

                    using (var package = new ExcelPackage(stream))
                    {
                        var rowCount = package.Workbook.Worksheets[2].Dimension.Rows;
                        #region Contract domain categories data 
                        await InitializeDomainData();
                        var startingContractorIdx = this._contractorQueries.GetTotalNumber();
                        var startingContractorId = this._contractorQueries.GetLatestId();
                        var startingContractId = this._outContractQueries.GetLatestId();
                        var startingContractPckId = this._servicePackageQueries.GetLatestId();
                        var startingChannelPointId = this._outputChannelPointQueries.GetLatestId();
                        #endregion
                        var outContracts = new List<OutContract>();
                        var importingContractModels = new List<ImportOutContract>();
                        var importingContractorModels = new List<ImportContractor>();
                        var importingPromotions = new List<ImportOutContractPackagePromotion>();
                        var duplicateRows = new Dictionary<string, string>();
                        var duplicateContract = 0;
                        var duplicateChannel = 0;

                        var contractorId = startingContractorId;
                        for (int row = 5; row <= rowCount; row++)
                        {
                            var wsHandler = new ExcelWorksheetHandler(package.Workbook.Worksheets[2], row);

                            var contractCurrency = "DONG".EqualsIgnoreCase(wsHandler.GetAscii("AA"))
                                ? CurrencyUnit.VND
                                : CurrencyUnit.USD;
                            bool isDupplicateContract = false;
                            bool isTVService = wsHandler.GetAscii("AI").EqualsIgnoreCase("TV");

                            OutContract newOutContract;
                            var contractCode = wsHandler.Get("A");
                            if (string.IsNullOrWhiteSpace(contractCode))
                            {
                                commandResponse.AddError($"Import thất bại. Dữ liệu không hợp lệ tại dòng thứ {row}, số hợp đồng trống.", "ContractCode");
                                return commandResponse;
                            }

                            if (contractCode.Length < 6)
                            {
                                contractCode = contractCode.PadLeft(6, '0');
                            }

                            if (importingContractModels
                            .Any(c => c.ContractCode.Equals(contractCode, StringComparison.OrdinalIgnoreCase)))
                            {
                                isDupplicateContract = true;
                                newOutContract = outContracts.First(c => c.ContractCode.Equals(contractCode, StringComparison.OrdinalIgnoreCase));

                                duplicateContract++;

                                if (!isTVService)
                                {
                                    if (duplicateRows.TryGetValue(contractCode, out var cIds))
                                    {
                                        if (cIds.Split(",").Contains(wsHandler.Get("AF")))
                                        {
                                            duplicateChannel++;
                                            //commandResponse.AddError($"Import thất bại. Dữ liệu không hợp lệ tại dòng thứ {row}, mã C-Id đã tồn tại.", "ContractCode");
                                            continue;
                                        }
                                        else
                                        {
                                            cIds += "," + wsHandler.Get("AF");
                                        }
                                    }
                                    else
                                    {
                                        var insertedCIds = string.Join(",", newOutContract.ServicePackages.Select(c => c.CId));
                                        if (insertedCIds.Split(",").Contains(wsHandler.Get("AF")))
                                        {
                                            duplicateChannel++;
                                            //commandResponse.AddError($"Import thất bại. Dữ liệu không hợp lệ tại dòng thứ {row}, mã C-Id đã tồn tại.", "ContractCode");
                                            continue;
                                        }
                                        else
                                        {
                                            duplicateRows.Add(contractCode, insertedCIds + "," + wsHandler.Get("AF"));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                newOutContract = new OutContract
                                {
                                    IsActive = true,
                                    ContractCode = contractCode,
                                    CurrencyUnitId = contractCurrency.Id,
                                    CurrencyUnitCode = contractCurrency.CurrencyUnitCode,
                                    ContractorHTCId = Contractor.HTCHeadQuarter.Id,
                                    MarketAreaName = wsHandler.Get("L"),
                                    CityName = wsHandler.Get("I"),
                                    DistrictName = wsHandler.Get("J"),
                                    ProjectName = wsHandler.Get("AE"),
                                    TimeLine = new ContractTimeLine()
                                    {
                                        PaymentPeriod = wsHandler.GetInt("V") ?? 1,
                                        Signed = wsHandler.GetDateTime("O"),
                                        Effective = wsHandler.GetDateTime("P"),
                                        RenewPeriod = wsHandler.GetInt("U") ?? 12
                                    },
                                    Payment = new PaymentMethod(
                                    "TRA-TRUOC".Equals(wsHandler.GetAscii("AC"), StringComparison.OrdinalIgnoreCase)
                                        ? (int)PaymentMethodForm.Prepaid
                                        : (int)PaymentMethodForm.Postpaid,
                                    "CHUYEN-KHOAN".Equals(wsHandler.GetAscii("AB"), StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                                    string.IsNullOrWhiteSpace(wsHandler.Get("AO")) ? wsHandler.Get("BB") : wsHandler.Get("AO")),
                                    NumberBillingLimitDays = 30,
                                    CreatedDate = DateTime.UtcNow.AddHours(7),
                                    ContractTypeId = "CA-NHAN".Equals(wsHandler.GetAscii("B"), StringComparison.OrdinalIgnoreCase)
                                    ? OutContractType.Individual.Id
                                    : OutContractType.Enterprise.Id,
                                    ContractStatusId = ContractStatus.Signed.Id,
                                    ContractViolationType = 1
                                };
                            }

                            if (isTVService)
                            {
                                newOutContract.ContractNote = $"Mã hợp đồng dịch vụ truyền hình: {wsHandler.Get("AH")}";
                            }

                            if (newOutContract.TimeLine.Signed.HasValue)
                            {
                                newOutContract.TimeLine.Expiration = newOutContract.TimeLine.Signed.Value.AddMonths(
                                    newOutContract.TimeLine.RenewPeriod
                                ).AddDays(-1);
                            }

                            var contractMarketArea = await this.BindMarketAreaId(newOutContract.MarketAreaName);
                            newOutContract.MarketAreaId = contractMarketArea.MarketAreaId;

                            var contractLocation = this.BindLocation(wsHandler.Get("I"), wsHandler.Get("J"));
                            newOutContract.CityId = contractLocation.CityId;
                            newOutContract.CityName = contractLocation.CityName;
                            newOutContract.DistrictId = contractLocation.DistrictId;
                            newOutContract.DistrictName = contractLocation.DistrictName;

                            var contractProject = await this.BindProjectId(wsHandler.Get("AD"),
                                wsHandler.Get("AE"),
                                newOutContract.DistrictName,
                                newOutContract.DistrictId,
                                newOutContract.CityName,
                                newOutContract.CityId,
                                newOutContract.MarketAreaId);
                            newOutContract.ProjectId = contractProject.Item1;
                            newOutContract.ProjectName = contractProject.Item2;

                            newOutContract.OrganizationUnitId = this.BindOrganizationUnit(wsHandler.Get("Y"));


                            var importingContractorModel = new ImportContractor()
                            {
                                ContractorFullName = wsHandler.Get("F"),
                                ContractorShortName = wsHandler.Get("G"),
                                ContractorCode = wsHandler.Get("E"),
                                ContractorPhone = this.ResolvePhoneNumber(wsHandler.Get("H")),
                                ContractorEmail = string.Empty,
                                ContractorAddress = wsHandler.Get("K"),
                                ContractorTaxIdNo = wsHandler.Get("AP"),
                                IsBuyer = true,
                                IsPartner = false,
                                IsEnterprise = newOutContract.ContractTypeId == OutContractType.Enterprise.Id,
                                ContractorCity = newOutContract.CityName,
                                ContractorCityId = newOutContract.CityId,
                                ContractorDistrict = newOutContract.DistrictName,
                                ContractorDistrictId = newOutContract.DistrictId
                            };

                            var bindingContractorResult = this.BindContractor(importingContractorModel, contractorId);
                            newOutContract.ContractorId = bindingContractorResult.Item1;
                            if (bindingContractorResult.Item2)
                            {
                                importingContractorModel.Id = bindingContractorResult.Item1;
                                importingContractorModel.IdentityGuid = Guid.NewGuid().ToString();
                                importingContractorModel.ApplicationUserIdentityGuid = importingContractorModel.IdentityGuid;
                                importingContractorModels.Add(importingContractorModel);
                                contractorId = bindingContractorResult.Item1;
                                startingContractorIdx++;
                            }

                            var contractSrv = await FindService(wsHandler.Get("AI"), wsHandler.Get("AG"));

                            if (!isDupplicateContract)
                            {
                                newOutContract.Id = ++startingContractId;
                            }

                            var channelEndPoint = new CUOutputChannelPointCommand()
                            {
                                CurrencyUnitCode = newOutContract.CurrencyUnitCode,
                                CurrencyUnitId = newOutContract.CurrencyUnitId,
                                InstallationAddress = new InstallationAddress()
                                {
                                    Building = wsHandler.Get("BC"),
                                    Floor = wsHandler.Get("BD"),
                                    RoomNumber = wsHandler.Get("BE"),
                                    Street = wsHandler.Get("BB"),
                                    DistrictId = contractLocation.DistrictId,
                                    District = contractLocation.DistrictName,
                                    CityId = contractLocation.CityId,
                                    City = contractLocation.CityName,
                                    Country = this.Vietnam.Name,
                                    CountryId = this.Vietnam.LocationId
                                },
                                LocationId = Guid.NewGuid().ToString(),
                                PointType = OutputChannelPointTypeEnum.Output,
                            };

                            var channelCommand = new CUOutContractChannelCommand
                            {
                                EndPoint = channelEndPoint,
                                PaymentTargetId = newOutContract.ContractorId.Value,
                                ProjectId = newOutContract.ProjectId,
                                ProjectName = newOutContract.ProjectName,
                                CreatedBy = "Hệ thống",
                                CreatedDate = DateTime.UtcNow,
                                StatusId = OutContractServicePackageStatus.Developed.Id,
                                FlexiblePricingTypeId = FlexiblePricingType.FixedPricing.Id,
                                Type = ServiceChannelType.Output,
                                CId = isTVService ? string.Empty : wsHandler.Get("AF"),
                                CurrencyUnitCode = contractCurrency.CurrencyUnitCode,
                                CurrencyUnitId = contractCurrency.Id,
                                HasStartAndEndPoint = false,
                                IsInFirstBilling = false,
                                ServiceId = contractSrv.Id,
                                ServiceName = contractSrv.ServiceName,
                                PackagePrice = wsHandler.GetDecimal("AR"),
                                OrgPackagePrice = wsHandler.GetDecimal("AR"),
                                InstallationFee = wsHandler.GetDecimal("AS"),
                                OtherFee = wsHandler.GetDecimal("AT"),
                                TimeLine = new BillingTimeLine
                                {
                                    Signed = wsHandler.GetDateTime("O") ?? DateTime.Now.AddHours(7),
                                    Effective = wsHandler.GetDateTime("P"),
                                    PaymentPeriod = wsHandler.GetInt("V") ?? 1,
                                    DaysSuspended = 0,
                                    StartBilling = wsHandler.GetDateTime("Q"),
                                    LatestBilling = null,
                                    PaymentForm = (int)PaymentMethodForm.Prepaid,
                                    PrepayPeriod = wsHandler.GetInt("AV") ?? 0,
                                    DaysPromotion = wsHandler.GetInt("AX").HasValue ? wsHandler.GetInt("AX").Value * 30 : 0
                                },
                                IsActive = true,
                                IsTechnicalConfirmation = newOutContract.ProjectId.HasValue,
                                PaymentTarget = new CUContractorCommand()
                                {
                                    ContractorFullName = wsHandler.Get("F"),
                                    ContractorShortName = wsHandler.Get("G"),
                                    ContractorCode = wsHandler.Get("E"),
                                    ContractorPhone = this.ResolvePhoneNumber(wsHandler.Get("H")),
                                    ContractorEmail = string.Empty,
                                    ContractorAddress = wsHandler.Get("K"),
                                    ContractorTaxIdNo = wsHandler.Get("AP"),
                                    IsBuyer = true,
                                    IsPartner = false,
                                    IsEnterprise = newOutContract.ContractTypeId == OutContractType.Enterprise.Id
                                },
                                RadiusAccount = wsHandler.Get("BJ"),
                                RadiusPassword = wsHandler.Get("BK")
                            };

                            if (!string.IsNullOrWhiteSpace(wsHandler.GetAscii("AK")) && !isTVService)
                            {
                                var createSrvPackageCmd = new CUServicePackageCommand()
                                {
                                    ServiceId = contractSrv.Id,
                                    ServiceName = contractSrv.ServiceName,
                                    PackageName = wsHandler.Get("AJ"),
                                    PackageCode = wsHandler.GetAscii("AK"),
                                    DomesticBandwidthUomId = this.DefaultSpeedUnit.Id,
                                    DomesticBandwidthUom = this.DefaultSpeedUnit.Label,
                                    InternationalBandwidthUomId = this.DefaultSpeedUnit.Id,
                                    InternationalBandwidthUom = this.DefaultSpeedUnit.Label,
                                    CreateBy = "Hệ thống",
                                    Price = wsHandler.GetDecimal("AR"),
                                    InternationalBandwidth = wsHandler.GetInt("AM") ?? 0,
                                    DomesticBandwidth = wsHandler.GetInt("AL") ?? 0
                                };

                                var servicePackage = await this.BindServicePackageId(createSrvPackageCmd);
                                if (!servicePackage.Item1.HasValue)
                                {
                                    throw new ContractDomainException("Service package cannot be bound");
                                }

                                createSrvPackageCmd.PackageId = servicePackage.Item1.Value;
                                createSrvPackageCmd.PackageName = servicePackage.Item2;
                                createSrvPackageCmd.PackageCode = servicePackage.Item3;

                                channelCommand.DomesticBandwidth = createSrvPackageCmd.DomesticBandwidth;
                                channelCommand.DomesticBandwidthUom = createSrvPackageCmd.DomesticBandwidthUom;
                                channelCommand.InternationalBandwidth = createSrvPackageCmd.InternationalBandwidth;
                                channelCommand.InternationalBandwidthUom = createSrvPackageCmd.InternationalBandwidthUom;

                                channelCommand.BandwidthLabel = $"{createSrvPackageCmd.DomesticBandwidth}{createSrvPackageCmd.DomesticBandwidthUom}/" +
                                    $"{createSrvPackageCmd.InternationalBandwidth}{createSrvPackageCmd.InternationalBandwidthUom}";
                                channelCommand.PackageName = createSrvPackageCmd.PackageName;
                                channelCommand.ServicePackageId = createSrvPackageCmd.PackageId;
                            }

                            var contractPackageEntity = newOutContract.AddServicePackage(channelCommand);
                            contractPackageEntity.Id = ++startingChannelPointId;
                            contractPackageEntity.EndPoint.Id = ++startingChannelPointId;
                            contractPackageEntity.EndPointChannelId = contractPackageEntity.EndPoint.Id;

                            if (!string.IsNullOrEmpty(wsHandler.Get("AW")) &&
                                wsHandler.GetInt("AX").HasValue &&
                                wsHandler.GetInt("AX") > 1)
                            {
                                var promotion = this.Promotions.FirstOrDefault(c =>
                                    c.PromotionCode.Equals(wsHandler.Get("AW"), StringComparison.OrdinalIgnoreCase));
                                if (promotion != null)
                                {
                                    var promotionDetail = promotion.PromotionDetails.First();
                                    var promotionCmd = new ImportOutContractPackagePromotion()
                                    {
                                        IsApplied = true,
                                        NumberMonthApplied = wsHandler.GetInt("AX").Value,
                                        OutContractServicePackageId = contractPackageEntity.Id,
                                        PromotionId = promotion.Id,
                                        PromotionDetailId = promotionDetail.Id,
                                        PromotionName = promotion.PromotionName,
                                        PromotionType = promotion.PromotionType,
                                        PromotionTypeName = promotion.PromotionTypeString,
                                        PromotionValue = promotionDetail.NumberOfMonthApplied,
                                        PromotionValueType = promotionDetail.PromotionValueType
                                    };

                                    importingPromotions.Add(promotionCmd);
                                }
                            }

                            var nextBillingDate = wsHandler.GetDateTime("R");
                            if (nextBillingDate.HasValue)
                            {
                                contractPackageEntity.SetNextBillingDate(nextBillingDate.Value);
                            }

                            defaultVATTaxCmd.OutContractServicePackageId = contractPackageEntity.Id;
                            defaultVATTaxCmd.TaxValue = wsHandler.GetFloat("AU") ?? TaxCategory.VAT.TaxValue;

                            contractPackageEntity.AddOrUpdateTaxValue(defaultVATTaxCmd);
                            contractPackageEntity.CalculateTotal();
                            newOutContract.CalculateTotal();

                            if (isDupplicateContract)
                            {
                                importingContractModels.RemoveAll(c => c.ContractCode.Equals(newOutContract.ContractCode));
                            }
                            else
                            {
                                outContracts.Add(newOutContract);
                            }
                            var importingModel = this._mapper.Map<ImportOutContract>(newOutContract);
                            importingModel.ContractorCode = wsHandler.Get("E");
                            importingContractModels.Add(importingModel);
                        }

                        var channelsCount = importingContractModels.Select(c => c.ServicePackages.Count).Sum();
                        timer.Stop();
                        var milestone1 = timer.Elapsed;
                        Console.WriteLine("Resolving excel file:" + milestone1.ToString(@"m\:ss"));
                        timer.Reset();
                        timer.Start();
                        await this.ImportHandler(importingContractorModels, importingContractModels, importingPromotions);
                        timer.Stop();
                        var milestone2 = timer.Elapsed;
                        Console.WriteLine("Inserting data:" + milestone2.ToString(@"m\:ss"));
                        Console.WriteLine("Total:" + milestone2.Add(milestone1).ToString(@"m\:ss"));
                        commandResponse.SetResult(startingContractorId);

                        await _contractIntegrationEventService
                            .AddAndSaveEventAsync(new InsertBulkApplicationUserFromContractorIntegrationEvent(startingContractorId));
                        return commandResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task ImportHandler(List<ImportContractor> contractors,
            List<ImportOutContract> contracts,
            List<ImportOutContractPackagePromotion> promotions)
        {
            var importingOutputChannels = contracts
                .SelectMany(c => c.ServicePackages)
                .Select(c => c.EndPoint)
                .ToList();

            var importingOutContractPckTaxes = contracts
                .SelectMany(c => c.ServicePackages)
                .SelectMany(s => s.TaxValues)
                .Select(t => (object)t)
                .ToList();

            var importingOutContractSrvPcks = contracts
                .SelectMany(c => c.ServicePackages)
                .Select(s => (object)s)
                .ToList();

            var importingOutContracts = contracts.Select(c => (object)c).ToList();

            var importingOutContractors = contractors.Select(c => (object)c).ToList();

            var importingContractTotals = contracts
                .SelectMany(c => c.ContractTotalByCurrencies)
                .Select(ct => (object)ct).ToList();

            var importedOutContractors = await _contractorRepository.InsertBulk(importingOutContractors);
            if (importingOutContractors.Count < importedOutContractors) throw new ContractDomainException();

            var importedOutputChannels = await _outputChannelPointRepository.InsertBulk(importingOutputChannels);
            if (importingOutputChannels.Count < importedOutputChannels) throw new ContractDomainException();

            var importedOutContracts = await _outContractRepository.InsertBulk(importingOutContracts);
            if (importedOutContracts <= 0) throw new ContractDomainException();

            var importedContractTotals = await _contractTotalByCurrencyRepository.InsertBulk(importingContractTotals);
            if (importedContractTotals <= 0) throw new ContractDomainException();

            var importedOutContractSrvPcks = await _contractSrvPckRepository.InsertBulk(importingOutContractSrvPcks);
            if (importedOutContractSrvPcks <= 0) throw new ContractDomainException();

            var importedContractPckTaxes = await _contractPckTaxRepository.InsertBulk(importingOutContractPckTaxes);
            if (importedContractPckTaxes <= 0) throw new ContractDomainException();

            if (promotions.Count > 0)
            {
                var importedContractSrvPcks = await _promotionForContractRepository.InsertBulk(promotions);
                if (importedContractSrvPcks <= 0) throw new ContractDomainException();
            }

        }

        private async Task InitializeDomainData()
        {
            this.ExistContractCodes = this._outContractQueries.GetContractCodes();
            this.MarketAreas = this._marketAreaQueries.GetAll()?.ToList() ?? new List<MarketAreaDTO>();
            this.Projects = this._projectQueries.GetAll()?.ToList() ?? new List<ProjectDTO>();
            this.Contractors = this._contractorQueries.GetAll()?.ToList() ?? new List<ContractorDTO>();
            this.ServiceGroups = this._serviceGroupsQueries.GetAll()?.ToList() ?? new List<ServiceGroupDTO>();
            this.Services = this._servicesQueries.GetAll()?.ToList() ?? new List<ServiceDTO>();
            this.ServicePackages = this._servicePackageQueries.GetAll()?.ToList() ?? new List<ServicePackageDTO>();
            this.TaxCategories = this._taxCategoriesQueries.GetAll()?.ToList() ?? new List<TaxCategoryDTO>();

            this.Vietnam = await this._locationGrpcService.GetByLocationCode("VN");
            this.VietnamCities = await this._locationGrpcService.GetListByLevel(1);
            this.VietnamDistricts = await this._locationGrpcService.GetListByLevel(2);
            this.OrganizationUnits = (await this._organizationUnitGrpcService.GetAll())?.ToList() ?? new List<OrganizationUnitDTO>();
            this.Promotions = this._promotionQueries.GetAll()?.ToList() ?? new List<PromotionDTO>();
        }

        private (string CityId, string CityName, string DistrictId, string DistrictName, string AreaCode) BindLocation(string cityName, string districtName)
        {
            var city = this.VietnamCities.First(c => c.Name.ToAscii().ContainsIgnoreCase(cityName.ToAscii()));
            var district = this.VietnamDistricts.First(c => c.Name.ToAscii().ContainsIgnoreCase(districtName.ToAscii()));

            return (city?.LocationId?.Trim(), city?.Name?.Trim(), district?.LocationId?.Trim(), district?.Name?.Trim(), city?.Code);
        }

        private async Task<(int?, string)> BindProjectId(string projectName,
            string projectCode,
            string district,
            string districtId,
            string city,
            string cityId,
            int? marketAreaId = null)
        {
            if (string.IsNullOrWhiteSpace(projectName) || string.IsNullOrWhiteSpace(projectCode)) return default;
            if (this.Projects.All(x => !x.ProjectCode.Equals(projectCode, StringComparison.OrdinalIgnoreCase)))
            {
                var addProjectCmd = new CUProjectCommand
                {
                    ProjectName = projectName,
                    ProjectCode = projectCode,
                    District = district,
                    DistrictId = districtId,
                    City = city,
                    CityId = cityId,
                    MarketAreaId = marketAreaId
                };

                var addProjectCmdResponse = await _mediator.Send(addProjectCmd);
                if (!addProjectCmdResponse.IsSuccess)
                    throw new ContractDomainException();
                this.Projects.Add(addProjectCmdResponse.Result);
                return (addProjectCmdResponse.Result.Id, addProjectCmdResponse.Result.ProjectName);
            }
            else
            {
                var existedProject = this.Projects.First(x => x.ProjectCode.Equals(projectCode, StringComparison.OrdinalIgnoreCase));
                return (existedProject.Id, existedProject.ProjectName);
            }
        }

        public (int, bool) BindContractor(ImportContractor contractor, int startingContractorId)
        {
            var existingContractor = this.Contractors.FirstOrDefault(c => c.ContractorFullName.EqualsIgnoreCase(contractor.ContractorFullName) &&
                    (string.IsNullOrEmpty(c.ContractorPhone) || c.ContractorPhone.Contains(contractor.ContractorPhone)));

            if (existingContractor != null)
            {
                return (existingContractor.Id, false);
            }
            else
            {
                contractor.Id = startingContractorId + 1;
                this.Contractors.Add(_mapper.Map<ContractorDTO>(contractor));
                return (contractor.Id, true);
            }
        }

        public async Task<(int MarketAreaId, string MarketAreaCode)> BindMarketAreaId(string marketAreaName)
        {
            if (string.IsNullOrWhiteSpace(marketAreaName))
            {
                throw new ContractDomainException();
            }

            if (this.MarketAreas.All(x => !x.MarketName.Equals(marketAreaName, StringComparison.OrdinalIgnoreCase)))
            {
                var addMarketAreaCmd = new CUMarketAreaCommand
                {
                    MarketName = marketAreaName,
                    MarketCode = marketAreaName?.ToAscii("_").ToUpper(),
                };
                var addMarketAreaResponse = await _mediator.Send(addMarketAreaCmd);
                if (!addMarketAreaResponse.IsSuccess) throw new ContractDomainException();

                this.MarketAreas.Add(addMarketAreaResponse.Result);
                return (addMarketAreaResponse.Result.Id, addMarketAreaResponse.Result.MarketCode);
            }
            else
            {
                var existedMarketArea = this.MarketAreas.First(x => x.MarketName.Equals(marketAreaName, StringComparison.OrdinalIgnoreCase));
                return (existedMarketArea.Id, existedMarketArea.MarketCode);
            }
        }

        public string BindOrganizationUnit(string orgName)
        {
            var salesmanOrg = this.OrganizationUnits
                .FirstOrDefault(o => o.Name.ToAscii().EqualsIgnoreCase(orgName.ToAscii()));
            return salesmanOrg.IdentityGuid.ToString();
        }

        public async Task<ServiceDTO> FindService(string serviceCode, string serviceName)
        {
            var result = this.Services
                    .Find(c => c.ServiceCode.Equals(serviceCode, StringComparison.OrdinalIgnoreCase));

            if (result == null)
            {
                var serviceGroup = ServiceGroups.FirstOrDefault(c => c.GroupCode.Equals("NTD"));
                var createNewServiceCmd = new ServiceCommand
                {
                    GroupId = serviceGroup?.Id,
                    GroupName = serviceGroup?.GroupName,
                    ServiceName = serviceName,
                    ServiceCode = serviceCode,
                    IsActive = true,
                    HasCableKilometers = false,
                    HasStartAndEndPoint = false,
                    HasDistinguishBandwidth = false,
                    HasLineQuantity = false,
                    HasPackages = true
                };

                var saveServiceResponse = await _serviceRepository.CreateAndSave(createNewServiceCmd);
                var serviceDto = _mapper.Map<ServiceDTO>(saveServiceResponse.Result);
                this.Services.Add(serviceDto);
                return serviceDto;
            }

            return result;
        }

        private string ResolveName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;

            var resultAsArray = new List<string>();
            var optimizedSource = new Regex(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Multiline).Replace(name.Trim(), " ");
            var sourceParts = Regex.Split(optimizedSource, @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (var part in sourceParts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;
                if (part.Length == 1 && new Regex(@"[""\'\(\)\[\]\{\}\<\>]").IsMatch(part))
                {
                    resultAsArray.Add(part);
                }
                else
                {
                    resultAsArray.Add(part.ToUpperFirstLetterOnly() + (sourceParts.Last().Equals(part) ? string.Empty : " "));
                }
            }
            return string.Join("", resultAsArray);
        }
        private string ResolvePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return string.Empty;

            var resultAsArray = new List<string>();
            var optimizedSource = new Regex(@"[\s\.]", RegexOptions.IgnoreCase | RegexOptions.Multiline).Replace(phoneNumber, string.Empty);
            var phoneNumberMatches = new Regex(@"\d+", RegexOptions.IgnoreCase | RegexOptions.Multiline).Matches(optimizedSource);
            foreach (var match in phoneNumberMatches)
            {
                var phoneNo = match.ToString();
                if (phoneNo.Length == 10)
                {
                    resultAsArray.Add(phoneNo);
                }
                else if (phoneNo.Length > 10)
                {
                    foreach (var item in phoneNo.SplitByLength(10))
                    {
                        if (item.Length == 10) resultAsArray.Add(item);
                    }
                }
            }
            return string.Join(",", resultAsArray);
        }
        //private string ResolveUniqeContractCode(string contractCode)
        //{
        //    if (!this.ExistContractCodes.Contains(contractCode) || !contractCode.Contains('/'))
        //    {
        //        this.ExistContractCodes.Add(contractCode);
        //        return contractCode;
        //    }

        //    var resolveCodeStrBuilder = new List<string>();
        //    string codePrefix = contractCode.Split('/').First();
        //    foreach (var item in contractCode.Split('/'))
        //    {
        //        if (codePrefix.Equals(item))
        //        {
        //            var timePart = item.Split('-')[0];
        //            var indexPart = item.Split('-')[1];
        //            if (int.TryParse(indexPart, out var index))
        //            {
        //                indexPart = (++index).ToString().PadLeft(indexPart.Length, '0');
        //            }
        //            resolveCodeStrBuilder.Add($"{timePart}-{indexPart}");
        //        }
        //        else
        //        {
        //            resolveCodeStrBuilder.Add(item);
        //        }
        //    }

        //    var resolvedCode = string.Join('/', resolveCodeStrBuilder);
        //    if (this.ExistContractCodes.Contains(resolvedCode))
        //    {
        //        return this.ResolveUniqeContractCode(resolvedCode);
        //    }

        //    this.ExistContractCodes.Add(resolvedCode);
        //    return resolvedCode;
        //}

        public async Task<(int?, string, string)> BindServicePackageId(CUServicePackageCommand createSrvPackageCmd)
        {
            if (string.IsNullOrWhiteSpace(createSrvPackageCmd.PackageCode)) return default;

            var existedSrvPackage = this.ServicePackages.FirstOrDefault(s =>
                    RemoveSpecialCharactors(s.PackageCode).Equals(RemoveSpecialCharactors(createSrvPackageCmd.PackageCode), StringComparison.OrdinalIgnoreCase));
            if (existedSrvPackage != null)
            {
                return (existedSrvPackage.Id, existedSrvPackage.PackageName, existedSrvPackage.PackageCode);
            }
            else
            {
                var createdSrvPackageRsp = await _mediator.Send(createSrvPackageCmd);
                if (!createdSrvPackageRsp.IsSuccess) throw new ContractDomainException();

                this.ServicePackages.Add(createdSrvPackageRsp.Result);
                return (createdSrvPackageRsp.Result.Id, createdSrvPackageRsp.Result.PackageName, createdSrvPackageRsp.Result.PackageCode);
            }
        }

        public string RemoveSpecialCharactors(string value)
        {
            return Regex.Replace(value, @"[^0-9a-zA-Z]+", "");
        }

        public int GetContractStatus(string contractStatus)
        {
            switch (contractStatus?.DeepTrim()?.ToAscii()?.ToUpper())
            {
                case "THANH-LY":
                    return ContractStatus.Liquidated.Id;
                default:
                    return ContractStatus.Signed.Id;
            }
        }

        public int GetContractTypeId(string contractorName)
        {
            if (contractorName.ToString().Equals("Chi nhánh") || contractorName.ToString().Equals("Công ty")
                || contractorName.ToString().Equals("CT") || contractorName.ToString().Equals("CTY"))
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }
}
