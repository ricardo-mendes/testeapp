using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ScheduleItemClientController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RepositoryBase<ScheduleItemClient> _repository;
        private readonly RepositoryBase<ScheduleItemEmployee> _scheduleItemEmployeeRepository;
        private readonly RepositoryBase<Employee> _employeeRepository;
        private readonly RepositoryBase<ScheduleItem> _scheduleItemRepository;

        public ScheduleItemClientController(
            UnitOfWork unitOfWork,
            RepositoryBase<ScheduleItemEmployee> scheduleItemEmployeeRepository,
            RepositoryBase<Employee> employeeRepository,
            RepositoryBase<ScheduleItemClient> repository,
            RepositoryBase<ScheduleItem> scheduleItemRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _scheduleItemEmployeeRepository = scheduleItemEmployeeRepository;
            _employeeRepository = employeeRepository;
            _repository = repository;
            _scheduleItemRepository = scheduleItemRepository;
        }

        //Não é usado
        [HttpGet("{clientId}")]
        public List<ScheduleItemClientResponseDto> Get(long clientId)
        {
            // scheduleItem
            var scheduleItemList = _scheduleItemRepository.GetAll(s => s.IsActive).ToList();

            // scheduleItemEmployee
            var employeeId = _employeeRepository.GetAll(e => e.ClientId == clientId).Select(e => e.Id).FirstOrDefault();

            var scheduleItemEmployeeList = _scheduleItemEmployeeRepository.GetAll(s => scheduleItemList.Select(si => si.Id)
                .Contains(s.ScheduleItemId) && s.EmployeeId == employeeId).ToList();

            var responseList = new List<ScheduleItemClientResponseDto>();

            foreach (var scheduleItem in scheduleItemList)
            {
                var scheduleItemEmployee = scheduleItemEmployeeList
                        .FirstOrDefault(se => se.ScheduleItemId == scheduleItem.Id);

                var response = new ScheduleItemClientResponseDto
                {
                    ScheduleItemId = scheduleItem.Id,
                    ScheduleItemName = scheduleItem.Name,
                    IsActive = scheduleItemEmployee != null ? scheduleItemEmployee.IsActive : false,
                    Price = scheduleItemEmployee != null ? scheduleItemEmployee.Price : 0,
                };

                responseList.Add(response);
            }

            return responseList;
        }

        [HttpPost]
        public void Post([FromBody]ScheduleItemClientEmployeeDto dto)
        {
            // scheduleItem
            var scheduleItemList = _scheduleItemRepository.GetAll(s => dto.ScheduleItemPriceList.Select(si => si.ScheduleItemId).Contains(s.Id)).ToList();
            var scheduleItemIdList = scheduleItemList.Select(s => s.Id);

            //-------------------
            /*scheduleItemClient*/
            //-------------------
            var scheduleItemClientExistingList = _repository.GetAll(s => s.ClientId == dto.ClientId).ToList();
            var scheduleItemIdExistingOfClientList = scheduleItemClientExistingList.Select(s => s.ScheduleItemId);
            var scheduleItemClientNewList = new List<ScheduleItemClient>();
            
            // Não tem o serviço associado, então cadastra
            foreach (var scheduleItemId in scheduleItemIdList.Where(scheduleItemId => !scheduleItemIdExistingOfClientList.Contains(scheduleItemId)))
            {
                var scheduleItemClient = new ScheduleItemClient
                {
                    ScheduleItemId = dto.ScheduleItemPriceList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemId).ScheduleItemId,
                    ClientId = dto.ClientId,
                    IsActive = true
                };

                scheduleItemClientNewList.Add(scheduleItemClient);
            }

            // Tem o serviço cadastrado, mas ele está inativo no banco, muda para ativo
            foreach (var scheduleItemId in scheduleItemIdList.Where(scheduleItemId => scheduleItemIdExistingOfClientList.Contains(scheduleItemId) 
                && !scheduleItemClientExistingList.FirstOrDefault(si => si.ScheduleItemId == scheduleItemId).IsActive))
            {
                var scheduleItemClientExisting = scheduleItemClientExistingList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemId);
                scheduleItemClientExisting.IsActive = true;
            }

            // Está ativo no banco, mas foi desmarcado na tela, mudar para inativo
            var scheduleItemIdActiveExistingOfClientList = scheduleItemClientExistingList.Where(s => s.IsActive).Select(s => s.ScheduleItemId);

            foreach (var scheduleItemId in scheduleItemIdActiveExistingOfClientList
                .Where(scheduleItemId => !scheduleItemIdList.Contains(scheduleItemId)))
            {
                var scheduleItemClientExisting = scheduleItemClientExistingList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemId);
                scheduleItemClientExisting.IsActive = false;
            }

            _repository.AddRange(scheduleItemClientNewList);
            _repository.UpdateRange(scheduleItemClientExistingList);

            //-------------------
            /*scheduleItemEmployee*/
            //-------------------

            var scheduleItemEmployeeExistingList = _scheduleItemEmployeeRepository.GetAll(s => s.EmployeeId == dto.EmployeeId).ToList();
            var scheduleItemIdExistingOfEmployeeList = scheduleItemEmployeeExistingList.Select(s => s.ScheduleItemId);
            var scheduleItemEmployeeNewList = new List<ScheduleItemEmployee>();

            // Não tem o serviço associado, então cadastra
            foreach (var scheduleItemId in scheduleItemIdList.Where(scheduleItemId => !scheduleItemIdExistingOfEmployeeList.Contains(scheduleItemId)))
            {
                var scheduleItemEmployee = new ScheduleItemEmployee
                {
                    ScheduleItemId = dto.ScheduleItemPriceList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemId).ScheduleItemId,
                    Price = dto.ScheduleItemPriceList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemId).Price,
                    EmployeeId = dto.EmployeeId,
                    IsActive = true,
                    Hour = 1,
                    Minutes = 0,
                    Name = scheduleItemList.FirstOrDefault(s => s.Id == scheduleItemId).Name
                };

                scheduleItemEmployeeNewList.Add(scheduleItemEmployee);
            }

            // Tem o serviço cadastrado, mas ele está inativo no banco, muda para ativo
            foreach (var scheduleItemId in scheduleItemIdList.Where(scheduleItemId => scheduleItemIdExistingOfEmployeeList.Contains(scheduleItemId)
                && !scheduleItemEmployeeExistingList.FirstOrDefault(si => si.ScheduleItemId == scheduleItemId).IsActive))
            {
                var scheduleItemEmployeeExisting = scheduleItemEmployeeExistingList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemId);
                scheduleItemEmployeeExisting.IsActive = true;
            }

            // Está ativo no banco, foi marcado na tela, então atualizar price e name
            var scheduleItemEmployeeActiveExistingList = scheduleItemEmployeeExistingList.Where(s => s.IsActive);

            foreach (var scheduleItemEmployee in scheduleItemEmployeeActiveExistingList
                .Where(scheduleItem => scheduleItemIdList.Contains(scheduleItem.ScheduleItemId)))
            {
                var scheduleItemEmployeeExisting = scheduleItemEmployeeExistingList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemEmployee.ScheduleItemId);
                scheduleItemEmployeeExisting.Price = dto.ScheduleItemPriceList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemEmployee.ScheduleItemId).Price;
                scheduleItemEmployeeExisting.Name = scheduleItemList.FirstOrDefault(s => s.Id == scheduleItemEmployee.ScheduleItemId).Name;
            }

            // Está ativo no banco, mas foi desmarcado na tela, mudar para inativo
            foreach (var scheduleItemEmployee in scheduleItemEmployeeActiveExistingList
                .Where(scheduleItem => !scheduleItemIdList.Contains(scheduleItem.ScheduleItemId)))
            {
                var scheduleItemEmployeeExisting = scheduleItemEmployeeExistingList.FirstOrDefault(s => s.ScheduleItemId == scheduleItemEmployee.ScheduleItemId);
                scheduleItemEmployeeExisting.IsActive = false;
            }

            _scheduleItemEmployeeRepository.AddRange(scheduleItemEmployeeNewList);
            _scheduleItemEmployeeRepository.UpdateRange(scheduleItemEmployeeExistingList);

            _unitOfWork.Commit();
        }

    }
}
 