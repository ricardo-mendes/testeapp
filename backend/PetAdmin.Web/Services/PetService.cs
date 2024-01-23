using FluentValidator;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdmin.Web.Services
{
    public class PetService : Notifiable
    {
        private readonly RepositoryBase<Pet> _repository;
        private readonly AttachmentService _attachmentService;

        public PetService(
            RepositoryBase<Pet> repository,
            AttachmentService attachmentService)
        {
            _repository = repository;
            _attachmentService = attachmentService;
        }

        public Pet Create(PetDto dto)
        {
            var pet = PetMapper.DtoToEntity(dto);

            if (dto.Photo != null)
            {
                try
                {
                    _attachmentService.VerifyPhoto(dto.Photo);

                    if (_attachmentService.Notifications == null || !_attachmentService.Notifications.Any())
                    {
                        var photoResponse = _attachmentService.UploadAndGetPhotoPet(pet.Uid, dto.Photo);

                        if (_attachmentService.Notifications == null || !_attachmentService.Notifications.Any())
                        {
                            pet.PhotoName = photoResponse.PhotoName;
                            pet.PhotoUrl = photoResponse.PhotoUrl;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Erro ao salvar foto no cadastro do pet == ", ex.Message);
                }
            }

            return _repository.Add(pet);
        }

        public void Update(PetDto dto)
        {
            var pet = _repository.GetById(dto.Id);
            pet = PetMapper.DtoToEntity(dto, pet);

            _repository.Update(pet);
        }

        public async Task DeletePhotoAsync(Pet pet)
        {
            if (!string.IsNullOrWhiteSpace(pet.PhotoUrl))
            {
                await _attachmentService.DeletePhotoUrlPetAsync(pet.PhotoName);

                pet.PhotoName = null;
                pet.PhotoUrl = null;

                _repository.Update(pet);
            }
        }

        public void UpdatePhoto(Pet pet, PhotoDto photoDto)
        {
            if (photoDto != null)
            {
                var photoResponse = _attachmentService.UploadAndGetPhotoPet(pet.Uid, photoDto);

                if (_attachmentService.Notifications != null && _attachmentService.Notifications.Any())
                {
                    AddNotifications(_attachmentService.Notifications);
                    return;
                }
                    

                pet.PhotoName = photoResponse.PhotoName;
                pet.PhotoUrl = photoResponse.PhotoUrl;

                _repository.Update(pet);
            }
        }
    }
}
