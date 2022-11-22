using AutoMapper;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ExchangeRateRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContractManagement.Infrastructure.Services
{
    public interface IExchangeRateService
    {
        double? ExchangeRate(string fromCode, string toCode);
        List<ExchangeRateDTO> ExchangeRates(string codes);
        List<ExchangeRateDTO> GetAllExchangeRate();
        Task<bool> SynchronizeExchangeRates();
    }
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IMapper _mapper;
        private readonly IExchangeRateQueries _exchangeRateQueries;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        public ExchangeRateService(IMapper mapper, 
            IExchangeRateQueries exchangeRateQueries, 
            IExchangeRateRepository exchangeRateRepository)
        {
            _mapper = mapper;
            _exchangeRateQueries = exchangeRateQueries;
            this._exchangeRateRepository = exchangeRateRepository;
        }
        public double? ExchangeRate(string fromCode, string toCode)
        {
            if (string.IsNullOrEmpty(toCode)) toCode = "VND";
            if (string.IsNullOrEmpty(fromCode)) return null;

            if (fromCode != "VND" && toCode != "VND") return null;


            string codeReq = fromCode == "VND" ? toCode : fromCode;
            var exchangeRates = _exchangeRateQueries.GetAllInDate(DateTime.Now);
            if (exchangeRates != null && exchangeRates.Count() > 0)
            {
                if (exchangeRates.Where(e => e.CurrencyCode == codeReq).Count() > 0)
                {
                    var modelRs = exchangeRates.Where(e => e.CurrencyCode == codeReq).FirstOrDefault();
                    if (modelRs.TransferValue!=0)
                    {
                        if (fromCode == "VND") return 1 / modelRs.TransferValue;
                        return modelRs.TransferValue;
                    }
                }
            }
            return null;
        }

        public List<ExchangeRateDTO> ExchangeRates(string codes)
        {
            if (string.IsNullOrEmpty(codes)) return null;

            var rs = new List<ExchangeRateDTO>();

            var exchangeRates = _exchangeRateQueries.GetAllInDate(DateTime.Now);
            var lstCodeReq = codes.Split(",");
            lstCodeReq = lstCodeReq.Distinct().ToArray();

            if (lstCodeReq.Contains("VND"))
            {
                rs.Add(new ExchangeRateDTO
                {
                    CurrencyCode = "VND",
                    CurrencyName = "Đồng",
                    Sell = "1",
                    Transfer = "1",
                    Buy = "1",
                    SellValue = 1,
                    TransferValue = 1,
                    BuyValue = 1
                });
            }
            if (exchangeRates != null && exchangeRates.Count() > 0)
            {
                foreach (string code in lstCodeReq)
                {
                    if (exchangeRates.Where(e => e.CurrencyCode == code).Count() > 0)
                    {
                        rs.Add(exchangeRates.FirstOrDefault(e => e.CurrencyCode == code));
                    }
                }
            }

            return rs;
        }

        public List<ExchangeRateDTO> GetAllExchangeRate()
        {
            return _exchangeRateQueries.GetAllInDate(DateTime.Now).ToList(); ;
        }

        public Task<bool> SynchronizeExchangeRates()
        {
            return _exchangeRateRepository.SyncExchangeRate();
        }
    }
}
