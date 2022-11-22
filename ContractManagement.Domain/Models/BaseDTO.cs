using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ContractManagement.Domain.Seed;
using GenericRepository.Configurations;
using GenericRepository.Extensions;

namespace ContractManagement.Domain.Models
{
    public abstract class BaseDTO
    {
        public int Id { get; set; }
    }
}
