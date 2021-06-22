using AutoMapper;
using BillManager.EF;
using BillManager.Interfaces;
using BillManager.Models.ModelsDTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillManager.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UsersService(ApplicationDbContext context, ILogger<UsersService> logger, IMapper mapper)
        {
            this._context = context;
            this._logger = logger;
            this._mapper = mapper;
        }

        public ResponseDTO EditUser(UserDTO userDTO)
        {
            _logger.LogInformation("Executing EditUser method");
            var user = _context.ApplicationUser.Where(u => u.Id == userDTO.Id).SingleOrDefault();

            if (user == null)
            {
                return new ResponseDTO()
                {
                    Code = 400,
                    Message = $"ApplicationUser with id {userDTO.Id} doesn't exist in db",
                    Status = "Error"
                };

            }
            user.IdPaid = userDTO.IsPaid;
            user.Email = userDTO.Mail;
            user.UserName = userDTO.Name;
            user.PasswordHash = userDTO.Password;
            user.PhoneNumber = userDTO.TelNumber;

            try
            {
                _context.ApplicationUser.Update(user);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new ResponseDTO() { Code = 400, Message = e.Message, Status = "Error" };
            }

            return new ResponseDTO() { Code = 200, Message = "Edit user success", Status = "Success" };
        }

        public UsersDTO GetAllUsers()
        {
            _logger.LogInformation("Executing GetAllUsers method");
            var users = _context.ApplicationUser.ToList();

            UsersDTO userDTO = new UsersDTO() { };
            userDTO.usersList = new List<UserDTO>();

            foreach(var u in users)
            {
                userDTO.usersList.Add(this._mapper.Map<UserDTO>(u));
            }
            return userDTO;
        }

        public ResponseAfterAutDTO GetIdAndRoleForUserById(string mail)
        {
            _logger.LogInformation("Executing GetIdAndRoleForUserById method");
            var user = _context.ApplicationUser.Where(u => u.Email == mail).SingleOrDefault();
            var roleId = _context.UserRoles.Where(r => r.UserId == user.Id).SingleOrDefault().RoleId;
            var roleName = _context.Roles.Where(n => n.Id == roleId).SingleOrDefault().Name;
            var isAdmin = (roleName == "Admin") ? true : false;

            return new ResponseAfterAutDTO
            {
                Code = 200,
                Message = "User logged",
                Status = "Success",
                IdUser = user.Id,
                Mail = mail,
                IsAdmin = isAdmin
            };
        }
    }
}
