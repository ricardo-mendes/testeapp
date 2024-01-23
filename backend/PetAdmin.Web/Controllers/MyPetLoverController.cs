using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Dto.Base;
using PetAdmin.Web.Dto.PetLover;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Filters;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Services;
using PetAdmin.Web.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MyPetLoverController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly PetLoverRepository _repository;
        private readonly RepositoryBase<SchedulePet> _schedulePetRepository;
        private readonly RepositoryBase<Pet> _petRepository;
        private readonly UserService _userService;
        private readonly MyPetLoverService _myPetLoverService;
        private readonly PetLoverService _petLoverService;
        private readonly IUserApplication _userApplication;

        public MyPetLoverController(
            UserService userService,
            UnitOfWork unitOfWork,
            PetLoverRepository repository,
            RepositoryBase<SchedulePet> schedulePetRepository,
            RepositoryBase<Pet> petRepository,
            MyPetLoverService myPetLoverService,
            PetLoverService petLoverService,
            IUserApplication userApplication)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _userService = userService;
            _schedulePetRepository = schedulePetRepository;
            _petRepository = petRepository;
            _myPetLoverService = myPetLoverService;
            _petLoverService = petLoverService;
            _userApplication = userApplication;
        }

        [HttpGet("{id}")]
        public ActionResult Get(long id)
        {
            var includes = new List<Expression<Func<PetLover, object>>>
            {
                c => c.PetList,
                c => c.Location
            };

            var petLover = _repository.GetById(id, includes);

            //Autorização
            if (petLover.ClientId != _userApplication.GetClientId())
                return Unauthorized();
            //----------

            return Ok(PetLoverMapper.EntityToDto(petLover));
        }

        [HttpGet("schedules/{petLoverId}")]
        public ActionResult GetAllSchedules(long petLoverId)
        {
            //Autorização
            var petLover = _repository.GetById(petLoverId);

            if (petLover.ClientId != _userApplication.GetClientId())
                return Unauthorized();
            //----------

            var includes = new List<Expression<Func<SchedulePet, object>>>
            {
                c => c.Schedule,
                c => c.Pet
            };

            DateTime utcNow = DateTime.UtcNow;
            DateTime today = DateTime.Now.Date;

            try
            {
                TimeZoneInfo brazilianHour = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                today = TimeZoneInfo.ConvertTimeFromUtc(utcNow, brazilianHour).Date;
            }
            catch (Exception)
            {
            }
            
            var schedulePetList = _schedulePetRepository
                .GetAll(s => s.PetLoverId == petLoverId && s.Schedule.Date.Date >= today , s => s.Schedule.Date, includes);

            var resultList = new List<MyPetLoverScheduleResultDto>();

            foreach (var schedulePet in schedulePetList)
            {
                var result = new MyPetLoverScheduleResultDto
                {
                    DateWithHour = schedulePet.Schedule.DateWithHour,
                    PetName = schedulePet.Pet.Name,
                    Status = schedulePet.Status
                };

                resultList.Add(result);
            }

            return Ok(resultList);
        }

        [HttpPost]
        [ValidateClientIdFilter]
        public async Task<IActionResult> Post([FromBody]PetLoverDto dto)
        {
            if (_petLoverService.EmailIsRegistered(dto.Email, dto.ClientId.GetValueOrDefault()))
                return await Response(null, _petLoverService.Notifications);

            var petLover = PetLoverMapper.DtoToEntity(dto, null, false);

            _repository.Add(petLover);
            _unitOfWork.Commit();

            return await Response(null, _userService.Notifications);
        }

        [HttpGet("client/{clientId}")]
        [ValidateClientIdFilter]
        public ICollection<MyPetLoverResultDto> GetAll(long clientId)
        {
            return _repository.GetAllMyPetLoverResultDtoByClientId(clientId);
        }

        [HttpPut]
        [ValidateClientIdFilter]
        public async Task<IActionResult> Put([FromBody]PetLoverDto dto)
        {
            if (dto.Id == default(long))
                return NotFound();

            // Update PetLover
            var includes = new List<Expression<Func<PetLover, object>>>
            {
                c => c.Location
            };

            var petLover = _repository.GetById(dto.Id, includes);

            if (petLover.ClientId != _userApplication.GetClientId())
                return Unauthorized();

            if (petLover == null)
                return NotFound();

            if (_petLoverService.EmailIsRegistered(dto.Email, dto.ClientId.GetValueOrDefault(), petLover.Id))
                return await Response(null, _petLoverService.Notifications);

            petLover = PetLoverMapper.DtoToEntity(dto, petLover);
            _repository.Update(petLover);

            // Update Pets 
            var petIdList = dto.PetList.Where(p => p.Id != default(long)).Select(p => p.Id);
            var petExistingList = _petRepository.GetAll(p => petIdList.Contains(p.Id), p => p.Id);

            foreach (var petExisting in petExistingList)
            {
                if (petExisting.PetLoverId != petLover.Id)
                    return Unauthorized();

                petExisting.PetLoverId = petLover.Id;
                PetMapper.DtoToEntity(dto.PetList.FirstOrDefault(p => p.Id == petExisting.Id), petExisting);
            }
                
            if (petExistingList.Any())
                _petRepository.UpdateRange(petExistingList.ToList());

            // Create Pets
            var petNewDtoList = dto.PetList.Where(p => p.Id == default(long));
            var petList = new List<Pet>();

            foreach (var petDto in petNewDtoList)
            {
                petDto.PetLoverId = petLover.Id;
                petList.Add(PetMapper.DtoToEntity(petDto));
            }
                
            if (petList.Any())
                _petRepository.AddRange(petList);

            // Commit
            _unitOfWork.Commit();

            return await Response(null, _userService.Notifications);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var petLover = _repository.GetById(id);

            //Autorização
            if (petLover.ClientId != _userApplication.GetClientId())
                return Unauthorized();
            //----------

            if (_myPetLoverService.CanNotDelete(id))
                return await Response(null, _myPetLoverService.Notifications);

            var petList = _petRepository.GetAll(p => p.PetLoverId == id, p => p.Id)
                .ToList();

            _petRepository.DeleteRange(petList);
            _repository.Delete(petLover);
            _unitOfWork.Commit();

            return await Response(null, _userService.Notifications);
        }
    }
}