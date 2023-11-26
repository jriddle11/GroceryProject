using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GroceryProject
{
    public class Receipt
    {
        public string ID;
        public string StoreName = "NULL";
        public string Street = "NULL";
        public string City = "NULL";
        public string State = "NULL";
        public int PostalCode = 0;

        public decimal Total = 0;
        public decimal SubTotal = 0;
        public decimal Tax1 = 0;
        public decimal Tax2 = 0;

        public string PaymentType = "NULL";
        public string PhoneNumber = "NULL";

        public string Date = "NULL";
        public string Time = "NULL";
        

        public List<PurchasedItem> PurchasedItems = new();

        public Receipt()
        {

        }

        public Receipt(ReceiptReader reader)
        {
            StoreName = reader.FindStoreName();
            PaymentType = reader.FindPaymentType();
            PhoneNumber = reader.FindPhoneNum();
            Date = reader.FindDate();
            Time = reader.FindTime();
            decimal[] totals = reader.FindTaxAndTotals();
            SubTotal = totals[0];
            Total = totals[1];
            Tax1 = totals[2];
            Tax2 = totals[3];
            PurchasedItems = reader.FindItems();
            ID = reader.FindReceiptID();
            string[] addr = reader.FindAddress();
            Street = addr[0];
            City = addr[1];
            State = addr[2];
            int.TryParse(addr[3], out PostalCode);
        }
    }
}
