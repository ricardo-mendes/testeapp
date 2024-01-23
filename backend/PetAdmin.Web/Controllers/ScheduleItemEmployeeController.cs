using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Services.Identity;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ScheduleItemEmployeeController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RepositoryBase<ScheduleItemEmployee> _repository;
        private readonly RepositoryBase<Employee> _employeeRepository;
        private readonly IUserApplication _userApplication;

        public ScheduleItemEmployeeController(
            UnitOfWork unitOfWork,
            RepositoryBase<ScheduleItemEmployee> repository,
            RepositoryBase<Employee> employeeRepository,
             IUserApplication userApplication)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _userApplication = userApplication;
            _employeeRepository = employeeRepository;
        }

        //Não usado
        [HttpGet("getallbyemployeeandscheduleitem")]
        public ICollection<ScheduleItemEmployeeDto> GetAllByEmployeeAndScheduleItem([FromQuery] ScheduleItemEmployeeDto dto)
        {
            var includes = new List<Expression<Func<ScheduleItemEmployee, object>>>
            {
                s => s.ScheduleItem
            };

            var entityList = _repository
                .GetAll(c => c.ScheduleItemId == dto.ScheduleItemId && c.EmployeeId == dto.EmployeeId && c.IsActive, 
                c => c.Id , includes);

            var dtoList = new List<ScheduleItemEmployeeDto>();

            foreach (var entity in entityList)
            {
                dtoList.Add(ScheduleItemEmployeeMapper.EntityToDto(entity));
            }

            return dtoList;
        }

        //Usado para mostrar os dados do serviço do employee quando o petlover escolher um petshop
        [HttpGet("getallbyemployee")]
        public ICollection<ScheduleItemEmployeeDto> GetAllByEmployee([FromQuery] long employeeId)
        {
            var includes = new List<Expression<Func<ScheduleItemEmployee, object>>>
            {
                s => s.ScheduleItem
            };

            var entityList = _repository
                .GetAll(c => c.EmployeeId == employeeId && c.IsActive,
                c => c.Id, includes);

            var dtoList = new List<ScheduleItemEmployeeDto>();

            foreach (var entity in entityList)
            {
                dtoList.Add(ScheduleItemEmployeeMapper.EntityToDto(entity));
            }

            return dtoList;
        }

        [HttpGet("getallbyemployeewithinactive")]
        public ICollection<ScheduleItemEmployeeDto> GetAllByEmployeeWithInactive([FromQuery] long employeeId)
        {
            var entityList = _repository
                .GetAll(c => c.EmployeeId == employeeId,
                c => c.Id);

            var dtoList = new List<ScheduleItemEmployeeDto>();

            foreach (var entity in entityList)
            {
                dtoList.Add(ScheduleItemEmployeeMapper.EntityToDto(entity));
            }

            return dtoList;
        }

        //Não Usado
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ScheduleItemEmployeeDto dto)
        {
            var entity = _repository.GetById(dto.Id);
            entity = ScheduleItemEmployeeMapper.DtoToEntity(dto, entity);
            _repository.Update(entity);
         
            _unitOfWork.Commit();

            return await Response(null, null);
        }

        [HttpPut("updatelist")]
        public async Task<IActionResult> UpdateList([FromBody] ICollection<ScheduleItemEmployeeDto> dtoList)
        {
            var clientId = _userApplication.GetClientId();

            if (clientId == 0)
                return Unauthorized();

            var employeeId = _employeeRepository.GetAll(e => e.ClientId == clientId).FirstOrDefault().Id;

            //------------

            var scheduleItemEmployeeNewDtoList = dtoList.Where(e => e.Id == 0);
            var scheduleItemEmployeeNewList = new List<ScheduleItemEmployee>();

            foreach (var dto in scheduleItemEmployeeNewDtoList)
            {
                dto.EmployeeId = employeeId;
                scheduleItemEmployeeNewList.Add(ScheduleItemEmployeeMapper.DtoToEntity(dto));
            }
                
            if (scheduleItemEmployeeNewList.Any())
                _repository.AddRange(scheduleItemEmployeeNewList);

            //------------

            var scheduleItemEmployeeExistingList = _repository.GetAll(s => dtoList.Where(e => e.Id != 0).Select(d => d.Id).Contains(s.Id));

            foreach (var scheduleItemEmployee in scheduleItemEmployeeExistingList)
            {
                if (scheduleItemEmployee.EmployeeId != employeeId)
                    return Unauthorized();

                var dto = dtoList.FirstOrDefault(d => d.Id == scheduleItemEmployee.Id);
                ScheduleItemEmployeeMapper.DtoToEntity(dto, scheduleItemEmployee);
            }

            if (scheduleItemEmployeeExistingList.Any())
                _repository.UpdateRange(scheduleItemEmployeeExistingList.ToList());

            //------------

            _unitOfWork.Commit();




            return await Response(null, null);
        }
    }
}