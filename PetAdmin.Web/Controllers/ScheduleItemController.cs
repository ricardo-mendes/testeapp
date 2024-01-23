using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ScheduleItemController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RepositoryBase<ScheduleItem> _repository;
        private readonly RepositoryBase<ScheduleItemClient> _scheduleItemClientRepository;
        private readonly RepositoryBase<ScheduleItemEmployee> _scheduleItemEmployeeRepository;
        private readonly RepositoryBase<Client> _clientRepository;

        public ScheduleItemController(
            UnitOfWork unitOfWork,
            RepositoryBase<ScheduleItem> repository,
            RepositoryBase<Client> clientRepository,
            RepositoryBase<ScheduleItemClient> scheduleItemClientRepository,
            RepositoryBase<ScheduleItemEmployee> scheduleItemEmployeeRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _clientRepository = clientRepository;
            _scheduleItemClientRepository = scheduleItemClientRepository;
            _scheduleItemEmployeeRepository = scheduleItemEmployeeRepository;
        }

        [HttpGet("{id}")]
        public ScheduleItemDto Get(long id)
        {
            return ScheduleItemMapper.EntityToDto(_repository.GetById(id));
        }

        //Usado para pegar os serviços para o petlover marcar
        [HttpGet("getall")]
        public ICollection<ScheduleItemDto> GetAll()
        {
            var entityList = _repository.GetAll(c => c.IsActive, c => c.Name);
            var dtoList = new List<ScheduleItemDto>();

            foreach (var entity in entityList)
            {
                dtoList.Add(ScheduleItemMapper.EntityToDto(entity));
            }

            return dtoList;
        }

        [HttpGet("getallbyclient/{clientId}")]
        public ICollection<ScheduleItemDto> GetAllByClient(long clientId)
        {
            var includes = new List<Expression<Func<ScheduleItemClient, object>>>
            {
                c => c.ScheduleItem
            };

            var scheduleItemClientList = _scheduleItemClientRepository.GetAll((s => s.ClientId == clientId && s.IsActive), (s => s.ClientId), includes);

            var scheduleItemDtoList = new List<ScheduleItemDto>();

            foreach (var scheduleItemClient in scheduleItemClientList)
            {
                scheduleItemDtoList.Add(ScheduleItemMapper.EntityToDto(scheduleItemClient.ScheduleItem));
            }

            return scheduleItemDtoList;
        }

        [HttpGet("getallbyemployee/{employeeId}")]
        public ICollection<ScheduleItemDto> GetAllByEmployee(long employeeId)
        {
            var includes = new List<Expression<Func<ScheduleItemEmployee, object>>>
            {
                c => c.ScheduleItem
            };

            var scheduleItemEmployeeList = _scheduleItemEmployeeRepository.GetAll((s => s.EmployeeId == employeeId), (s => s.EmployeeId), includes);

            var scheduleItemDtoList = new List<ScheduleItemDto>();

            foreach (var scheduleItemEmployee in scheduleItemEmployeeList)
            {
                scheduleItemDtoList.Add(ScheduleItemMapper.EntityToDto(scheduleItemEmployee.ScheduleItem));
            }

            return scheduleItemDtoList;
        }

        [HttpPut]
        public void Put([FromBody]ScheduleItemDto dto)
        {
            var entity = _repository.GetById(dto.Id);
            entity = ScheduleItemMapper.DtoToEntity(dto, entity);

            _repository.Update(entity);
            _unitOfWork.Commit();
        }

    }
}