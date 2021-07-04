using BillManager.EF;
using BillManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillManager.Models.ModelsDTO;

namespace BillManager.Controllers
{
    public class InformationController : Controller
    {
        private readonly InformationsService _informationsService;
        private readonly ILogger<InformationController> _logger;
        
        public InformationController(InformationsService informationsService, ILogger<InformationController> logger)
        {
            this._informationsService = informationsService;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/information/add")]
        public IActionResult AddInformation([FromBody] InformationDTO informationDTO)
        {
            _logger.LogInformation("Executing AddInformation controller");

            return Ok(_informationsService.AddInformation(informationDTO));
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("/api/information/edit")]
        public IActionResult EditInformation([FromBody] InformationDTO informationDTO)
        {
            _logger.LogInformation("Executing EditInformation controller");

            return Ok(_informationsService.EditInformation(informationDTO));
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("/api/information/delete/{informationId}")]
        public IActionResult DeleteInformation(string informationId)
        {
            _logger.LogInformation("Executing DeleteInformation controller");

            return Ok(_informationsService.DeleteInformation(informationId));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/information/getAll/{userId}")]
        public IActionResult GetAllByUser(string userId)
        {
            _logger.LogInformation("Executing GetAllByUser controller");

            return Ok(_informationsService.GetAllByUser(userId));
        }
    }
}
