using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillManager.Interfaces;
using BillManager.EF;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BillManager.Models.ModelsDTO;
using BillManager.Models;

namespace BillManager.Services
{
    public class InformationsService : IInformationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public InformationsService(ApplicationDbContext context, ILogger<InformationsService> logger, IMapper mapper)
        {
            this._context = context;
            this._logger = logger;
            this._mapper = mapper;            
        }

        public ResponseDTO AddInformation(InformationDTO informationDTO)
        {
            _logger.LogInformation("Executing AddInformation method");
            try
            {
                _context.Information.Add(_mapper.Map<Information>(informationDTO));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = e.Message,
                    Status = "Error"
                };
            }
            return new ResponseDTO()
            {
                Code = 200,
                Message = "Added information to db",
                Status = "Success"
            };
        }

        public ResponseDTO EditInformation(InformationDTO informationDTO)
        {
            _logger.LogInformation("Executing EditInformation method");
            if(_context.Information.Where(i => i.Id == informationDTO.Id).Count() == 0)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = "Information doesn't exist in db",
                    Status = "Error"
                };
            }
            try
            {
                _context.Information.Update(_mapper.Map<Information>(informationDTO));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = e.Message,
                    Status = "Error"
                };
            }
            return new ResponseDTO()
            {
                Code = 200,
                Message = "Updated information",
                Status = "Success"
            };
        }

        public ResponseDTO DeleteInformation(string email)
        {
            _logger.LogInformation("Executing DeleteInformation method");
            var informationToRemove = _context.Information.Where(e => e.User.Email == email).SingleOrDefault();

            if(informationToRemove == null)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = $"Information about user with email {email} doesn't exist.",
                    Status = "Error"
                };
            }
            try
            {
                _context.Information.Remove(informationToRemove);
            }
            catch(Exception e)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = e.Message,
                    Status = "Error"
                };
            }
            return new ResponseDTO()
            {
                Code = 200,
                Message = "Information deleted from db.",
                Status = "Success"
            };
        }

        public InformationsDTO GetAllByUser(string mail)
        {
            var result = _context.Information.Where(m => m.User.Email == mail).ToList();

            InformationsDTO informationDTO = new InformationsDTO() { };
            informationDTO.informationList = new List<InformationDTO>();
            foreach(Information information in result)
            {
                informationDTO.informationList.Add(_mapper.Map<InformationDTO>(information));
            }
            return informationDTO;
        }
    }
}
