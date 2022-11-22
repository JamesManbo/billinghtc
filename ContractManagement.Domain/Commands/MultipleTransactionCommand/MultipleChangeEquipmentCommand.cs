using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.MultipleTransactionCommand
{
    public class MultipleChangeEquipmentCommand
        : BaseMultipleTransactionCommand, IRequest<ActionResponse>
    {
        public int ReclaimEquipmentTypeId { get; set; }
        public string ReclaimEquipmentTypeName { get; set; }
        public string ReclaimEquipmentUom { get; set; }
        public float ReclaimUnit { get; set; }

        public int NewEquipmentTypeId { get; set; }
        public string NewEquipmentTypeName { get; set; }
        public string NewEquipmentTypeUom { get; set; }
        public float NewUnit { get; set; }
        public MultipleChangeEquipmentCommand()
        {
        }
    }
}
