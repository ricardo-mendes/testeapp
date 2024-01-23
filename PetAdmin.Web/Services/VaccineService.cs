using FluentValidator;
using Microsoft.EntityFrameworkCore.Internal;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Mappers;
using PetAdmin.Web.Models.Domain;
using System;
using System.Threading.Tasks;

namespace PetAdmin.Web.Services
{
    public class VaccineService : Notifiable
    {
        private readonly RepositoryBase<Vaccine> _repository;
        private readonly AttachmentService _attachmentService;

        public VaccineService(
            RepositoryBase<Vaccine> repository,
            AttachmentService attachmentService)
        {
            _repository = repository;
            _attachmentService = attachmentService;
        }

        public VaccineDto Create(VaccineDto dto)
        {
            var vaccine = VaccineMapper.DtoToEntity(dto);

            if (dto.Photo != null)
            {
                try
                {
                    _attachmentService.VerifyPhoto(dto.Photo);

                    if (_attachmentService.Notifications == null || !_attachmentService.Notifications.Any())
                    {
                        var photoResponse = _attachmentService.UploadAndGetPhotoVaccine(vaccine.Uid, dto.Photo);

                        if (_attachmentService.Notifications == null || !_attachmentService.Notifications.Any())
                        {
                            vaccine.PhotoName = photoResponse.PhotoName;
                            vaccine.PhotoUrl = photoResponse.PhotoUrl;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao salvar foto no cadastro da vacina == ", ex.Message);
                }
            }

            return VaccineMapper.EntityToDto(_repository.Add(vaccine));
        }

        public async Task DeletePhotoAsync(Vaccine vaccine)
        {
            if (!string.IsNullOrWhiteSpace(vaccine.PhotoUrl))
            {
                await _attachmentService.DeletePhotoUrlVaccineAsync(vaccine.PhotoName);

                vaccine.PhotoName = null;
                vaccine.PhotoUrl = null;

                _repository.Update(vaccine);
            }
        }

        public void UpdatePhotoUrl(Vaccine vaccine, PhotoDto photoDto)
        {
            if (photoDto != null)
            {
                var photoResponse = _attachmentService.UploadAndGetPhotoVaccine(vaccine.Uid, photoDto);

                if (_attachmentService.Notifications != null && _attachmentService.Notifications.Any())
                {
                    AddNotifications(_attachmentService.Notifications);
                    return;
                }

                vaccine.PhotoName = photoResponse.PhotoName;
                vaccine.PhotoUrl = photoResponse.PhotoUrl;

                _repository.Update(vaccine);
            }
        }
    }
}
