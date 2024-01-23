using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserService _userService;
        private readonly EmailService _emailService;

        public UserController(UserService userService,
            UnitOfWork unitOfWork,
            EmailService emailService)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserDto dto)
        {
            var userAuth = _userService.Login(dto.UserLogin, dto.Password);
            return await Response(userAuth, _userService.Notifications);
        }

        [HttpPatch("generatenewpassword/{email}")]
        public async Task<IActionResult> GenerateNewPassword(string email)
        {
             var newPassword = _userService.SetNewPassword(email);

            if (!_userService.Notifications.Any())
            {
                _unitOfWork.Commit();

                //Enviar Email
                _emailService.SendEmailWithNewPassword(email, newPassword);
                //------------
            }

            return await Response(null, _userService.Notifications);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]UserChangeDto dto)
        {
            var userAuth = _userService.ChangePassword(dto.UserLogin, dto.CurrentPassword, dto.NewPassword);

            return await Response(userAuth, _userService.Notifications);
        }
    }
}