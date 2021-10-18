
using iText.Barcodes;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using QuantumX.qrCode.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumX.qrCode.Logic
{
    public class CodeGenerator
    {
        RequestModel request;

        public CodeGenerator()
        {            
        }

        List<CodeModel> codes;
        List<CodeModel> Codes => this.codes = this.codes ?? GetCodes();

        private List<CodeModel> GetCodes() => 
            this.request.Items
                .Select(v => new CodeModel(
                       this.request.Prefix+ v.Value,
                       new BarcodeQRCode(this.request.Prefix+v.Value),
                       v.Title)).ToList();



        private float GetPoints(int mm) =>
            (mm / 27.4f) * 72f;

        public byte[] GenerateDocument(RequestModel request)            
        {
            this.request = request;
                        
            
            int rowsPerPage = (int)Math.Floor(request.DocumentHeight / (decimal)(this.request.Size + this.request.Margin));
            int colsPerPage = (int)Math.Floor(request.DocumentWidth / (decimal)(this.request.Size + this.request.Margin));

            float itemSize = GetPoints(this.request.Size);
            float realSize = GetPoints(this.request.Size + this.request.Margin);
            float realDocumentHeight = GetPoints(this.request.DocumentHeight);
            float marginLeft = (GetPoints(this.request.DocumentWidth) - (realSize * colsPerPage)) / 2f;
            float marginTop = (GetPoints(this.request.DocumentHeight) - (realSize * rowsPerPage)) / 2f;

            int position = 0;
            int row = 0;
            int col = 0;
            using (MemoryStream stream = new MemoryStream())
            {

                using (PdfWriter writer = new PdfWriter(stream))
                using (PdfDocument pdfDocument = new PdfDocument(writer))
                using (Document document = new Document(pdfDocument))
                {
                    pdfDocument.SetDefaultPageSize(new PageSize(
                        GetPoints(request.DocumentWidth),
                        GetPoints(request.DocumentHeight)
                        ));
                    var page = pdfDocument.AddNewPage();
                    while (true)
                    {
                        if (position >= this.Codes.Count)
                        {
                            position = 0;
                            this.request.Repeat--;                            
                        }
                        if (col >= colsPerPage)
                        {
                            col = 0;
                            row++;
                        }
                        if (row >= rowsPerPage)
                        {
                            row = 0;                            
                            if (this.request.Repeat <= 0)
                                break;
                            page = pdfDocument.AddNewPage();
                        }
                        var item = this.Codes[position++];
                        document.Add(
                            new Image(item.Code.CreateFormXObject(ColorConstants.BLACK, pdfDocument))
                                .SetWidth(realSize)
                                .SetHeight(realSize)
                                .SetFixedPosition(
                                    marginLeft + (realSize * col),
                                    realDocumentHeight - (marginTop + (realSize * row))-realSize
                                ));
                        col++;
                    }
                    stream.Flush();
                }
                stream.Flush();
                File.WriteAllBytes(@$"k:\qrcodes\{Guid.NewGuid()}.pdf", stream.GetBuffer());
                return stream.GetBuffer();

            }
        }

    }
}
