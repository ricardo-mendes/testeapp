using FluentValidator;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Models.Domain;
using System;
using System.Linq;

namespace PetAdmin.Web.Services
{
    public class MyPetLoverService : Notifiable
    {
        private readonly RepositoryBase<SchedulePet> _schedulePetRepository;

        public MyPetLoverService(
            RepositoryBase<SchedulePet> schedulePetRepository)
        {
            _schedulePetRepository = schedulePetRepository;
        }

        public bool CanNotDelete(long petLoverId)
        {
            var schedulePetList = _schedulePetRepository.GetAll(s => s.PetLoverId == petLoverId).ToList();
            if (schedulePetList.Any())
            {
                AddNotification("MyPetLover", "Não é possível excluir, pois esse PetLover já realizou agendamentos");
                return true;
            }
            else
                return false;
        } 
    }
}
