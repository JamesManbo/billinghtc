using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class ContractSimpleDTO
    {
        public int Id { get; set; }
        public string ContractCode { get; set; }
        public string ContractStatusName { get; set; }
        public ContractorDTO Contractor { get; set; }
        public List<OutContractServicePackageSimpleDTO> ServicePackages { get; set; }
    }
}
