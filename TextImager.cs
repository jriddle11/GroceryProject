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
        public string ConvertImageToText(string pathName)
        {
            IronTesseract IronOcr = new IronTesseract();
            using (var input = new OcrInput(pathName))
            {
                input.Scale(200);
                input.Sharpen();
                input.Contrast();
                var Result = IronOcr.Read(input);
                return Result.Text;
            }
            
        }
    }
}
