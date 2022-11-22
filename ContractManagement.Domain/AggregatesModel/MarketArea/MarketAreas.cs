using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.MarketArea
{
    public class MarketArea : Entity
    {
        public static MarketArea North = new MarketArea(1,null, "MB", "Miền Bắc", 0, "");
        public static MarketArea Central = new MarketArea(2, null, "MT", "Miền Trung", 0, "");
        public static MarketArea South = new MarketArea(3, null, "MN", "Miền Nam", 0, "");


        public MarketArea(int id, int? parentId, string marketCode, string marketName, int treeLevel, string treePath)
        {
            Id = id;
            ParentId = parentId;
            MarketCode = marketCode;
            MarketName = marketName;
            TreeLevel = treeLevel;
            TreePath = treePath;
        }

        public MarketArea()
        {

        }

        public static IEnumerable<MarketArea> Seeds() => new MarketArea[]{ North , Central, South};

        public int? ParentId { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int TreeLevel { get; set; }
        public string TreePath { get; set; }
    }
}
