using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.ServiceCommand
{
    public class ServiceCommand
    {
        public ServiceCommand()
        {
            ServicePrices = new List<ServicePackagePriceCommand>();
            ServiceLevelAgreements = new List<CUServiceLevelAgreementCommand>();
        }
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasStartAndEndPoint { get; set; }
        public bool HasPackages { get; set; }
        public bool HasLineQuantity { get; set; }
        public bool HasCableKilometers { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public PictureDTO Avatar { get; set; }
        public int AvatarId { get; set; }

        public List<CUServiceLevelAgreementCommand> ServiceLevelAgreements { get; set; }
        public int[] DeletedSLAs { get; set; }
        public List<ServicePackagePriceCommand> ServicePrices { get; set; }
    }
}
