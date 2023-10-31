using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;

namespace GroceryProject
{
    class TextImager
    {
        private string[] supportedStoreNames = { "WALMART", "ALMART", "WLMART", "WLMRT", "TARGET", "ARGET", "TRGET", "TARGE" };
        private string[] totalKeyWords = { "SUBTOTAL", "SBTOTAL", "UBTOTAL", "SUBTTAL", "SUBTOTA", "TOTAL", "0TAL", "OTAL", "TAX" };
        private int storeIndex = 0;
        private int totalIndex = 0;

        public string ConvertImageToText(string pathName)
        {

            IronTesseract IronOcr = new IronTesseract()
            {

                Language = OcrLanguage.EnglishFast,
                Configuration = new TesseractConfiguration()
                {
                    ReadBarCodes = false,
                    BlackListCharacters = "`ë|^®-<#:¥!~»¢;[]/\\()<>+°",
                }
            };


            using (var input = new OcrInput(pathName))
            {
                input.Deskew();
                input.Sharpen();
                input.Dilate();
                input.Binarize();
                input.Open();
                var Result = IronOcr.Read(input);
                return Result.Text;
            }

        }
        public string FindStoreName(string[] doc)
        {
            string storeName = "NULL";
            foreach(string s in doc)
            {
                if (StoreFound(s))
                {
                    storeName = SharpenStoreName(s);
                    break;
                }
            }
            return storeName;
        }

        public decimal[] FindTaxAndTotals(string[] doc)
        {
            decimal[] totals = { 0m, 0m, 0m, 0m};
            foreach(string s in doc)
            {
                if (CheckIfTotalOrTax(s))
                {
                    SharpenTotalAndTaxes(s, totals);
                }
            }
            return totals;
        }

        public List<string[]> FindItems(string[] doc)
        {
            List<string[]> items = new List<string[]>();
            foreach(string line in doc)
            {
                string[] arr = line.Split(' ');
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
                try
                {
                    ulong code = ulong.Parse(line[i]);
                    if (code > 100000)
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
                catch
                {

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
            string[] line = s.Split(' ');
            decimal amount = 0;
            try
            {
                amount = decimal.Parse(line[line.Length - 1]);
                
            }
            catch{

            }
            return amount;

        }
        public string FindAddress(string[] doc)
        {
            return "NULL";
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
