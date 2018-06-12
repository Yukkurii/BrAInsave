using BrAInsave.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BrAInsave.Controllers
{
    public class HomeController : Controller
    {
        public class CognitiveServiceConnection
        {
            public static string subscriptionKey = "1617d8c5cf1145fcabe716e600b6b6ae";
            public static string baseURI = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/";
        }

        public async Task<ActionResult> Index()
        {
            string imgURL = "https://jiafengtrystorage.blob.core.windows.net/samples-workitems/patient1";
            string imageWithFaces = "{\"url\":\"" + imgURL + "\"}";
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var faceAttributes = "age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", CognitiveServiceConnection.subscriptionKey);

            HttpResponseMessage response;

            // Request parameters
            queryString["returnFaceId"] = "true";
            queryString["returnFaceLandmarks"] = "false";
            queryString["returnFaceAttributes"] = faceAttributes;
            var uri = CognitiveServiceConnection.baseURI + "detect?" + queryString;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(imageWithFaces);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }
            string result = await response.Content.ReadAsStringAsync();

            ViewBag.faceDetectionResult = result;
            ViewBag.imgURL = imgURL;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}