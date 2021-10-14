using iText.Barcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumX.qrCode.Models
{
    public class CodeModel
    {
        public CodeModel(
            string value,
            BarcodeQRCode code,
            string title = null
            )
        {
            this.Code = code;
            this.Value = value;
            this.Title = title;
        }

        public BarcodeQRCode Code { get; set; }
        public string Value { get; set; }
        public string Title { get; set; }

    }
}
