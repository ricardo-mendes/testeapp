using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Dto.Base;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Filters;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Localization;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PetLoverController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RepositoryBase<PetLover> _repository;
        private readonly RepositoryBase<SchedulePet> _schedulePetRepository;
        private readonly UserService _userService;
        private readonly LocationGoogleService _locationGoogleService;
        private readonly PetLoverLocationClientService _petLoverLocationClientService;

        public PetLoverController(
            UserService userService,
            UnitOfWork unitOfWork,
            RepositoryBase<PetLover> repository,
            RepositoryBase<SchedulePet> schedulePetRepository,
            LocationGoogleService locationGoogleService,
            PetLoverLocationClientService petLoverLocationClientService)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _userService = userService;
            _schedulePetRepository = schedulePetRepository;
            _locationGoogleService = locationGoogleService;
            _petLoverLocationClientService = petLoverLocationClientService;
        }

        [HttpGet("{id}")]
        [ValidatePetLoverIdFilter("id")]
        public PetLoverDto Get(long id)
        {
            var includes = new List<Expression<Func<PetLover, object>>>
            {
                c => c.PetList,
                c => c.Location
            };
            return PetLoverMapper.EntityToDto(_repository.GetById(id, includes));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]PetLoverDto dto)
        {
            if (_userService.EmailIsRegistered(dto.Email))
            {
                return await Response(null, _userService.Notifications);
            }

            using (var transaction = _repository.GetContext().Database.BeginTransaction())
            {
                //User
                var user = _userService.Create(dto);

                if (user == null)
                    return await Response(null, _userService.Notifications);

                //PetLover
                var petLover = PetLoverMapper.DtoToEntity(dto, user.Id);

                await _locationGoogleService.SetLatitudeAndLongitudeAsync(petLover.Location);
                if (_locationGoogleService.Invalid)
                    return await Response(null, _locationGoogleService.Notifications);

                _petLoverLocationClientService.CreateAndUpdateListByPetLover(petLover);

                _repository.Add(petLover);

                _unitOfWork.Commit();

                transaction.Commit();  
            }

            //var userAuth = _userService.Login(dto.Email, dto.User.Password);
            return await Response(null, _userService.Notifications);
        }

        [HttpPut]
        [ValidatePetLoverIdFilter("Id")]
        public async Task<IActionResult> Put([FromBody]PetLoverDto dto)
        {
            var includes = new List<Expression<Func<PetLover, object>>>
            {
                c => c.Location
            };

            var petLover = _repository.GetById(dto.Id, includes);
            petLover = PetLoverMapper.DtoToEntityWithoutEmail(dto, petLover);

            await _locationGoogleService.SetLatitudeAndLongitudeAsync(petLover.Location);
            if (_locationGoogleService.Invalid)
                return await Response(null, _locationGoogleService.Notifications);

            _repository.Update(petLover);

            _petLoverLocationClientService.CreateAndUpdateListByPetLover(petLover);

            _unitOfWork.Commit();

            return await Response(null, _userService.Notifications);
        }

        //NÃO USADO NO FRONT
        [HttpGet("getall")]
        public BaseListDto<PetLoverDto> GetAll([FromQuery]PetLoverGetAllDto getAll)
        {
            BaseListDto<PetLover> entityList = _repository.GetAll(c => c.IsActive, c => c.Name, getAll.Skip, getAll.Take);

            var dtoList = new BaseListDto<PetLoverDto>
            {
                Total = entityList.Total,
                Result = new List<PetLoverDto>()
            };

            foreach (var petLover in entityList.Result)
            {
                var dto = PetLoverMapper.EntityToDto(petLover);

                dto.IsClientOfPetShop = _schedulePetRepository
                        .GetAll(s => s.PetLoverId == petLover.Id && s.Schedule.EmployeeId == getAll.EmployeeId,
                        s => s.CreationTime,
                        getAll.Skip, getAll.Take).Result.Any();

                dtoList.Result.Add(dto);
            }

            return dtoList;
        }
    }
}