using Azure.Storage.Blobs;
using FluentValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Dto.Response;
using PetAdmin.Web.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetAdmin.Web.Services
{
    public class AttachmentService : Notifiable
    {
        private readonly IConfiguration _configuration;

        public AttachmentService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void VerifyPhoto(PhotoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                AddNotification("Foto", "Informe um nome para a foto");
                return;
            }
            if (string.IsNullOrWhiteSpace(dto.File))
            {
                AddNotification("Foto", "Informe o arquivo da foto");
                return;
            }
            if (!IsAllowedExtensionType(dto.Name))
            {
                AddNotification("Foto", "Extensão do arquivo não é permitida");
                return;
            }
        }

        private bool IsAllowedExtensionType(string fileName)
        {
            string[] extensionAllowedList = { "png", "jpeg", "jpg" };

            string fileExtension = Path.GetExtension(fileName);

            return fileExtension.ContainsAny(extensionAllowedList);
        }

        public PhotoResponse UploadAndGetPhotoPet(Guid id, PhotoDto photo)
        {
            var storageConnection = _configuration["Storage:ConnectionString"];
            var container = _configuration["Storage:PetContainer"];
            return UploadPhoto(id, photo, storageConnection, container);
        }

        public PhotoResponse UploadAndGetPhotoVaccine(Guid id, PhotoDto photo)
        {
            var storageConnection = _configuration["Storage:ConnectionString"];
            var container = _configuration["Storage:VaccineContainer"];
            return UploadPhoto(id, photo, storageConnection, container);
        }

        public PhotoResponse UploadAndGetPhotoClient(Guid id, PhotoDto photo)
        {
            var storageConnection = _configuration["Storage:ConnectionString"];
            var container = _configuration["Storage:ClientContainer"];
            return UploadPhoto(id, photo, storageConnection, container);
        }

        private PhotoResponse UploadPhoto(Guid id, PhotoDto photo, string storageConnection, string container)
        {
            try
            {
                var fileName = id.ToString() + "_" + DateTime.UtcNow.ToLongTimeString() + '_' + photo.Name;

                var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(photo.File, "");

                // Gera um array de Bytes
                byte[] imageBytes = Convert.FromBase64String(data);

                // Define o BLOB no qual a imagem será armazenada
                var blobClient = new BlobClient(storageConnection, container, fileName);

                // Envia a imagem
                using (var stream = new MemoryStream(imageBytes))
                {
                    if (FileSizeIsGreaterThanAllowed(stream))
                    {
                        AddNotification("Foto", "Tamanho da foto excede 1MB");
                        return null;
                    }

                    blobClient.Upload(stream);
                }

                return new PhotoResponse(fileName, blobClient.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                AddNotification("Foto", ex.Message);
                return null;
            }
        }

        private bool FileSizeIsGreaterThanAllowed(Stream streamFile)
        {
            int maxSizeMbAllowed = 20;
            int maxMbAllowed = maxSizeMbAllowed * 1024 * 1024;

            return streamFile.Length > maxMbAllowed;
        }

        public async Task DeletePhotoUrlPetAsync(string photoName)
        {
            var containerName = _configuration["Storage:PetContainer"];
            await DeletePhotoAsync(photoName, containerName);
        }

        public async Task DeletePhotoUrlVaccineAsync(string photoName)
        {
            var containerName = _configuration["Storage:VaccineContainer"];
            await DeletePhotoAsync(photoName, containerName);
        }

        public async Task DeletePhotoUrlClientAsync(string photoName)
        {
            var containerName = _configuration["Storage:ClientContainer"];
            await DeletePhotoAsync(photoName, containerName);
        }

        private async Task DeletePhotoAsync(string photoName, string containerName)
        {
            var containerUrlBase = _configuration["Storage:ContainerUrlBase"];

            Uri baseUri = new Uri(containerUrlBase);
            Uri containerUri = new Uri(baseUri, containerName);

            var accountName = _configuration["Storage:AccountName"];
            var storageKey = _configuration["Storage:AccountKey"];

            var storage = new StorageCredentials(accountName, storageKey);

            var cloudBlobContainer = new CloudBlobContainer(containerUri, storage);
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(photoName);
            await blob.DeleteAsync();
        }
    }
}
