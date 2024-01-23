using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using EFCore.BulkExtensions;
using GeoCoordinatePortable;
using FluentValidator;
using PetAdmin.Web.Dto;

namespace PetAdmin.Web.Services
{
    public class PetLoverLocationClientService : Notifiable
    {
        private readonly ClientRepository _clientRepository;
        private readonly PetLoverLocationClientRepository _petLoverLocationClientRepository;
        private readonly PetLoverRepository _petLoverRepository;

        public PetLoverLocationClientService(
            ClientRepository clientRepository,
            PetLoverLocationClientRepository petLoverLocationClientRepository,
            PetLoverRepository petLoverRepository)
        {
            _clientRepository = clientRepository;
            _petLoverLocationClientRepository = petLoverLocationClientRepository;
            _petLoverRepository = petLoverRepository;
        }

        public void CreateAndUpdateListByClientUid(Guid clientUid)
        {
            var includes = new List<Expression<Func<Client, object>>>
            {
                c => c.Location
            };

            var client = _clientRepository.GetAll(c => c.Uid == clientUid, c => c.Id, includes).FirstOrDefault();
            
            RemoveAllByClient(client);

            var petLoverList = _petLoverRepository.GetAllWithLocation();
            var petLoverLocationClientList = new List<PetLoverLocationClient>();

            if (client.Location.Latitude != default && client.Location.Longitue != default)
            {
                foreach (var petLover in petLoverList)
                {
                    if (petLover.Latitude != default && petLover.Longitue != default)
                    {
                        var petLoverLocationClient = new PetLoverLocationClient
                        {
                            PetLoverId = petLover.PetLoverId,
                            ClientId = client.Id,
                        };

                        petLoverLocationClient.Distance = GetDistance(client, petLover);

                        petLoverLocationClientList.Add(petLoverLocationClient);
                    }
                }

                if (petLoverLocationClientList.Any())
                    _petLoverLocationClientRepository.AddRange(petLoverLocationClientList);
            }
        }

        public void CreateAndUpdateListByPetLover(PetLover petLover)
        {
            RemoveAllByPetLover(petLover);

            var clientList = _clientRepository.GetAllWithLocation();
            var petLoverLocationClientList = new List<PetLoverLocationClient>();

            if (petLover.Location.Latitude != default && petLover.Location.Longitue != default)
            {
                foreach (var client in clientList)
                {
                    if (client.Latitude != default && client.Longitue != default)
                    {
                        var petLoverLocationClient = new PetLoverLocationClient
                        {
                            PetLoverId = petLover.Id,
                            ClientId = client.ClientId,
                        };

                        petLoverLocationClient.Distance = GetDistance(petLover, client);

                        petLoverLocationClientList.Add(petLoverLocationClient);
                    }
                }

                if (petLoverLocationClientList.Any())
                    _petLoverLocationClientRepository.AddRange(petLoverLocationClientList);
            }
        }

        private double GetDistance(Client client, Dto.PetLover.PetLoverLocationDto petLover)
        {
            var locA = new GeoCoordinate(petLover.Latitude, petLover.Longitue);
            var locB = new GeoCoordinate(client.Location.Latitude, client.Location.Longitue);

            var distance = locA.GetDistanceTo(locB);
            return distance;
        }

        private double GetDistance(PetLover petLover, ClientLocationDto client)
        {
            var locA = new GeoCoordinate(petLover.Location.Latitude, petLover.Location.Longitue);
            var locB = new GeoCoordinate(client.Latitude, client.Longitue);
            
            var distance = locA.GetDistanceTo(locB);
            return distance;
        }

        private void RemoveAllByClient(Client client)
        {
            var petLoverLocationClientToRemoveList = _petLoverLocationClientRepository.GetAll(p => p.ClientId == client.Id);

            if (petLoverLocationClientToRemoveList.Any())
                _petLoverLocationClientRepository.DeleteRange(petLoverLocationClientToRemoveList.ToList());
        }

        private void RemoveAllByPetLover(PetLover petLover)
        {
            var petLoverLocationClientToRemoveList = _petLoverLocationClientRepository.GetAll(p => p.PetLoverId == petLover.Id);

            if (petLoverLocationClientToRemoveList.Any())
                _petLoverLocationClientRepository.DeleteRange(petLoverLocationClientToRemoveList.ToList());
        }
    }
}
