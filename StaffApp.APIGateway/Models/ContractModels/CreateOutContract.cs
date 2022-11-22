using StaffApp.APIGateway.Models.BaseContractModels;
using StaffApp.APIGateway.Models.InContractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class CreateOutContract : CreateUpdateContractBase
    {
        //Mã dịch vụ đối tác
        public string AgentContractCode { get; set; }
        //Thông tin note quang
        public string FiberNodeInfo { get; set; }
        ///public List<CUContractEquipment> Equipments { get; set; }
        public List<CUContractServicePackage> ServicePackages { get; set; }
    }
}
