using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using Capture = Emgu.CV.Capture;

namespace TourAgency.Utilities
{
    public static class NeironStats
    {
       public static void Start()
       {
           Session.CurrentSession.IsCameraUsed = true;
           myCapture = new Capture();
           myCapture.ImageGrabbed += MyCaptureOnImageGrabbed;
           myCapture.Start();
        }

        private static  void MyCaptureOnImageGrabbed(object sender, EventArgs eventArgs)
        {
            try
            {
                myCapture.ImageGrabbed -= MyCaptureOnImageGrabbed;
                using (Mat image = new Mat())
                {

                    myCapture.Retrieve(image);

                    //Image<Bgr, byte> img = image.ToImage<Bgr, byte>();
                    //Ищем признаки лица
                    var Faces = Cascade.DetectMultiScale(image, 1.5);
                    foreach (var face in Faces)
                    {
                         MakeDetectGender();
                        StatisticStop();
                        return;
                    }

                    myCapture.ImageGrabbed += MyCaptureOnImageGrabbed;
                }

            }
            catch 
            {
             //
            }
        }

        public static void StatisticStop()
        {
            try
            {

            myCapture.ImageGrabbed -= MyCaptureOnImageGrabbed;
            myCapture.Stop();
                myCapture.Dispose();

            }
            catch (Exception e)
            {
                
            }
            Session.CurrentSession.IsCameraUsed = false;
        }

        private static Capture myCapture;
        private static CascadeClassifier Cascade = new CascadeClassifier("haarcascade_frontalface_alt2.xml");

        private static async Task MakeDetectGender()
        {
            HttpClient hc = new HttpClient();
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36 OPR/49.0.2725.64");
            hc.DefaultRequestHeaders.Add("X-Requested-With","XMLHttpRequest");
           StreamContent sc = new StreamContent(new MemoryStream(File.ReadAllBytes("Content/statistic.png")));
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            await await hc
                .PostAsync("https://www.how-old.net/Home/Analyze?isTest=False&source=&version=www.how-old.net", sc)
                .ContinueWith(async
                    r =>
                {
                    if (r.IsFaulted) return;
                    string result = await r.Result.Content.ReadAsStringAsync();
                    result = result.Replace("\\\\r\\\\n", String.Empty).Replace("\\", String.Empty);
                    result = result.Substring(1, result.Length - 2);
                    var allGenders = Regex.Matches(result, "(?<=\"gender\": \").*?(?=\")");
                    if(allGenders.Count !=0)
                        foreach (Match allGender in allGenders)
                        {
                           await WriteTabe(allGender.Value.ToLower());
                        }
                });
        }


        private static async Task WriteTabe(string gender)
        {
            HttpClient hc = new HttpClient();
            await hc.GetAsync(
                $"http://195.133.1.197/api/statistic/putstat?gender={gender}&terminalname={Session.CurrentSession.NameTerminal}").ContinueWith(async  r=>
            {
                string res = await r.Result.Content.ReadAsStringAsync();
            });

        }

    }

   
    
}
