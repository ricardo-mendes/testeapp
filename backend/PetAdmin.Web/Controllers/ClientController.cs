using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PetAdmin.Web.Dto.Schedule;
using Microsoft.AspNetCore.Identity;
using PetAdmin.Web.Filters;
using PetAdmin.Web.Localization;
using PetAdmin.Web.Services.Identity;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ClientController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RepositoryBase<Client> _repository;
        private readonly RepositoryBase<Employee> _employeeRepository;
        RepositoryBase<ScheduleItem> _scheduleItemRepository;
        private readonly RepositoryBase<ScheduleItemEmployee> _scheduleItemEmployeeRepository;
        private readonly UserService _userService;
        private readonly ScheduleItemClientRepository _scheduleItemClientRepository;
        private readonly ClientService _clientService;
        private readonly LocationGoogleService _locationGoogleService;
        private readonly IUserApplication _userApplication;
        private readonly PetLoverRepository _petLoverRepository;

        public ClientController(
            UserService userService,
            UnitOfWork unitOfWork,
            RepositoryBase<Client> repository,
            RepositoryBase<Employee> employeeRepository,
            RepositoryBase<ScheduleItem> scheduleItemRepository,
            RepositoryBase<ScheduleItemEmployee> scheduleItemEmployeeRepository,
            ScheduleItemClientRepository scheduleItemClientRepository,
            ClientService clientService,
            LocationGoogleService locationGoogleService,
            IUserApplication userApplication,
            PetLoverLocationClientRepository petLoverLocationClientRepository,
            PetLoverRepository petLoverRepository
            )
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _repository = repository;
            _employeeRepository = employeeRepository;
            _scheduleItemRepository = scheduleItemRepository;
            _scheduleItemEmployeeRepository = scheduleItemEmployeeRepository;
            _scheduleItemClientRepository = scheduleItemClientRepository;
            _clientService = clientService;
            _locationGoogleService = locationGoogleService;
            _userApplication = userApplication;
            _petLoverRepository = petLoverRepository;
        }

        //Não Usado
        [HttpGet("{id}")]
        [ValidateClientIdFilter("Id")]
        public ClientDto Get(long id)
        {
            var includes = new List<Expression<Func<Client, object>>>
            {
                c => c.EmployeeList,
                c => c.Location
            };
            return ClientMapper.EntityToDto(_repository.GetById(id, includes));
        }

        [HttpGet("getall")]
        public ICollection<ClientDto> GetAll()
        {
            var includes = new List<Expression<Func<Client, object>>>
            {
                c => c.EmployeeList,
                c => c.Location
            };

            var petLoverId = _userApplication.GetPetLoverId();

            if (petLoverId == default)
                return new List<ClientDto>();

            var entityList = _repository.GetAll(c => c.IsActive, c => c.Name, includes);
            
            var dtoList = new List<ClientDto>();

            var petLoverWithLocation = _petLoverRepository.GetWithLocationById(petLoverId);

            foreach (var entity in entityList)
            {
                dtoList.Add(ClientMapper.EntityToDto(entity, petLoverWithLocation.Latitude, petLoverWithLocation.Longitue));
                //if (petLoverWithLocation.Latitude != default && petLoverWithLocation.Longitue != default)
                //    dtoList.Add(ClientMapper.EntityToDto(entity, petLoverWithLocation.Latitude, petLoverWithLocation.Longitue));
            }

            //dtoList = dtoList
            //    .Where(d => d.Distance < 8000)
            //    .OrderBy(d => d.Distance)
            //    .ToList();

            return dtoList;
        }

        //Clientes para o petlover escolher de acordo com o serviço escolhido
        [HttpGet("getallbyscheduleitem/{scheduleItemId}")]
        public ICollection<ClientDto> GetAllByScheduleItem(long scheduleItemId)
        {
            var scheduleItemClientList = _scheduleItemClientRepository.GetAllByScheduleItemId(scheduleItemId);

            var clientDtoList = new List<ClientDto>();

            foreach (var scheduleItemClient in scheduleItemClientList)
            {
                clientDtoList.Add(ClientMapper.EntityToDto(scheduleItemClient.Client));
            }

            return clientDtoList;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ClientDto dto)
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

                //Add Client
                var client = ClientMapper.DtoToEntity(dto, user.Id);

                await _locationGoogleService.SetLatitudeAndLongitudeAsync(client.Location);
                if (_locationGoogleService.Invalid)
                    return await Response(null, _locationGoogleService.Notifications);

                _repository.Add(client);
                dto.Id = client.Id;

                //Add Employee
                var employeeDefault = EmployeeMapper.ClientDtoToEntity(dto);
                _employeeRepository.Add(employeeDefault);

                _unitOfWork.Commit();

                transaction.Commit();
            }

            var userAuth = _userService.Login(dto.Email, dto.User.Password);
            return await Response(userAuth, _userService.Notifications);
        }

        [HttpPut]
        [ValidateClientIdFilter("Id")]
        public async Task<IActionResult> Put([FromBody]ClientDto dto)
        {
            var includes = new List<Expression<Func<Client, object>>>
            {
                c => c.EmployeeList,
                c => c.Location
            };

            var client = _repository.GetById(dto.Id, includes);
            client = ClientMapper.DtoToEntity(dto, client);

            await _locationGoogleService.SetLatitudeAndLongitudeAsync(client.Location);
            if (_locationGoogleService.Invalid)
                return await Response(null, _locationGoogleService.Notifications);

            _repository.Update(client);

            var employeeDefault = EmployeeMapper.ClientDtoToEntity(dto, client.EmployeeList.FirstOrDefault());
            _employeeRepository.Update(employeeDefault);

            _unitOfWork.Commit();

            return await Response(null, _userService.Notifications);
        }

        [HttpPatch("DeletePhoto")]
        [ValidateClientIdFilter("Id")]
        public async Task<IActionResult> DeletePhoto([FromBody] ClientDto dto)
        {
            var client = _repository.GetById(dto.Id);

            await _clientService.DeletePhotoAsync(client);

            return await Response(null, _clientService.Notifications);
        }

        [HttpPatch("UpdatePhoto")]
        [ValidateClientIdFilter("Id")]
        public async Task<IActionResult> UpdatePhoto([FromBody] ClientDto dto)
        {
            var client = _repository.GetById(dto.Id);

            _clientService.UpdatePhotoUrl(client, dto);

            return await Response(dto, _clientService.Notifications);
        }
    }
}