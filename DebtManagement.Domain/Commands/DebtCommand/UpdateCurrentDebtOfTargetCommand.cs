using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.Domain.Commands.DebtCommand
{
    public class UpdateCurrentDebtOfTargetCommand : IRequest<bool>
    {
        public int TargetId { get; set; }
        public decimal DebtAmount { get; set; }
        public bool Increase { get; set; } = true;
    }
}
