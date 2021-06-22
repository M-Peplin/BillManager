using BillManager.Interfaces;
using BillManager.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillManager.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillsService _billService;
        private readonly ILogger<BillController> _logger;

        public BillController(IBillsService billService, ILogger<BillController> logger)
        {
            this._billService = billService;
            this._logger = logger;
        }

        [HttpPost]
        [Route("/api/bill/add")]
        public IActionResult AddBill([FromBody]BillDTO billDTO)
        {
            _logger.LogInformation("Executing AddBill controller");
            return Ok(_billService.AddBill(billDTO));
        }

        [HttpPut]
        [Route("/api/bill/edit")]
        public IActionResult EditBill([FromBody]BillDTO billDTO)
        {
            _logger.LogInformation("Executing EditBill controller");
            return Ok(_billService.EditBill(billDTO));
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("/api/bill/delete/{mail}")]
        public IActionResult DeleteBill(string mail)
        {
            _logger.LogInformation("Executing DeleteBill controller");
            return Ok(_billService.DeleteBill(mail));
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("/api/bill/getAll/{mail}")]
        public IActionResult GetAllBillByUser(string mail)
        {
            _logger.LogInformation("Exexuting GetAllBillByUser");
            return Ok(_billService.GetAllBillByUser(mail));
        }
    }
}
