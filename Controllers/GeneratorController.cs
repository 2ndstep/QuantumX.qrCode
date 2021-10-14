using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuantumX.qrCode.Logic;
using QuantumX.qrCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumX.qrCode.Controllers
{
    [Route("")]
    [ApiController]
    public class GeneratorController : ControllerBase
    {
        readonly CodeGenerator generator;
        public GeneratorController(CodeGenerator generator) => this.generator = generator;
        [HttpPost]
        public byte[] Get(RequestModel model) => this.generator.GenerateDocument(model);
    }
}
