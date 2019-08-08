using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureQueueStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("");

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("test");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage("Hello, World");
            queue.AddMessage(message);

            // Peek at the next message
            CloudQueueMessage peekedMessage = queue.PeekMessage();


            // Display message.
            Console.WriteLine(peekedMessage.AsString);

            // Get the next message
            CloudQueueMessage retrievedMessage = queue.GetMessage();

            //Process the message in less than 30 seconds, and then delete the message
            queue.DeleteMessage(retrievedMessage);

            // Peek at the next message
            CloudQueueMessage peekedMessageAgain = queue.PeekMessage();

            foreach (CloudQueueMessage newMessage in queue.GetMessages(20, TimeSpan.FromMinutes(5)))
            {
                // Process all messages in less than 5 minutes, deleting each message after processing.
                queue.DeleteMessage(newMessage);
            }

            Console.ReadKey();
        }
    }
}
