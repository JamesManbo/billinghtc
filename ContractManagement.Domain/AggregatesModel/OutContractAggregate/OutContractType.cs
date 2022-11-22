using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    [Table("OutContractTypes")]
    public class OutContractType : Enumeration
    {
        public static OutContractType Individual = new OutContractType(1, "Cá nhân");
        public static OutContractType Enterprise = new OutContractType(2, "Doanh nghiệp");

        public OutContractType(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<OutContractType> List() => new[]
            {Individual, Enterprise};

        public static OutContractType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (state == null)
            {
                throw new ContractDomainException(
                    $"Possible values for OutContractTypes: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }


        public static OutContractType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ContractDomainException(
                    $"Possible values for OutContractTypes: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}