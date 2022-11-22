using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Bindings
{
    public interface IBind
    {
        public void Binding(IBaseRequest command, bool isUpdate = false);
    }
}
