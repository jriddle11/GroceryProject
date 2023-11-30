using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryProject
{
    public static class ReceiptGenerator
    {
        private static string[] _commonLastNames = {
            "smith", "johnson", "williams", "jones", "brown",
            "davis", "miller", "wilson", "moore", "taylor",
            "anderson", "thomas", "jackson", "white", "harris",
            "martin", "thompson", "garcia", "martinez", "robinson",
            "clark", "rodriguez", "lewis", "lee", "walker",
            "hall", "allen", "young", "hernandez", "king",
            "wright", "lopez", "hill", "scott", "green",
            "adams", "baker", "gonzalez", "nelson", "carter",
            "mitchell", "perez", "roberts", "turner", "phillips",
            "campbell", "parker", "evans", "edwards", "collins",
            "stewart", "sanchez", "morris", "rogers", "reed",
            "cook", "morgan", "bell", "murphy", "bailey",
            "rivera", "cooper", "richardson", "cox", "howard",
            "ward", "torres", "peterson", "gray", "ramirez",
            "james", "watson", "brooks", "kelly", "sanders",
            "price", "bennett", "wood", "barnes", "ross",
            "henderson", "coleman", "jenkins", "perry", "powell",
            "long", "patterson", "hughes", "flores", "washington",
            "butler", "simmons", "foster", "gonzales", "bryant"
        };

        private static char[] _lowercaseFirstInitial = {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w'
        };

        private static string[] _colors = {
            "Red", "Orange", "Yellow", "Green", "Blue",
            "Indigo", "Violet", "Black", "White", "Gray",
            "Brown", "Pink", "Purple", "Cyan", "Magenta",
            "Olive", "Lime", "Teal", "Navy", "Maroon"
        };

        private static string[] _animals = {
            "Dog", "Cat", "Lion", "Elephant", "Tiger",
            "Giraffe", "Monkey", "Zebra", "Penguin", "Koala",
            "Panda", "Snake", "Rabbit", "Dolphin", "Owl",
            "Bear", "Fox", "Kangaroo", "Parrot", "Horse"
        };

        public static List<string> GenerateEmails()
        {
            List<string> result = new List<string>();
            foreach(string lastname in _commonLastNames)
            {
                Random r = new Random();
                int rInt = r.Next(0, 23);
                result.Add(_lowercaseFirstInitial[rInt] + lastname + "@ksu.edu");
            }
            return result;
        }

        public static string GeneratePassword()
        {
            Random random = new Random();
            int color = random.Next(0, 20);
            int animal = random.Next(0, 20);
            return _colors[color] + _animals[animal] + color + animal;
        }

        public static List<Receipt> GenerateReceipts(int count)
        {
            List<Receipt> result = new List<Receipt>();
            Random r = new Random();
            for(int i = 0; i < count; ++i)
            {
                Receipt receipt = new Receipt();
                receipt.ID = r.Next(10000, 1000000) + "";
                int rand = r.Next(0, 3);
                if(rand == 0)
                {
                    receipt.StoreName = "DILLONS";
                    receipt.Street = "130 SARBER LN";
                }
                else if(rand == 1)
                {
                    receipt.StoreName = "WALMART";
                    receipt.Street = "101 BlUEMONT AVE";
                }
                else
                {
                    receipt.StoreName = "TARGET";
                    receipt.Street = "800 COMMONS PL";
                }
            
                receipt.City = "MANHATTAN";
                receipt.State = "KS";
                receipt.PostalCode = 66502;
                receipt.PhoneNumber = "999-999-9999";
                receipt.PaymentType = PaymentMethod.Debit;
                List<PurchasedItem> purchasedItems = new List<PurchasedItem>();
                receipt.Total = GenerateItems(purchasedItems);
                receipt.PurchasedItems = purchasedItems;
                receipt.SubTotal = receipt.Total - 3;
                receipt.Tax1 = 2;
                receipt.Tax2 = 1;
                result.Add(receipt);
            }
            return result;
        }

        public static decimal GenerateItems(List<PurchasedItem> list)
        {
            Random r = new Random();
            decimal total = 0;
            int rand = r.Next(8, 30);
            for(int i = 0; i < rand; ++i)
            {
                PurchasedItem item = RandItem();
                total += (item.Price * item.Quantity);
                list.Add(item);
            }
            return total;
        }

        public static PurchasedItem RandItem()
        {
            Random rand = new Random();
            int type = rand.Next(20);
            PurchasedItem item = new PurchasedItem();
            switch (type)
            {
                case 0:
                    item.Name = "COKE";
                    item.Price = 1.96m;
                    item.Quantity = rand.Next(1,5);
                    break;
                case 1:
                    item.Name = "CREAMER";
                    item.Price = 4.26m;
                    item.Quantity = rand.Next(1,4);
                    break;
                case 2:
                    item.Name = "GV 46SV PIMA";
                    item.Price = 3.96m;
                    item.Quantity = rand.Next(1,7);
                    break;
                case 3:
                    item.Name = "BUTTER UNSAL";
                    item.Price = 5.98m;
                    item.Quantity = rand.Next(1,3);
                    break;
                case 4:
                    item.Name = "KFT SINGLES";
                    item.Price = 7.26m;
                    item.Quantity = rand.Next(1,5);
                    break;
                case 5:
                    item.Name = "GUACOMOLE";
                    item.Price = 4.18m;
                    item.Quantity = rand.Next(1,4);
                    break;
                case 6:
                    item.Name = "EGGS 60CT";
                    item.Price = 6.50m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 7:
                    item.Name = "BNLS CKNTHIG";
                    item.Price = 15.50m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 8:
                    item.Name = "GC A-P FLOUR";
                    item.Price = 4.30m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 9:
                    item.Name = "TAQUITOS";
                    item.Price = 6.48m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 10:
                    item.Name = "J RIDE WRM";
                    item.Price = 5.98m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 11:
                    item.Name = "MONSTER 12PK";
                    item.Price = 19.98m;
                    item.Quantity = 1;
                    break;
                case 12:
                    item.Name = "KOSHER DILLS";
                    item.Price = 7.23m;
                    item.Quantity = rand.Next(1, 4);
                    break;
                case 13:
                    item.Name = "JASMINE RICE";
                    item.Price = 11.87m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 14:
                    item.Name = "TOTINOS";
                    item.Price = 1.97m;
                    item.Quantity = rand.Next(1, 5);
                    break;
                case 15:
                    item.Name = "HT PEANUT B";
                    item.Price = 4.48m;
                    item.Quantity = rand.Next(1, 4);
                    break;
                case 16:
                    item.Name = "BROC FLOR 12";
                    item.Price = 1.16m;
                    item.Quantity = rand.Next(1, 10);
                    break;
                case 17:
                    item.Name = "5LB 80% GB R";
                    item.Price = 19.76m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 18:
                    item.Name = "FL STK BACON";
                    item.Price = 18.98m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                case 19:
                    item.Name = "MM TC SAl/TK";
                    item.Price = 16.98m;
                    item.Quantity = rand.Next(1, 3);
                    break;
                default:
                    item.Name = "COKE";
                    item.Price = 1.96m;
                    item.Quantity = rand.Next(5);
                    break;
            }
            return item;
        }
    }
}
