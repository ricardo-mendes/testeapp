using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto.Base;
using PetAdmin.Web.Dto.Schedule;
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
    public class SchedulePetController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly SchedulePetRepository _repository;
        private readonly RepositoryBase<PetLover> _petLoverRepository;
        private readonly RepositoryBase<Pet> _petRepository;
        private readonly RepositoryBase<Schedule> _scheduleRepository;
        private readonly RepositoryBase<Employee> _employeeRepository;
        private readonly SchedulePetService _schedulePetService;
        private readonly EmailService _emailService;
        private readonly SchedulePetRepository _schedulePetRepository;
        private readonly PetLoverService _petLoverService;
        private readonly SchedulePetRangeService _schedulePetRangeService;
        private readonly IUserApplication _userApplication;
        private readonly RepositoryBase<ScheduleItemEmployee> _scheduleItemEmployeeRepository;

        public SchedulePetController(
            UnitOfWork unitOfWork,
            SchedulePetRepository repository,
            SchedulePetService schedulePetService,
            RepositoryBase<PetLover> petLoverRepository,
            RepositoryBase<Pet> petRepository,
            EmailService emailService,
            RepositoryBase<Employee> employeeRepository,
            RepositoryBase<Schedule> scheduleRepository,
            SchedulePetRepository schedulePetRepository,
            PetLoverService petLoverService,
            SchedulePetRangeService schedulePetRangeService,
            IUserApplication userApplication,
            RepositoryBase<ScheduleItemEmployee> scheduleItemEmployeeRepository,
            NotificationHandler notificationHandler)
            : base(unitOfWork, notificationHandler)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _schedulePetService = schedulePetService;
            _petLoverRepository = petLoverRepository;
            _petRepository = petRepository;
            _emailService = emailService;
            _employeeRepository = employeeRepository;
            _scheduleRepository = scheduleRepository;
            _schedulePetRepository = schedulePetRepository;
            _petLoverService = petLoverService;
            _schedulePetRangeService = schedulePetRangeService;
            _userApplication = userApplication;
            _scheduleItemEmployeeRepository = scheduleItemEmployeeRepository;
        }

        [HttpGet("getallforemployee")]
        public IActionResult GetAllForEmployee([FromQuery]ScheduleGetAllEmployee getAll)
        {
            //Autorização
            var clientId = _employeeRepository.GetById(getAll.EmployeeId).ClientId;

            if (clientId != _userApplication.GetClientId())
                return Unauthorized();
            //----------

            return Ok(_repository.GetAllScheduleEmployeeResult(getAll));
        }

        [HttpGet("getallbypetlover/{petLoverId}")]
        [ValidatePetLoverIdFilter]
        public ICollection<SchedulePetLoverResultDto> GetAllByPetLover(long petLoverId)
        {
            return _repository.GetAllByPetLoverId(petLoverId);
        }

        [HttpPost]
        [ValidateScheduleFilter]
        public async Task<IActionResult> Create([FromBody]SchedulePetDto dto)
        {
            if (IsUnauthorizedForCreateSchedulPet(dto))
                return Unauthorized();

            var schedulePet =_schedulePetService.CreateSchedulePet(dto);
           
            if (!_schedulePetService.Notifications.Any())
            {
                _unitOfWork.Commit();

                //Enviar Email
                var pet = _petRepository.GetById(dto.PetId.Value);
                var petLover = _petLoverRepository.GetById(dto.PetLoverId.Value);

                if (dto.Status == ScheduleStatusEnum.Confirmed)
                {
                    var schedulePetConfirmEmailDto = _schedulePetRepository.GetSchedulePetConfirmEmailDto(schedulePet.Id);
                    _emailService.SendEmailAfterConfirmedSchedule(schedulePetConfirmEmailDto);
                }   
                else
                    _emailService.SendEmailAfterRequestedSchedule(petLover, pet, petLover.UserId != null ? true : false);
                //------------
            }

            return await Response(null, _schedulePetService.Notifications);
        }

        [HttpPost("createRange")]
        public async Task<IActionResult> CreateRange([FromBody]SchedulePetRangeDto dto)
        {
            if (IsUnauthorizedForCreateSchedulPetRange(dto))
                return Unauthorized();

            _schedulePetRangeService.CreateRangeSchedulePet(dto);

            if (!_schedulePetRangeService.Notifications.Any())
                _unitOfWork.Commit();

            return await Response(null, _schedulePetRangeService.Notifications);
        }

        [HttpPost("CreateWithoutUser")]
        public async Task<IActionResult> CreateWithoutUser([FromBody]SchedulePetDto dto)
        {
            if (dto.PetLover.ClientId != _userApplication.GetClientId())
                return Unauthorized();

            if (!string.IsNullOrEmpty(dto.PetLover.Email) && !string.IsNullOrWhiteSpace(dto.PetLover.Email) 
                && _petLoverService.EmailIsRegistered(dto.PetLover.Email, dto.PetLover.ClientId.GetValueOrDefault()))
            return await Response(null, _petLoverService.Notifications);

            if (dto.ScheduleItemEmployeeId == null)
            {
                RaisError("Informe o serviço");
                return await Response(null);
            }
               
            // Cadastra o PetLover
            var petLover = PetLoverMapper.DtoToEntity(dto.PetLover, null, false);
            _petLoverRepository.Add(petLover);

            // Cadastra o Pet
            dto.Pet.PetLoverId = petLover.Id;
            var pet = PetMapper.DtoToEntity(dto.Pet);
            _petRepository.Add(pet);

            // Faz o agendamento
            dto.PetLoverId = petLover.Id;
            dto.PetId = pet.Id;
            var schedulePet = _schedulePetService.CreateSchedulePet(dto);

            if (!_schedulePetService.Notifications.Any())
            {
                _unitOfWork.Commit();

                //Enviar Email
                var schedulePetConfirmEmailDto = _schedulePetRepository.GetSchedulePetConfirmEmailDto(schedulePet.Id);
                _emailService.SendEmailAfterConfirmedSchedule(schedulePetConfirmEmailDto);
                //------------
            }

            return await Response(null, _schedulePetService.Notifications);
        }

        //Usado apenas pelo PetLover
        [HttpPatch("availablestatus")]
        public async Task<IActionResult> AvailableStatus([FromBody]SchedulePetLoverResultDto dto)
        {
            if (_userApplication.GetPetLoverId() == 0)
                return Unauthorized();
   
            //Autorização
            var petLoverId = _userApplication.GetPetLoverId();
            var petLoverIdOfSchedulePet = _schedulePetRepository.GetById(dto.SchedulePetId)?.PetLoverId;
            if (petLoverIdOfSchedulePet.HasValue)
            {
                if (Convert.ToInt64(petLoverId) != petLoverIdOfSchedulePet.Value)
                    return Unauthorized();
            }
            //----------

            _schedulePetService.UpdateToAvailableStatus(dto);
            _unitOfWork.Commit();

            return await Response(null, _schedulePetService.Notifications);
        }

        [HttpPatch("cancelstatus")]
        public async Task<IActionResult> CancelStatus([FromBody]SchedulePetLoverResultDto dto)
        {
            if (IsUnauthorizedForChangeStatus(dto))
                return Unauthorized();

            _schedulePetService.UpdateToAvailableStatus(dto);

            if (!_schedulePetService.Notifications.Any())
            {
                var schedulePetCancelEmailDto = _schedulePetRepository.GetSchedulePetCancelEmailDto(dto.SchedulePetId);

                _unitOfWork.Commit();

                if (schedulePetCancelEmailDto != null)
                    _emailService.SendEmailAfterCanceledSchedule(schedulePetCancelEmailDto);
            }

            return await Response(null, _schedulePetService.Notifications);
        }

        [HttpPatch("confirmedstatus")]
        public async Task<IActionResult> Confirmedstatus([FromBody]SchedulePetDto dto)
        {
            if (IsUnauthorizedForChangeStatus(dto))
                return Unauthorized();

            _schedulePetService.UpdateToConfirmedStatus(dto);

            if (!_schedulePetService.Notifications.Any())
            {
                _unitOfWork.Commit();

                var schedulePetConfirmEmailDto = _schedulePetRepository.GetSchedulePetConfirmEmailDto(dto.Id);
                if (schedulePetConfirmEmailDto != null)
                    _emailService.SendEmailAfterConfirmedSchedule(schedulePetConfirmEmailDto);
            }

            return await Response(null, _schedulePetService.Notifications);
        }

        [HttpPatch("completedstatus")]
        public async Task<IActionResult> CompletedStatus([FromBody]SchedulePetDto schedulePetDto)
        {
            //if (IsUnauthorizedForChangeStatus(schedulePetDto))
            //    return Unauthorized();

            _schedulePetService.UpdateToCompletedStatus(schedulePetDto);

            if (!_schedulePetService.Notifications.Any())
            {
                _unitOfWork.Commit();

                var schedulePetCompleteEmailDto = _schedulePetRepository.GetSchedulePetCompletedEmailDto(schedulePetDto.Id);
                if (schedulePetCompleteEmailDto != null)
                    _emailService.SendEmailAfterCompletedSchedule(schedulePetCompleteEmailDto);
            }

            return await Response(null, _schedulePetService.Notifications);
        }

        //Não está sendo usado no Front
        [HttpGet("getallbypet/{petId}")]
        public ICollection<SchedulePetDto> GetAllByPet(long petId)
        {
            var includes = new List<Expression<Func<SchedulePet, object>>>
            {
                c => c.Pet,
                c => c.PetLover,
                c => c.Schedule
            };

            var entityList = _repository.GetAll(s => s.PetId == petId, s => s.Schedule.DateWithHour, includes);
            var dtoList = new List<SchedulePetDto>();

            entityList.ToList().ForEach(entity => dtoList.Add(SchedulePetMapper.EntityToDto(entity)));

            return dtoList;
        }

        //--------------------------------

        private bool IsUnauthorizedForCreateSchedulPet(SchedulePetDto dto)
        {
            if (_userApplication.GetPetLoverId() == 0 && _userApplication.GetClientId() == 0)
                return true;

            if (_userApplication.GetPetLoverId() > 0)
            {
                var petLoverIdOfPet = _petRepository.GetById(dto.PetId.GetValueOrDefault()).PetLoverId;

                var petLoverIdLogged = _userApplication.GetPetLoverId();
                if (petLoverIdOfPet != petLoverIdLogged)
                    return true;
            }
            else if (_userApplication.GetClientId() > 0)
            {
                var petLover = _petLoverRepository.GetById(dto.PetLoverId.GetValueOrDefault());

                if (petLover != null)
                {
                    var clientIdLogged = _userApplication.GetClientId();
                    if (petLover.ClientId != clientIdLogged)
                        return true;
                }

                var petLoverIdOfPet = _petRepository.GetById(dto.PetId.GetValueOrDefault()).PetLoverId;

                if (petLoverIdOfPet != dto.PetLoverId)
                    return true;
            }

            return false;
        }

        private bool IsUnauthorizedForCreateSchedulPetRange(SchedulePetRangeDto dto)
        {
            if (_userApplication.GetClientId() == 0)
                return true;
            
            var petLover = _petLoverRepository.GetById(dto.PetLoverId);

            if (petLover != null)
            {
                var clientIdLogged = _userApplication.GetClientId();
                if (petLover.ClientId != clientIdLogged)
                    return true;
            }

            var petLoverIdOfPet = _petRepository.GetById(dto.PetId).PetLoverId;

            if (petLoverIdOfPet != dto.PetLoverId)
                return true;
 
            return false;
        }

        private bool IsUnauthorizedForChangeStatus(SchedulePetLoverResultDto dto)
        {
            var schedulePet = _repository.GetById(dto.SchedulePetId);
            var employeeId = _scheduleItemEmployeeRepository.GetById(schedulePet.ScheduleItemEmployeeId.GetValueOrDefault()).EmployeeId;
            var clientId = _employeeRepository.GetById(employeeId).ClientId;

            if (clientId != _userApplication.GetClientId())
                return true;

            return false;
        }

        private bool IsUnauthorizedForChangeStatus(SchedulePetDto dto)
        {
            var schedulePet = _repository.GetById(dto.Id);
            var employeeId = _scheduleItemEmployeeRepository.GetById(schedulePet.ScheduleItemEmployeeId.GetValueOrDefault()).EmployeeId;
            var clientId = _employeeRepository.GetById(employeeId).ClientId;

            if (clientId != _userApplication.GetClientId())
                return true;

            return false;
        }
    }
}