﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryProject
{
    public class Receipt
    {
        public string ID;
        public string StoreName = "NULL";
        public decimal Total = 0;
        public decimal SubTotal = 0;
        public decimal Tax1 = 0;
        public decimal Tax2 = 0;

        public List<string[]> PurchasedItems = new List<string[]>();

        public Receipt(ReceiptReader reader)
        {
            StoreName = reader.FindStoreName();
            decimal[] totals = reader.FindTaxAndTotals();
            SubTotal = totals[0];
            Total = totals[1];
            Tax1 = totals[2];
            Tax2 = totals[3];
            PurchasedItems = reader.FindItems();
            ID = reader.FindReceiptID();
        }
    }
}
