using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.MultipleTransactionCommand
{
    public class MultipleReclaimEquipmentCommand
        : BaseMultipleTransactionCommand, IRequest<ActionResponse>
    {
        public int EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public string EquipmentUom { get; set; }
        public float ReclaimUnit { get; set; }
        public MultipleReclaimEquipmentCommand()
        {
        }
    }
}
