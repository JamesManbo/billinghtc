using ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContractManagement.Infrastructure.Repositories.ExchangeRateRepository
{
    public interface IExchangeRateRepository : ICrudRepository<ExchangeRate, int>
    {
        Task<bool> SyncExchangeRate();
    }
    public class ExchangeRateRepository : CrudRepository<ExchangeRate, int>, IExchangeRateRepository
    {
        public ExchangeRateRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
        }

        public async Task<bool> SyncExchangeRate()
        {
            string url = "https://www.vietcombank.com.vn/exchangerates/ExrateXML.aspx";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            XmlSerializer serializer = new XmlSerializer(typeof(ExrateList));
            ExrateList exrateList = (ExrateList)serializer.Deserialize(responseStream);

            int effectedNumber = 0;
            if (exrateList != null && exrateList.Exrates != null && exrateList.Exrates.Count > 0)
            {
                for (int i = 0; i < exrateList.Exrates.Count; i++)
                {
                    var exchangeRate = exrateList.Exrates.ElementAt(i);
                    var addModel = exchangeRate.MapTo<ExchangeRate>(_configAndMapper.MapperConfig);
                    if (double.TryParse(addModel.Buy, out var buyValue)) addModel.BuyValue = buyValue;
                    if (double.TryParse(addModel.Transfer, out var transferValue)) addModel.TransferValue = transferValue;
                    if (double.TryParse(addModel.Sell, out var sell)) addModel.SellValue = sell;

                    Create(addModel);
                }

                effectedNumber = await SaveChangeAsync();
            }

            return effectedNumber > 0;
        }
    }
}
