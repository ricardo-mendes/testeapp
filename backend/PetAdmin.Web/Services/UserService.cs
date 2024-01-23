using FluentValidator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Models.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PetAdmin.Web.Services
{
    public class UserService : Notifiable
    {
        private readonly PetContext _petContext;
        private readonly TokenService _tokenService;
        private readonly RepositoryBase<Pet> _petRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly NotificationHandler _notificationHandler;

        public UserService(PetContext petContext,
            TokenService tokenService,
            RepositoryBase<Pet> petRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            NotificationHandler notificationHandler)
        {
            _petContext = petContext;
            _tokenService = tokenService;
            _petRepository = petRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationHandler = notificationHandler;
        }

        public User Create(ClientDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                UserTypeId = (int)UserTypeEnum.Client,
                IsActive = true,
                Email = dto.Email,
                UserName = dto.Email,
                EmailConfirmed = true,
                CreationTime = DateTime.Now,
            };

            var result = _userManager.CreateAsync(user, dto.User.Password).Result;

            if (!result.Succeeded)
            {
                var errorMessage = string.Empty;

                foreach (var error in result.Errors)
                {
                    if (errorMessage == string.Empty)
                        errorMessage += error.Description;
                    else
                        errorMessage += " / " + error.Description;
                }

                AddNotification("User", errorMessage);
            }

            return result.Succeeded ? user : null;
        }

        public User Create(PetLoverDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                UserTypeId = (int)UserTypeEnum.PetLover,
                IsActive = true,
                Email = dto.Email,
                UserName = dto.Email,
                EmailConfirmed = true,
                CreationTime = DateTime.Now,
            };

            var result = _userManager.CreateAsync(user, dto.User.Password).Result;

            if (!result.Succeeded)
            {
                var errorMessage = string.Empty;

                foreach (var error in result.Errors)
                {
                    if (errorMessage == string.Empty)
                        errorMessage += error.Description;
                    else
                        errorMessage += " / " + error.Description;
                }

                AddNotification("User", errorMessage);
            }

            return result.Succeeded ? user : null;
        }

        public UserAuth Login(string userName, string password)
        {
            UserAuth userAuth = new UserAuth();

            var userEntity = _petContext.Users
                .FirstOrDefault(u => u.UserName == userName);

            if (userEntity == null)
            {
                AddNotification("User", "Email ou Senha Incorretos");
                return userAuth;
            }

            if (userEntity.UserTypeId == (int)UserTypeEnum.Client)
            {
                userEntity = _petContext.Users
                    .Where(u => u.UserName == userName)
                    .Include(u => u.ClientList)
                        .ThenInclude(c => c.Location)
                    .Include(u => u.ClientList)
                        .ThenInclude(c => c.EmployeeList)
                    .FirstOrDefault();
            }
            else
            {
                userEntity = _petContext.Users
                    .Where(u => u.UserName == userName)
                    .Include(u => u.PetLoverList)
                        .ThenInclude(c => c.Location)
                    .FirstOrDefault();
            }

            if (userEntity != null)
            {
                var result = _signInManager.PasswordSignInAsync(userName, password, false, true).Result;

                if (result.Succeeded)
                {
                    userAuth = _tokenService.BuildUserAuthObject(userEntity);
                    userAuth.UserLogin = userEntity.UserName;
                    userAuth.UserId = userEntity.Id;
                    userAuth.Client = userEntity.ClientList != null && userEntity.ClientList.Any() ? ClientMapper.EntityToDto(userEntity.ClientList.FirstOrDefault()) : null;
                    userAuth.PetLover = userEntity.PetLoverList != null && userEntity.PetLoverList.Any() ? PetLoverMapper.EntityToDto(userEntity.PetLoverList.FirstOrDefault()) : null;
                }
                else if (result.IsLockedOut)
                {
                    AddNotification("User,", "Usuário temporariamente bloqueado por tentativas inválidas");
                    return userAuth;
                }
                else
                {
                    AddNotification("User", "Email ou Senha Incorretos");
                    return userAuth;
                }
            }
            else
            {
                AddNotification("User", "Email ou Senha Incorretos");
                return userAuth;
            }

            if (userAuth.PetLover != null)
            {
                var petList = _petRepository.GetAll(p => p.PetLoverId == userAuth.PetLover.Id);
                foreach (var pet in petList)
                    userAuth.PetLover.PetList.Add(PetMapper.EntityToDto(pet));
            }

            return userAuth;
        }

        public bool EmailIsRegistered(string userName)
        {
            if (_petContext.Users.Where(u => u.UserName == userName || u.Email == userName).Any())
            {
                AddNotification("Email", "Email já está cadastrado");
                return true;
            }

            return false;
        }

        //----------------------------

        public UserAuth ChangePassword(string userName, string currentPassword, string newPassword)
        {
            var userAuth = Login(userName, currentPassword);

            if (userAuth.IsAuthenticated)
            {
                var userEntity = _petContext.Users
                .FirstOrDefault(u => u.UserName == userName);

                var token = _userManager.GeneratePasswordResetTokenAsync(userEntity).Result;
                var resetPassResult = _userManager.ResetPasswordAsync(userEntity, token, newPassword).Result;

                if (!resetPassResult.Succeeded)
                    AddNotification("User", "Erro ao trocar ");
            }

            return userAuth;
        }

        public string SetNewPassword(string email)
        {
            var user = _petContext.Users.FirstOrDefault(u => u.UserName == email);

            if (user == null)
            {
                AddNotification("User", "Usuário não foi encontrado");
                return string.Empty;
            }

            string newPassword = GeneratePassword(3, 3, 3);

            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var resetPassResult = _userManager.ResetPasswordAsync(user, token, newPassword).Result;

            if (resetPassResult.Succeeded)
                return newPassword;
            else
            {
                AddNotification("User", "Não foi possível trocar a senha");
                return string.Empty;
            }  
        }

        private string GeneratePassword(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);
        }
    }
}
