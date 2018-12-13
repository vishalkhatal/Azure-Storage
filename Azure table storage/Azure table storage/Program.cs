using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_table_storage
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount cloudStorageAccount =
    CloudStorageAccount.Parse
    (System.Configuration.ConfigurationSettings.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
            Console.WriteLine("Enter Table Name to create");
            string tableName = Console.ReadLine();
            CloudTable cloudTable = tableClient.GetTableReference(tableName);
            CreateNewTable(cloudTable);
            InsertRecordToTable(cloudTable);
            UpdateRecordInTable(cloudTable);
            DisplayTableRecords(cloudTable);
            DeleteRecordinTable(cloudTable);
            DropTable(cloudTable);

        }
        public static void CreateNewTable(CloudTable table)
        {
            if (!table.CreateIfNotExists())
            {
                Console.WriteLine("Table {0} already exists", table.Name);
                return;
            }
            Console.WriteLine("Table {0} created", table.Name);
        }
        public static void InsertRecordToTable(CloudTable table)
        {
            Console.WriteLine("Enter customer type");
            string customerType = Console.ReadLine();
            Console.WriteLine("Enter customer ID");
            string customerID = Console.ReadLine();
            Console.WriteLine("Enter customer name");
            string customerName = Console.ReadLine();
            Console.WriteLine("Enter customer details");
            string customerDetails = Console.ReadLine();
            Customer customerEntity = new Customer();
            customerEntity.customerType = customerType;
            customerEntity.customerID = Int32.Parse(customerID);
            customerEntity.customerDetails = customerDetails;
            customerEntity.customerName = customerName;
            customerEntity.AssignPartitionKey();
            customerEntity.AssignRowKey();
            Customer custEntity = RetrieveRecord(table, customerType, customerID);
            if (custEntity == null)
            {
                TableOperation tableOperation = TableOperation.Insert(customerEntity);
                table.Execute(tableOperation);
                Console.WriteLine("Record inserted");
            }
            else
            {
                Console.WriteLine("Record exists");
            }
        }
        public static Customer RetrieveRecord(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<Customer>(partitionKey, rowKey);
            TableResult tableResult = table.Execute(tableOperation);
            return tableResult.Result as Customer;
        }
        public static void UpdateRecordInTable(CloudTable table)
        {
            Console.WriteLine("Enter customer type");
            string customerType = Console.ReadLine();
            Console.WriteLine("Enter customer ID");
            string customerID = Console.ReadLine();
            Console.WriteLine("Enter customer name");
            string customerName = Console.ReadLine();
            Console.WriteLine("Enter customer details");
            string customerDetails = Console.ReadLine();
            Customer customerEntity = RetrieveRecord(table, customerType, customerID);
            if (customerEntity != null)
            {
                customerEntity.customerDetails = customerDetails;
                customerEntity.customerName = customerName;
                TableOperation tableOperation = TableOperation.Replace(customerEntity);
                table.Execute(tableOperation);
                Console.WriteLine("Record updated");
            }
            else
            {
                Console.WriteLine("Record does not exists");
            }
        }
        public static void DisplayTableRecords(CloudTable table)
        {
            TableQuery<Customer> tableQuery = new TableQuery<Customer>();
            foreach (Customer customerEntity in table.ExecuteQuery(tableQuery))
            {
                Console.WriteLine("Customer ID : {0}", customerEntity.customerID);
                Console.WriteLine("Customer Type : {0}", customerEntity.customerType);
                Console.WriteLine("Customer Name : {0}", customerEntity.customerName);
                Console.WriteLine("Customer Details : {0}", customerEntity.customerDetails);
                Console.WriteLine("******************************");
            }
        }
        public static void DeleteRecordinTable(CloudTable table)
        {
            Console.WriteLine("Enter customer type");
            string customerType = Console.ReadLine();
            Console.WriteLine("Enter customer ID");
            string customerID = Console.ReadLine();
            Customer customerEntity = RetrieveRecord(table, customerType, customerID);
            if (customerEntity != null)
            {
                TableOperation tableOperation = TableOperation.Delete(customerEntity);
                table.Execute(tableOperation);
                Console.WriteLine("Record deleted");
            }
            else
            {
                Console.WriteLine("Record does not exists");
            }
        }
        public static void DropTable(CloudTable table)
        {
            if (!table.DeleteIfExists())
            {
                Console.WriteLine("Table does not exists");
            }
        }
    }
}
