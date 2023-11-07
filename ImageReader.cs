using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace GroceryProject
{
    public class ImageReader
    {
        public string ImagePath { get; set; }
        public string Text { get; private set; }
        private byte[] ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                return imageBytes;
            }
        }

        public void OpenImage()
        {
            // Configure open file dialog box
            var Dialog = new Microsoft.Win32.OpenFileDialog();
            Dialog.Filter = "Image Files(*.jpg; *.jpeg)| *.jpg; *.jpeg;";
            Text = "";
            // Show open file dialog box
            bool? result = Dialog.ShowDialog();
            if(result == true)
            {
                ImagePath = Dialog.FileName;
            }
        }

        public async Task ReadImage()
        {
            if (string.IsNullOrEmpty(ImagePath))
                return;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("K87283460488957"), "apikey"); //Added api key in form data
                form.Add(new StringContent("eng"), "language");

                form.Add(new StringContent("2"), "ocrengine");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("true"), "istable");

                if (string.IsNullOrEmpty(ImagePath) == false)
                {
                    byte[] imageData = File.ReadAllBytes(ImagePath);
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");
                }

                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

                string strContent = await response.Content.ReadAsStringAsync();



                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);

                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        Text += ocrResult.ParsedResults[i].ParsedText;
                    }
                    Text = Text.ToUpper();
                    var charsToRemove = new string[] { "@", ",", ";", "'", "\t", ":" };
                    foreach (var c in charsToRemove)
                    {
                        Text = Text.Replace(c, " ");
                    }
                }
                else
                {
                    
                }



            }
            catch (Exception exception)
            {
                
            }
        }
    }
}
