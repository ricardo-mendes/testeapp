using FluentValidator;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Models.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdmin.Web.Services
{
    public class ClientService : Notifiable
    {
        private readonly RepositoryBase<Client> _repository;
        private readonly AttachmentService _attachmentService;

        public ClientService(
            RepositoryBase<Client> repository,
            AttachmentService attachmentService)
        {
            _repository = repository;
            _attachmentService = attachmentService;
        }

        public async Task DeletePhotoAsync(Client client)
        {
            if (!string.IsNullOrWhiteSpace(client.PhotoUrl))
            {
                await _attachmentService.DeletePhotoUrlClientAsync(client.PhotoName);

                client.PhotoName = null;
                client.PhotoUrl = null;

                _repository.Update(client);
            }
        }

        public void UpdatePhotoUrl(Client client, ClientDto dto)
        {
            if (dto.Photo != null)
            {
                var photoResponse = _attachmentService.UploadAndGetPhotoClient(client.Uid, dto.Photo);

                if (_attachmentService.Notifications != null && _attachmentService.Notifications.Any())
                {
                    AddNotifications(_attachmentService.Notifications);
                    return;
                }

                client.PhotoName = photoResponse.PhotoName;
                client.PhotoUrl = photoResponse.PhotoUrl;
                dto.PhotoUrl = photoResponse.PhotoUrl;

                _repository.Update(client);
            }
        }
    }
}
