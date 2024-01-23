using FluentValidator;
using PetAdmin.Web.Dto.Base;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
using PetAdmin.Web.Infra;

namespace PetAdmin.Web.Services
{
    public class ScheduleService : Notifiable
    {
        private readonly RepositoryBase<Schedule> _repository;
        private readonly RepositoryBase<SchedulePet> _schedulePetRepository;
        private readonly RepositoryBase<ScheduleItemEmployee> _scheduleItemEmployeeRepository;
        private readonly UnitOfWork _unitOfWork;

        public ScheduleService(
            RepositoryBase<Schedule> repository,
            RepositoryBase<SchedulePet> schedulePetRepository,
            RepositoryBase<ScheduleItemEmployee> scheduleItemEmployeeRepository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _schedulePetRepository = schedulePetRepository;
            _scheduleItemEmployeeRepository = scheduleItemEmployeeRepository;
            _unitOfWork = unitOfWork;
        }

        //As colunas Date e DateWithHour vão ser únicas no banco 
        public void AddDateRange(SchedulePostDto dto)
        {
            if (dto.DaysOfWeek == null || (dto.DaysOfWeek != null && !dto.DaysOfWeek.Any()))
            {
                AddNotification("Agenda", "Escolha no mínimo um dia da semana");
                return;
            }

            if (dto.InitialHour < 0 || dto.InitialHour > 23 || dto.EndHour < 0 || dto.EndHour> 23)
            {
                AddNotification("Agenda", "Horário Inválido");
                return;
            }

            if (dto.EndHour < dto.InitialHour)
            {
                AddNotification("Agenda", "Horário final não pode ser menor que o inicial");
                return;
            }

            if (dto.EndDate < dto.InitialDate)
            {
                AddNotification("Agenda", "Data final não pode ser menor que a inicial");
                return;
            }

            var initialDate = dto.InitialDate;
            var endDate = dto.EndDate;

            var scheduleList = new List<Schedule>();
            try
            {
                var employeeScheduleExistingList = _repository.GetAll(s => s.EmployeeId == dto.EmployeeId && s.Date.Date >= initialDate.Date, 
                    s => s.DateWithHour).ToList();

                var schedulePetList = new List<SchedulePet>();

                for (int i = 0; i < employeeScheduleExistingList.Count(); i = i + 2000)
                {
                    var employeeScheduleIdList = employeeScheduleExistingList.Skip(i).Take(2000).Select(e => e.Id);

                    var schedulePetInForList = _schedulePetRepository
                    .GetAll(p => p.ScheduleId.HasValue && employeeScheduleIdList.Contains(p.ScheduleId.Value)).ToList();

                    if (schedulePetInForList.Any())
                        schedulePetList.AddRange(schedulePetInForList);
                }

                if (schedulePetList.Any())
                {
                    var schedule = employeeScheduleExistingList.FirstOrDefault(s => s.Id == schedulePetList.FirstOrDefault().ScheduleId.GetValueOrDefault());
                    AddNotification("Agenda", $"Não é possível atualizar a agenda, pois o dia {schedule.Date.ToShortDateString()} tem agendamento . Qualquer dúvida entre em contato com o suporte por favor.");
                    return;
                }

                TimeSpan period = endDate.Date - initialDate.Date;

                //Preenche o scheduleList com os dias
                for (int i = 0; i <= period.Days; i++)
                {
                    //Lógica para saber se o dia da semana vai ser inserido
                    if (dto.DaysOfWeek.Contains((int)initialDate.DayOfWeek))
                    {
                        //Preenche o scheduleList com as horas
                        for (int hour = dto.InitialHour; hour <= dto.EndHour; hour++)
                        {
                            var schedule = new Schedule
                            {
                                Date = initialDate.Date,
                                EmployeeId = dto.EmployeeId,
                                Status = (int)ScheduleStatusEnum.Available,
                                IsActive = true,
                                QuantityAllowed = dto.QuantityAllowed == 0 ? 3 : dto.QuantityAllowed,
                                QuantityOccupied = 0
                            };

                            schedule.DateWithHour = schedule.Date.AddHours(hour).AddMinutes(0);
                            if (hour < 12)
                                schedule.Period = (int)PeriodEnum.Morning;
                            else if (hour >= 12 && hour < 18)
                                schedule.Period = (int)PeriodEnum.Afternoon;
                            else
                                schedule.Period = (int)PeriodEnum.Night;

                            scheduleList.Add(schedule);
                        }
                    }
                    initialDate = initialDate.AddDays(1);
                }

                using (var transaction = _repository.GetContext().Database.BeginTransaction())
                {
                    if (employeeScheduleExistingList.Any())
                        _repository.DeleteRange(employeeScheduleExistingList);

                    _repository.GetContext().SaveChanges();

                    _repository.AddRange(scheduleList);

                    _repository.GetContext().SaveChanges();

                    transaction.Commit();
                }
            }
            catch(Exception ex)
            {
                AddNotification("Erro Genérico", "Ocorreu um erro inesperado: " + ex.Message);
            }
        }

        public BaseListDto<ScheduleDto> GetAllByEmployeeId(ScheduleGetAllEmployee getAll)
        {
            var initialDate = getAll.InitialDate.HasValue ? getAll.InitialDate.Value : DateTime.Now.Date;

            var scheduleList = _repository.GetAll(s => s.EmployeeId == getAll.EmployeeId && s.Date.Date >= initialDate.Date && s.Date.Date <= getAll.EndDate.Date
                && s.QuantityOccupied < s.QuantityAllowed && s.Status != (int)ScheduleStatusEnum.Blocked,
                s => s.DateWithHour);

            var dtoList = new BaseListDto<ScheduleDto>
            {
                Result = new List<ScheduleDto>()
            };

            foreach (var schedule in scheduleList)
            {
                dtoList.Result.Add(ScheduleMapper.EntityToDto(schedule));
            }

            return dtoList;
        }

        public void UpdateToAvailableStatus(Schedule entity, ScheduleDto scheduleDto)
        {
            if (entity.Status != (int)ScheduleStatusEnum.Blocked)
            {
                AddNotification("Status", "O horário já está livre");
            }
            else
            {
                entity = ScheduleMapper.DtoToAvailableStatus(scheduleDto, entity);
                _repository.Update(entity);
            }
        }

        public void UpdateToBlockedStatus(Schedule entity, ScheduleDto dto)
        {
            if (entity.Status == (int)ScheduleStatusEnum.Blocked)
            {
                AddNotification("Status", "Horário já está bloqueado");
            }
            else
            {
                entity = ScheduleMapper.DtoToBlockedStatus(dto, entity);
                _repository.Update(entity);
            }
        }

    }
}
