
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IPromotionQueries : IQueryRepository
    {
        IEnumerable<PromotionDTO> GetAll(RequestFilterModel filterModel = null);
        IPagedList<PromotionDTO> GetList(RequestFilterModel filterModel);
        PromotionDTO GetPromotionById(int id);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
        List<PromotionDetailDTO> GetPromotionDetail(int id);
        IEnumerable<AvailablePromotionDto> GetAvailablePromotions();
        IEnumerable<AvailablePromotionDto> GetAvailablePromotions(int serviceId, int servicePackageId);
        PromotionForContract GetCurPromotionOfProduct(int outContractId, int servicePackageId, int? promotionDetailId);
        // IEnumerable<AvailablePromotionDto> GetAvailablePromotionForContract(int serviceId, int servicePackageId, int outContractServicePackageId);
        IEnumerable<AvailablePromotionDto> GetAvailablePromotionForContract(AvailablePromotionModelFilter promotionFiler);
        IEnumerable<AvailablePromotionDto> GetAvailablePromotionByContractId(int outContractId);
    }
    public class PromotionQueries : QueryRepository<Promotion, int>, IPromotionQueries
    {
        public PromotionQueries(ContractDbContext context) : base(context)
        {

        }
        public IEnumerable<PromotionDTO> GetAll(RequestFilterModel filterModel = null)
        {
            var dapperExecution = BuildByTemplate<PromotionDTO>(filterModel);
            dapperExecution.SqlBuilder.Select(@"CASE WHEN promotionType = 1 THEN 'Giảm trừ cước' 
                                                WHEN promotionType = 2 THEN 'Tặng thời gian sử dụng'
                                                WHEN promotionType = 3 THEN 'Tặng sản phẩm'
                                                ELSE 'Khuyến mại khác'
                                                END AS promotionTypeString");
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.Quantity");
            dapperExecution.SqlBuilder.Select("t2.City");
            dapperExecution.SqlBuilder.Select("t2.CityId");
            dapperExecution.SqlBuilder.Select("t2.District");
            dapperExecution.SqlBuilder.Select("t2.DistrictId");
            dapperExecution.SqlBuilder.Select("t2.Country");
            dapperExecution.SqlBuilder.Select("t2.CountryId");
            dapperExecution.SqlBuilder.Select("t2.CurrencyUnitCode");
            dapperExecution.SqlBuilder.Select("t2.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t2.NumberOfMonthApplied");
            dapperExecution.SqlBuilder.Select("t2.PromotionValueType");
            dapperExecution.SqlBuilder.Select("t2.MinPaymentPeriod");
            dapperExecution.SqlBuilder.Select(@"case when t2.UpdatedBy is not null then t2.UpdatedBy
                                                    else t2.CreatedBy
                                                    end as UpdatePerson");
            dapperExecution.SqlBuilder.Select(@"case when t2.PromotionValueType = 1 then 'Giảm trừ cước (% giá trị)'
                            when t2.PromotionValueType = 2 then 'Giảm trừ cước (tiền mặt)'
                            when t2.PromotionValueType = 3 then 'Tặng thời gian sử dụng (theo tháng)'
                            when t2.PromotionValueType = 4 then 'Tặng thời gian sử dụng (theo ngày)'
                            else 'Khuyến mại khác'
                            end as PromotionValueTypeString");

            dapperExecution.SqlBuilder.InnerJoin("PromotionDetails AS t2 ON t1.Id = t2.PromotionId");
            var cached = new Dictionary<int, PromotionDTO>();
            return dapperExecution.ExecuteQuery<PromotionDTO, PromotionDetailDTO>((promotion, promotionDetail) =>
            {
                if (!cached.TryGetValue(promotion.Id, out var cachedObj))
                {
                    cachedObj = promotion;
                    cached.Add(promotion.Id, cachedObj);
                }

                if (promotionDetail != null)
                {
                    if (cachedObj.PromotionDetails.All(p => p.Id != promotionDetail.Id))
                    {
                        cachedObj.PromotionDetails.Add(promotionDetail);
                    }
                }
                return cachedObj;
            }, "Id");
        }

        public IPagedList<PromotionDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<PromotionDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select(@"CASE WHEN promotionType = 1 THEN 'Giảm trừ cước' 
                                                WHEN promotionType = 2 THEN 'Tặng thời gian sử dụng'
                                                WHEN promotionType = 3 THEN 'Tặng sản phẩm'
                                                ELSE 'Khuyến mại khác'
                                                END AS promotionTypeString");
            return dapperExecution.ExecutePaginateQuery();
        }

        public PromotionDTO GetPromotionById(int id)
        {
            PromotionDTO result = null;
            var dapperExecution = BuildByTemplate<PromotionDTO>();
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.Quantity");
            dapperExecution.SqlBuilder.Select("t2.City");
            dapperExecution.SqlBuilder.Select("t2.CityId");
            dapperExecution.SqlBuilder.Select("t2.District");
            dapperExecution.SqlBuilder.Select("t2.DistrictId");
            dapperExecution.SqlBuilder.Select("t2.Country");
            dapperExecution.SqlBuilder.Select("t2.CountryId");
            dapperExecution.SqlBuilder.Select("t2.CurrencyUnitCode");
            dapperExecution.SqlBuilder.Select("t2.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t2.NumberOfMonthApplied");
            dapperExecution.SqlBuilder.Select("t2.MinPaymentPeriod");
            dapperExecution.SqlBuilder.Select("t3.Id as ServiceId");
            dapperExecution.SqlBuilder.Select("t3.ServiceName");
            dapperExecution.SqlBuilder.Select("t4.Id as servicePackageId");
            dapperExecution.SqlBuilder.Select("t4.PackageName as servicePackageName");
            dapperExecution.SqlBuilder.Select("t5.Id as ProjectId");
            dapperExecution.SqlBuilder.Select("t5.ProjectName");
            dapperExecution.SqlBuilder.Select("t2.PromotionValueType");
            dapperExecution.SqlBuilder.Select(@"case when t2.UpdatedBy is not null then t2.UpdatedBy
                                                    else t2.CreatedBy
                                                    end as UpdatePerson");
            dapperExecution.SqlBuilder.Select(@"case when t2.PromotionValueType = 1 then 'Giảm trừ cước (% giá trị)'
                            when t2.PromotionValueType = 2 then 'Giảm trừ cước (tiền mặt)'
                            when t2.PromotionValueType = 3 then 'Tặng thời gian sử dụng (theo tháng)'
                            when t2.PromotionValueType = 4 then 'Tặng thời gian sử dụng (theo ngày)'
                            when t2.PromotionValueType = 5 and t6.IsOurProduct = 1 then CONCAT_WS('','Tặng sản phẩm công ty ', PromotionValue)
                            when t2.PromotionValueType = 5 and t6.IsOurProduct = 0 then CONCAT_WS('','Tặng sản phẩm khác ', PromotionValue)
                            else 'Khuyến mại khác'
                            end as PromotionValueTypeString");

            dapperExecution.SqlBuilder.Select("t2.IsActive");
            dapperExecution.SqlBuilder.Select("t6.Id");
            dapperExecution.SqlBuilder.Select("t6.PromotionDetailId");
            dapperExecution.SqlBuilder.Select("t6.ProductId");
            dapperExecution.SqlBuilder.Select("t6.Quantity");
            dapperExecution.SqlBuilder.Select("t6.ProductName");
            dapperExecution.SqlBuilder.Select("t6.IsOurProduct");


            dapperExecution.SqlBuilder.LeftJoin("PromotionDetails t2 ON t2.PromotionId = t1.Id");
            dapperExecution.SqlBuilder.LeftJoin("Services as t3 ON t2.ServiceId = t3.Id");
            dapperExecution.SqlBuilder.LeftJoin("ServicePackages as t4 ON t4.Id = t2.ServicePackageId");
            dapperExecution.SqlBuilder.LeftJoin("Projects as t5 ON t5.Id= t2.ProjectId");
            dapperExecution.SqlBuilder.LeftJoin("PromotionProducts as t6 ON t2.Id= t6.PromotionDetailId");

            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("ifnull(t2.IsActive,true) = TRUE");
            dapperExecution.SqlBuilder.Where("ifnull(t2.IsDeleted,false) = FALSE");
            dapperExecution.SqlBuilder.Where("ifnull(t6.IsActive,true) = TRUE");
            dapperExecution.SqlBuilder.Where("ifnull(t6.IsDeleted,false) = FALSE");


            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            dapperExecution.ExecuteQuery<PromotionDTO, PromotionDetailDTO, PromotionProductDTO>((promotion, promotionDetail, promotionProduct) =>
            {
                if (result == null) result = promotion;
                if (promotionDetail != null)
                {
                    if (result.PromotionDetails.All(p => p.Id != promotionDetail.Id))
                    {
                        if (promotionProduct != null)
                        {
                            promotionDetail.PromotionProducts.Add(promotionProduct);
                        }
                        result.PromotionDetails.Add(promotionDetail);
                    }
                    else
                    {
                        var existedPromotionDetail = result.PromotionDetails.First(e => e.Id == promotionDetail.Id); if (promotionProduct != null)
                        {
                            existedPromotionDetail.PromotionProducts.Add(promotionProduct);
                        }
                    }
                }

                return default;
            }, "Id,Id");

            return result;
        }

        /// <summary>
        /// for combobox
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.ServiceId AS ParentId");
            dapperExecution.SqlBuilder.Select("CONCAT(t1.PackageName, '(', t1.BandwidthLabel, ')', ' - ', FORMAT(t1.Price, 0), 'đ') AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            return dapperExecution.ExecuteQuery();
        }
        public List<PromotionDetailDTO> GetPromotionDetail(int id)
        {
            var dapperExecution = BuildByTemplate<PromotionDetailDTO>();
            dapperExecution.SqlBuilder.Select("t2.ServiceName");
            dapperExecution.SqlBuilder.Select("t3.PackageName");
            dapperExecution.SqlBuilder.Select(@"case when t2.PromotionValueType = 1 then 'Giảm trừ cước (% giá trị)'
                            when t2.PromotionValueType = 2 then 'Giảm trừ cước (tiền mặt)'
                            when t2.PromotionValueType = 3 then 'Tặng thời gian sử dụng (theo tháng)'
                            when t2.PromotionValueType = 4 then 'Tặng thời gian sử dụng (theo ngày)'
                            when t2.PromotionValueType = 5 and t6.IsOurProduct = 1 then CONCAT_WS('','Tặng sản phẩm công ty ', PromotionValue)
                            when t2.PromotionValueType = 5 and t6.IsOurProduct = 1 CONCAT_WS('','Tặng sản phẩm khác ', PromotionValue)
                            else 'Khuyến mại khác'
                            end as PromotionValueTypeString");

            dapperExecution.SqlBuilder.InnerJoin("Services as t2 ON t1.ServiceId = t2.Id");
            dapperExecution.SqlBuilder.InnerJoin("ServicePackages as t3 ON t3.Id = t1.ServicePackageId");
            dapperExecution.SqlBuilder.InnerJoin("Projects as t4 ON t4.Id= t1.ProjectId");
            dapperExecution.SqlBuilder.Where("t1.PromotionId = @Id", new { id });
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            var lstPriceRaw = dapperExecution.ExecuteQuery();
            var rs = new List<PromotionDetailDTO>();
            foreach (PromotionDetailDTO model in lstPriceRaw)
            {
                rs.Add(new PromotionDetailDTO()
                {
                    Id = model.Id,
                    ProjectId = model.ProjectId,
                    ProjectName = model.ProjectName,
                    PromotionCode = model.PromotionCode,
                    PromotionValueType = model.PromotionValueType,
                    ServiceId = model.ServiceId,
                    ServiceName = model.ServiceName,
                    ServicePackageId = model.ServicePackageId,
                    ServicePackageName = model.ServicePackageName,
                    Quantity = model.Quantity,
                    MinPaymentPeriod = model.MinPaymentPeriod,
                });
            }

            return rs;
        }

        public IEnumerable<AvailablePromotionDto> GetAvailablePromotions(int serviceId, int servicePackageId)
        {
            var dapperExecution = BuildByTemplate<AvailablePromotionDto>();
            dapperExecution.SqlBuilder.Select("t2.Id AS PromotionDetailId");
            dapperExecution.SqlBuilder.Select("t2.PromotionValueType");
            dapperExecution.SqlBuilder.Select("t2.MinPaymentPeriod");
            dapperExecution.SqlBuilder.Select("t2.Quantity");
            dapperExecution.SqlBuilder.Select("t3.PackageName");
            dapperExecution.SqlBuilder.Select("t4.ServiceName");
            dapperExecution.SqlBuilder.Select("t5.OutContractServicePackageId");
            dapperExecution.SqlBuilder.Select("t5.Id AS PromotionForContractId");
            dapperExecution.SqlBuilder.Select(@"CASE 
                            WHEN t2.PromotionValueType = 1 THEN CONCAT('Giảm trừ cước (% giá trị): ',Quantity,'%',', áp dụng trong: ',t2.NumberOfMonthApplied,' tháng')                                   
						    WHEN t2.PromotionValueType = 2 THEN CONCAT('Giảm trừ cước (tiền mặt): ',Quantity,' vnd',', áp dụng trong: ',t2.NumberOfMonthApplied,' tháng')
                            
                            WHEN t2.PromotionValueType = 3 THEN CONCAT('Tặng thời gian sử dụng (theo tháng): ', Quantity ,' tháng')
                            WHEN t2.PromotionValueType = 4 THEN CONCAT('Tặng thời gian sử dụng (theo ngày): ', Quantity ,' ngày')
                            WHEN t2.PromotionValueType = 5 THEN CONCAT('Tặng sản phẩm','')
                            ELSE 'Khuyến mại khác'
                            END AS Description");

            dapperExecution.SqlBuilder.LeftJoin("PromotionDetails AS t2 ON t1.Id = t2.PromotionId");
            dapperExecution.SqlBuilder.LeftJoin("ServicePackages AS t3 ON t3.Id = t2.ServicePackageId");
            dapperExecution.SqlBuilder.LeftJoin("Services AS t4 ON t4.Id = t2.ServiceId");
            dapperExecution.SqlBuilder.LeftJoin("PromotionForContracts AS t5 ON t2.Id = t5.PromotionDetailId");

            if (serviceId > 0)
            {
                dapperExecution.SqlBuilder.Where("t2.ServiceId = @serviceId",
                    new { serviceId }
                );
            }

            if (servicePackageId > 0)
            {
                dapperExecution.SqlBuilder.Where("t2.ServicePackageId = @servicePackageId",
                    new { servicePackageId }
                );
            }

            dapperExecution.SqlBuilder.Where("NOW() BETWEEN t1.StartDate AND t1.EndDate");
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE AND t1.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("t2.IsActive = TRUE AND t2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("t3.IsActive = TRUE AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("t4.IsActive = TRUE AND t4.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("IFNULL(t5.IsActive,1) = 1 AND IFNULL(t5.IsDeleted,0) = 0");

            return dapperExecution.ExecuteQuery().Distinct();

        }
        public IEnumerable<AvailablePromotionDto> GetAvailablePromotionForContract(AvailablePromotionModelFilter promotionFiler)
        {
            var dapperExecution = BuildByTemplate<AvailablePromotionDto>();
            dapperExecution.SqlBuilder.Select("t2.Id as PromotionDetailId");
            dapperExecution.SqlBuilder.Select("t2.ServiceId");
            dapperExecution.SqlBuilder.Select("t2.ServicePackageId");
            dapperExecution.SqlBuilder.Select("t2.ProjectId");
            dapperExecution.SqlBuilder.Select("t2.CityId");
            dapperExecution.SqlBuilder.Select("t2.DistrictId");
            dapperExecution.SqlBuilder.Select("t2.SubjectId");
            dapperExecution.SqlBuilder.Select("t2.PromotionValueType");
            dapperExecution.SqlBuilder.Select("t2.Quantity");
            dapperExecution.SqlBuilder.Select("t2.NumberOfMonthApplied");
            dapperExecution.SqlBuilder.Select("t2.MinPaymentPeriod");
            dapperExecution.SqlBuilder.Select("t3.PackageName");
            dapperExecution.SqlBuilder.Select("t4.ServiceName");

            string sql = @"'Chương trình: ',t1.PromotionName,', áp dụng từ ngày ',DATE_FORMAT(t1.StartDate,'%d/%m/%Y'),' đến ngày ',DATE_FORMAT(t1.EndDate,'%d/%m/%Y'),'. Nội dung: '";
            dapperExecution.SqlBuilder.Select(@"case 
                when t2.PromotionValueType = 1 then CONCAT(" + sql + @",'Giảm trừ cước (% giá trị): ',Quantity,'%',', áp dụng trong: ',t2.NumberOfMonthApplied,' tháng')
                when t2.PromotionValueType = 2 then CONCAT(" + sql + @",'Giảm trừ cước (tiền mặt): ',Quantity,t2.CurrencyUnitCode,', áp dụng trong: ',t2.NumberOfMonthApplied,' tháng')
                when t2.PromotionValueType = 3 then CONCAT(" + sql + @",'Tặng thời gian sử dụng (theo tháng): ', Quantity ,' tháng')
                when t2.PromotionValueType = 4 then CONCAT(" + sql + @",'Tặng thời gian sử dụng (theo ngày): ', Quantity ,' ngày')
                when t2.PromotionValueType = 5 then CONCAT(" + sql + @",'Tặng sản phẩm','%')
                else 'Khuyến mại khác'
                end as Description");

            dapperExecution.SqlBuilder.LeftJoin("PromotionDetails as t2 ON t1.Id = t2.PromotionId");
            dapperExecution.SqlBuilder.LeftJoin("ServicePackages as t3 ON t3.Id = t2.ServicePackageId");
            dapperExecution.SqlBuilder.LeftJoin("Services as t4 ON t4.Id = t2.ServiceId");
            if (promotionFiler.OutContractServicePackageIds != null)
            {
                dapperExecution.SqlBuilder.Select("t5.OutContractServicePackageId");
                dapperExecution.SqlBuilder.Select("t5.Id as PromotionForContractId");
                dapperExecution.SqlBuilder.Select("t5.OutContractServicePackageId");
                dapperExecution.SqlBuilder.LeftJoin(
                    @"( SELECT  pct.Id, pct.PromotionId,pct.PromotionDetailId,pct.OutContractServicePackageId
                        FROM PromotionForContracts as pct  
                        where (pct.OutContractServicePackageId IN @outContractServicePackageIds )
                    ) as t5 ON t2.Id = t5.PromotionDetailId ", new { outContractServicePackageIds = promotionFiler.OutContractServicePackageIds });

            }

            dapperExecution.SqlBuilder.Where(" t2.CurrencyUnitId = @currencyUnitId ", new { currencyUnitId = promotionFiler.CurrencyUnitId });

            var sSqlWhere = "( ( t2.ServiceId IN @serviceIds AND  IFNULL(t2.ServicePackageId,0) = 0 ";
            sSqlWhere += promotionFiler.ServicePackageIds.Count > 0 ? " OR ( t2.ServicePackageId IN @servicePackageIds ) )" : ")";

            sSqlWhere += promotionFiler.CityIds.Count > 0 ? " OR ( t2.cityId IN @cityIds AND  IFNULL(t2.districtId,'') = '' " : "OR ( t2.cityId = 0 ";
            sSqlWhere += promotionFiler.DistrictIds.Count > 0 ? " OR t2.districtId IN @districtIds ) " : ")";

            sSqlWhere += promotionFiler.CountryIds.Count > 0 ? " OR ( t2.CountryId IN @countryIds )" : "";

            sSqlWhere += promotionFiler.ProjectIds.Count > 0 ? " OR ( t2.ProjectId IN @projectIds )" : "";

            sSqlWhere += promotionFiler.SubjectIds.Count > 0 ? " OR ( t2.SubjectId IN @subjectIds )" : "";

            sSqlWhere += ")";

            //dapperExecution.SqlBuilder.Where("( ( t2.ServiceId IN @serviceIds AND  IFNULL(t2.ServicePackageId,0) = 0 OR t2.ServicePackageId IN @servicePackageIds )" +
            //    " OR ( t2.cityId IN @cityIds AND  IFNULL(t2.districtId,'') = '' OR t2.districtId IN @districtIds ) " +
            //    " OR ( t2.CountryId IN @countryIds )" +
            //    " OR ( t2.ProjectId IN @projectIds )" +
            //    " OR ( t2.SubjectId IN @subjectIds )" +
            //    ")", new 
            dapperExecution.SqlBuilder.Where(sSqlWhere, new
            {
                serviceIds = promotionFiler.ServiceIds,
                servicePackageIds = promotionFiler.ServicePackageIds,
                cityIds = promotionFiler.CityIds,
                districtIds = promotionFiler.DistrictIds,
                countryIds = promotionFiler.CountryIds,
                projectIds = promotionFiler.ProjectIds,
                subjectIds = promotionFiler.SubjectIds
            });

            if (promotionFiler.ToDates.Count > 1)
            {
                string sCondition = "";

                for (int i = 0; i < promotionFiler.ToDates.Count - 1; i++)
                {
                    if (i > 0) sCondition += " OR ";
                    sCondition += "( DATE(" + promotionFiler.ToDates[i] + ")  BETWEEN DATE(t1.StartDate) and DATE(t1.EndDate) OR DATE("
                                       + promotionFiler.FromDates[i] + ") BETWEEN DATE(t1.StartDate) and DATE(t1.EndDate) " + ")";
                }
            }
            else
            {
                var fromDate = promotionFiler.FromDates.FirstOrDefault().ToString("yyyy-MM-dd");
                var toDate = promotionFiler.ToDates.FirstOrDefault().ToString("yyyy-MM-dd");
                dapperExecution.SqlBuilder.Where(" (DATE(@fromDate) BETWEEN DATE(t1.StartDate) and DATE(t1.EndDate) OR DATE(@toDate) BETWEEN DATE(t1.StartDate) and DATE(t1.EndDate) )", new { fromDate, toDate });
            }
            dapperExecution.SqlBuilder.Where("IFNULL(t1.IsActive ,TRUE) = TRUE AND IFNULL(t1.IsDeleted,FALSE)= FALSE");
            dapperExecution.SqlBuilder.Where("IFNULL(t2.IsActive ,TRUE) = TRUE  AND IFNULL(t2.IsDeleted,FALSE)= FALSE");
            dapperExecution.SqlBuilder.Where("IFNULL(t3.IsActive ,TRUE) = TRUE  AND IFNULL(t3.IsDeleted,FALSE)= FALSE");
            dapperExecution.SqlBuilder.Where("IFNULL(t4.IsActive ,TRUE) = TRUE  AND IFNULL(t4.IsDeleted,FALSE)= FALSE");



            return dapperExecution.ExecuteQuery().Distinct();
        }

        public PromotionForContract GetCurPromotionOfProduct(int outContractId, int servicePackageId, int? promotionDetailId)
        {
            var dapperExecution = BuildByTemplate<PromotionForContract>();
            dapperExecution.SqlBuilder.Where("t1.ContractId = @ContractId", new { outContractId });
            dapperExecution.SqlBuilder.Where("t1.PromotionDetailId = @PromotionDetailId", new { promotionDetailId });
            dapperExecution.SqlBuilder.Where("t1.ServicePackageId = @servicePackageId", new { servicePackageId });
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE AND t1.IsDeleted = FALSE");
            return dapperExecution.ExecuteQuery().FirstOrDefault();
        }

        public IEnumerable<AvailablePromotionDto> GetAvailablePromotionByContractId(int outContractId)
        {
            var dapperExecution = BuildByTemplate<AvailablePromotionDto>();
            dapperExecution.SqlBuilder.Select("t2.Id as PromotionDetailId");
            dapperExecution.SqlBuilder.Select("t2.PromotionValueType");
            dapperExecution.SqlBuilder.Select("t2.Quantity");
            dapperExecution.SqlBuilder.Select("t2.NumberOfMonthApplied");
            dapperExecution.SqlBuilder.Select("t2.MinPaymentPeriod");

            dapperExecution.SqlBuilder.InnerJoin("PromotionDetails as t2 ON t1.Id = t2.PromotionId");
            dapperExecution.SqlBuilder.LeftJoin("OutContractServicePackages AS csp " +
                "ON (csp.ServiceId = t2.ServiceId  AND csp.ServicePackageId = t2.ServicePackageId ) " +
                "OR (csp.InstallationAddress_CityId = t2.CityId AND csp.InstallationAddress_DistrictId = t2.DistrictId )");

            dapperExecution.SqlBuilder.Where("csp.OutContractId = @outContractId", new { outContractId });

            return dapperExecution.ExecuteQuery().Distinct();
        }

        public IEnumerable<AvailablePromotionDto> GetAvailablePromotions()
        {
            return this.GetAvailablePromotions(0, 0);
        }
    }
}
