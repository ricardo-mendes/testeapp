using Microsoft.EntityFrameworkCore;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Extensions;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetAdmin.Web.Infra.Repositories
{
    public class SchedulePetRepository : RepositoryBase<SchedulePet>
    {
        private readonly PetContext _context;

        public SchedulePetRepository(PetContext context)
            : base(context)
        {
            _context = context;
        }

        public ICollection<SchedulePetLoverResultDto> GetAllByPetLoverId(long petLoverId)
        {
            var dbBaseQuery = from schedulePet in _context.SchedulePets
                                join pet in _context.Pets.AsNoTracking() on schedulePet.PetId equals pet.Id
                                join schedule in _context.Schedules.AsNoTracking() on schedulePet.ScheduleId equals schedule.Id
                                join employee in _context.Employees.AsNoTracking() on schedule.EmployeeId equals employee.Id
                                join client in _context.Clients.AsNoTracking() on employee.ClientId equals client.Id
                                join location in _context.Locations.AsNoTracking() on client.LocationId equals location.Id
                                join scheduleItemEmployee in _context.ScheduleItemEmployees.AsNoTracking() on schedulePet.ScheduleItemEmployeeId equals scheduleItemEmployee.Id
                                join scheduleItem in _context.ScheduleItems.AsNoTracking() on scheduleItemEmployee.ScheduleItemId equals scheduleItem.Id
                              
                                where schedulePet.PetLoverId == petLoverId
                                orderby schedule.DateWithHour descending
                                select new SchedulePetLoverResultDto
                                {
                                   SchedulePetId = schedulePet.Id,
                                   PetName = pet.Name,
                                   DateWithHour = schedule.DateWithHour,
                                   EmployeeId = employee.Id,
                                   ScheduleStatus = schedulePet.Status,
                                   ClientName = client.Name,
                                   ClientAddress = location.StreetName + ", " + location.StreetNumber,
                                   ClientComplement = location.Complement,
                                   ClientNeighborhood = location.Neighborhood,
                                   EmployeePhoneNumber = employee.PhoneNumber,
                                   ScheduleItemEmployeeName = scheduleItem.Name + " " + scheduleItemEmployee.Name,
                                   ScheduleItemEmployeePrice = scheduleItemEmployee.Price.Value,
                                   LastModificationTime = schedulePet.LastModificationTime
                               };

            return dbBaseQuery.ToList();
        }

        public ICollection<ScheduleEmployeeResultDto> GetAllScheduleEmployeeResult(ScheduleGetAllEmployee getAll)
        {
            var initialDate = getAll.InitialDate.HasValue ? getAll.InitialDate.Value : DateTime.Now.Date;

            if (getAll.OnlyScheduleOccupied)
            {
                var dbBaseQuery = from schedule in _context.Schedules.AsNoTracking()
                                  join schedulePet in _context.SchedulePets.AsNoTracking() on schedule.Id equals schedulePet.ScheduleId
                                  join pet in _context.Pets.AsNoTracking() on schedulePet.PetId equals pet.Id 
                                  join petLover in _context.PetLovers.AsNoTracking() on schedulePet.PetLoverId equals petLover.Id 
                                  join scheduleItemEmployee in _context.ScheduleItemEmployees.AsNoTracking() on schedulePet.ScheduleItemEmployeeId equals scheduleItemEmployee.Id
                                  join scheduleItem in _context.ScheduleItems.AsNoTracking() on scheduleItemEmployee.ScheduleItemId equals scheduleItem.Id

                                  where schedule.EmployeeId == getAll.EmployeeId && schedule.Date.Date >= initialDate.Date
                                    && schedule.Date.Date <= getAll.EndDate.Date && 
                                    (schedulePet.Status == (int)ScheduleStatusEnum.Requested || schedulePet.Status == (int)ScheduleStatusEnum.Confirmed)
                                  orderby schedule.DateWithHour
                                  select new ScheduleEmployeeResultDto
                                  {
                                      ScheduleId = schedule.Id,
                                      SchedulePetId = schedulePet != null ? schedulePet.Id : (long?)null,
                                      PetName = pet.Name,
                                      PetBreedName = pet.Breed,
                                      PetLoverName = petLover.Name,
                                      PetLoverPhoneNumber = petLover.PhoneNumber,
                                      Date = schedule.DateWithHour,
                                      Status = schedulePet.Status,
                                      ScheduleItemId = scheduleItemEmployee.ScheduleItemId,
                                      ScheduleItemName = scheduleItem.Name + " " + scheduleItemEmployee.Name,
                                      ScheduleItemPrice = scheduleItemEmployee.Price,
                                      PetNote = pet.Note,
                                      QuantityAllowed = schedule.QuantityAllowed,
                                      QuantityOccupied = schedule.QuantityOccupied,
                                      LastModificationTime = schedulePet.LastModificationTime
                                  };

                return dbBaseQuery.ToList();
            }
            else
            {
                var dbBaseQuery = from schedule in _context.Schedules.AsNoTracking()

                                  join schedulePet in _context.SchedulePets.AsNoTracking() on
                                     schedule.Id equals schedulePet.ScheduleId into spl
                                  from schedulePetLeft in spl.DefaultIfEmpty()

                                  join pet in _context.Pets.AsNoTracking() on
                                      schedulePetLeft.PetId equals pet.Id into pl
                                  from petLeft in pl.DefaultIfEmpty()

                                  join petLover in _context.PetLovers.AsNoTracking() on
                                      schedulePetLeft.PetLoverId equals petLover.Id into pll
                                  from petLoverLeft in pll.DefaultIfEmpty()

                                  join scheduleItemEmployee in _context.ScheduleItemEmployees.AsNoTracking() on
                                      schedulePetLeft.ScheduleItemEmployeeId equals scheduleItemEmployee.Id into sps
                                  from scheduleItemEmployeeLeft in sps.DefaultIfEmpty()

                                  join scheduleItem in _context.ScheduleItems.AsNoTracking() on
                                      scheduleItemEmployeeLeft.ScheduleItemId equals scheduleItem.Id into si
                                  from scheduleItemLeft in si.DefaultIfEmpty()

                                  where schedule.EmployeeId == getAll.EmployeeId && schedule.Date.Date >= initialDate.Date
                                    && schedule.Date.Date <= getAll.EndDate.Date
                                  orderby schedule.DateWithHour
                                  select new ScheduleEmployeeResultDto
                                  {
                                      ScheduleId = schedule.Id,
                                      SchedulePetId = schedulePetLeft != null ? schedulePetLeft.Id : (long?)null,
                                      PetName = petLeft != null ? petLeft.Name : null,
                                      PetBreedName = petLeft != null ? petLeft.Breed : null,
                                      PetLoverName = petLoverLeft != null ? petLoverLeft.Name : null,
                                      PetLoverPhoneNumber = petLoverLeft != null ? petLoverLeft.PhoneNumber : null,
                                      Date = schedule.DateWithHour,
                                      Status = schedulePetLeft != null ? schedulePetLeft.Status : schedule.Status,
                                      ScheduleItemId = scheduleItemEmployeeLeft != null ? scheduleItemEmployeeLeft.ScheduleItemId : 0,
                                      ScheduleItemName = scheduleItemEmployeeLeft != null ? scheduleItemLeft.Name + " " + scheduleItemEmployeeLeft.Name : null,
                                      ScheduleItemPrice = scheduleItemEmployeeLeft != null ? scheduleItemEmployeeLeft.Price : null,
                                      PetNote = petLeft != null ? petLeft.Note : null,
                                      QuantityAllowed = schedule.QuantityAllowed,
                                      QuantityOccupied = schedule.QuantityOccupied,
                                      LastModificationTime = schedulePetLeft != null ? schedulePetLeft.LastModificationTime : null,
                                  };

                return dbBaseQuery.ToList();
            }  
        }

        public SchedulePetCancelEmailDto GetSchedulePetCancelEmailDto(long schedulePetId)
        {
            var dbBaseQuery = from schedulePet in _context.SchedulePets
                              join pet in _context.Pets.AsNoTracking() on schedulePet.PetId equals pet.Id
                              join petLover in _context.PetLovers.AsNoTracking() on schedulePet.PetLoverId equals petLover.Id
                              join schedule in _context.Schedules.AsNoTracking() on schedulePet.ScheduleId equals schedule.Id
                              join employee in _context.Employees.AsNoTracking() on schedule.EmployeeId equals employee.Id
                              where schedulePet.Id == schedulePetId
                              select new SchedulePetCancelEmailDto
                              {
                                  PetName = pet.Name,
                                  PetLoverName = petLover.Name,
                                  PetLoverEmail = petLover.Email,
                                  DateWithHour = schedule.DateWithHour,
                                  EmployeeEmail = employee.Email
                              };

            return dbBaseQuery.FirstOrDefault();
        }

        public SchedulePetConfirmEmailDto GetSchedulePetConfirmEmailDto(long schedulePetId)
        {
            var dbBaseQuery = from schedulePet in _context.SchedulePets
                              join pet in _context.Pets.AsNoTracking() on schedulePet.PetId equals pet.Id
                              join petLover in _context.PetLovers.AsNoTracking() on schedulePet.PetLoverId equals petLover.Id
                              join schedule in _context.Schedules.AsNoTracking() on schedulePet.ScheduleId equals schedule.Id
                              where schedulePet.Id == schedulePetId
                              select new SchedulePetConfirmEmailDto
                              {
                                  PetName = pet.Name,
                                  PetLoverName = petLover.Name,
                                  PetLoverEmail = petLover.Email,
                                  DateWithHour = schedule.DateWithHour,
                                  IsUser = petLover.UserId != null
                              };

            return dbBaseQuery.FirstOrDefault();
        }

        public SchedulePetCompleteEmailDto GetSchedulePetCompletedEmailDto(long schedulePetId)
        {
            var dbBaseQuery = from schedulePet in _context.SchedulePets
                              join pet in _context.Pets.AsNoTracking() on schedulePet.PetId equals pet.Id
                              join petLover in _context.PetLovers.AsNoTracking() on schedulePet.PetLoverId equals petLover.Id
                              join schedule in _context.Schedules.AsNoTracking() on schedulePet.ScheduleId equals schedule.Id
                              join scheduleItemEmployee in _context.ScheduleItemEmployees.AsNoTracking() on schedulePet.ScheduleItemEmployeeId equals scheduleItemEmployee.Id
                              join scheduleItem in _context.ScheduleItems.AsNoTracking() on scheduleItemEmployee.ScheduleItemId equals scheduleItem.Id
                              where schedulePet.Id == schedulePetId
                              select new SchedulePetCompleteEmailDto
                              {
                                  PetName = pet.Name,
                                  PetLoverName = petLover.Name,
                                  PetLoverEmail = petLover.Email,
                                  ScheduleItemName = scheduleItem.Name,
                                  IsUser = petLover.UserId != null
                              };

            return dbBaseQuery.FirstOrDefault();
        }
    }
}
