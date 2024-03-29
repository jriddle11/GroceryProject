﻿using System;
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

        public PaymentMethod PaymentType;
        public string PhoneNumber = "NULL";

        public DateTime ReceiptDate;
        

        public List<PurchasedItem> PurchasedItems = new();
        /// <summary>
        /// Empty Constructor for the Receipt class
        /// </summary>
        public Receipt()
        {

        }
        /// <summary>
        /// Constructor that takes in a receipt reader and fills the values of the receipt
        /// </summary>
        /// <param name="reader">A receipt reader to read from</param>
        public Receipt(ReceiptReader reader)
        {
            StoreName = reader.FindStoreName();
            PaymentType = reader.FindPaymentType();
            PhoneNumber = reader.FindPhoneNum();
            ReceiptDate = reader.FindReceiptDate();
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

    public enum PaymentMethod { 
        Cash, Credit, Debit
    }
}
