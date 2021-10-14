using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumX.qrCode.Models
{
    public class RequestModel
    {

        public int Size { get; set; }
        public int Margin { get; set; }
        public int DocumentWidth { get; set; }
        public int DocumentHeight { get; set; }
        public int? Repeat { get; set; }

        public string Prefix { get; set; }
        public bool FillFinalPage { get; set; } = false;
        public List<ItemModel> Items { get; set; }

        public class ItemModel
        {
            public string Value { get; set; }
            public string Title { get; set; }
        }
    }
}
