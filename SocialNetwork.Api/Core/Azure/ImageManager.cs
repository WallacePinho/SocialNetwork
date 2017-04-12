using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetwork.Api.Core.Azure {
    public class ImageManager {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;
        private const string _blobContainerName = "images";


        public ImageManager() {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            _blobClient = _storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference(_blobContainerName);
            _blobContainer.CreateIfNotExists();
            _blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        public async Task<string> UploadFileAsync(string path) {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(GetRandomBlobName(Path.GetExtension(path)));
            await blob.UploadFromFileAsync(path);
            File.Delete(path);
            return blob.Uri.ToString();
        }

        

        private string GetRandomBlobName(string filename) {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
    }
}