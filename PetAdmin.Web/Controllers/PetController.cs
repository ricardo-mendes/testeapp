using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PetAdmin.Web.Filters;
using PetAdmin.Web.Services.Identity;
using PetAdmin.Web.Services;
using System.Dynamic;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PetController : BaseController
    {
        private readonly RepositoryBase<Pet> _repository;
        private readonly RepositoryBase<Vaccine> _vaccineRepository;
        private readonly IUserApplication _userApplication;
        private readonly PetService _petService;

        public PetController(
            UnitOfWork unitOfWork,
            RepositoryBase<Pet> repository,
            RepositoryBase<Vaccine> vaccineRepository,
            IUserApplication userApplication,
            PetService petService)
            : base(unitOfWork)
        {
            _repository = repository;
            _vaccineRepository = vaccineRepository;
            _userApplication = userApplication;
            _petService = petService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var petDto = PetMapper.EntityToDto(_repository.GetById(id));

            if (petDto.PetLoverId != Convert.ToInt64(_userApplication.GetPetLoverId()))
                return Unauthorized();

            return Ok(petDto);
        }

        [HttpGet("getallbypetloverid/{petLoverId}")]
        [ValidatePetLoverIdFilter]
        public ICollection<PetDto> GetAllByEmployeeId(long petLoverId)
        {
            var petList = _repository.GetAll(p => p.PetLoverId == petLoverId, p => p.Name);
            var petDtoList = new List<PetDto>();

            if (petList.Any())
            {
                var petIdList = petList.Select(p => p.Id);
                var vaccineList = _vaccineRepository.GetAll(v => petIdList.Contains(v.PetId));

                foreach (var entity in petList)
                {
                    var dto = PetMapper.EntityToDto(entity);

                    if (vaccineList != null && vaccineList.Select(v => v.PetId).Contains(entity.Id))
                        dto.HasVaccine = true;

                    petDtoList.Add(dto);
                }
            }

            return petDtoList;
        }

        [HttpPost]
        [ValidatePetLoverIdFilter]
        public async Task<IActionResult> Post([FromBody]PetDto dto)
        {
            var response = _petService.Create(dto);
            return await Response(response, _petService.Notifications);
        }

        [HttpPut]
        [ValidatePetLoverIdFilter]
        public async Task<IActionResult> Put([FromBody]PetDto dto)
        {
            _petService.Update(dto);
            return await Response(null, _petService.Notifications);
        }

        [HttpPatch("DeletePhoto")]
        public async Task<IActionResult> DeletePhoto([FromBody] PetDto dto)
        {
            var pet = _repository.GetById(dto.Id);

            if (pet.PetLoverId != Convert.ToInt64(_userApplication.GetPetLoverId()))
                return Unauthorized();

            await _petService.DeletePhotoAsync(pet);

            return await Response(null, _petService.Notifications);
        }

        [HttpPatch("UpdatePhoto")]
        public async Task<IActionResult> UpdatePhoto([FromBody] PetDto dto)
        {
            var pet = _repository.GetById(dto.Id);

            if (pet.PetLoverId != Convert.ToInt64(_userApplication.GetPetLoverId()))
                return Unauthorized();

            _petService.UpdatePhoto(pet, dto.Photo);

            return await Response(null, _petService.Notifications);
        }
    }
}