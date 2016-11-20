using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using KumaazuDotnetCoreAzure.Options;

namespace KumaazuDotnetCoreAzure.Models
{
    public class Blob
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// https://docs.microsoft.com/ja-jp/azure/storage/storage-dotnet-how-to-use-blobs#a-namelist-blobs-in-pages-asynchronouslyablob-%E3%82%92%E3%83%9A%E3%83%BC%E3%82%B8%E3%81%A7%E9%9D%9E%E5%90%8C%E6%9C%9F%E3%81%AB%E4%B8%80%E8%A6%A7%E8%A1%A8%E7%A4%BA%E3%81%99%E3%82%8B
        /// </remarks>
        public static async Task<List<Blob>> Get(StorageOption storage)
        {
            StorageCredentials cred = new StorageCredentials(storage.Account, storage.Key);

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = new CloudStorageAccount(cred, true);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mlb");

            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;
            List<Blob> blobs = new List<Blob>();

            //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
            //When the continuation token is null, the last page has been returned and execution can exit the loop.
            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);
                foreach (CloudBlockBlob blobItem in resultSegment.Results)
                {
                    blobs.Add(new Blob
                    {
                        Name =blobItem.Name,
                        Uri = blobItem.Uri,
                    });
                }

                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);

            return blobs;
        }
    }
}
