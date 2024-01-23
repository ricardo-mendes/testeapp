using FluentValidator;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Models.Domain;
using System;
using System.Linq;

namespace PetAdmin.Web.Services
{
    public class PetLoverService : Notifiable
    {
        private readonly RepositoryBase<SchedulePet> _schedulePetRepository;

        public PetLoverService(
            RepositoryBase<SchedulePet> schedulePetRepository)
        {
            _schedulePetRepository = schedulePetRepository;
        }

        public bool EmailIsRegistered(string email, long clientId, long? petLoverId = null)
        {
            if (petLoverId == null)
            {
                if (_schedulePetRepository.GetContext().PetLovers.Where(p => p.Email == email && p.ClientId == clientId).Any())
                {
                    AddNotification("PetLover", "Já existe outro dono de pet cadastrado com esse Email");
                    return true;
                }
            }
            else
            {
                if (_schedulePetRepository.GetContext().PetLovers.Where(p => p.Id != petLoverId.Value && p.Email == email && p.ClientId == clientId).Any())
                {
                    AddNotification("PetLover", "Já existe outro dono de pet cadastrado com esse Email");
                    return true;
                }
            }

            return false;
        }
    }
}
