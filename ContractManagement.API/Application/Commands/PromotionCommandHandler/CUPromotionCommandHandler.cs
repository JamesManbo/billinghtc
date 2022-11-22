using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Commands.PromotionCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.PromotionRepository;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.PromotionCommandHandler
{
    public class CUPromotionCommandHandler : IRequestHandler<CUPromotionCommand, ActionResponse<PromotionDTO>>
    {
        private readonly IPromotionRepository _PromotionRepository;
        private readonly IPromotionDetailRepository _PromotionDetailRepository;
        private readonly IPromotionDetailQueries _promotionDetailQueries;
        private readonly IPromotionProductRepository _promotionProductRepository;

        public CUPromotionCommandHandler(IPromotionRepository PromotionRepository, 
                                            IPromotionDetailRepository PromotionDetailRepository, 
                                            IPromotionDetailQueries promotionQueries,
                                            IPromotionProductRepository promotionProductRepository)
        {
            _PromotionRepository = PromotionRepository;
            _PromotionDetailRepository = PromotionDetailRepository;
            _promotionDetailQueries = promotionQueries;
            _promotionProductRepository = promotionProductRepository;
        }

        public async Task<ActionResponse<PromotionDTO>> Handle(CUPromotionCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<PromotionDTO>();
            if (_PromotionRepository.CheckExitPromotionCode(request.PromotionCode, request.Id))
            {
                commandResponse.AddError("Mã khuyến mại đã tồn tại ", nameof(request.PromotionCode));
                return commandResponse;
            }

            if (request.Id == 0)
            {
                request.IsActive = true;
                request.IsDeleted = false;
                var rs = await _PromotionRepository.CreateAndSave(request);
                if (request.PromotionDetails != null)
                {
                    foreach (var item in request.PromotionDetails)
                    {
                        var promotionDetail = new PromotionDetail();
                        promotionDetail.CreatedBy = request.CreatedBy;
                        promotionDetail.CreatedDate = request.CreatedDate;
                        promotionDetail.PromotionId = rs.Result.Id;
                        promotionDetail.ProjectId = item.ProjectId;
                        promotionDetail.PromotionValue = item.PromotionValue;
                        promotionDetail.PromotionValueType = item.PromotionValueType;
                        promotionDetail.Quantity = item.Quantity;
                        promotionDetail.CurrencyUnitCode = item.CurrencyUnitCode;
                        promotionDetail.CurrencyUnitId = item.CurrencyUnitId;
                        promotionDetail.ServiceId = item.ServiceId;
                        promotionDetail.ServicePackageId = item.ServicePackageId;
                        promotionDetail.NumberOfMonthApplied = item.NumberOfMonthApplied;
                        promotionDetail.IsActive = true;
                        promotionDetail.IsDeleted = false;
                        promotionDetail.City = item.City;
                        promotionDetail.CityId = item.CityId;
                        promotionDetail.Country = item.Country;
                        promotionDetail.CountryId = item.CountryId;
                        promotionDetail.District = item.District;
                        promotionDetail.DistrictId = item.DistrictId;
                        promotionDetail.SubjectId = item.SubjectId;
                        promotionDetail.MinPaymentPeriod = item.MinPaymentPeriod;
                        await _PromotionDetailRepository.CreateAndSave(promotionDetail);
                    }
                }
                commandResponse.CombineResponse(rs);
            }
            else
            {
                string lstId = "";
                try
                {
                    if (request.PromotionDetails != null && request.PromotionDetails.Count > 0)
                    {
                        foreach (var item in request.PromotionDetails)
                        {                           
                            lstId = lstId + item.Id.ToString() + ",";
                            if (item.IsChange)
                            {
                                if (item.IsActive)
                                {
                                    ActionResponse<PromotionDetail> rs = new ActionResponse<PromotionDetail>();
                                    var promotionDetailExits = _promotionDetailQueries.GetPromotionDetails(item.Id);
                                    // item.ServiceId, item.ServicePackageId, item.ProjectId);      
                                    if (promotionDetailExits != null)
                                    {
                                        promotionDetailExits.CreatedBy = request.UpdatedBy;
                                        promotionDetailExits.CreatedDate = request.UpdatedDate;
                                        promotionDetailExits.PromotionId = request.Id;
                                        promotionDetailExits.ProjectId = item.ProjectId;
                                        promotionDetailExits.PromotionValue = item.PromotionValue;
                                        promotionDetailExits.PromotionValueType = item.PromotionValueType;
                                        promotionDetailExits.Quantity = item.Quantity;
                                        promotionDetailExits.ServiceId = item.ServiceId;
                                        promotionDetailExits.ServicePackageId = item.ServicePackageId;
                                        promotionDetailExits.NumberOfMonthApplied = item.NumberOfMonthApplied;
                                        promotionDetailExits.IsActive = true;
                                        promotionDetailExits.IsDeleted = false;
                                        promotionDetailExits.City = item.City;
                                        promotionDetailExits.CityId = item.CityId;
                                        promotionDetailExits.Country = item.Country;
                                        promotionDetailExits.CountryId = item.CountryId;
                                        promotionDetailExits.District = item.District;
                                        promotionDetailExits.DistrictId = item.DistrictId;
                                        promotionDetailExits.SubjectId = item.SubjectId;
                                        await _PromotionDetailRepository.UpdateAndSave(promotionDetailExits);
                                        //await _promotionProductRepository.UpdatePromotionProduct(rs.Result.Id, promotionDetailExits.Id);
                                    }
                                    else
                                    {
                                        var promotionDetail = new PromotionDetail
                                        {
                                            CreatedBy = request.UpdatedBy,
                                            CreatedDate = request.UpdatedDate,
                                            PromotionId = request.Id,
                                            ProjectId = item.ProjectId,
                                            PromotionValue = item.PromotionValue,
                                            PromotionValueType = item.PromotionValueType,
                                            Quantity = item.Quantity,
                                            ServiceId = item.ServiceId,
                                            ServicePackageId = item.ServicePackageId,
                                            NumberOfMonthApplied = item.NumberOfMonthApplied,
                                            City = item.City,
                                            CityId = item.CityId,
                                            Country = item.Country,
                                            CountryId = item.CountryId,
                                            District = item.District,
                                            DistrictId = item.DistrictId,
                                            SubjectId = item.SubjectId,
                                            IsActive = true,
                                            IsDeleted = false
                                        };
                                        rs = await _PromotionDetailRepository.CreateAndSave(promotionDetail);
                                    }

                                    if (item.PromotionProducts.Count > 0)
                                    {
                                        foreach (var pro in item.PromotionProducts)
                                        {
                                            if (!pro.IsChange)
                                            {
                                                continue;
                                            }

                                            if (pro.Id > 0)
                                            {
                                                await _promotionProductRepository.UpdateAndSave(pro);
                                                continue;
                                            }

                                            var promotionProduct = new PromotionProduct
                                            {
                                                PromotionDetailId = rs.Result.Id,
                                                ProductId = pro.ProductId,
                                                ProductName = pro.ProductName,
                                                Quantity = pro.Quantity
                                            };
                                            promotionProduct.IsOurProduct = promotionProduct.ProductId != 0;
                                            await _promotionProductRepository.CreateAndSave(promotionProduct);
                                        }
                                    }
                                }
                                else
                                {
                                    await _PromotionDetailRepository.UpdateAndSave(item);
                                }
                            }
                        }
                        if(request.DeletePromotionDetails != null && request.DeletePromotionDetails.Count > 0)
                        {
                            foreach (var item in request.DeletePromotionDetails)
                            {
                                if(item.Id==0)
                                {
                                    continue;
                                }    
                                item.IsActive = false;
                                await _PromotionDetailRepository.UpdateAndSave(item);
                            }
                        }    
                        
                    }
                    commandResponse.CombineResponse(await _PromotionRepository.UpdateAndSave(request));
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return commandResponse;
        }


    }
}
