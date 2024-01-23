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
    public class SchedulePetService : Notifiable
    {
        private readonly RepositoryBase<SchedulePet> _repository;
        private readonly RepositoryBase<ScheduleItemEmployee> _scheduleItemEmployeeRepository;
        private readonly RepositoryBase<Schedule> _scheduleRepository;

        public SchedulePetService(
            RepositoryBase<SchedulePet> repository,
            RepositoryBase<ScheduleItemEmployee> scheduleItemEmployeeRepository,
            RepositoryBase<Schedule> scheduleRepository)
        {
            _repository = repository;
            _scheduleItemEmployeeRepository = scheduleItemEmployeeRepository;
            _scheduleRepository = scheduleRepository;
        }

        public BaseListDto<SchedulePetDto> GetAllByEmployeeId(ScheduleGetAllEmployee getAll)
        {
            var initialDate = getAll.InitialDate.HasValue ? getAll.InitialDate.Value : DateTime.Now.Date;

            var includes = new List<Expression<Func<SchedulePet, object>>>
            {
                c => c.Schedule,
                c => c.Pet,
                c => c.PetLover
            };

            var schedulePetList = _repository.GetAll(s => s.Schedule.EmployeeId == getAll.EmployeeId && s.Schedule.Date.Date >= initialDate.Date && s.Schedule.Date <= getAll.EndDate.Date,
                s => s.Schedule.DateWithHour, includes);

            var dtoList = new BaseListDto<SchedulePetDto>
            {
                Result = new List<SchedulePetDto>()
            };

            foreach (var schedulePet in schedulePetList)
            {
                dtoList.Result.Add(SchedulePetMapper.EntityToDto(schedulePet));
            }

            return dtoList;
        }

        public SchedulePet CreateSchedulePet(SchedulePetDto dto)
        {
            var schedule = _scheduleRepository.GetById(dto.ScheduleId.Value);

            if (schedule == null)
            {
                AddNotification("Agenda", "Horário Inválido");
                return null;
            }

            if (schedule != null && schedule.Status == (int)ScheduleStatusEnum.Blocked)
            {
                AddNotification("Agenda", "Não é possível reservar um horário bloqueado");
                return null;
            }

            var schedulePetEntity = _repository.GetAll(s => s.PetId == dto.PetId && s.ScheduleId == dto.ScheduleId, s => s.Schedule.DateWithHour).FirstOrDefault();

            if (schedulePetEntity != null)
            {
                AddNotification("Agenda", "O pet já está agendado nesse horário");
                return null;
            }

            if (schedule.QuantityOccupied >= schedule.QuantityAllowed)
            {
                AddNotification("Agenda", "O horário está cheio");
                return null;
            }

            schedule.QuantityOccupied += 1;
            _scheduleRepository.Update(schedule);

            var entity = SchedulePetMapper.DtoToEntity(dto);
            var response = _repository.Add(entity);
            return response;
        }

        public void UpdateToAvailableStatus(SchedulePetLoverResultDto dto)
        {
            var entity = _repository.GetById(dto.SchedulePetId);
            var schedule = _scheduleRepository.GetById(entity.ScheduleId.GetValueOrDefault());
            schedule.QuantityOccupied--;

            if (entity.Status == (int)ScheduleStatusEnum.Completed)
            {
                AddNotification("Status", "O serviço já foi finalizado");
            }
            else
            {
                entity = SchedulePetMapper.DtoToAvailableStatus(entity);

                _repository.Update(entity);
                _scheduleRepository.Update(schedule);
            }
        }

        public void UpdateToCanceledStatus(SchedulePetLoverResultDto dto)
        {
            var entity = _repository.GetById(dto.SchedulePetId);

            if (entity.Status == (int)ScheduleStatusEnum.Completed)
            {
                AddNotification("Status", "O serviço já foi finalizado");
            }
            else if (entity.Status == (int)ScheduleStatusEnum.Canceled)
            {
                AddNotification("Status", "O agendamento já foi cancelado");
            }
            else
            {
                entity = SchedulePetMapper.DtoToCanceledStatus(entity);
                _repository.Update(entity);

                var schedule = _scheduleRepository.GetById(entity.ScheduleId.Value);

                if (schedule.QuantityOccupied > 0)
                {
                    schedule.QuantityOccupied -= 1;
                    _scheduleRepository.Update(schedule);
                }
            }
        }

        public void UpdateToConfirmedStatus(SchedulePetDto dto)
        {
            var entity = _repository.GetById(dto.Id);

            if (entity.Status == (int)ScheduleStatusEnum.Blocked || entity.Status == (int)ScheduleStatusEnum.Completed)
            {
                AddNotification("Status", "Não é possível confirmar o horário");
            }
            else
            {
                entity = SchedulePetMapper.DtoToConfirmedStatus(dto, entity);
                _repository.Update(entity);
            }
        }

        public void UpdateToCompletedStatus(SchedulePetDto dto)
        {
            var entity = _repository.GetById(dto.Id);

            if (entity.Status != (int)ScheduleStatusEnum.Confirmed)
            {
                AddNotification("Status", "Não é possível finalizar um agendamento que não foi confirmado");
            }
            else
            {
                entity = SchedulePetMapper.DtoToCompletedStatus(dto, entity);
                _repository.Update(entity);
            }
        }

        public void UpdateToBlockedStatus(SchedulePetDto dto)
        {
            var entity = _repository.GetById(dto.Id);

            if (entity.Status != (int)ScheduleStatusEnum.Available)
            {
                AddNotification("Status", "Não é possível bloquear um horário ocupado");
            }
            else
            {
                entity = SchedulePetMapper.DtoToBlockedStatus(dto, entity);
                _repository.Update(entity);
            }
        }
    }
}
