using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillManager.Interfaces;
using BillManager.Models.ModelsDTO;
using BillManager.EF;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BillManager.Models;

namespace BillManager.Services
{
    public class BillsService : IBillsService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public BillsService(ApplicationDbContext _context, ILogger<BillsService> _logger, IMapper _mapper)
        {
            this.context = _context;
            this.logger = _logger;
            this.mapper = _mapper;
        }

        public ResponseDTO AddBill(BillDTO billDTO)
        {
            logger.LogInformation("Executing AddBill method");
            try
            {
                var bill = mapper.Map<Bill>(billDTO);
                context.Bill.Add(bill);                
                context.SaveChanges();
            }
            catch(Exception e)
            {
                return new ResponseDTO() { Code = 404, Message = e.Message, Status = "Error during bill adding" };
            }

            return new ResponseDTO { Code = 200, Message = "Bill added to db", Status = "Success" };
        }

        public ResponseDTO EditBill(BillDTO billDTO)
        {
            logger.LogInformation("Executing EditBill method");

            if(context.Bill.Where(b => b.Name == billDTO.Name).Count() == 0)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = $"Bill with id {billDTO.Name} doesn't exist in db",
                    Status = "Error"
                };
            }
            try
            {
                var bill = context.Bill.Update(mapper.Map<Bill>(billDTO));
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
                Message = "Edited bill in db",
                Status = "Success"
            };  
        }
        public ResponseDTO DeleteBill(string mail)
        {
            logger.LogInformation("Executing method DeleteBill");
            var billToRemove = context.Bill.Where(b => b.User.Email == mail).SingleOrDefault();
            if(billToRemove == null)
            {
                return new ResponseDTO() { Code = 400, Message = "Bill doesn't exist!", Status = "Error" };
            }
            try
            {
                context.Bill.Remove(billToRemove);
            }
            catch (Exception e)
            {
                return new ResponseDTO() { Code = 400, Message = e.Message, Status = "Error" };
            }
            return new ResponseDTO() { Code = 200, Message = "Bill deleted", Status = "Success" };
        }

        public BillsDTO GetAllBillByUser(string email)
        {
            logger.LogInformation("Executing GetAllBillByUser");
            List<Bill> billList = context.Bill.Where(b => b.User.Email == email).ToList();

            BillsDTO billsDTO = new BillsDTO() { };
            billsDTO.billList = new List<BillDTO>();

            foreach(Bill bill in billList)
            {
                billsDTO.billList.Add(mapper.Map<BillDTO>(bill));
            }
            billsDTO.billList = billsDTO.billList.OrderByDescending(o => o.Year).ToList();
            return billsDTO;
        }
    }
}
