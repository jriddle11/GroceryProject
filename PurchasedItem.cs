using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryProject
{
    public class PurchasedItem
    {
        /// <summary>
        /// The Name of the item
        /// </summary>
        public string Name;
        /// <summary>
        /// The Code of the item
        /// </summary>
        public int Quantity;
        /// <summary>
        /// The Price of the item
        /// </summary>
        public decimal Price;

        public override bool Equals(object obj)
        {
            if(obj is PurchasedItem p)
            {
                if(p.Name == Name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
