using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Seed;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    [Table("ServiceLevelAgreements")]
    public class ServiceLevelAgreement
        : ServiceLevelAgreementAbstraction, IBind
    {
        public ServiceLevelAgreement() { }
        public ServiceLevelAgreement(CUServiceLevelAgreementCommand command)
        {
            this.Binding(command);
        }

        public ServiceLevelAgreement(string label, string content)
        {
            Label = label;
            Content = content;
        }

        public int OutContractId { get; set; }
        public int? OutContractServicePackageId { get; set; }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUServiceLevelAgreementCommand contractSLACommand)
            {
                this.OutContractId = contractSLACommand.OutContractId;
                this.OutContractServicePackageId = contractSLACommand.OutContractServicePackageId;

                if (contractSLACommand.ServiceId.HasValue)
                    this.ServiceId = contractSLACommand.ServiceId.Value;

                this.IsDefault = contractSLACommand.IsDefault;
                this.Uid = contractSLACommand.Uid;
                this.Label = contractSLACommand.Label;
                this.Content = contractSLACommand.Content;
                this.Id = contractSLACommand.Id;
            }

            if (command is CUTransactionSLACommand transSLACommand)
            {
                if (transSLACommand.ServiceId.HasValue)
                    this.ServiceId = transSLACommand.ServiceId.Value;

                this.IsDefault = transSLACommand.IsDefault;
                this.Uid = transSLACommand.Uid;
                this.Label = transSLACommand.Label;
                this.Content = transSLACommand.Content;
                this.Id = transSLACommand.Id;
            }
        }
    }
}
