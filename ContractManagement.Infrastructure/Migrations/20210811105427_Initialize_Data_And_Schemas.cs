using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.Migrations
{
    public partial class Initialize_Data_And_Schemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrasInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    ManualAPIPort = table.Column<int>(nullable: false),
                    SSHPort = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrasInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ChannelGroupName = table.Column<string>(nullable: true),
                    ChannelGroupCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ChannelGroupType = table.Column<int>(nullable: false),
                    ContractorIdGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    ContractorFullName = table.Column<string>(nullable: true),
                    ContractorShortName = table.Column<string>(nullable: true),
                    ContractorUserName = table.Column<string>(nullable: true),
                    ContractorCode = table.Column<string>(nullable: true),
                    ContractorPhone = table.Column<string>(nullable: true),
                    ContractorEmail = table.Column<string>(nullable: true),
                    ContractorFax = table.Column<string>(nullable: true),
                    AccountingCustomerCode = table.Column<string>(maxLength: 256, nullable: true),
                    ContractorCity = table.Column<string>(nullable: true),
                    ContractorCityId = table.Column<string>(nullable: true),
                    ContractorDistrict = table.Column<string>(nullable: true),
                    ContractorDistrictId = table.Column<string>(nullable: true),
                    ContractorAddress = table.Column<string>(nullable: true),
                    ContractorIdNo = table.Column<string>(nullable: true),
                    ContractorTaxIdNo = table.Column<string>(nullable: true),
                    IsEnterprise = table.Column<bool>(nullable: false),
                    IsBuyer = table.Column<bool>(nullable: false),
                    IsPartner = table.Column<bool>(nullable: false),
                    IsHTC = table.Column<bool>(nullable: false),
                    UserIdentityGuid = table.Column<string>(nullable: true),
                    ApplicationUserIdentityGuid = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    Representative = table.Column<string>(maxLength: 256, nullable: true),
                    Position = table.Column<string>(maxLength: 256, nullable: true),
                    AuthorizationLetter = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                name: "EquipmentPictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PictureId = table.Column<int>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentPictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    UnitOfMeasurementId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Specifications = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    DeviceSupplies = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    CurrencyName = table.Column<string>(nullable: true),
                    Buy = table.Column<string>(nullable: true),
                    BuyValue = table.Column<double>(nullable: false),
                    Transfer = table.Column<string>(nullable: true),
                    TransferValue = table.Column<double>(nullable: false),
                    Sell = table.Column<string>(nullable: true),
                    SellValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlexiblePricingType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlexiblePricingType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InContractStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InContractStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InContractTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InContractTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagementBusinessBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    BusinessBlockName = table.Column<string>(maxLength: 256, nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementBusinessBlocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    TreeLevel = table.Column<int>(nullable: false),
                    TreePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutContractServicePackageStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContractServicePackageStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutContractStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContractStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutContractTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContractTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputChannelPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    LocationId = table.Column<string>(maxLength: 68, nullable: true),
                    PointType = table.Column<int>(nullable: false),
                    InstallationAddress_Building = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_Floor = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_RoomNumber = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_Street = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_District = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_DistrictId = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_City = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_CityId = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    OtherFee = table.Column<decimal>(nullable: false),
                    MonthlyCost = table.Column<decimal>(nullable: false),
                    EquipmentAmount = table.Column<decimal>(nullable: false),
                    ApplyFeeToChannel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputChannelPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    FileName = table.Column<string>(maxLength: 256, nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Order = table.Column<int>(nullable: true),
                    PictureType = table.Column<int>(nullable: false),
                    Extension = table.Column<string>(nullable: false),
                    RedirectLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromotionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PromotionId = table.Column<int>(nullable: false),
                    PromotionValueType = table.Column<int>(nullable: false),
                    PromotionValue = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    DistrictId = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    AreaId = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<string>(nullable: true),
                    ServicePackageId = table.Column<int>(nullable: false),
                    NumberOfMonthApplied = table.Column<int>(nullable: false),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromotionProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PromotionDetailId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsOurProduct = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PromotionCode = table.Column<string>(nullable: true),
                    PromotionName = table.Column<string>(nullable: true),
                    PromotionType = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromotionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    PromotionName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromotionValueTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionValueTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RadiusServerInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    MarketAreaId = table.Column<int>(nullable: true),
                    DatabasePort = table.Column<int>(nullable: false),
                    ServerName = table.Column<string>(nullable: true),
                    SSHUserName = table.Column<string>(nullable: true),
                    SSHPassword = table.Column<string>(nullable: true),
                    DatabaseUserName = table.Column<string>(nullable: true),
                    DatabasePassword = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiusServerInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReasonTypeContractTerminations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonTypeContractTerminations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salesman",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IdentityGuid = table.Column<string>(maxLength: 20, nullable: true),
                    FullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salesman", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    GroupCode = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    TaxName = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    TaxValue = table.Column<float>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    ExplainTax = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryPayingContracts",
                columns: table => new
                {
                    OutContractId = table.Column<int>(nullable: false),
                    ServicePackageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryPayingContracts", x => new { x.OutContractId, x.ServicePackageId });
                });

            migrationBuilder.CreateTable(
                name: "TransactionChannelPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    LocationId = table.Column<string>(maxLength: 68, nullable: true),
                    PointType = table.Column<int>(nullable: false),
                    InstallationAddress_Building = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_Floor = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_RoomNumber = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_Street = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_District = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_DistrictId = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_City = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationAddress_CityId = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    OtherFee = table.Column<decimal>(nullable: false),
                    MonthlyCost = table.Column<decimal>(nullable: false),
                    EquipmentAmount = table.Column<decimal>(nullable: false),
                    ApplyFeeToChannel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionChannelPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ReasonType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Permission = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsBaseOfType = table.Column<bool>(nullable: false),
                    ConversionRate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractorProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ContractorId = table.Column<int>(nullable: false),
                    ContractorStructureId = table.Column<int>(nullable: true),
                    ContractorCategoryId = table.Column<int>(nullable: true),
                    ContractorGroupIds = table.Column<string>(nullable: true),
                    ApplicationUserIdentityGuid = table.Column<string>(maxLength: 68, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractorProperties_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InContracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    ContractCode = table.Column<string>(maxLength: 256, nullable: true),
                    AgentId = table.Column<string>(maxLength: 256, nullable: true),
                    AgentCode = table.Column<string>(maxLength: 256, nullable: true),
                    MarketAreaId = table.Column<int>(nullable: true),
                    MarketAreaName = table.Column<string>(maxLength: 256, nullable: true),
                    CityId = table.Column<string>(maxLength: 128, nullable: true),
                    CityName = table.Column<string>(maxLength: 256, nullable: true),
                    DistrictId = table.Column<string>(maxLength: 128, nullable: true),
                    DistrictName = table.Column<string>(maxLength: 256, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 256, nullable: true),
                    ContractTypeId = table.Column<int>(nullable: true),
                    ContractStatusId = table.Column<int>(nullable: false),
                    ContractorId = table.Column<int>(nullable: true),
                    ContractorHTCId = table.Column<int>(nullable: true),
                    SignedUserId = table.Column<string>(nullable: true),
                    SignedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    OrganizationUnitName = table.Column<string>(maxLength: 256, nullable: true),
                    SalesmanId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    Payment_Form = table.Column<int>(nullable: true),
                    Payment_Method = table.Column<int>(nullable: true),
                    Payment_Address = table.Column<string>(nullable: true),
                    TimeLine_RenewPeriod = table.Column<int>(nullable: true),
                    TimeLine_PaymentPeriod = table.Column<int>(nullable: true),
                    TimeLine_Expiration = table.Column<DateTime>(nullable: true),
                    TimeLine_Liquidation = table.Column<DateTime>(nullable: true),
                    TimeLine_Effective = table.Column<DateTime>(nullable: true),
                    TimeLine_Signed = table.Column<DateTime>(nullable: true),
                    TotalTaxPercent = table.Column<float>(nullable: false),
                    IsIncidentControl = table.Column<bool>(nullable: false),
                    IsControlUsageCapacity = table.Column<bool>(nullable: false),
                    NumberBillingLimitDays = table.Column<int>(nullable: false),
                    InterestOnDefferedPayment = table.Column<int>(nullable: true),
                    ContractViolation = table.Column<int>(nullable: true),
                    ContractViolationType = table.Column<int>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(maxLength: 256, nullable: true),
                    IsHasOneCurrency = table.Column<bool>(nullable: false),
                    FiberNodeInfo = table.Column<string>(nullable: true),
                    ContractNote = table.Column<string>(nullable: true),
                    OrganizationUnitId = table.Column<string>(nullable: true),
                    CashierUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InContracts_ContractStatus_ContractStatusId",
                        column: x => x.ContractStatusId,
                        principalTable: "ContractStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InContracts_Contractors_ContractorHTCId",
                        column: x => x.ContractorHTCId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InContracts_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutContracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    IdentityGuid = table.Column<string>(maxLength: 68, nullable: true),
                    ContractCode = table.Column<string>(maxLength: 256, nullable: true),
                    AgentId = table.Column<string>(maxLength: 256, nullable: true),
                    AgentCode = table.Column<string>(maxLength: 256, nullable: true),
                    MarketAreaId = table.Column<int>(nullable: true),
                    MarketAreaName = table.Column<string>(maxLength: 256, nullable: true),
                    CityId = table.Column<string>(maxLength: 128, nullable: true),
                    CityName = table.Column<string>(maxLength: 256, nullable: true),
                    DistrictId = table.Column<string>(maxLength: 128, nullable: true),
                    DistrictName = table.Column<string>(maxLength: 256, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 256, nullable: true),
                    ContractTypeId = table.Column<int>(nullable: true),
                    ContractStatusId = table.Column<int>(nullable: false),
                    ContractorId = table.Column<int>(nullable: true),
                    ContractorHTCId = table.Column<int>(nullable: true),
                    SignedUserId = table.Column<string>(nullable: true),
                    SignedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    OrganizationUnitName = table.Column<string>(maxLength: 256, nullable: true),
                    SalesmanId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    Payment_Form = table.Column<int>(nullable: true),
                    Payment_Method = table.Column<int>(nullable: true),
                    Payment_Address = table.Column<string>(nullable: true),
                    TimeLine_RenewPeriod = table.Column<int>(nullable: true),
                    TimeLine_PaymentPeriod = table.Column<int>(nullable: true),
                    TimeLine_Expiration = table.Column<DateTime>(nullable: true),
                    TimeLine_Liquidation = table.Column<DateTime>(nullable: true),
                    TimeLine_Effective = table.Column<DateTime>(nullable: true),
                    TimeLine_Signed = table.Column<DateTime>(nullable: true),
                    TotalTaxPercent = table.Column<float>(nullable: false),
                    IsIncidentControl = table.Column<bool>(nullable: false),
                    IsControlUsageCapacity = table.Column<bool>(nullable: false),
                    NumberBillingLimitDays = table.Column<int>(nullable: false),
                    InterestOnDefferedPayment = table.Column<int>(nullable: true),
                    ContractViolation = table.Column<int>(nullable: true),
                    ContractViolationType = table.Column<int>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(maxLength: 256, nullable: true),
                    IsHasOneCurrency = table.Column<bool>(nullable: false),
                    FiberNodeInfo = table.Column<string>(nullable: true),
                    ContractNote = table.Column<string>(nullable: true),
                    AgentContractCode = table.Column<string>(nullable: true),
                    OrganizationUnitId = table.Column<string>(nullable: true),
                    CashierUserId = table.Column<string>(maxLength: 68, nullable: true),
                    CashierUserName = table.Column<string>(maxLength: 256, nullable: true),
                    CashierFullName = table.Column<string>(maxLength: 256, nullable: true),
                    IsAutomaticGenerateReceipt = table.Column<bool>(nullable: false),
                    CustomerCareStaffUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutContracts_ContractStatus_ContractStatusId",
                        column: x => x.ContractStatusId,
                        principalTable: "ContractStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutContracts_Contractors_ContractorHTCId",
                        column: x => x.ContractorHTCId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutContracts_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 256, nullable: true),
                    ProjectCode = table.Column<string>(maxLength: 256, nullable: true),
                    MarketAreaId = table.Column<int>(nullable: true),
                    AgentContractCodeId = table.Column<int>(nullable: true),
                    NumberOfUnits = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    NumberOfRooms = table.Column<int>(nullable: true),
                    NumberOfSupporters = table.Column<int>(nullable: false),
                    IdentityGuid = table.Column<string>(maxLength: 128, nullable: true),
                    BusinessBlockId = table.Column<int>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    DistrictId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_MarketAreas_MarketAreaId",
                        column: x => x.MarketAreaId,
                        principalTable: "MarketAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    DigitalSignatureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractForms_Pictures_DigitalSignatureId",
                        column: x => x.DigitalSignatureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    ServiceCode = table.Column<string>(nullable: true),
                    ServiceName = table.Column<string>(nullable: true),
                    HasStartAndEndPoint = table.Column<bool>(nullable: false),
                    HasPackages = table.Column<bool>(nullable: false),
                    HasLineQuantity = table.Column<bool>(nullable: false),
                    HasCableKilometers = table.Column<bool>(nullable: false),
                    HasDistinguishBandwidth = table.Column<bool>(nullable: false),
                    AvatarId = table.Column<int>(nullable: false),
                    ServicePrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_ServiceGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ServiceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    OutputChannelPointId = table.Column<int>(nullable: false),
                    OutContractPackageId = table.Column<int>(nullable: false),
                    EquipmentName = table.Column<string>(nullable: true),
                    EquipmentPictureUrl = table.Column<string>(nullable: true),
                    EquipmentUom = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    ExaminedUnit = table.Column<float>(nullable: false),
                    RealUnit = table.Column<float>(nullable: false),
                    ReclaimedUnit = table.Column<float>(nullable: false),
                    IsInSurveyPlan = table.Column<bool>(nullable: false),
                    IsFree = table.Column<bool>(nullable: false),
                    HasToReclaim = table.Column<bool>(nullable: false),
                    SerialCode = table.Column<string>(nullable: true),
                    MacAddressCode = table.Column<string>(nullable: true),
                    DeviceCode = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Specifications = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: false),
                    EquipmentStatusId = table.Column<int>(nullable: true),
                    SubTotal = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    ExaminedSubTotal = table.Column<decimal>(nullable: false),
                    ExaminedGrandTotal = table.Column<decimal>(nullable: false),
                    RealSubTotal = table.Column<decimal>(nullable: false),
                    RealGrandTotal = table.Column<decimal>(nullable: false),
                    ReclaimedSubTotal = table.Column<decimal>(nullable: false),
                    ReclaimedGrandTotal = table.Column<decimal>(nullable: false),
                    TransactionId = table.Column<int>(nullable: true),
                    TransactionServicePackageId = table.Column<int>(nullable: true),
                    ContractEquipmentId = table.Column<int>(nullable: true),
                    OldEquipmentId = table.Column<int>(nullable: false),
                    IsOld = table.Column<bool>(nullable: true),
                    ServiceName = table.Column<string>(nullable: true),
                    PackageName = table.Column<string>(nullable: true),
                    IsAcceptanced = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionEquipments_EquipmentStatuses_EquipmentStatusId",
                        column: x => x.EquipmentStatusId,
                        principalTable: "EquipmentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionEquipments_TransactionChannelPoints_OutputChannel~",
                        column: x => x.OutputChannelPointId,
                        principalTable: "TransactionChannelPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InContractServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    InContractId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    ShareType = table.Column<int>(nullable: false),
                    PointType = table.Column<int>(nullable: false),
                    ServiceName = table.Column<string>(nullable: true),
                    SharedPackagePercent = table.Column<float>(nullable: false),
                    SharedInstallFeePercent = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InContractServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InContractServices_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InContractTaxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    InContractId = table.Column<int>(nullable: true),
                    TaxCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InContractTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InContractTaxes_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    InContractId = table.Column<int>(nullable: true),
                    OutContractId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 512, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 10, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfos_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactInfos_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractContents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ContractFormId = table.Column<int>(nullable: true),
                    OutContractId = table.Column<int>(nullable: true),
                    InContractId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DigitalSignatureId = table.Column<int>(nullable: true),
                    ContractFormSignatureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractContents_Pictures_ContractFormSignatureId",
                        column: x => x.ContractFormSignatureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractContents_Pictures_DigitalSignatureId",
                        column: x => x.DigitalSignatureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractContents_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractContents_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractTotalByCurrencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    OutContractId = table.Column<int>(nullable: true),
                    InContractId = table.Column<int>(nullable: true),
                    PromotionTotalAmount = table.Column<decimal>(nullable: false),
                    ServicePackageAmount = table.Column<decimal>(nullable: false),
                    TotalTaxAmount = table.Column<decimal>(nullable: false),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    OtherFee = table.Column<decimal>(nullable: false),
                    EquipmentAmount = table.Column<decimal>(nullable: false),
                    SubTotalBeforeTax = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    GrandTotalBeforeTax = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTotalByCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractTotalByCurrencies_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractTotalByCurrencies_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    MarketAreaId = table.Column<int>(nullable: true),
                    MarketAreaName = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<string>(nullable: true),
                    ContractorId = table.Column<int>(nullable: true),
                    ContractType = table.Column<int>(nullable: false),
                    ContractCode = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    HandleUserId = table.Column<string>(nullable: true),
                    AcceptanceStaff = table.Column<string>(nullable: true),
                    OrganizationUnitId = table.Column<string>(nullable: true),
                    ReasonType = table.Column<int>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    AcceptanceNotes = table.Column<string>(nullable: true),
                    SuspendHandleFee = table.Column<decimal>(nullable: true),
                    RestoreHandleFee = table.Column<decimal>(nullable: true),
                    ChaningLocationFee = table.Column<decimal>(nullable: true),
                    ChangeEquipmentFee = table.Column<decimal>(nullable: true),
                    UpgradeFee = table.Column<decimal>(nullable: true),
                    ChangingPackageFee = table.Column<decimal>(nullable: true),
                    IsTechnicalConfirmation = table.Column<bool>(nullable: true),
                    IsAppendix = table.Column<bool>(nullable: true),
                    OutContractId = table.Column<int>(nullable: true),
                    HasEquipment = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionReasons_ReasonType",
                        column: x => x.ReasonType,
                        principalTable: "TransactionReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTechnicians",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    UserTechnicianId = table.Column<string>(nullable: true),
                    TechnicianName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTechnicians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTechnicians_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: true),
                    PictureId = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    PackageCode = table.Column<string>(nullable: true),
                    PackageName = table.Column<string>(nullable: true),
                    BandwidthLabel = table.Column<string>(nullable: true),
                    InternationalBandwidth = table.Column<float>(nullable: false),
                    DomesticBandwidth = table.Column<float>(nullable: false),
                    InternationalBandwidthUom = table.Column<string>(nullable: true),
                    InternationalBandwidthUomId = table.Column<int>(nullable: true),
                    DomesticBandwidthUom = table.Column<string>(nullable: true),
                    DomesticBandwidthUomId = table.Column<int>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackages_UnitOfMeasurement_DomesticBandwidthUomId",
                        column: x => x.DomesticBandwidthUomId,
                        principalTable: "UnitOfMeasurement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePackages_UnitOfMeasurement_InternationalBandwidthUomId",
                        column: x => x.InternationalBandwidthUomId,
                        principalTable: "UnitOfMeasurement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePackages_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    OutputChannelPointId = table.Column<int>(nullable: false),
                    OutContractPackageId = table.Column<int>(nullable: false),
                    EquipmentName = table.Column<string>(nullable: true),
                    EquipmentPictureUrl = table.Column<string>(nullable: true),
                    EquipmentUom = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    ExaminedUnit = table.Column<float>(nullable: false),
                    RealUnit = table.Column<float>(nullable: false),
                    ReclaimedUnit = table.Column<float>(nullable: false),
                    IsInSurveyPlan = table.Column<bool>(nullable: false),
                    IsFree = table.Column<bool>(nullable: false),
                    HasToReclaim = table.Column<bool>(nullable: false),
                    SerialCode = table.Column<string>(nullable: true),
                    MacAddressCode = table.Column<string>(nullable: true),
                    DeviceCode = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Specifications = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: false),
                    EquipmentStatusId = table.Column<int>(nullable: true),
                    SubTotal = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    ExaminedSubTotal = table.Column<decimal>(nullable: false),
                    ExaminedGrandTotal = table.Column<decimal>(nullable: false),
                    RealSubTotal = table.Column<decimal>(nullable: false),
                    RealGrandTotal = table.Column<decimal>(nullable: false),
                    ReclaimedSubTotal = table.Column<decimal>(nullable: false),
                    ReclaimedGrandTotal = table.Column<decimal>(nullable: false),
                    TransactionEquipmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractEquipments_EquipmentStatuses_EquipmentStatusId",
                        column: x => x.EquipmentStatusId,
                        principalTable: "EquipmentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractEquipments_OutputChannelPoints_OutputChannelPointId",
                        column: x => x.OutputChannelPointId,
                        principalTable: "OutputChannelPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractEquipments_TransactionEquipments_TransactionEquipmen~",
                        column: x => x.TransactionEquipmentId,
                        principalTable: "TransactionEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    InContractId = table.Column<int>(nullable: true),
                    OutContractId = table.Column<int>(nullable: true),
                    TransactionId = table.Column<int>(nullable: true),
                    ResourceStorage = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    FileType = table.Column<int>(nullable: false),
                    Extension = table.Column<string>(nullable: true),
                    RedirectLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionServicePackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CableRoutingNumber = table.Column<string>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    ServicePackageId = table.Column<int>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: false),
                    PackageName = table.Column<string>(maxLength: 256, nullable: true),
                    IsFreeStaticIp = table.Column<bool>(nullable: false),
                    BandwidthLabel = table.Column<string>(maxLength: 256, nullable: true),
                    InternationalBandwidth = table.Column<float>(nullable: false),
                    DomesticBandwidth = table.Column<float>(nullable: false),
                    InternationalBandwidthUom = table.Column<string>(nullable: true),
                    DomesticBandwidthUom = table.Column<string>(nullable: true),
                    TimeLine_PrepayPeriod = table.Column<int>(nullable: true),
                    TimeLine_PaymentPeriod = table.Column<int>(nullable: true),
                    TimeLine_Effective = table.Column<DateTime>(nullable: true),
                    TimeLine_Signed = table.Column<DateTime>(nullable: true),
                    TimeLine_StartBilling = table.Column<DateTime>(nullable: true),
                    TimeLine_LatestBilling = table.Column<DateTime>(nullable: true),
                    TimeLine_NextBilling = table.Column<DateTime>(nullable: true),
                    TimeLine_SuspensionStartDate = table.Column<DateTime>(nullable: true),
                    TimeLine_SuspensionEndDate = table.Column<DateTime>(nullable: true),
                    TimeLine_TerminateDate = table.Column<DateTime>(nullable: true),
                    TimeLine_DaysSuspended = table.Column<int>(nullable: true),
                    TimeLine_DaysPromotion = table.Column<int>(nullable: true),
                    TimeLine_PaymentForm = table.Column<int>(nullable: true),
                    CustomerCode = table.Column<string>(maxLength: 256, nullable: true),
                    CId = table.Column<string>(maxLength: 256, nullable: true),
                    RadiusAccount = table.Column<string>(maxLength: 256, nullable: true),
                    RadiusPassword = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    OtherFee = table.Column<decimal>(nullable: false),
                    OrgPackagePrice = table.Column<decimal>(nullable: false),
                    PackagePrice = table.Column<decimal>(nullable: false),
                    LineQuantity = table.Column<float>(nullable: false),
                    CableKilometers = table.Column<float>(nullable: true),
                    StartPointChannelId = table.Column<int>(nullable: true),
                    EndPointChannelId = table.Column<int>(nullable: false),
                    PromotionAmount = table.Column<decimal>(nullable: false),
                    EquipmentAmount = table.Column<decimal>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    SubTotalBeforeTax = table.Column<decimal>(nullable: false),
                    GrandTotalBeforeTax = table.Column<decimal>(nullable: false),
                    TaxPercent = table.Column<float>(nullable: false),
                    TaxAmount = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    TransactionServicePackageId = table.Column<int>(nullable: true),
                    OldId = table.Column<int>(nullable: true),
                    IsInFirstBilling = table.Column<bool>(nullable: false),
                    RadiusServerId = table.Column<int>(nullable: true),
                    ChannelGroupId = table.Column<int>(nullable: false),
                    PaymentTargetId = table.Column<int>(nullable: false),
                    FlexiblePricingTypeId = table.Column<int>(nullable: false),
                    MaxSubTotal = table.Column<decimal>(nullable: true),
                    MinSubTotal = table.Column<decimal>(nullable: true),
                    IsDefaultSLAByServiceId = table.Column<byte>(nullable: false),
                    IsRadiusAccountCreated = table.Column<bool>(nullable: false),
                    IsHasServicePackage = table.Column<bool>(nullable: false),
                    IsTechnicalConfirmation = table.Column<bool>(nullable: true),
                    HasStartAndEndPoint = table.Column<bool>(nullable: false),
                    HasDistinguishBandwidth = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    OutContractId = table.Column<int>(nullable: true),
                    InContractId = table.Column<int>(nullable: true),
                    OutContractServicePackageId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false),
                    IsOld = table.Column<bool>(nullable: true),
                    IsAcceptanced = table.Column<bool>(nullable: true),
                    NeedEnterStartPoint = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionServicePackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionServicePackages_TransactionChannelPoints_EndPoint~",
                        column: x => x.EndPointChannelId,
                        principalTable: "TransactionChannelPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionServicePackages_Contractors_PaymentTargetId",
                        column: x => x.PaymentTargetId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionServicePackages_TransactionChannelPoints_StartPoi~",
                        column: x => x.StartPointChannelId,
                        principalTable: "TransactionChannelPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionServicePackages_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackagePrice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    PriceValue = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackagePrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackagePrice_ServicePackages_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "ServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackageRadiusServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ServicePackageId = table.Column<int>(nullable: false),
                    RadiusServerId = table.Column<int>(nullable: false),
                    RadiusServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackageRadiusServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackageRadiusServices_ServicePackages_ServicePackageId",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutContractServicePackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    OutContractId = table.Column<int>(nullable: true),
                    InContractId = table.Column<int>(nullable: true),
                    CableRoutingNumber = table.Column<string>(nullable: true),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    ServicePackageId = table.Column<int>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: false),
                    PackageName = table.Column<string>(maxLength: 256, nullable: true),
                    IsFreeStaticIp = table.Column<bool>(nullable: false),
                    BandwidthLabel = table.Column<string>(maxLength: 256, nullable: true),
                    InternationalBandwidth = table.Column<float>(nullable: false),
                    DomesticBandwidth = table.Column<float>(nullable: false),
                    InternationalBandwidthUom = table.Column<string>(nullable: true),
                    DomesticBandwidthUom = table.Column<string>(nullable: true),
                    TimeLine_PrepayPeriod = table.Column<int>(nullable: true),
                    TimeLine_PaymentPeriod = table.Column<int>(nullable: true),
                    TimeLine_Effective = table.Column<DateTime>(nullable: true),
                    TimeLine_Signed = table.Column<DateTime>(nullable: true),
                    TimeLine_StartBilling = table.Column<DateTime>(nullable: true),
                    TimeLine_LatestBilling = table.Column<DateTime>(nullable: true),
                    TimeLine_NextBilling = table.Column<DateTime>(nullable: true),
                    TimeLine_SuspensionStartDate = table.Column<DateTime>(nullable: true),
                    TimeLine_SuspensionEndDate = table.Column<DateTime>(nullable: true),
                    TimeLine_TerminateDate = table.Column<DateTime>(nullable: true),
                    TimeLine_DaysSuspended = table.Column<int>(nullable: true),
                    TimeLine_DaysPromotion = table.Column<int>(nullable: true),
                    TimeLine_PaymentForm = table.Column<int>(nullable: true),
                    CustomerCode = table.Column<string>(maxLength: 256, nullable: true),
                    CId = table.Column<string>(maxLength: 256, nullable: true),
                    RadiusAccount = table.Column<string>(maxLength: 256, nullable: true),
                    RadiusPassword = table.Column<string>(maxLength: 256, nullable: true),
                    InstallationFee = table.Column<decimal>(nullable: false),
                    OtherFee = table.Column<decimal>(nullable: false),
                    OrgPackagePrice = table.Column<decimal>(nullable: false),
                    PackagePrice = table.Column<decimal>(nullable: false),
                    LineQuantity = table.Column<float>(nullable: false),
                    CableKilometers = table.Column<float>(nullable: true),
                    StartPointChannelId = table.Column<int>(nullable: true),
                    EndPointChannelId = table.Column<int>(nullable: false),
                    PromotionAmount = table.Column<decimal>(nullable: false),
                    EquipmentAmount = table.Column<decimal>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    SubTotalBeforeTax = table.Column<decimal>(nullable: false),
                    GrandTotalBeforeTax = table.Column<decimal>(nullable: false),
                    TaxPercent = table.Column<float>(nullable: false),
                    TaxAmount = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    TransactionServicePackageId = table.Column<int>(nullable: true),
                    OldId = table.Column<int>(nullable: true),
                    IsInFirstBilling = table.Column<bool>(nullable: false),
                    RadiusServerId = table.Column<int>(nullable: true),
                    ChannelGroupId = table.Column<int>(nullable: false),
                    PaymentTargetId = table.Column<int>(nullable: false),
                    FlexiblePricingTypeId = table.Column<int>(nullable: false),
                    MaxSubTotal = table.Column<decimal>(nullable: true),
                    MinSubTotal = table.Column<decimal>(nullable: true),
                    IsDefaultSLAByServiceId = table.Column<byte>(nullable: false),
                    IsRadiusAccountCreated = table.Column<bool>(nullable: false),
                    IsHasServicePackage = table.Column<bool>(nullable: false),
                    IsTechnicalConfirmation = table.Column<bool>(nullable: true),
                    HasStartAndEndPoint = table.Column<bool>(nullable: false),
                    HasDistinguishBandwidth = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContractServicePackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackages_OutputChannelPoints_EndPointChann~",
                        column: x => x.EndPointChannelId,
                        principalTable: "OutputChannelPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackages_FlexiblePricingType_FlexiblePrici~",
                        column: x => x.FlexiblePricingTypeId,
                        principalTable: "FlexiblePricingType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackages_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackages_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackages_Contractors_PaymentTargetId",
                        column: x => x.PaymentTargetId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackages_OutputChannelPoints_StartPointCha~",
                        column: x => x.StartPointChannelId,
                        principalTable: "OutputChannelPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackages_TransactionServicePackages_Transa~",
                        column: x => x.TransactionServicePackageId,
                        principalTable: "TransactionServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionChannelTaxes",
                columns: table => new
                {
                    TaxCategoryId = table.Column<int>(nullable: false),
                    TransactionServicePackageId = table.Column<int>(nullable: false),
                    TaxCategoryName = table.Column<string>(maxLength: 256, nullable: true),
                    TaxCategoryCode = table.Column<string>(maxLength: 256, nullable: true),
                    TaxValue = table.Column<float>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionChannelTaxes", x => new { x.TaxCategoryId, x.TransactionServicePackageId });
                    table.ForeignKey(
                        name: "FK_TransactionChannelTaxes_TaxCategories_TaxCategoryId",
                        column: x => x.TaxCategoryId,
                        principalTable: "TaxCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionChannelTaxes_TransactionServicePackages_Transacti~",
                        column: x => x.TransactionServicePackageId,
                        principalTable: "TransactionServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionPriceBusTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
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
                    IsDomestic = table.Column<bool>(nullable: true),
                    TransactionServicePackageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPriceBusTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionPriceBusTables_TransactionServicePackages_Transac~",
                        column: x => x.TransactionServicePackageId,
                        principalTable: "TransactionServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionPromotionForContracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    OutContractServicePackageId = table.Column<int>(nullable: true),
                    PromotionDetailId = table.Column<int>(nullable: false),
                    PromotionValue = table.Column<decimal>(nullable: false),
                    PromotionValueType = table.Column<int>(nullable: true),
                    IsApplied = table.Column<bool>(nullable: false),
                    NumberMonthApplied = table.Column<int>(nullable: false),
                    PromotionId = table.Column<int>(nullable: false),
                    PromotionName = table.Column<string>(nullable: true),
                    PromotionType = table.Column<int>(nullable: false),
                    PromotionTypeName = table.Column<string>(nullable: true),
                    TransactionServicePackageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPromotionForContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionPromotionForContracts_TransactionServicePackages_~",
                        column: x => x.TransactionServicePackageId,
                        principalTable: "TransactionServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionSLAs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Uid = table.Column<string>(maxLength: 68, nullable: true),
                    Label = table.Column<string>(maxLength: 256, nullable: true),
                    Content = table.Column<string>(maxLength: 2000, nullable: true),
                    TransactionServicePackageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSLAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionSLAs_TransactionServicePackages_TransactionServic~",
                        column: x => x.TransactionServicePackageId,
                        principalTable: "TransactionServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChannelPriceBusTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
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
                    IsDomestic = table.Column<bool>(nullable: true),
                    ChannelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPriceBusTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelPriceBusTables_OutContractServicePackages_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractSharingRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Uid = table.Column<string>(nullable: true),
                    ChannelTemporaryId = table.Column<string>(nullable: true),
                    ChannelName = table.Column<string>(nullable: true),
                    ChannelCid = table.Column<string>(nullable: true),
                    InContractId = table.Column<int>(nullable: false),
                    InContractCode = table.Column<string>(maxLength: 256, nullable: true),
                    OutChannelId = table.Column<int>(nullable: false),
                    OutContractId = table.Column<int>(nullable: true),
                    OutContractCode = table.Column<string>(maxLength: 256, nullable: true),
                    SharingType = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    TotalAmountAfterTax = table.Column<decimal>(nullable: false),
                    CostTerm = table.Column<int>(nullable: false),
                    TaxMoney = table.Column<decimal>(nullable: false),
                    CurrencyUnitId = table.Column<int>(nullable: false),
                    CurrencyUnitCode = table.Column<string>(nullable: true),
                    OutContractServicePackageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSharingRevenues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenues_InContracts_InContractId",
                        column: x => x.InContractId,
                        principalTable: "InContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenues_OutContracts_OutContractId",
                        column: x => x.OutContractId,
                        principalTable: "OutContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenues_OutContractServicePackages_OutContra~",
                        column: x => x.OutContractServicePackageId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutContractServicePackageClearings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    Change = table.Column<decimal>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    OutContractServicePackageId = table.Column<int>(nullable: false),
                    IsDaysPlus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContractServicePackageClearings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackageClearings_OutContractServicePackage~",
                        column: x => x.OutContractServicePackageId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutContractServicePackageTaxes",
                columns: table => new
                {
                    TaxCategoryId = table.Column<int>(nullable: false),
                    OutContractServicePackageId = table.Column<int>(nullable: false),
                    TaxCategoryName = table.Column<string>(maxLength: 256, nullable: true),
                    TaxCategoryCode = table.Column<string>(maxLength: 256, nullable: true),
                    TaxValue = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutContractServicePackageTaxes", x => new { x.OutContractServicePackageId, x.TaxCategoryId });
                    table.ForeignKey(
                        name: "FK_OutContractServicePackageTaxes_OutContractServicePackages_Ou~",
                        column: x => x.OutContractServicePackageId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutContractServicePackageTaxes_TaxCategories_TaxCategoryId",
                        column: x => x.TaxCategoryId,
                        principalTable: "TaxCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionForContracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    OutContractServicePackageId = table.Column<int>(nullable: true),
                    PromotionDetailId = table.Column<int>(nullable: false),
                    PromotionValue = table.Column<decimal>(nullable: false),
                    PromotionValueType = table.Column<int>(nullable: true),
                    IsApplied = table.Column<bool>(nullable: false),
                    NumberMonthApplied = table.Column<int>(nullable: false),
                    PromotionId = table.Column<int>(nullable: false),
                    PromotionName = table.Column<string>(nullable: true),
                    PromotionType = table.Column<int>(nullable: false),
                    PromotionTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionForContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionForContracts_OutContractServicePackages_OutContract~",
                        column: x => x.OutContractServicePackageId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceLevelAgreements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Uid = table.Column<string>(maxLength: 68, nullable: true),
                    Label = table.Column<string>(maxLength: 256, nullable: true),
                    Content = table.Column<string>(maxLength: 2000, nullable: true),
                    OutContractId = table.Column<int>(nullable: false),
                    OutContractServicePackageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLevelAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceLevelAgreements_OutContractServicePackages_OutContrac~",
                        column: x => x.OutContractServicePackageId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackageSuspensionTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    OutContractServicePackageId = table.Column<int>(nullable: false),
                    SuspensionStartDate = table.Column<DateTime>(nullable: false),
                    SuspensionEndDate = table.Column<DateTime>(nullable: true),
                    DaysSuspended = table.Column<int>(nullable: false),
                    DiscountAmount = table.Column<decimal>(nullable: false),
                    RemainingAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackageSuspensionTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackageSuspensionTimes_OutContractServicePackages_Out~",
                        column: x => x.OutContractServicePackageId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractSharingRevenueLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Culture = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    OrganizationPath = table.Column<string>(nullable: true),
                    CsrUid = table.Column<string>(nullable: true),
                    CsrId = table.Column<int>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    SharingType = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    ServicePackageId = table.Column<int>(nullable: true),
                    ServicePackageName = table.Column<string>(maxLength: 256, nullable: true),
                    InServiceChannelId = table.Column<int>(nullable: true),
                    OutServiceChannelId = table.Column<int>(nullable: true),
                    SharedInstallFeePercent = table.Column<float>(nullable: false),
                    SharedPackagePercent = table.Column<float>(nullable: false),
                    SharedFixedAmount = table.Column<decimal>(nullable: false),
                    PointType = table.Column<int>(nullable: false),
                    PointTypeName = table.Column<string>(nullable: true),
                    OutContractCode = table.Column<string>(nullable: true),
                    CsMaintenanceUid = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSharingRevenueLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenueLines_ContractSharingRevenues_CsrId",
                        column: x => x.CsrId,
                        principalTable: "ContractSharingRevenues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractSharingRevenueLines_OutContractServicePackages_OutSe~",
                        column: x => x.OutServiceChannelId,
                        principalTable: "OutContractServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ContractStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chờ ký" },
                    { 2, "Đã ký" },
                    { 3, "Đã nghiệm thu" },
                    { 4, "Tạm ngưng" },
                    { 5, "Đã thanh lý" },
                    { 6, "Trình ký" },
                    { 9, "Hủy" },
                    { 10, "Khác" }
                });

            migrationBuilder.InsertData(
                table: "Contractors",
                columns: new[] { "Id", "AccountingCustomerCode", "ApplicationUserIdentityGuid", "AuthorizationLetter", "ContractorAddress", "ContractorCity", "ContractorCityId", "ContractorCode", "ContractorDistrict", "ContractorDistrictId", "ContractorEmail", "ContractorFax", "ContractorFullName", "ContractorIdNo", "ContractorPhone", "ContractorShortName", "ContractorTaxIdNo", "ContractorUserName", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IdentityGuid", "IsActive", "IsBuyer", "IsDeleted", "IsEnterprise", "IsHTC", "IsPartner", "OrganizationPath", "ParentId", "Position", "Representative", "UpdatedBy", "UpdatedDate", "UserIdentityGuid" },
                values: new object[] { 1, null, null, null, "Tầng 6 – Lotus Building, số 2, Duy Tân, Phường Dịch Vọng Hậu, Quận Cầu Giấy, Thành phố Hà Nội, Việt Nam", null, null, "HTC_ITC", null, null, "info@htc-itc.com.vn", null, "Công ty cổ phần HTC viễn thông quốc tế", null, "(024) 3573 9419", null, "0102362584", null, "ADMINISTRATOR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, null, true, false, false, true, true, false, null, null, null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "CurrencyUnits",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "CurrencyUnitCode", "CurrencyUnitName", "CurrencyUnitSymbol", "Description", "DisplayOrder", "IsActive", "IsDeleted", "IssuingCountry", "OrganizationPath", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "USD", "Dollar", "$", null, 0, true, false, "United State", null, null, null },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "VND", "Đồng", "đ", null, 0, true, false, "Việt Nam", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "EquipmentStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Đã hủy" },
                    { 5, "Đã thu hồi" },
                    { 4, "Đang chờ thu hồi" },
                    { 3, "Đã triển khai" },
                    { 1, "Trong kế hoạch triển khai" },
                    { 7, "Không thể thu hồi" },
                    { 8, "Tạm giữ" }
                });

            migrationBuilder.InsertData(
                table: "FlexiblePricingType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Bustable hàng tháng" },
                    { 5, "Bustable hàng ngày" },
                    { 3, "Đơn giá lũy kế theo dung lượng sử dụng" },
                    { 2, "Đơn giá cố định có tính vượt mức" },
                    { 1, "Đơn giá cố định hàng tháng" }
                });

            migrationBuilder.InsertData(
                table: "InContractStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Thanh lý" },
                    { 5, "Khác" },
                    { 2, "Hoàn thành" },
                    { 1, "Trình ký" },
                    { 4, "Hủy" }
                });

            migrationBuilder.InsertData(
                table: "InContractTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Bảo trì, bảo dưỡng" },
                    { 3, "Phân chia doanh thu" },
                    { 2, "Phân chia hoa hồng" },
                    { 1, "Thuê kênh" }
                });

            migrationBuilder.InsertData(
                table: "MarketAreas",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "MarketCode", "MarketName", "OrganizationPath", "ParentId", "TreeLevel", "TreePath", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "MT", "Miền Trung", null, null, 0, "", null, null },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "MB", "Miền Bắc", null, null, 0, "", null, null },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "MN", "Miền Nam", null, null, 0, "", null, null }
                });

            migrationBuilder.InsertData(
                table: "OutContractStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Tạm ngưng" },
                    { 1, "Chờ ký" },
                    { 3, "Đã nghiệm thu" },
                    { 5, "Đã thanh lý(Hủy)" },
                    { 2, "Đã ký" }
                });

            migrationBuilder.InsertData(
                table: "OutContractTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Doanh nghiệp" },
                    { 1, "Cá nhân" }
                });

            migrationBuilder.InsertData(
                table: "PointTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "" },
                    { 2, "Điểm cuối" },
                    { 1, "Điểm đầu" }
                });

            migrationBuilder.InsertData(
                table: "PromotionTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsDeleted", "OrganizationPath", "PromotionName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "", 0, true, false, null, "Tặng thời gian sử dụng", null, null },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "", 0, true, false, null, "Tặng cước dịch vụ", null, null },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "", 0, true, false, null, "Tặng sản phẩm", null, null }
                });

            migrationBuilder.InsertData(
                table: "PromotionValueTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Tặng sản phẩm khác" },
                    { 7, "Khuyến mại khác" },
                    { 5, "Tặng sản phẩm công ty" },
                    { 3, "Tặng thời gian sử dụng (theo tháng)" },
                    { 2, "Giảm trừ cước (tiền mặt)" },
                    { 1, "Giảm trừ cước (% giá trị)" },
                    { 4, "Tặng thời gian sử dụng (theo ngày)" }
                });

            migrationBuilder.InsertData(
                table: "ReasonTypeContractTerminations",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "Name", "OrganizationPath", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "HTC ITC dừng cung cấp dịch vụ", null, null, null },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Yêu cầu của khách hàng", null, null, null },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Quá hạn thanh toán công nợ", null, null, null },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Lý do khác", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "GroupCode", "GroupName", "IsActive", "IsDeleted", "OrganizationPath", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 16, "", new DateTime(2020, 10, 19, 7, 44, 0, 123, DateTimeKind.Unspecified).AddTicks(4350), "", 0, "DVT", "Nhóm dịch vụ thoại IP", true, true, "", "", null },
                    { 8, "", new DateTime(2020, 10, 19, 8, 28, 34, 323, DateTimeKind.Unspecified).AddTicks(360), "", 0, "DVTD", "Nhóm các dịch vụ truyền dẫn", true, false, "", "", null },
                    { 5, "", new DateTime(1900, 1, 20, 8, 42, 44, 138, DateTimeKind.Unspecified).AddTicks(1960), "", 0, "DVTT01", "Nhóm dịch vụ TT truyền thông", true, true, "", "", new DateTime(1900, 1, 20, 8, 42, 51, 149, DateTimeKind.Unspecified).AddTicks(4590) },
                    { 4, "", new DateTime(2020, 9, 9, 2, 12, 15, 264, DateTimeKind.Unspecified), "", 0, "CNTTGPTH", "Nhóm các dịch vụ CNTT/ Giải pháp tích hợp", true, false, "", "", new DateTime(1900, 1, 20, 8, 42, 11, 620, DateTimeKind.Unspecified).AddTicks(3180) },
                    { 3, "", new DateTime(1900, 1, 20, 2, 12, 0, 105, DateTimeKind.Unspecified).AddTicks(2170), "", 0, "GTGT", "Nhóm dịch vụ GTGT", true, false, "", "", null },
                    { 2, "", new DateTime(1900, 1, 20, 2, 11, 48, 891, DateTimeKind.Unspecified).AddTicks(6470), "", 0, "DVT", "Nhóm dịch vụ thoại IP", true, false, "", "", null },
                    { 1, "", new DateTime(2020, 9, 9, 2, 11, 35, 964, DateTimeKind.Unspecified).AddTicks(6270), "", 0, "NTD", "Nhóm truyền dẫn", true, false, "", "", null },
                    { 10, "", new DateTime(2020, 10, 19, 8, 28, 58, 726, DateTimeKind.Unspecified), "", 0, "NDVT", "Nhóm dịch vụ thoại IP", true, false, "", "", new DateTime(2020, 10, 19, 8, 32, 29, 706, DateTimeKind.Unspecified).AddTicks(1280) },
                    { 12, "", new DateTime(2020, 10, 19, 7, 43, 30, 728, DateTimeKind.Unspecified).AddTicks(1010), "", 0, "CNTTGPTH", "Nhóm các dịch vụ CNTT/ Giải pháp tích hợp", true, true, "", "", null },
                    { 18, "", new DateTime(2020, 10, 19, 8, 30, 9, 26, DateTimeKind.Unspecified).AddTicks(6230), "", 0, "CNTT", "Nhóm dịch vụ CNTT", true, false, "", "", null },
                    { 14, "", new DateTime(2020, 10, 19, 7, 43, 47, 721, DateTimeKind.Unspecified).AddTicks(4780), "", 0, "GTGT", "Nhóm dịch vụ GTGT", true, true, "", "", null }
                });

            migrationBuilder.InsertData(
                table: "TaxCategories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "ExplainTax", "IsActive", "IsDeleted", "OrganizationPath", "TaxCode", "TaxName", "TaxValue", "UpdatedBy", "UpdatedDate", "UserId", "UserName" },
                values: new object[,]
                {
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "", true, false, null, "WHT", "Thuế nhà thầu nước ngoài", 10f, null, null, 1, "admin" },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "", true, false, null, "VAT", "Giá trị gia tăng", 10f, null, null, 1, "admin" }
                });

            migrationBuilder.InsertData(
                table: "TransactionReasons",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "IsActive", "IsDeleted", "Name", "OrganizationPath", "ReasonType", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Sự cố hạ tầng kỹ thuật nơi triển khai", null, 1, null, null },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Lý do khác", null, 1, null, null },
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "HTC ITC dừng cung cấp dịch vụ", null, 2, null, null },
                    { 8, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Yêu cầu của khách hàng", null, 2, null, null },
                    { 9, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Quá hạn thanh toán công nợ", null, 2, null, null },
                    { 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Lý do khác", null, 2, null, null },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Sự cố hệ thống HTC-ITC", null, 1, null, null },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Bảo trì hệ thống HTC-ITC", null, 1, null, null },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Yêu cầu của khách hàng", null, 1, null, null },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, false, "Dịch chuyển địa điểm", null, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "TransactionStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chờ triển khai" },
                    { 5, "Thiết bị đã nhập kho" },
                    { 3, "Từ chối nghiệm thu" },
                    { 2, "Đã triển khai" },
                    { 6, "Hoàn thành" },
                    { 4, "Đã nghiệm thu" }
                });

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Name", "Permission" },
                values: new object[,]
                {
                    { 10, "Nâng cấp băng thông", "APPROVED_UPGRADE_BANDWIDTH_OUT_CONTRACT" },
                    { 9, "Nâng cấp thiết bị", "APPROVED_UPGRADE_EQUIPMENTS_OUT_CONTRACT" },
                    { 8, "Thanh lý hợp đồng", "APPROVED_TERMINATE_OUT_CONTRACT" },
                    { 7, "Thu hồi thiết bị", "APPROVED_RECLAIM_EQUIPMENT_OUT_CONTRACT" },
                    { 6, "Thay đổi thiết bị", "APPROVED_CHANGE_EQUIPMENT_OUT_CONTRACT" },
                    { 5, "Dịch chuyển địa điểm", "APPROVED_CHANGE_LOCATION_OUT_CONTRACT" },
                    { 4, "Hủy dịch vụ", "APPROVED_TERMINATE_SERVICE_PACKAGE_OUT_CONTRACT" },
                    { 3, "Tạm ngưng dịch vụ", "APPROVED_SUSPEND_SERVICE_PACKAGE_OUT_CONTRACT" },
                    { 2, "Điều chỉnh gói cước", "APPROVED_CHANGE_SERVICE_PACKAGE_OUT_CONTRACT" },
                    { 1, "Thêm mới dịch vụ/gói cước", "APPROVED_ADD_NEW_SERVICE_PACKAGE_OUT_CONTRACT" },
                    { 12, "Triển khai hợp đồng mới", "APPROVED_DEPLOY_NEW_OUT_CONTRACT" },
                    { 11, "Khôi phục dịch vụ", "APPROVED_RESTORE_SERVICE_PACKAGE_OUT_CONTRACT" }
                });

            migrationBuilder.InsertData(
                table: "UnitOfMeasurement",
                columns: new[] { "Id", "ConversionRate", "CreatedBy", "CreatedDate", "Culture", "Description", "DisplayOrder", "IsActive", "IsBaseOfType", "IsDeleted", "IsRequired", "Label", "OrganizationPath", "Type", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Cái", 0, true, true, false, true, "Cái", null, 0, null, null },
                    { 2, 1m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Mét", 0, true, true, false, true, "m", null, 1, null, null },
                    { 3, 3600m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Giờ", 0, true, false, false, true, "h", null, 2, null, null },
                    { 4, 60m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Phút", 0, true, false, false, true, "m", null, 2, null, null },
                    { 5, 1m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Giây", 0, true, true, false, true, "s", null, 2, null, null },
                    { 6, 1m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Kilobit/s", 0, true, true, false, true, "Kbps", null, 3, null, null },
                    { 7, 1024m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Megabit/s", 0, true, false, false, true, "Mbps", null, 3, null, null },
                    { 8, 1048576m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Gigabit/s", 0, true, false, false, true, "Gbps", null, 3, null, null },
                    { 9, 1m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Kilobyte", 0, true, true, false, true, "kB", null, 4, null, null },
                    { 10, 1024m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Megabyte", 0, true, false, false, true, "MB", null, 4, null, null },
                    { 11, 1048576m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Gigabyte", 0, true, false, false, true, "GB", null, 4, null, null },
                    { 12, 1073741824m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Terabyte", 0, true, false, false, true, "TB", null, 4, null, null },
                    { 13, 1099511627776m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PetaByte", 0, true, false, false, true, "PB", null, 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "AvatarId", "CreatedBy", "CreatedDate", "Culture", "DisplayOrder", "GroupId", "HasCableKilometers", "HasDistinguishBandwidth", "HasLineQuantity", "HasPackages", "HasStartAndEndPoint", "IsActive", "IsDeleted", "OrganizationPath", "ServiceCode", "ServiceName", "ServicePrice", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 0, "", new DateTime(2020, 9, 9, 4, 20, 48, 323, DateTimeKind.Unspecified), "", 0, 1, false, true, false, false, true, true, false, "", "01", "Internet trực tiếp (Internet leased line - ILL)", 0m, "", new DateTime(2020, 9, 9, 6, 47, 0, 966, DateTimeKind.Unspecified).AddTicks(1590) },
                    { 2, 0, "", new DateTime(2020, 9, 9, 4, 21, 30, 719, DateTimeKind.Unspecified).AddTicks(3380), "", 0, 1, false, false, false, false, false, true, false, "", "02", "Internet bán ra nước ngoài", 0m, "", null },
                    { 3, 0, "", new DateTime(2020, 9, 9, 4, 27, 55, 793, DateTimeKind.Unspecified), "", 0, 1, false, false, false, false, true, true, false, "", "03", "Kênh thuê riêng trong nước (SDH,)", 0m, "", new DateTime(2020, 9, 15, 2, 34, 44, 515, DateTimeKind.Unspecified).AddTicks(6240) },
                    { 4, 0, "", new DateTime(2020, 9, 9, 4, 28, 10, 304, DateTimeKind.Unspecified).AddTicks(2800), "", 0, 1, true, false, true, false, false, true, false, "", "04", "Thuê sợi quang", 0m, "", null },
                    { 5, 0, "", new DateTime(2020, 9, 9, 4, 28, 24, 554, DateTimeKind.Unspecified).AddTicks(490), "", 0, 1, false, false, false, false, false, true, false, "", "05", "Kênh thuê riêng quốc tế (Có điểm kết nối ngoài Việt Nam)", 0m, "", null },
                    { 6, 0, "", new DateTime(2020, 9, 9, 4, 29, 54, 479, DateTimeKind.Unspecified).AddTicks(610), "", 0, 1, false, false, false, false, false, true, false, "", "06", "VPN/Metro/MPLS trong nước (Các loại IP)", 0m, "", null },
                    { 7, 0, "", new DateTime(2020, 9, 9, 4, 30, 6, 652, DateTimeKind.Unspecified).AddTicks(6790), "", 0, 1, false, false, false, false, false, true, false, "", "07", "VPN/Metro/MPLS quốc tế ", 0m, "", null },
                    { 8, 0, "", new DateTime(2020, 9, 9, 4, 30, 25, 66, DateTimeKind.Unspecified).AddTicks(3260), "", 0, 1, false, true, false, true, false, true, false, "", "08", "Internet băng rộng FTTH ", 0m, "", null },
                    { 9, 0, "", new DateTime(2020, 9, 9, 4, 31, 26, 432, DateTimeKind.Unspecified), "", 0, 1, false, false, false, false, true, true, false, "", "09", "DV truyền dẫn khác", 0m, "", new DateTime(2020, 9, 14, 7, 42, 29, 20, DateTimeKind.Unspecified).AddTicks(8370) },
                    { 10, 0, "", new DateTime(2020, 9, 9, 4, 33, 34, 692, DateTimeKind.Unspecified).AddTicks(60), "", 0, 2, false, false, false, false, false, true, false, "", "10", "Điện thoại cố định", 0m, "", null },
                    { 11, 0, "", new DateTime(2020, 9, 9, 4, 33, 58, 286, DateTimeKind.Unspecified).AddTicks(1170), "", 0, 2, false, false, false, false, false, true, false, "", "11", "SMS Marketing/SMS", 0m, "", null },
                    { 12, 0, "", new DateTime(2020, 9, 9, 4, 34, 40, 991, DateTimeKind.Unspecified).AddTicks(2040), "", 0, 2, false, false, false, false, false, true, false, "", "12", "1900/1800", 0m, "", null },
                    { 13, 0, "", new DateTime(2020, 9, 9, 4, 37, 15, 139, DateTimeKind.Unspecified).AddTicks(4560), "", 0, 2, false, false, false, false, false, true, false, "", "13", "Tổng đài ảo", 0m, "", null },
                    { 14, 0, "", new DateTime(2020, 9, 9, 4, 37, 30, 897, DateTimeKind.Unspecified).AddTicks(5020), "", 0, 2, false, false, false, false, false, true, false, "", "14", "Các dịch vụ thoại IP khác", 0m, "", null },
                    { 15, 0, "", new DateTime(2020, 9, 9, 4, 38, 7, 741, DateTimeKind.Unspecified).AddTicks(5550), "", 0, 3, false, false, false, false, false, true, false, "", "15", "Các dịch vụ Hosting (Tele + Web + data center, thiết kế web, thuê đặt máy chủ)", 0m, "", null },
                    { 16, 0, "", new DateTime(2020, 9, 9, 4, 38, 17, 737, DateTimeKind.Unspecified), "", 0, 3, false, false, false, false, false, true, false, "", "16", "Các dịch vụ GTGT khác (Video…)", 0m, "", new DateTime(2020, 10, 14, 8, 43, 45, 787, DateTimeKind.Unspecified).AddTicks(9530) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_InContractId",
                table: "AttachmentFiles",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_OutContractId",
                table: "AttachmentFiles",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_TransactionId",
                table: "AttachmentFiles",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPriceBusTables_ChannelId",
                table: "ChannelPriceBusTables",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_InContractId",
                table: "ContactInfos",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_OutContractId",
                table: "ContactInfos",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractContents_ContractFormSignatureId",
                table: "ContractContents",
                column: "ContractFormSignatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractContents_DigitalSignatureId",
                table: "ContractContents",
                column: "DigitalSignatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractContents_InContractId",
                table: "ContractContents",
                column: "InContractId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractContents_OutContractId",
                table: "ContractContents",
                column: "OutContractId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractEquipments_EquipmentStatusId",
                table: "ContractEquipments",
                column: "EquipmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractEquipments_OutputChannelPointId",
                table: "ContractEquipments",
                column: "OutputChannelPointId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractEquipments_TransactionEquipmentId",
                table: "ContractEquipments",
                column: "TransactionEquipmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractForms_DigitalSignatureId",
                table: "ContractForms",
                column: "DigitalSignatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractorProperties_ContractorId",
                table: "ContractorProperties",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractorProperties_Id",
                table: "ContractorProperties",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_IdentityGuid",
                table: "Contractors",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_CsrId",
                table: "ContractSharingRevenueLines",
                column: "CsrId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenueLines_OutServiceChannelId",
                table: "ContractSharingRevenueLines",
                column: "OutServiceChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenues_InContractId",
                table: "ContractSharingRevenues",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenues_OutContractId",
                table: "ContractSharingRevenues",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSharingRevenues_OutContractServicePackageId",
                table: "ContractSharingRevenues",
                column: "OutContractServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTotalByCurrencies_InContractId",
                table: "ContractTotalByCurrencies",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTotalByCurrencies_OutContractId",
                table: "ContractTotalByCurrencies",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_InContracts_ContractCode",
                table: "InContracts",
                column: "ContractCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InContracts_ContractStatusId",
                table: "InContracts",
                column: "ContractStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_InContracts_ContractorHTCId",
                table: "InContracts",
                column: "ContractorHTCId");

            migrationBuilder.CreateIndex(
                name: "IX_InContracts_ContractorId",
                table: "InContracts",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_InContracts_IdentityGuid",
                table: "InContracts",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InContractServices_InContractId",
                table: "InContractServices",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_InContractTaxes_InContractId",
                table: "InContractTaxes",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContracts_ContractCode",
                table: "OutContracts",
                column: "ContractCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutContracts_ContractStatusId",
                table: "OutContracts",
                column: "ContractStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContracts_ContractorHTCId",
                table: "OutContracts",
                column: "ContractorHTCId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContracts_ContractorId",
                table: "OutContracts",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContracts_IdentityGuid",
                table: "OutContracts",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackageClearings_OutContractServicePackage~",
                table: "OutContractServicePackageClearings",
                column: "OutContractServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackages_EndPointChannelId",
                table: "OutContractServicePackages",
                column: "EndPointChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackages_FlexiblePricingTypeId",
                table: "OutContractServicePackages",
                column: "FlexiblePricingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackages_InContractId",
                table: "OutContractServicePackages",
                column: "InContractId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackages_OutContractId",
                table: "OutContractServicePackages",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackages_PaymentTargetId",
                table: "OutContractServicePackages",
                column: "PaymentTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackages_StartPointChannelId",
                table: "OutContractServicePackages",
                column: "StartPointChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackages_TransactionServicePackageId",
                table: "OutContractServicePackages",
                column: "TransactionServicePackageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutContractServicePackageTaxes_TaxCategoryId",
                table: "OutContractServicePackageTaxes",
                column: "TaxCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IdentityGuid",
                table: "Projects",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_MarketAreaId",
                table: "Projects",
                column: "MarketAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechnicians_ProjectId",
                table: "ProjectTechnicians",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionForContracts_OutContractServicePackageId",
                table: "PromotionForContracts",
                column: "OutContractServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Salesman_IdentityGuid",
                table: "Salesman",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLevelAgreements_OutContractServicePackageId",
                table: "ServiceLevelAgreements",
                column: "OutContractServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackagePrice_ChannelId",
                table: "ServicePackagePrice",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageRadiusServices_ServicePackageId",
                table: "ServicePackageRadiusServices",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_DomesticBandwidthUomId",
                table: "ServicePackages",
                column: "DomesticBandwidthUomId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_InternationalBandwidthUomId",
                table: "ServicePackages",
                column: "InternationalBandwidthUomId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_ServiceId",
                table: "ServicePackages",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageSuspensionTimes_OutContractServicePackageId",
                table: "ServicePackageSuspensionTimes",
                column: "OutContractServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_GroupId",
                table: "Services",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxCategories_Id",
                table: "TaxCategories",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionChannelTaxes_TransactionServicePackageId",
                table: "TransactionChannelTaxes",
                column: "TransactionServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEquipments_EquipmentStatusId",
                table: "TransactionEquipments",
                column: "EquipmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEquipments_OutputChannelPointId",
                table: "TransactionEquipments",
                column: "OutputChannelPointId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPriceBusTables_TransactionServicePackageId",
                table: "TransactionPriceBusTables",
                column: "TransactionServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPromotionForContracts_TransactionServicePackageId",
                table: "TransactionPromotionForContracts",
                column: "TransactionServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OutContractId",
                table: "Transactions",
                column: "OutContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReasonType",
                table: "Transactions",
                column: "ReasonType");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionServicePackages_EndPointChannelId",
                table: "TransactionServicePackages",
                column: "EndPointChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionServicePackages_PaymentTargetId",
                table: "TransactionServicePackages",
                column: "PaymentTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionServicePackages_StartPointChannelId",
                table: "TransactionServicePackages",
                column: "StartPointChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionServicePackages_TransactionId",
                table: "TransactionServicePackages",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSLAs_TransactionServicePackageId",
                table: "TransactionSLAs",
                column: "TransactionServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_Id",
                table: "UnitOfMeasurement",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentFiles");

            migrationBuilder.DropTable(
                name: "BrasInformation");

            migrationBuilder.DropTable(
                name: "CardTypes");

            migrationBuilder.DropTable(
                name: "ChannelGroups");

            migrationBuilder.DropTable(
                name: "ChannelPriceBusTables");

            migrationBuilder.DropTable(
                name: "ContactInfos");

            migrationBuilder.DropTable(
                name: "ContractContents");

            migrationBuilder.DropTable(
                name: "ContractEquipments");

            migrationBuilder.DropTable(
                name: "ContractForms");

            migrationBuilder.DropTable(
                name: "ContractorProperties");

            migrationBuilder.DropTable(
                name: "ContractSharingRevenueLines");

            migrationBuilder.DropTable(
                name: "ContractTotalByCurrencies");

            migrationBuilder.DropTable(
                name: "CurrencyUnits");

            migrationBuilder.DropTable(
                name: "EquipmentPictures");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "InContractServices");

            migrationBuilder.DropTable(
                name: "InContractStatus");

            migrationBuilder.DropTable(
                name: "InContractTaxes");

            migrationBuilder.DropTable(
                name: "InContractTypes");

            migrationBuilder.DropTable(
                name: "ManagementBusinessBlocks");

            migrationBuilder.DropTable(
                name: "OutContractServicePackageClearings");

            migrationBuilder.DropTable(
                name: "OutContractServicePackageStatuses");

            migrationBuilder.DropTable(
                name: "OutContractServicePackageTaxes");

            migrationBuilder.DropTable(
                name: "OutContractStatus");

            migrationBuilder.DropTable(
                name: "OutContractTypes");

            migrationBuilder.DropTable(
                name: "PointTypes");

            migrationBuilder.DropTable(
                name: "ProjectTechnicians");

            migrationBuilder.DropTable(
                name: "PromotionDetails");

            migrationBuilder.DropTable(
                name: "PromotionForContracts");

            migrationBuilder.DropTable(
                name: "PromotionProducts");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "PromotionTypes");

            migrationBuilder.DropTable(
                name: "PromotionValueTypes");

            migrationBuilder.DropTable(
                name: "RadiusServerInformation");

            migrationBuilder.DropTable(
                name: "ReasonTypeContractTerminations");

            migrationBuilder.DropTable(
                name: "Salesman");

            migrationBuilder.DropTable(
                name: "ServiceLevelAgreements");

            migrationBuilder.DropTable(
                name: "ServicePackagePrice");

            migrationBuilder.DropTable(
                name: "ServicePackageRadiusServices");

            migrationBuilder.DropTable(
                name: "ServicePackageSuspensionTimes");

            migrationBuilder.DropTable(
                name: "TemporaryPayingContracts");

            migrationBuilder.DropTable(
                name: "TransactionChannelTaxes");

            migrationBuilder.DropTable(
                name: "TransactionPriceBusTables");

            migrationBuilder.DropTable(
                name: "TransactionPromotionForContracts");

            migrationBuilder.DropTable(
                name: "TransactionSLAs");

            migrationBuilder.DropTable(
                name: "TransactionStatuses");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "TransactionEquipments");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "ContractSharingRevenues");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "ServicePackages");

            migrationBuilder.DropTable(
                name: "TaxCategories");

            migrationBuilder.DropTable(
                name: "EquipmentStatuses");

            migrationBuilder.DropTable(
                name: "OutContractServicePackages");

            migrationBuilder.DropTable(
                name: "MarketAreas");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurement");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "OutputChannelPoints");

            migrationBuilder.DropTable(
                name: "FlexiblePricingType");

            migrationBuilder.DropTable(
                name: "InContracts");

            migrationBuilder.DropTable(
                name: "TransactionServicePackages");

            migrationBuilder.DropTable(
                name: "ServiceGroups");

            migrationBuilder.DropTable(
                name: "TransactionChannelPoints");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "OutContracts");

            migrationBuilder.DropTable(
                name: "TransactionReasons");

            migrationBuilder.DropTable(
                name: "ContractStatus");

            migrationBuilder.DropTable(
                name: "Contractors");
        }
    }
}
