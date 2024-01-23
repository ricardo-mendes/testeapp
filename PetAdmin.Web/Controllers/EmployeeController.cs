using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : BaseController
    {
        private readonly RepositoryBase<Employee> _repository;

        public EmployeeController(
            UnitOfWork unitOfWork,
            RepositoryBase<Employee> repository)
            : base(unitOfWork)
        {
            _repository = repository;
        }

        //Não Usado
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmployeeDto dto)
        {
            var employee = EmployeeMapper.DtoToEntity(dto);
            var response = _repository.Add(employee);
            return await Response(response, null);
        }

        //Não Usado
        [HttpGet("{id}")]
        public EmployeeDto Get(long id)
        {
            return  EmployeeMapper.EntityToDto(_repository.GetById(id));
        }

        //Informações adicionais do petshop que o petlover marcou o agendamento
        [HttpGet("getbyidwithclient/{employeeId}")]
        public EmployeeDto GetByIdWithClient(long employeeId)
        {
            var includes = new List<Expression<Func<Employee, object>>>
            {
                c => c.Client
            };

            var employeeEntity = _repository.GetAll(p => p.Id == employeeId, p => p.Name, includes).FirstOrDefault();

            var employeeDto = EmployeeMapper.EntityToDto(employeeEntity, employeeEntity.Client);

            return employeeDto;
        }

        //Não usado
        [HttpGet("getallbyclientid/{clientId}")]
        public ICollection<EmployeeDto> GetAllByClientId(long clientId)
        {
            var employeeList = _repository.GetAll(p => p.ClientId == clientId, p => p.Name);
            var employeeDtoList = new List<EmployeeDto>();

            foreach (var entity in employeeList)
            {
                var dto = EmployeeMapper.EntityToDto(entity);
                employeeDtoList.Add(dto);
            }

            return employeeDtoList;
        }

        //Não usado
        [HttpGet("getallbyclientandscheduleitem")]
        public ICollection<EmployeeDto> GetAllByClientAndScheduleItem(long clientId, long scheduleItemId)
        {
            var includes = new List<Expression<Func<Employee, object>>>
            {
                c => c.ScheduleItemEmployeeList
            };

            var employeeList = _repository.GetAll(p => p.ClientId == clientId, p => p.Name, includes);

            var employeeDtoList = new List<EmployeeDto>();

            foreach (var entity in employeeList)
            {
                bool employeeHasScheduleItemId = false;
                foreach (var scheduleItemEmployee in entity.ScheduleItemEmployeeList.Where(s => s.IsActive))
                {
                    if (scheduleItemEmployee.ScheduleItemId == scheduleItemId)
                    {
                        employeeHasScheduleItemId = true;
                        break;
                    }   
                }

                if (employeeHasScheduleItemId)
                {
                    var dto = EmployeeMapper.EntityToDto(entity);
                    employeeDtoList.Add(dto);
                }
            }

            return employeeDtoList;
        }
    }
}