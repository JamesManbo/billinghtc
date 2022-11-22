using System.Collections.Generic;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.Commons
{
    public class Discount : ValueObject
    {
        public float Percent { get; set; }
        public decimal Amount { get; set; }
        public bool Type { get; set; } // true: percent, false: amount

        protected Discount()
        {
        }

        public Discount(float percent = 0, decimal amount = 0, bool type = true)
        {
            Percent = percent;
            Amount = amount;
            Type = type;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Percent;
            yield return Amount;
            yield return Type;
        }
    }
}