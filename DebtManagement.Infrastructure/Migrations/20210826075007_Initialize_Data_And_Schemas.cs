using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtManagement.Infrastructure.Migrations
{
    public partial class Initialize_Data_And_Schemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClearingStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearingStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    CurrencyUnitName = table.Column<string>(nullable: true),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    IssuingCountry = table.Column<string>(nullable: true),
                    CurrencyUnitSymbol = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethodTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVoucherStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVoucherStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherInPaymentVoucher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PaymentVoucherId = table.Column<int>(nullable: false),
                    ReceiptVoucherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherInPaymentVoucher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryGeneratingVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ReceiptVoucherId = table.Column<string>(nullable: true),
                    ReceiptVoucherDetailId = table.Column<string>(nullable: true),
                    VoucherTargetId = table.Column<string>(nullable: true),
                    DebtHistoryId = table.Column<string>(nullable: true),
                    VoucherTaxId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryGeneratingVouchers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherAutoGenerateHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Records = table.Column<long>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    TryTimes = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherAutoGenerateHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherPaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsPassive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherPaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherTargets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    TargetFullName = table.Column<string>(maxLength: 256, nullable: true),
                    TargetCode = table.Column<string>(maxLength: 256, nullable: true),
                    TargetAddress = table.Column<string>(maxLength: 1000, nullable: true),
                    TargetPhone = table.Column<string>(maxLength: 256, nullable: true),
                    TargetEmail = table.Column<string>(maxLength: 256, nullable: true),
                    TargetFax = table.Column<string>(maxLength: 256, nullable: true),
                    TargetIdNo = table.Column<string>(maxLength: 256, nullable: true),
                    TargetTaxIdNo = table.Column<string>(maxLength: 256, nullable: true),
                    TargetBRNo = table.Column<string>(nullable: true),
                    IsEnterprise = table.Column<bool>(nullable: false),
                    IsPayer = table.Column<bool>(nullable: false),
                    IsPartner = table.Column<bool>(nullable: false),
                    UserIdentityGuid = table.Column<string>(nullable: true),
                    ApplicationUserIdentityGuid = table.Column<string>(nullable: true),
                    CurrentDebt = table.Column<decimal>(nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clearings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    CodeClearing = table.Column<string>(nullable: true),
                    CalculatorUserId = table.Column<string>(nullable: true),
                    PaymentConfirmerUserId = table.Column<int>(nullable: false),
                    ClearingDate = table.Column<DateTime>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Payment_Form = table.Column<int>(nullable: true),
                    Payment_Method = table.Column<int>(nullable: true),
                    Payment_Address = table.Column<string>(nullable: true),
                    Payment_BankAccount = table.Column<string>(nullable: true),
                    Payment_BankName = table.Column<string>(nullable: true),
                    Payment_BankBranch = table.Column<string>(nullable: true),
                    TargetId = table.Column<int>(maxLength: 68, nullable: false),
                    CreatedUserId = table.Column<string>(nullable: true),
                    OrganizationUnitId = table.Column<string>(nullable: true),
                    ApprovalUserId = table.Column<string>(nullable: true),
                    CancelReason = table.Column<string>(nullable: true),
                    MarketAreaId = table.Column<int>(nullable: false),
                    MarketAreaName = table.Column<string>(nullable: true),
                    ClearingTotal = table.Column<decimal>(nullable: false),
                    TotalReceipt = table.Column<decimal>(nullable: false),
                    TotalPayment = table.Column<decimal>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ExchangeRateApplyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clearings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clearings_VoucherTargets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "VoucherTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    MarketAreaId = table.Column<int>(nullable: true),
                    MarketAreaName = table.Column<string>(maxLength: 256, nullable: true),
                    ContractCode = table.Column<string>(maxLength: 256, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 256, nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    VoucherCode = table.Column<string>(maxLength: 256, nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ReductionReason = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedUserId = table.Column<string>(maxLength: 68, nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 512, nullable: true),
                    OrganizationUnitId = table.Column<string>(maxLength: 68, nullable: true),
                    OrganizationUnitName = table.Column<string>(maxLength: 512, nullable: true),
                    InvoiceCode = table.Column<string>(maxLength: 68, nullable: true),
                    AccountingCode = table.Column<string>(maxLength: 68, nullable: true),
                    Source = table.Column<string>(maxLength: 68, nullable: true),
                    ApprovedUserId = table.Column<string>(maxLength: 68, nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    InvoiceReceivedDate = table.Column<DateTime>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(maxLength: 256, nullable: true),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ExchangeRateApplyDate = table.Column<DateTime>(nullable: false),
                    PaymentPeriod = table.Column<int>(nullable: false),
                    ReductionFreeTotal = table.Column<decimal>(nullable: false),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    EquipmentTotalAmount = table.Column<decimal>(nullable: false),
                    PromotionTotalAmount = table.Column<decimal>(nullable: false),
                    SubTotalBeforeTax = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    ClearingTotal = table.Column<decimal>(nullable: false),
                    CashTotal = table.Column<decimal>(nullable: false),
                    OtherFee = table.Column<decimal>(nullable: false),
                    GrandTotalBeforeTax = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    OpeningDebtAmount = table.Column<decimal>(nullable: false),
                    OpeningDebtPaidAmount = table.Column<decimal>(nullable: false),
                    GrandTotalIncludeDebt = table.Column<decimal>(nullable: false),
                    PaidTotal = table.Column<decimal>(nullable: false),
                    RemainingTotal = table.Column<decimal>(nullable: false),
                    TaxAmount = table.Column<decimal>(nullable: false),
                    Payment_Form = table.Column<int>(nullable: true),
                    Payment_Method = table.Column<int>(nullable: true),
                    Payment_Address = table.Column<string>(nullable: true),
                    Payment_BankAccount = table.Column<string>(nullable: true),
                    Payment_BankName = table.Column<string>(nullable: true),
                    Payment_BankBranch = table.Column<string>(nullable: true),
                    Discount_Percent = table.Column<float>(nullable: true),
                    Discount_Amount = table.Column<decimal>(nullable: true),
                    Discount_Type = table.Column<bool>(nullable: true),
                    IsEnterprise = table.Column<bool>(nullable: false),
                    IsLock = table.Column<bool>(nullable: false),
                    IsAutomaticGenerate = table.Column<bool>(nullable: false),
                    ClearingId = table.Column<int>(nullable: true),
                    NumberBillingLimitDays = table.Column<int>(nullable: false),
                    NumberDaysOverdue = table.Column<int>(nullable: false),
                    CancellationReason = table.Column<string>(nullable: true),
                    InContractId = table.Column<int>(nullable: false),
                    FixedGrandTotal = table.Column<decimal>(nullable: false),
                    PaymentApprovalUserId = table.Column<string>(nullable: true),
                    CashierUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_Clearings_ClearingId",
                        column: x => x.ClearingId,
                        principalTable: "Clearings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_VoucherTargets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "VoucherTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    MarketAreaId = table.Column<int>(nullable: true),
                    MarketAreaName = table.Column<string>(maxLength: 256, nullable: true),
                    ContractCode = table.Column<string>(maxLength: 256, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 256, nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    VoucherCode = table.Column<string>(maxLength: 256, nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ReductionReason = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedUserId = table.Column<string>(maxLength: 68, nullable: true),
                    CreatedUserName = table.Column<string>(maxLength: 512, nullable: true),
                    OrganizationUnitId = table.Column<string>(maxLength: 68, nullable: true),
                    OrganizationUnitName = table.Column<string>(maxLength: 512, nullable: true),
                    InvoiceCode = table.Column<string>(maxLength: 68, nullable: true),
                    AccountingCode = table.Column<string>(maxLength: 68, nullable: true),
                    Source = table.Column<string>(maxLength: 68, nullable: true),
                    ApprovedUserId = table.Column<string>(maxLength: 68, nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    InvoiceReceivedDate = table.Column<DateTime>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(maxLength: 256, nullable: true),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ExchangeRateApplyDate = table.Column<DateTime>(nullable: false),
                    PaymentPeriod = table.Column<int>(nullable: false),
                    ReductionFreeTotal = table.Column<decimal>(nullable: false),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    EquipmentTotalAmount = table.Column<decimal>(nullable: false),
                    PromotionTotalAmount = table.Column<decimal>(nullable: false),
                    SubTotalBeforeTax = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    ClearingTotal = table.Column<decimal>(nullable: false),
                    CashTotal = table.Column<decimal>(nullable: false),
                    OtherFee = table.Column<decimal>(nullable: false),
                    GrandTotalBeforeTax = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    OpeningDebtAmount = table.Column<decimal>(nullable: false),
                    OpeningDebtPaidAmount = table.Column<decimal>(nullable: false),
                    GrandTotalIncludeDebt = table.Column<decimal>(nullable: false),
                    PaidTotal = table.Column<decimal>(nullable: false),
                    RemainingTotal = table.Column<decimal>(nullable: false),
                    TaxAmount = table.Column<decimal>(nullable: false),
                    Payment_Form = table.Column<int>(nullable: true),
                    Payment_Method = table.Column<int>(nullable: true),
                    Payment_Address = table.Column<string>(nullable: true),
                    Payment_BankAccount = table.Column<string>(nullable: true),
                    Payment_BankName = table.Column<string>(nullable: true),
                    Payment_BankBranch = table.Column<string>(nullable: true),
                    Discount_Percent = table.Column<float>(nullable: true),
                    Discount_Amount = table.Column<decimal>(nullable: true),
                    Discount_Type = table.Column<bool>(nullable: true),
                    IsEnterprise = table.Column<bool>(nullable: false),
                    IsLock = table.Column<bool>(nullable: false),
                    IsAutomaticGenerate = table.Column<bool>(nullable: false),
                    ClearingId = table.Column<int>(nullable: true),
                    NumberBillingLimitDays = table.Column<int>(nullable: false),
                    NumberDaysOverdue = table.Column<int>(nullable: false),
                    OutContractId = table.Column<int>(nullable: false),
                    CashierCollectingDate = table.Column<DateTime>(nullable: true),
                    CashierReceivedDate = table.Column<DateTime>(nullable: true),
                    CancellationReason = table.Column<string>(nullable: true),
                    OffsetUpgradePackageAmount = table.Column<decimal>(nullable: false),
                    IsFirstVoucherOfContract = table.Column<bool>(nullable: false),
                    DiscountAmountSuspendTotal = table.Column<decimal>(nullable: false),
                    BadDebtApprovalContent = table.Column<string>(nullable: true),
                    IsBadDebt = table.Column<bool>(nullable: true),
                    NumberOfOpeningDebtHistories = table.Column<int>(nullable: false),
                    NumberOfDebtHistories = table.Column<int>(nullable: false),
                    InvalidIssuedDate = table.Column<bool>(nullable: false),
                    CashierDebtRemaningTotal = table.Column<decimal>(nullable: false),
                    TargetDebtRemaningTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptVouchers_Clearings_ClearingId",
                        column: x => x.ClearingId,
                        principalTable: "Clearings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptVouchers_VoucherTargets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "VoucherTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVoucherDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PaymentVoucherId = table.Column<int>(maxLength: 68, nullable: false),
                    ServiceId = table.Column<int>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    ServicePackageId = table.Column<int>(nullable: true),
                    ServicePackageName = table.Column<string>(maxLength: 256, nullable: true),
                    StartBillingDate = table.Column<DateTime>(nullable: false),
                    EndBillingDate = table.Column<DateTime>(nullable: false),
                    ServiceChannelId = table.Column<int>(nullable: true),
                    OutContractId = table.Column<int>(nullable: true),
                    PaymentPeriod = table.Column<int>(nullable: false),
                    OutContractIds = table.Column<string>(nullable: true),
                    TotalPerMonth = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    OtherFeeTotal = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVoucherDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentVoucherDetails_PaymentVouchers_PaymentVoucherId",
                        column: x => x.PaymentVoucherId,
                        principalTable: "PaymentVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVoucherPaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PaymentVoucherId = table.Column<int>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    PaymentMethodName = table.Column<string>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVoucherPaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentVoucherPaymentDetails_PaymentVouchers_PaymentVoucherId",
                        column: x => x.PaymentVoucherId,
                        principalTable: "PaymentVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVoucherTaxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    VoucherId = table.Column<int>(nullable: false),
                    TaxName = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    TaxValue = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVoucherTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentVoucherTaxes_PaymentVouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "PaymentVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionForReceiptVoucher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ReceiptVoucherId = table.Column<int>(nullable: true),
                    PromotionDetailId = table.Column<int>(nullable: false),
                    OutContractServicePackageId = table.Column<int>(nullable: false),
                    PromotionValue = table.Column<int>(nullable: true),
                    PromotionValueType = table.Column<int>(nullable: true),
                    PromotionAmount = table.Column<int>(nullable: false),
                    IsApplied = table.Column<bool>(nullable: false),
                    NumberMonthApplied = table.Column<int>(nullable: true),
                    ReceiptVoucherDetailId = table.Column<int>(nullable: false),
                    PromotionId = table.Column<int>(nullable: false),
                    PromotionName = table.Column<string>(nullable: true),
                    PromotionType = table.Column<int>(nullable: false),
                    PromotionTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionForReceiptVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionForReceiptVoucher_ReceiptVouchers_ReceiptVoucherId",
                        column: x => x.ReceiptVoucherId,
                        principalTable: "ReceiptVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherDebtHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ReceiptVoucherId = table.Column<int>(nullable: true),
                    ReceiptVoucherCode = table.Column<string>(nullable: true),
                    ReceiptVoucherContent = table.Column<string>(nullable: true),
                    SubstituteVoucherId = table.Column<int>(nullable: true),
                    CashierUserId = table.Column<string>(nullable: true),
                    CashierUserName = table.Column<string>(nullable: true),
                    CashierFullName = table.Column<string>(nullable: true),
                    NumberOfPaymentDetails = table.Column<int>(nullable: false),
                    IsAutomaticGenerate = table.Column<bool>(nullable: false),
                    OpeningCashierDebtTotal = table.Column<decimal>(nullable: false),
                    OpeningTargetDebtTotal = table.Column<decimal>(nullable: false),
                    CashingPaidTotal = table.Column<decimal>(nullable: false),
                    TransferringPaidTotal = table.Column<decimal>(nullable: false),
                    CashingAccountedTotal = table.Column<decimal>(nullable: false),
                    TransferringAccountedTotal = table.Column<decimal>(nullable: false),
                    IsOpeningDebtRecorded = table.Column<bool>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherDebtHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherDebtHistories_ReceiptVouchers_ReceiptVoucherId",
                        column: x => x.ReceiptVoucherId,
                        principalTable: "ReceiptVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherDebtHistories_ReceiptVouchers_SubstituteVouche~",
                        column: x => x.SubstituteVoucherId,
                        principalTable: "ReceiptVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(maxLength: 256, nullable: true),
                    ReceiptVoucherId = table.Column<int>(nullable: true),
                    ServiceId = table.Column<int>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    ServicePackageId = table.Column<int>(nullable: true),
                    ServicePackageName = table.Column<string>(maxLength: 256, nullable: true),
                    CId = table.Column<string>(nullable: true),
                    StartBillingDate = table.Column<DateTime>(nullable: true),
                    EndBillingDate = table.Column<DateTime>(nullable: true),
                    UsingMonths = table.Column<int>(nullable: false),
                    OrgPackagePrice = table.Column<decimal>(nullable: false),
                    IsHasPrice = table.Column<bool>(nullable: false),
                    PackagePrice = table.Column<decimal>(nullable: false),
                    PromotionAmount = table.Column<decimal>(nullable: false),
                    EquipmentTotalAmount = table.Column<decimal>(nullable: false),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    OffsetUpgradePackageAmount = table.Column<decimal>(nullable: false),
                    ReductionFee = table.Column<decimal>(nullable: false),
                    OtherFeeTotal = table.Column<decimal>(nullable: false),
                    SubTotalBeforeTax = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    TaxAmount = table.Column<decimal>(nullable: false),
                    TaxPercent = table.Column<float>(nullable: false),
                    GrandTotalBeforeTax = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    OutContractServicePackageId = table.Column<int>(nullable: false),
                    IsFirstDetailOfService = table.Column<bool>(nullable: false),
                    DiscountAmountSuspend = table.Column<decimal>(nullable: false),
                    SPSuspensionTimeIds = table.Column<string>(nullable: true),
                    IsAutomaticGenerate = table.Column<bool>(nullable: false),
                    IsShow = table.Column<bool>(nullable: false),
                    HasDistinguishBandwidth = table.Column<bool>(nullable: false),
                    HasStartAndEndPoint = table.Column<bool>(nullable: false),
                    PricingType = table.Column<int>(nullable: false),
                    OverloadUsageDataPrice = table.Column<decimal>(nullable: false),
                    IOverloadUsageDataPrice = table.Column<decimal>(nullable: false),
                    ConsumptionBasedPrice = table.Column<decimal>(nullable: false),
                    IConsumptionBasedPrice = table.Column<decimal>(nullable: false),
                    DataUsage = table.Column<decimal>(nullable: false),
                    DataUsageUnit = table.Column<decimal>(nullable: false),
                    IDataUsageUnit = table.Column<decimal>(nullable: false),
                    IDataUsage = table.Column<decimal>(nullable: false),
                    UsageDataAmount = table.Column<decimal>(nullable: false),
                    IUsageDataAmount = table.Column<decimal>(nullable: false),
                    IsMainPaymentChannel = table.Column<bool>(nullable: false),
                    IsJoinedPayment = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherDetails_ReceiptVouchers_ReceiptVoucherId",
                        column: x => x.ReceiptVoucherId,
                        principalTable: "ReceiptVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherPaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    DebtHistoryId = table.Column<int>(nullable: false),
                    ReceiptVoucherId = table.Column<int>(nullable: true),
                    PaymentMethod = table.Column<int>(nullable: false),
                    PaymentMethodName = table.Column<string>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CashierUserId = table.Column<string>(nullable: true),
                    CurrencyUnitCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherPaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherPaymentDetails_ReceiptVoucherDebtHistories_Deb~",
                        column: x => x.DebtHistoryId,
                        principalTable: "ReceiptVoucherDebtHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ReceiptVoucherId = table.Column<int>(nullable: true),
                    ClearingVoucherId = table.Column<int>(nullable: true),
                    ResourceStorage = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    FileType = table.Column<int>(nullable: false),
                    Extension = table.Column<string>(nullable: true),
                    RedirectLink = table.Column<string>(nullable: true),
                    ReceiptVoucherDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_Clearings_ClearingVoucherId",
                        column: x => x.ClearingVoucherId,
                        principalTable: "Clearings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_ReceiptVoucherDetails_ReceiptVoucherDetailId",
                        column: x => x.ReceiptVoucherDetailId,
                        principalTable: "ReceiptVoucherDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_ReceiptVouchers_ReceiptVoucherId",
                        column: x => x.ReceiptVoucherId,
                        principalTable: "ReceiptVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusTablePricingCalculators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ChannelId = table.Column<int>(nullable: true),
                    ChannelCid = table.Column<string>(nullable: true),
                    ReceiptVoucherLineId = table.Column<int>(nullable: false),
                    IsMainRcptVoucherLine = table.Column<bool>(nullable: false),
                    StartingBillingDate = table.Column<DateTime>(nullable: false),
                    Day = table.Column<DateTime>(nullable: false),
                    UsageDataByBaseUnit = table.Column<decimal>(nullable: false),
                    UsageData = table.Column<decimal>(nullable: false),
                    UsageDataUnit = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    IsDomestic = table.Column<bool>(nullable: true),
                    PricingType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTablePricingCalculators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusTablePricingCalculators_ReceiptVoucherDetails_ReceiptVouc~",
                        column: x => x.ReceiptVoucherLineId,
                        principalTable: "ReceiptVoucherDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelPriceBusTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ChannelId = table.Column<int>(nullable: true),
                    ReceiptVoucherLineId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    UsageValueFrom = table.Column<decimal>(nullable: true),
                    UsageBaseUomValueFrom = table.Column<decimal>(nullable: true),
                    UsageValueFromUomId = table.Column<int>(nullable: true),
                    UsageValueTo = table.Column<decimal>(nullable: true),
                    UsageBaseUomValueTo = table.Column<decimal>(nullable: true),
                    UsageValueToUomId = table.Column<int>(nullable: true),
                    BasedPriceValue = table.Column<decimal>(nullable: false),
                    PriceValue = table.Column<decimal>(nullable: false),
                    PriceUnitUomId = table.Column<int>(nullable: false),
                    IsDomestic = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPriceBusTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelPriceBusTables_ReceiptVoucherDetails_ReceiptVoucherLi~",
                        column: x => x.ReceiptVoucherLineId,
                        principalTable: "ReceiptVoucherDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherDetailReductions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ReceiptVoucherId = table.Column<int>(nullable: true),
                    ReceiptVoucherDetailId = table.Column<int>(nullable: false),
                    ReasonId = table.Column<string>(nullable: true),
                    ReductionReason = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    CId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherDetailReductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherDetailReductions_ReceiptVoucherDetails_Receipt~",
                        column: x => x.ReceiptVoucherDetailId,
                        principalTable: "ReceiptVoucherDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherLineTaxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    VoucherLineId = table.Column<int>(nullable: true),
                    TaxName = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    TaxValue = table.Column<float>(nullable: false),
                    IsAutomaticGenerate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptVoucherLineTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherLineTaxes_ReceiptVoucherDetails_VoucherLineId",
                        column: x => x.VoucherLineId,
                        principalTable: "ReceiptVoucherDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClearingStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mới tạo" },
                    { 2, "Đang chờ xử lý" },
                    { 3, "Đã hoàn thành" },
                    { 4, "Đã hủy" }
                });

            migrationBuilder.InsertData(
                table: "CurrencyUnits",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "CurrencyUnitCode", "CurrencyUnitName", "CurrencyUnitSymbol", "Description", "DisplayOrder", "IdentityGuid", "IsActive", "IsDeleted", "IssuingCountry", "OrganizationPath", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "VND", "Đồng", "đ", null, 0, null, true, false, "Việt Nam", null, null, null },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "USD", "Dollar", "$", null, 0, null, true, false, "United State", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethodTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tiền mặt" },
                    { 2, "Chuyển khoản" },
                    { 3, "Bù trừ" }
                });

            migrationBuilder.InsertData(
                table: "PaymentVoucherStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Đã hủy" },
                    { 4, "Đã thanh toán" },
                    { 5, "Quá hạn" },
                    { 7, "Kế toán từ chối" },
                    { 2, "Đã chuyển kế toán" },
                    { 1, "Đang chờ xử lý" },
                    { 3, "Đến hạn" }
                });

            migrationBuilder.InsertData(
                table: "ReceiptVoucherStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Đang chờ xử lý" },
                    { 6, "Thu hộ" },
                    { 2, "Đã chuyển kế toán" },
                    { 3, "Đã xuất hóa đơn" },
                    { 4, "Đã thu" },
                    { 5, "Đã hủy" },
                    { 7, "Đã xóa nợ xấu" },
                    { 8, "Nợ xấu" },
                    { 9, "Đã quá hạn thanh toán" }
                });

            migrationBuilder.InsertData(
                table: "ReceiptVoucherTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Phiếu thu tính cước không định kỳ" },
                    { 1, "Phiếu thu lần đầu" },
                    { 2, "Phiếu thu cước định kỳ" },
                    { 3, "Phiếu thu khác" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_ClearingVoucherId",
                table: "AttachmentFiles",
                column: "ClearingVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_OrganizationPath",
                table: "AttachmentFiles",
                column: "OrganizationPath");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_ReceiptVoucherDetailId",
                table: "AttachmentFiles",
                column: "ReceiptVoucherDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_ReceiptVoucherId",
                table: "AttachmentFiles",
                column: "ReceiptVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_BusTablePricingCalculators_ReceiptVoucherLineId",
                table: "BusTablePricingCalculators",
                column: "ReceiptVoucherLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPriceBusTables_ReceiptVoucherLineId",
                table: "ChannelPriceBusTables",
                column: "ReceiptVoucherLineId");

            migrationBuilder.CreateIndex(
                name: "IX_Clearings_OrganizationPath",
                table: "Clearings",
                column: "OrganizationPath");

            migrationBuilder.CreateIndex(
                name: "IX_Clearings_TargetId",
                table: "Clearings",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherDetails_OrganizationPath",
                table: "PaymentVoucherDetails",
                column: "OrganizationPath");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherDetails_PaymentVoucherId",
                table: "PaymentVoucherDetails",
                column: "PaymentVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherPaymentDetails_PaymentVoucherId",
                table: "PaymentVoucherPaymentDetails",
                column: "PaymentVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_ClearingId",
                table: "PaymentVouchers",
                column: "ClearingId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_OrganizationPath",
                table: "PaymentVouchers",
                column: "OrganizationPath");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_TargetId",
                table: "PaymentVouchers",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_VoucherCode",
                table: "PaymentVouchers",
                column: "VoucherCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherTaxes_VoucherId",
                table: "PaymentVoucherTaxes",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionForReceiptVoucher_ReceiptVoucherId",
                table: "PromotionForReceiptVoucher",
                column: "ReceiptVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDebtHistories_OrganizationPath",
                table: "ReceiptVoucherDebtHistories",
                column: "OrganizationPath");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDebtHistories_ReceiptVoucherId",
                table: "ReceiptVoucherDebtHistories",
                column: "ReceiptVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDebtHistories_SubstituteVoucherId",
                table: "ReceiptVoucherDebtHistories",
                column: "SubstituteVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetailReductions_ReceiptVoucherDetailId",
                table: "ReceiptVoucherDetailReductions",
                column: "ReceiptVoucherDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetails_IdentityGuid",
                table: "ReceiptVoucherDetails",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetails_OrganizationPath",
                table: "ReceiptVoucherDetails",
                column: "OrganizationPath");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetails_ReceiptVoucherId",
                table: "ReceiptVoucherDetails",
                column: "ReceiptVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherLineTaxes_VoucherLineId",
                table: "ReceiptVoucherLineTaxes",
                column: "VoucherLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherPaymentDetails_DebtHistoryId",
                table: "ReceiptVoucherPaymentDetails",
                column: "DebtHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_ClearingId",
                table: "ReceiptVouchers",
                column: "ClearingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_ContractCode",
                table: "ReceiptVouchers",
                column: "ContractCode");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_CreatedDate",
                table: "ReceiptVouchers",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_GrandTotal",
                table: "ReceiptVouchers",
                column: "GrandTotal");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_IdentityGuid",
                table: "ReceiptVouchers",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_IssuedDate",
                table: "ReceiptVouchers",
                column: "IssuedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_MarketAreaId",
                table: "ReceiptVouchers",
                column: "MarketAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_OrganizationPath",
                table: "ReceiptVouchers",
                column: "OrganizationPath");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_OrganizationUnitId",
                table: "ReceiptVouchers",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_OutContractId",
                table: "ReceiptVouchers",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_PaidTotal",
                table: "ReceiptVouchers",
                column: "PaidTotal");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_StatusId",
                table: "ReceiptVouchers",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_TargetId",
                table: "ReceiptVouchers",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_VoucherCode",
                table: "ReceiptVouchers",
                column: "VoucherCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryGeneratingVouchers_ReceiptVoucherDetailId",
                table: "TemporaryGeneratingVouchers",
                column: "ReceiptVoucherDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryGeneratingVouchers_ReceiptVoucherId",
                table: "TemporaryGeneratingVouchers",
                column: "ReceiptVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryGeneratingVouchers_VoucherTargetId",
                table: "TemporaryGeneratingVouchers",
                column: "VoucherTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherTargets_IdentityGuid",
                table: "VoucherTargets",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherTargets_OrganizationPath",
                table: "VoucherTargets",
                column: "OrganizationPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentFiles");

            migrationBuilder.DropTable(
                name: "BusTablePricingCalculators");

            migrationBuilder.DropTable(
                name: "ChannelPriceBusTables");

            migrationBuilder.DropTable(
                name: "ClearingStatuses");

            migrationBuilder.DropTable(
                name: "CurrencyUnits");

            migrationBuilder.DropTable(
                name: "PaymentMethodTypes");

            migrationBuilder.DropTable(
                name: "PaymentVoucherDetails");

            migrationBuilder.DropTable(
                name: "PaymentVoucherPaymentDetails");

            migrationBuilder.DropTable(
                name: "PaymentVoucherStatuses");

            migrationBuilder.DropTable(
                name: "PaymentVoucherTaxes");

            migrationBuilder.DropTable(
                name: "PromotionForReceiptVoucher");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherDetailReductions");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherInPaymentVoucher");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherLineTaxes");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherPaymentDetails");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherStatuses");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherTypes");

            migrationBuilder.DropTable(
                name: "TemporaryGeneratingVouchers");

            migrationBuilder.DropTable(
                name: "VoucherAutoGenerateHistories");

            migrationBuilder.DropTable(
                name: "VoucherPaymentMethods");

            migrationBuilder.DropTable(
                name: "PaymentVouchers");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherDetails");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherDebtHistories");

            migrationBuilder.DropTable(
                name: "ReceiptVouchers");

            migrationBuilder.DropTable(
                name: "Clearings");

            migrationBuilder.DropTable(
                name: "VoucherTargets");
        }
    }
}
