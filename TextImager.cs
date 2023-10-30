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
        private string[] supportedStoreNames = { "WALMART", "ALMART", "WLMART", "WLMRT", "TARGET", "TAGET", "TRGET", "TARGT" };
        private string[] totalKeyWords = { "SUBTOTAL", "SBTOTAL", "UBTOTAL", "SUBTTAL", "SUBTOTA", "TOTAL", "TTAL", "OTAL", "TAX" };
        private int storeIndex = 0;
        private int totalIndex = 0;

        public string ConvertImageToText(string pathName)
        {
            IronTesseract IronOcr = new IronTesseract();
            using (var input = new OcrInput(pathName))
            {
                input.Deskew();
                input.Scale(200);
                input.DeNoise();
                input.Sharpen();
                input.Contrast();
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
                    storeName = SharpenName(s);
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
                if (CheckIfTotal(s))
                {
                    SharpenTotalAndTaxes(s, totals);
                }
            }
            return totals;
        }

        private bool CheckIfTotal(string s)
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
                decimal subtotal = SharpenTotal(s);
                if(totals[0] < subtotal)
                {
                    totals[0] = subtotal;
                }
            }
            else if(totalIndex < 8)
            {
                decimal total = SharpenTotal(s);
                if (totals[1] < total)
                {
                    totals[1] = total;
                }
            }
            else
            {
                decimal tax = SharpenTotal(s);
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
        private decimal SharpenTotal(string s)
        {
            string[] line = s.Split(' ');
            int percentIndex = -1;
            int index = 0;
            decimal max = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i].Contains("%"))
                {
                    percentIndex = i;
                }
            }
            if (percentIndex >= 0)
            {
                index = percentIndex;
            }
            while (index < line.Length)
            {
                decimal amount = 0;
                try
                {
                    amount = Decimal.Parse(line[index]);
                }
                catch
                {
                }
                if (amount > max)
                {
                    max = amount;
                }
                index++;
            }
            return max;

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

        private string SharpenName(string s)
        {
            if(s == "NULL") { return "NULL"; }
            if(storeIndex < 4) { return "Walmart"; }
            else { return "Target"; }
        }
    }
}
