using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string[] supportedStoreNames = { "WALMART", "ALMART", "WLMART", "WLMRT", "TARGET", "ARGET", "TRGET", "TARGE" };
        /// <summary>
        /// Key terms to search the receipt text for the subtotal, total, tax1 and tax2
        /// </summary>
        private string[] totalKeyWords = { "SUBTOTAL", "SBTOTAL", "UBTOTAL", "SUBTTAL", "SUBTOTA", "TOTAL", "0TAL", "OTAL", "TAX" };
        /// <summary>
        /// The receipt line number where the store name was found
        /// </summary>
        private int storeIndex = 0;
        /// <summary>
        /// The receipt line number where a total, subtotal, tax1 or tax2 was found
        /// </summary>
        private int totalIndex = 0;
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

        public ReceiptReader(ImageReader reader)
        {
            Text = reader.Text;
            ReceiptLines = Text.Split('\n');
        }

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
                        if (id[i] != ' ' && id[i] !=':')
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
        public string FindStoreName()
        {
            string storeName = "NULL";
            foreach(string s in ReceiptLines)
            {
                if (StoreFound(s))
                {
                    storeName = SharpenStoreName(s);
                    break;
                }
            }
            return storeName;
        }

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

        public List<string[]> FindItems()
        {
            List<string[]> items = new List<string[]>();
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

        private void CheckAndGetItem(string[] line, List<string[]> list)
        {
            for (int i = 1; i < line.Length - 1; i++)
            {

                ulong code = 0;
                if (ulong.TryParse(line[i], out code))
                {
                    if (code > 1000000 && !line.Contains("DEBIT") && !line.Contains("TERMINAL"))
                    {
                        string[] item = new string[3];
                        item[1] = line[i];
                        string name = "";
                        for (int j = 0; j < i; j++)
                        {
                            name = name + line[j] + " ";
                        }
                        item[0] = name;
                        for (int y = i + 1; y < line.Length; ++y)
                        {
                            if (line[y].Length > 3)
                            {
                                try
                                {
                                    decimal price = decimal.Parse(line[y]);
                                    item[2] = price + "";

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

        private bool CheckIfTotalOrTax(string s)
        {
            for(int i = 0; i < totalKeyWords.Length; i++)
            {
                if (s.Contains(totalKeyWords[i]))
                {
                    totalIndex = i;
                    return true;
                }
            }
            return false;
        }

        private void SharpenTotalAndTaxes(string s, decimal[] totals)
        {
            if(totalIndex < 5)
            {
                decimal subtotal = SharpenAmount(s);
                if(totals[0] < subtotal)
                {
                    totals[0] = subtotal;
                }
            }
            else if(totalIndex < 8)
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
        private bool StoreFound(string s)
        {
            for(int i = 0; i < supportedStoreNames.Length; i++)
            {
                if (s.Contains(supportedStoreNames[i]))
                {
                    storeIndex = i;
                    return true;
                }
            }
            return false;
        }

        private string SharpenStoreName(string s)
        {
            if(s == "NULL") { return "NULL"; }
            if(storeIndex < 4) { return "Walmart"; }
            else { return "Target"; }
        }
    }
}
