using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Services.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PetAdmin.Web.Services;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class VaccineController : BaseController
    {
        private readonly RepositoryBase<Vaccine> _repository;
        private readonly RepositoryBase<Pet> _petRepository;
        private readonly IUserApplication _userApplication;
        private readonly VaccineService _vaccineService;

        public VaccineController(
            UnitOfWork unitOfWork,
            RepositoryBase<Vaccine> repository,
            RepositoryBase<Pet> petRepository,
            IUserApplication userApplication,
            VaccineService vaccineService)
            : base(unitOfWork)
        {
            _petRepository = petRepository;
            _repository = repository;
            _userApplication = userApplication;
            _vaccineService = vaccineService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var vaccine = VaccineMapper.EntityToDto(_repository.GetById(id));

            if (PetLoverIsNotAuthorized(vaccine.PetId))
                return Unauthorized();

            return Ok(vaccine);
        }

        [HttpGet("pet/{petId}")]
        public IActionResult GetAllByPetId(long petId)
        {
            var vaccineList = _repository.GetAll(v => v.PetId == petId);

            //Autorização
            var petList = _petRepository.GetAll(p => vaccineList.Select(v => v.PetId).Contains(p.Id));
            var petLoverIdList = petList.Select(p => p.PetLoverId);
            if (petLoverIdList.Any(petLoverId => petLoverId != Convert.ToInt64(_userApplication.GetPetLoverId())))
                return Unauthorized();
            //----------

            var dtoList = new List<VaccineDto>();

            foreach (var entity in vaccineList)
            {
                dtoList.Add(VaccineMapper.EntityToDto(entity));
            }

            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]VaccineDto dto)
        {
            if (PetLoverIsNotAuthorized(dto.PetId))
                return Unauthorized();

            var response = _vaccineService.Create(dto);

            return await Response(response, _vaccineService.Notifications);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]VaccineDto dto)
        {
            var vaccine = _repository.GetById(dto.Id);

            if (PetLoverIsNotAuthorized(vaccine.PetId))
                return Unauthorized();

            vaccine = VaccineMapper.DtoToEntity(dto, vaccine);
            _repository.Update(vaccine);

            return await Response(null, _vaccineService.Notifications);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var vaccine = _repository.GetById(id);

            if (PetLoverIsNotAuthorized(vaccine.PetId))
                return Unauthorized();

            _repository.Delete(vaccine);

            return await Response(null, null);
        }

        [HttpPatch("DeletePhoto")]
        public async Task<IActionResult> DeletePhoto([FromBody] VaccineDto dto)
        {
            var vaccine = _repository.GetById(dto.Id);

            if (PetLoverIsNotAuthorized(vaccine.PetId))
                return Unauthorized();

            await _vaccineService.DeletePhotoAsync(vaccine);

            return await Response(null, _vaccineService.Notifications);
        }

        [HttpPatch("UpdatePhoto")]
        public async Task<IActionResult> UpdatePhoto([FromBody] VaccineDto dto)
        {
            var vaccine = _repository.GetById(dto.Id);

            if (PetLoverIsNotAuthorized(vaccine.PetId))
                return Unauthorized();

            _vaccineService.UpdatePhotoUrl(vaccine, dto.Photo);

            return await Response(null, _vaccineService.Notifications);
        }

        private bool PetLoverIsNotAuthorized(long petId)
        {
            var petLoverid = _petRepository.GetById(petId).PetLoverId;
            return petLoverid != Convert.ToInt64(_userApplication.GetPetLoverId());
        }
    }
}