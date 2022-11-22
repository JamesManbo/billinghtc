using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Seed;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("ServicePackages")]
    public class ServicePackage : Entity
    {
        public ServicePackage()
        {
            this.ServicePackageRadiusServices = new HashSet<ServicePackageRadiusService>();
            this.CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
            this.CurrencyUnitId = CurrencyUnit.VND.Id;
            //this.PriceByCurrencyUnits = new HashSet<ServicePackagePrice>(); //Các giá theo từng đơn vị tiền
        }
        public int? ServiceId { get; set; }
        public int? PictureId { get; set; }
        public int? ParentId { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public int? InternationalBandwidthUomId { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public int? DomesticBandwidthUomId { get; set; }
        public decimal Price { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int CurrencyUnitId { get; set; }

        [IgnoreDataMember]
        public virtual HashSet<ServicePackageRadiusService> ServicePackageRadiusServices { get; set; }

        //public virtual HashSet<ServicePackagePrice> PriceByCurrencyUnits { get; set; }

        public void Update(CUServicePackageCommand cuServicePackageCommand)
        {
            ServiceId = cuServicePackageCommand.ServiceId;
            PackageCode = cuServicePackageCommand.PackageCode;
            PackageName = cuServicePackageCommand.PackageName;
            BandwidthLabel = cuServicePackageCommand.BandwidthLabel;
            InternationalBandwidth = cuServicePackageCommand.InternationalBandwidth;
            InternationalBandwidthUom = cuServicePackageCommand.InternationalBandwidthUom;
            InternationalBandwidthUomId = cuServicePackageCommand.InternationalBandwidthUomId;

            DomesticBandwidth = cuServicePackageCommand.DomesticBandwidth;
            DomesticBandwidthUom = cuServicePackageCommand.DomesticBandwidthUom;
            DomesticBandwidthUomId = cuServicePackageCommand.DomesticBandwidthUomId;

            Price = cuServicePackageCommand.Price;

            // PriceBeforeTax = cuServicePackageCommand.PriceBeforeTax;
            // HasToCollectMoney = cuServicePackageCommand.HasToCollectMoney;
            // HasFixedPrice = cuServicePackageCommand.IsHasPrice;

            IsActive = true;
            if (cuServicePackageCommand.ServicePackageRadiusServices != null
                && cuServicePackageCommand.ServicePackageRadiusServices.Any())
            {
                foreach (var updatingSrvPckRadiusSrv
                    in cuServicePackageCommand.ServicePackageRadiusServices)
                {
                    if (updatingSrvPckRadiusSrv == null) continue;
                    if (updatingSrvPckRadiusSrv.Id == 0)
                    {
                        var newSrvPckRadiusSrv = new ServicePackageRadiusService();
                        newSrvPckRadiusSrv.Binding(updatingSrvPckRadiusSrv);
                        ServicePackageRadiusServices.Add(newSrvPckRadiusSrv);
                    }
                    else
                    {
                        var updateSrvPckRadiusSrv = ServicePackageRadiusServices.First(s => s.Id == updatingSrvPckRadiusSrv.Id);
                        updateSrvPckRadiusSrv.Binding(updatingSrvPckRadiusSrv);
                    }
                }

                ServicePackageRadiusServices.RemoveWhere(
                    s => cuServicePackageCommand.ServicePackageRadiusServices.All(s => s.Id != s.Id));
            }
        }       
    }
}
