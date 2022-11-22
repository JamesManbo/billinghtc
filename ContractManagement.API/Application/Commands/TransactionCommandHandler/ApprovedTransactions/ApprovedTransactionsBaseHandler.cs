using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ApprovedTransactions
{
    public abstract class ApprovedTransactionsBaseHandler
    {

        public void MapEquipmentCommand(ref CUContractEquipmentCommand equipmentCommand
            , ServicePackageDTO packageInfo, EquipmentTypeDTO eqInfo)
        {
            equipmentCommand.EquipmentName = eqInfo.Name;
            equipmentCommand.EquipmentName = eqInfo.Name;
            equipmentCommand.EquipmentUom = eqInfo.UnitOfMeasurement;
            equipmentCommand.Manufacturer = eqInfo.Manufacturer;
            equipmentCommand.Specifications = eqInfo.Specifications;
            equipmentCommand.UnitPrice = eqInfo.Price;
        }
    }
}
