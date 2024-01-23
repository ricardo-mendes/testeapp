using FluentValidator;
using PetAdmin.Web.Dto.Base;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PetAdmin.Web.Services
{
    public class SchedulePetRangeService : Notifiable
    {
        private readonly RepositoryBase<SchedulePet> _repository;
        private readonly RepositoryBase<ScheduleItemEmployee> _scheduleItemEmployeeRepository;
        private readonly RepositoryBase<Schedule> _scheduleRepository;

        public SchedulePetRangeService(
            RepositoryBase<SchedulePet> repository,
            RepositoryBase<ScheduleItemEmployee> scheduleItemEmployeeRepository,
            RepositoryBase<Schedule> scheduleRepository)
        {
            _repository = repository;
            _scheduleItemEmployeeRepository = scheduleItemEmployeeRepository;
            _scheduleRepository = scheduleRepository;
        }

        public void CreateRangeSchedulePet(SchedulePetRangeDto dto)
        {
            ValidateDto(dto);

            if (Notifications.Any())
                return;

            dto.QuantityOfWeeks++;

            var firstSchedule = _scheduleRepository.GetById(dto.ScheduleId);

            if (firstSchedule == null)
            {
                AddNotification("Agenda", "Dia ou Horário Inexistente");
                return;
            }

            var dateOfWeekList = GetNextDatesOfWeeks(dto, firstSchedule);
            var scheduleList = GetScheduleList(firstSchedule, dateOfWeekList);

            if (scheduleList != null && scheduleList.Any(s => s.Status == (int)ScheduleStatusEnum.Blocked))
            {
                AddNotification("Agenda", "Existe horário bloqueado no período selecionado");
                return;
            }

            var scheduleIdList = scheduleList.Select(s => s.Id).ToList();

            ValidatePetAlreadyScheduled(dto, scheduleIdList);

            if (Notifications.Any())
                return;

            UpdateQuantityOccupiedInScheduleList(scheduleList);

            if (Notifications.Any())
                return;

            AddSchedulePetList(dto, firstSchedule, scheduleIdList);
        }

        public void ValidateDto(SchedulePetRangeDto dto)
        {

            if (dto.QuantityOfWeeks < 0)
            {
                AddNotification("Agenda", "Informe a quantidade de semanas que o agendamento vai se repetir");
                return;
            }

            if (dto.QuantityOfWeeks > 10)
            {
                AddNotification("Agenda", "A quantidade máxima de semanas precisa ser 10");
                return;
            }

            if (dto.PetId == default)
            {
                AddNotification("Agenda", "Informe o Pet");
                return;
            }

            if (dto.PetLoverId == default)
            {
                AddNotification("Agenda", "Informe o Dono do Pet");
                return;
            }

            if (dto.ScheduleItemEmployeeId == default)
            {
                AddNotification("Agenda", "Informe o Serviço");
                return;
            }
        }

        private void UpdateQuantityOccupiedInScheduleList(List<Schedule> scheduleList)
        {
            foreach (var schedule in scheduleList)
            {
                if (schedule.QuantityOccupied >= schedule.QuantityAllowed)
                {
                    AddNotification("Agenda", $"O dia {schedule.Date.Day}/{schedule.Date.Month} está com o horário ocupado");
                    return;
                }
            }

            foreach (var schedule in scheduleList)
                schedule.QuantityOccupied += 1;

            _scheduleRepository.UpdateRange(scheduleList);
        }

        private void AddSchedulePetList(SchedulePetRangeDto dto, Schedule firstSchedule, List<long> scheduleIdList)
        {
            var entityList = SchedulePetMapper.ScheduleListToEntityList(scheduleIdList, dto.PetLoverId, dto.PetId, dto.ScheduleItemEmployeeId);
            _repository.AddRange(entityList);
        }

        private static List<DateTime> GetNextDatesOfWeeks(SchedulePetRangeDto dto, Schedule firstSchedule)
        {
            var dateOfWeekList = new List<DateTime>();

            var dateOfNextWeek = firstSchedule.DateWithHour;

            for (int i = 0; i < (dto.QuantityOfWeeks - 1); i++)
            {
                dateOfNextWeek = dateOfNextWeek.AddDays(7);
                dateOfWeekList.Add(dateOfNextWeek);
            }

            return dateOfWeekList;
        }

        private List<Schedule> GetScheduleList(Schedule firstSchedule, List<DateTime> dateOfWeekList)
        {
            var scheduleList = new List<Schedule>
            {
                firstSchedule
            };

            var nextSchedulesList = _scheduleRepository
                .GetAll(s => dateOfWeekList.Contains(s.DateWithHour) && s.EmployeeId == firstSchedule.EmployeeId)   
                .ToList();

            if (nextSchedulesList.Any())
                scheduleList.AddRange(nextSchedulesList);

            return scheduleList;
        }

        private void ValidatePetAlreadyScheduled(SchedulePetRangeDto dto, List<long> scheduleIdList)
        {
            var schedulePetEntity = _repository
                .GetAll(s => s.PetId == dto.PetId && s.ScheduleId.HasValue && scheduleIdList.Contains(s.ScheduleId.Value), s => s.Schedule.DateWithHour)
                .FirstOrDefault();

            if (schedulePetEntity != null)
            {
                AddNotification("Agenda", "O pet já está agendado em algum horário no período selecionado");
                return;
            }
        }

        private DateTime GetToday()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime today = DateTime.Now.Date;

            try
            {
                TimeZoneInfo brazilianHour = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                today = TimeZoneInfo.ConvertTimeFromUtc(utcNow, brazilianHour).Date;
            }
            catch (Exception)
            {
            }

            return today;
        }
    }
}
