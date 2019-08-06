using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadImageOnBlob
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create storagecredentials object by reading the values from the configuration (appsettings.json)
            StorageCredentials storageCredentials = new StorageCredentials("teststorageaccountvk", "SAS Key");
            // Create cloudstorage account by passing the storagecredentials
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
            CloudBlobContainer container = blobClient.GetContainerReference("upload");
            // Get the reference to the block blob from the container
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("profilePic.jpg");
            // Upload the file
            using (Stream file = System.IO.File.OpenRead(@"C:\Users\vikhatal\Pictures\profilePic.jpg"))
            {

                blockBlob.UploadFromStream(file);

            }

            // Download the file
            using (var fileStream = System.IO.File.OpenWrite(@"C:\Users\vikhatal\Pictures\profilePic1.jpg"))
            {
                blockBlob.DownloadToStream(fileStream);
            }

            // Delete the blob.
            blockBlob.Delete();
        }
      
    }
}
