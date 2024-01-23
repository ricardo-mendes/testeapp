using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto.Base;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Services;
using PetAdmin.Web.Services.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ScheduleController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RepositoryBase<Schedule> _repository;
        private readonly ScheduleService _scheduleService;
        private readonly RepositoryBase<Employee> _employeeRepository;
        private readonly IUserApplication _userApplication;

        public ScheduleController(
            UnitOfWork unitOfWork,
            RepositoryBase<Schedule> repository,
            ScheduleService scheduleService,
            RepositoryBase<Employee> employeeRepository,
            IUserApplication userApplication)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _scheduleService = scheduleService;
            _employeeRepository = employeeRepository;
            _userApplication = userApplication;
        }

        [HttpPost("daterange")]
        public IActionResult DateRange([FromBody]SchedulePostDto schedulePostDto)
        {
            //Autorização
            var clientId = _employeeRepository.GetById(schedulePostDto.EmployeeId).ClientId;
            if (clientId != _userApplication.GetClientId())
                return Unauthorized();
            //----------

            _scheduleService.AddDateRange(schedulePostDto);

            if ((_scheduleService.Notifications != null && !_scheduleService.Notifications.Any()) || _scheduleService.Notifications == null)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    errors = _scheduleService.Notifications
                });
        }

        //Retorna os horários para o petlover escolher
        [HttpGet("getallforemployee")]
        public BaseListDto<ScheduleDto> GetAllForEmployee([FromQuery]ScheduleGetAllEmployee getAll)
        {
            return _scheduleService.GetAllByEmployeeId(getAll);
        }

        [HttpPatch("availablestatus")]
        public async Task<IActionResult> AvailableStatus([FromBody]ScheduleDto scheduleDto)
        {
            var schedule = _repository.GetById(scheduleDto.Id);

            //Autorização
            var clientId = _employeeRepository.GetById(schedule.EmployeeId).ClientId;
            if (clientId != _userApplication.GetClientId())
                return Unauthorized();
            //-----------

            _scheduleService.UpdateToAvailableStatus(schedule, scheduleDto);
            _unitOfWork.Commit();

            return await Response(null, _scheduleService.Notifications);
        }

        [HttpPatch("blockedstatus")]
        public async Task<IActionResult> Blockedstatus([FromBody]ScheduleDto scheduleDto)
        {
            var schedule = _repository.GetById(scheduleDto.Id);

            //Autorização
            var clientId = _employeeRepository.GetById(schedule.EmployeeId).ClientId;
            if (clientId != _userApplication.GetClientId())
                return Unauthorized();
            //-----------

            _scheduleService.UpdateToBlockedStatus(schedule, scheduleDto);
            _unitOfWork.Commit();

            return await Response(null, _scheduleService.Notifications);
        }
    }
}