using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace GroceryProject
{
    /// <summary>
    /// Definition of the ReceiptReader class
    /// </summary>
    public class ReceiptReader
    {
        /// <summary>
        /// The text of the read receipt
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// The receipt text split into lines
        /// </summary>
        public string[] ReceiptLines;
        /// <summary>
        /// Key terms to search the receipt text for store names
        /// </summary>
        private string[] _supportedStoreNames = { "WALMART", "TARGET", "WALGREENS", "CVS", "BEST BUY", "HOME DEPOT", "LOWE'S", "COSTCO", "KROGER", "SAFEWAY",
        "STARBUCKS", "MCDONALD'S", "SUBWAY", "DOLLAR GENERAL", "FAMILY DOLLAR", "7-ELEVEN", "DUNKIN'", "PETSMART", "PETCO", "BED BATH & BEYOND",
        "WHOLE FOODS MARKET", "TRADER JOE'S", "H-E-B", "PUBLIX", "WAWA", "RITE AID", "ALDI", "GAMESTOP", "CHICK-FIL-A", "PANERA BREAD",
        "AT&T", "VERIZON", "SPRINT", "T-MOBILE", "APPLE STORE", "MICROSOFT STORE", "NIKE", "ADIDAS", "GAP", "OLD NAVY",
        "BATH & BODY WORKS", "ULTA BEAUTY", "SEPHORA", "J.CREW", "FOREVER 21", "ZARA", "LULULEMON", "AMERICAN EAGLE", "URBAN OUTFITTERS"
        };
        /// <summary>
        /// Key terms to search the receipt text for the subtotal, total, tax1 and tax2
        /// </summary>
        private string[] _totalKeyWords = { "SUBTOTAL", "SBTOTAL", "UBTOTAL", "SUBTTAL", "SUBTOTA", "TOTAL", "0TAL", "OTAL", "TAX" };
        /// <summary>
        /// The receipt line number where a total, subtotal, tax1 or tax2 was found
        /// </summary>
        private int _totalIndex = 0;
        /// <summary>
        /// String array of all state abreviations
        /// </summary>
        private string[] _stateAbbreviations = {
         "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
        "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
        "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
        "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
        "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
        };
        /// <summary>
        /// Street suffixes used to find the address
        /// </summary>
        private string[] _streetSuffixes = {
        "AVENUE", "AVE", "BOULEVARD", "BLVD", "CIRCLE", "CIR", "COURT", "CT", "DRIVE", "DR",
        "LANE", "LN", "PLACE", "PL", "ROAD", "RD", "STREET", "ST", "TERRACE", "TER",
        "WAY", "CRESCENT", "CR", "SQUARE", "SQ", "ALLEY", "ALY", "PARK", "PK", "TRAIL", "TRL",
        "PATH", "HIGHWAY", "HWY", "LOOP", "RIDGE", "RD", "PARKWAY", "PLZ", "GROVE", "GRV", "VIEW", "HILL",
        "MALL", "WALK", "BRIDGE", "ROW", "COMMONS", "GATE", "CROSSING", "XING", "POINT", "PT",
        "EXPRESSWAY", "EXPWY", "CLOSE", "CL", "PASS", "PSS", "GARDENS", "GDNS", "COVE", "MEADOW",
        "MD", "HARBOR", "HBR", "GLEN", "LN", "RUN", "PIKE", "PK", "CANYON", "DIVIDE", "DV",
        "MOUNT", "MT", "TRAIL", "TRL",
        };
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reader">An Image reader with its suplied text</param>
        public ReceiptReader(ImageReader reader)
        {
            Text = reader.Text;
            ReceiptLines = Text.Split('\n');
        }
        public DateTime FindReceiptDate()
        {
            string d = FindDate();
            string t = FindTime();
            try
            {
                return DateTime.ParseExact(d + " " + t, "MM/dd/yy HH:mm", null);
            }
            catch(Exception e)
            {
                return DateTime.Now;
            }
            
        }
        /// <summary>
        /// Finds the address on the receipt
        /// </summary>
        /// <returns>A string array containing the city, state, street, and postal code</returns>
        public string[] FindAddress()
        {
            string[] address = new string[4];
            for(int i = 0; i < ReceiptLines.Length; ++i)
            {
                AddressCheck(ReceiptLines[i], address);
                StreetCheck(ReceiptLines[i], address);
            }
            return address;
        }
        /// <summary>
        /// Searches for the street address stores it in the address array
        /// </summary>
        /// <param name="line">The current line of the receipt</param>
        /// <param name="address">An array to store the street address</param>
        private void StreetCheck(string line, string[] address)
        {
            string[] arr = line.Split(" ");
            for (int i = 0; i < arr.Length; ++i)
            {
                foreach (string street in _streetSuffixes)
                {
                    if (arr[i] == street)
                    {
                        string addr = "";
                        for (int j = 0; j < i; ++j)
                        {
                            addr += arr[j] + " ";
                        }
                        addr += arr[i];
                        address[0] = addr;
                        return;
                        
                    }
                }
            }
        }
        /// <summary>
        /// Searches for the state and postal code of the address and stores it in the address array
        /// </summary>
        /// <param name="line">The current line of the receipt</param>
        /// <param name="address">The address array to store the result</param>
        private void AddressCheck(string line, string[] address)
        {
            string[] arr = line.Split(" ");
            int zip = 0;
            for (int i = 0; i < arr.Length; ++i)
            {
                foreach (string state in _stateAbbreviations)
                {
                    if (arr[i] == state && arr[i + 1].Length == 5)
                    {
                        if (int.TryParse(arr[i + 1], out zip))
                        {
                            if(arr[i - 1] != null)
                            {
                                address[1] = arr[i - 1];
                            }
                            address[2] = state;
                            address[3] = zip + "";
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Searches for the ID of the current receipt
        /// </summary>
        /// <returns>The receipt ID as a string</returns>
        public string FindReceiptID()
        {
            string id = "NULL";
            foreach (string s in ReceiptLines)
            {
                if (s.Contains("ID #"))
                {
                    id = s.Substring(s.IndexOf("ID #") + 4);
                    for(int i = 0; i < id.Length; i++)
                    {
                        if (Char.IsLetter(id[i]) || Char.IsDigit(id[i]))
                        {
                            id = id.Substring(i);
                            break;
                        }
                    }
                    break;
                }
            }
            return id;
        }
        /// <summary>
        /// Finds the method of payment for this receipt
        /// </summary>
        /// <returns>The payment type</returns>
        public string FindPaymentType()
        {
            foreach (string s in ReceiptLines)
            {
                if (s.Contains("DEBIT"))
                {
                    return "DEBIT";
                }
                else if (s.Contains("CREDIT"))
                {
                    return "CREDIT";
                }
            }
            return "CASH";
        }
        /// <summary>
        /// Searches for the phone number of the store on the receipt
        /// </summary>
        /// <returns>A phone number as a string</returns>
        public string FindPhoneNum()
        {
            
            foreach (string s in ReceiptLines)
            {
                if (s.Contains("-"))
                {
                    int count = 0;
                    foreach(char c in s)
                    {
                        if (c == '-')
                        {
                            ++count;
                        }
                           
                    }
                    if(count == 2)
                    {
                        return s.Substring(s.IndexOf("-") - 3, s.IndexOf("-") + 9);
                    }
                }
            }
            return "NULL";
        }
        /// <summary>
        /// Finds the date of this receipt
        /// </summary>
        /// <returns>A date as a string</returns>
        public string FindDate()
        {

            foreach (string s in ReceiptLines)
            {
                if (s.Contains("/"))
                {
                    int count = 0;
                    foreach (char c in s)
                    {
                        if (c == '/')
                        {
                            ++count;
                        }

                    }
                    if (count == 2)
                    {
                        return s.Substring(s.IndexOf("/") - 2, s.IndexOf("/") + 6);
                    }
                }
            }
            return "NULL";
        }
        /// <summary>
        /// Finds the time this receipt was made
        /// </summary>
        /// <returns>A string of the time</returns>
        public string FindTime()
        {

            foreach (string s in ReceiptLines)
            {
                if (s.Contains(":"))
                {
                    int count = 0;
                    foreach (char c in s)
                    {
                        if (c == ':')
                        {
                            ++count;
                        }

                    }
                    if (count == 2 && s.Length > 8)
                    {
                        return s.Substring(s.IndexOf(":") - 2, 5);
                    }
                }
            }
            return "NULL";
        }
        /// <summary>
        /// Finds the store name on this receipt
        /// </summary>
        /// <returns>The store name</returns>
        public string FindStoreName()
        {
            string storeName = "NULL";
            foreach(string s in ReceiptLines)
            {
                for (int i = 0; i < _supportedStoreNames.Length; i++)
                {
                    if (s.Contains(_supportedStoreNames[i]))
                    {
                        storeName = _supportedStoreNames[i];
                        break;
                    }
                }
            }
            return storeName;
        }
        /// <summary>
        /// Finds the subtotal, total, tax 1 and tax 2 of this receipt
        /// </summary>
        /// <returns>A decimal array containing the cost details</returns>
        public decimal[] FindTaxAndTotals()
        {
            decimal[] totals = { 0m, 0m, 0m, 0m};
            foreach(string s in ReceiptLines)
            {
                if (CheckIfTotalOrTax(s))
                {
                    SharpenTotalAndTaxes(s, totals);
                }
            }
            return totals;
        }
        /// <summary>
        /// Finds the purchased items on the receipt
        /// </summary>
        /// <returns>A list of PurchasedItems</returns>
        public List<PurchasedItem> FindItems()
        {
            List<PurchasedItem> items = new();
            foreach(string line in ReceiptLines)
            {
                string[] arr = line.Split(" ");
                if(arr.Length > 3)
                {
                    CheckAndGetItem(arr, items);
                }
            }
            return items;
        }
        /// <summary>
        /// Checks and retrieves an item from a line on the receipt
        /// </summary>
        /// <param name="line">The current line of the receipt</param>
        /// <param name="list">The list of purchased items</param>
        private void CheckAndGetItem(string[] line, List<PurchasedItem> list)
        {
            for (int i = 1; i < line.Length - 1; i++)
            {

                ulong code = 0;
                if (ulong.TryParse(line[i], out code))
                {
                    if (code > 1000000 && !line.Contains("DEBIT") && !line.Contains("TERMINAL"))
                    {
                        PurchasedItem item = new();
                        item.Code = line[i];
                        string name = "";
                        for (int j = 0; j < i; j++)
                        {
                            name = name + line[j] + " ";
                        }
                        item.Name = name;
                        for (int y = i + 1; y < line.Length; ++y)
                        {
                            if (line[y].Length > 3)
                            {
                                try
                                {
                                    decimal price = decimal.Parse(line[y]);
                                    item.Price = price;

                                }
                                catch
                                {

                                }
                            }
                        }

                        list.Add(item);
                    }
                }

            }
        }
        /// <summary>
        /// Checks if a line contains a total, subtotal or tax value
        /// </summary>
        /// <param name="s">The current line of the receipt</param>
        /// <returns>True if a cost is found, false otherwise</returns>
        private bool CheckIfTotalOrTax(string s)
        {
            for(int i = 0; i < _totalKeyWords.Length; i++)
            {
                if (s.Contains(_totalKeyWords[i]))
                {
                    _totalIndex = i;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// retrieves a subtotal, total, or tax and inserts it into the totals array
        /// </summary>
        /// <param name="s">The string to sharpen</param>
        /// <param name="totals">The array of totals</param>
        private void SharpenTotalAndTaxes(string s, decimal[] totals)
        {
            if(_totalIndex < 5)
            {
                decimal subtotal = SharpenAmount(s);
                if(totals[0] < subtotal)
                {
                    totals[0] = subtotal;
                }
            }
            else if(_totalIndex < 8)
            {
                decimal total = SharpenAmount(s);
                if (totals[1] < total)
                {
                    totals[1] = total;
                }
            }
            else
            {
                decimal tax = SharpenAmount(s);
                if(totals[2] == 0)
                {
                    totals[2] = tax;
                }
                else if(totals[3] == 0)
                {
                    totals[3] = tax;
                }
            }
        }
        /// <summary>
        /// cleans up the text to retrieve a subtotal, total, or tax
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private decimal SharpenAmount(string s)
        {
            string[] line = s.Split(" ");
            decimal amount = 0;
            for(int i = 0; i < line.Length; i++)
            {
                if (line[i].Contains(".") && !line[i + 1].Contains("%"))
                {
                    foreach(char c in line[i])
                    {
                        if (Char.IsLetter(c))
                        {
                            line[i].Replace(c, ' ');
                        }
                    }
                    decimal.TryParse(line[i], out amount);
                    break;
                }
            }
            return amount;

        }
    }
}
