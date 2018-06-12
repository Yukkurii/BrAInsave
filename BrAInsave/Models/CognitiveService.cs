using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;

namespace BrAInsave.Models
{
    public class CognitiveServiceConnection
    {
        public static string subscriptionKey = "1617d8c5cf1145fcabe716e600b6b6ae";
        public static string baseURI = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/";
    }

    public static class CognitiveService
    {
        public static string FaceDetction(string imgURL)
        {
            string imageWithFaces = "{\"url\":\"" + imgURL + "\"}";
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var faceAttributes = "age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", CognitiveServiceConnection.subscriptionKey);

            // Request parameters
            queryString["returnFaceId"] = "true";
            queryString["returnFaceLandmarks"] = "false";
            queryString["returnFaceAttributes"] = faceAttributes;
            var uri = CognitiveServiceConnection.baseURI + "detect?" + queryString;


            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(imageWithFaces);

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;//创建请求对象
            request.Method = "POST";//请求方式
            request.ContentType = "application/json";//链接类型

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            string responseString = new StreamReader(responseStream, System.Text.Encoding.Default).ReadToEnd();

            return responseString;
        }
    }
}