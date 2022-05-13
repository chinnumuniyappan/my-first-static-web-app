using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using BlazorApp.Shared;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;
using System.Net.Http;
using System.Net;
using Syncfusion.Drawing;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

namespace BlazorApp.Api
{
    public static class WeatherForecastFunction
    {
        private static string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing!";
            }

            return summary;
        }


        [FunctionName("WeatherForecast")]
        public static async Task<string> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, TraceWriter log)
        {
            //var randomNumber = new Random();
            //var temp = 0;

            //var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = temp = randomNumber.Next(-20, 55),
            //    Summary = GetSummary(temp)
            //}).ToArray();

            //return new OkObjectResult(result);

            PdfDocument document = new PdfDocument();
            //Add a page to the document.
            PdfPage page = document.Pages.Add();
            //Create PDF graphics for the page.
            PdfGraphics graphics = page.Graphics;
            //Set the standard font. 
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            //Draw the text.
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));
            PdfBitmap image = new PdfBitmap(await GetImageStreamAsync());
            page.Graphics.DrawImage(image, new RectangleF(0, 50, 500, 300));
            MemoryStream ms = new MemoryStream();
            //Save the PDF document  
            document.Save(ms);
            ms.Position = 0;
            return Convert.ToBase64String(ms.ToArray());
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new ByteArrayContent(ms.ToArray());
            //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = "PDFDocument.pdf"
            //};
            //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            //return response;
        }
        private static async Task<Stream> GetImageStreamAsync()
        {
            HttpClient client = new HttpClient();
           return await client.GetStreamAsync("https://cdn.pixabay.com/photo/2015/04/23/22/00/tree-736885_1280.jpg");
            //return await HttpClient.GetStreamAsync("https://cdn.pixabay.com/photo/2015/04/23/22/00/tree-736885_1280.jpg");
            //https://cdn.pixabay.com/photo/2015/04/23/22/00/tree-736885_1280.jpg
            //https://avatars.githubusercontent.com/u/9141961
        }
    }
}
