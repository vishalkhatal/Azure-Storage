using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_table_storage
{
    class Customer : TableEntity
    {
        public int customerID { get; set; }
        public string customerName { get; set; }
        public string customerDetails { get; set; }
        public string customerType { get; set; }
        public void AssignRowKey()
        {
            this.RowKey = customerID.ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = customerType;
        }

    }
}
