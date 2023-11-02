using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;

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
        /// Reads an image file and converts it to text (only .jpg supported currently)
        /// </summary>
        /// <param name="pathName">The image to read</param>
        /// <param name="scanType">The type of scan used. 0: Best for searching numbers. 1: Best for searching letters</param>
        public void Read(string pathName, int scanType)
        {
            IronTesseract IronOcr = new IronTesseract()
            {

                Language = OcrLanguage.EnglishFast,
                Configuration = new TesseractConfiguration()
                {
                    ReadBarCodes = false,
                    BlackListCharacters = "`ë|^®-:¥!~»¢;[]?/\\(){}@<>°",
                }
            };
            ConvertJPGToText(pathName, IronOcr, scanType);

        }
        /// <summary>
        /// Converts a .jpg file to text
        /// </summary>
        /// <param name="pathName">A .jpg file</param>
        /// <param name="IronOcr">An IronTesseract object</param>
        /// <param name="scanType">The scan type, 0 or 1</param>
        private void ConvertJPGToText(string pathName, IronTesseract IronOcr, int scanType)
        {
            using (var input = new OcrInput(pathName))
            {
                if (scanType == 0)
                {
                    input.Deskew();
                    input.Sharpen();
                    input.Dilate();
                    input.Binarize();
                    input.Open();
                    var Result = IronOcr.Read(input);
                    Text = Result.Text.ToUpper();
                    ReceiptLines = Text.Split("\n");
                }
                else
                {
                    IronOcr.Language = OcrLanguage.EnglishBest;
                    input.Scale(200);
                    input.Dilate();
                    input.Binarize();
                    input.Contrast();
                    input.Sharpen();
                    input.Open();

                    var Result = IronOcr.Read(input);
                    Text = Result.Text.ToUpper();
                    ReceiptLines = Text.Split("\n");
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
