using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using static System.String;

namespace ConsoleApplication2
{
    // Object[] details = { Shop.Session, "attribute.list", new Object[] { true, null } };
    //var detailsJson = Shop.SendApiRequest("call", details);
    class Shoper
    {
        public Shoper(string login, string password)
        {
            Session = Login(login, password);
        }

        public Shoper()
        {
        }

        public string Session;
        //shop url
        public static string ShopUrl = "";

        public static object FromJson(string input)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<object>(input);
        }

        public static string ToJson(object input)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(input);
        }

        public object SendApiRequest(string method, object[] methodParams)
        {
            object o = null;
            var request = WebRequest.Create(ShopUrl + "/webapi/json/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var dataStream = request.GetRequestStream();

            var postParams = new Dictionary<string, object> {{"method", method}, {"params", methodParams}};

            var jsonEncodedParams = ToJson(postParams);
            var encoding = new System.Text.UTF8Encoding();
            var byteArray = encoding.GetBytes("json=" + jsonEncodedParams);

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var webResponse = request.GetResponse();
            var responseStream = webResponse.GetResponseStream();
            if (responseStream != null)
            {
                var reader = new StreamReader(responseStream);
                var text = reader.ReadToEnd();

                var response = FromJson(text);
                return response;
            }
            return o;
        }

        private static string Login(string login, string password)
        {
            object[] methodParams = {login, password};
            var shop = new Shoper();
            var response = shop.SendApiRequest("Login", methodParams);
            string session = null;

            var objects = response as Dictionary<string, object>;
            if (objects != null)
            {
                var d = objects;
                if (d.ContainsKey("error"))
                {
                    Console.WriteLine("Wystąpił błąd: {0}, kod: {1}", d["error"], d["code"]);
                }
            }
            else if (response is string)
            {
                session = (string) response;
            }
            return session;
        }

        public void SaveToJson(object list, string name)
        {
            var jsonSerialiser = new JavaScriptSerializer {MaxJsonLength = 2147483644};
            var jsonItems = jsonSerialiser.Serialize(list);
            File.WriteAllText(name, jsonItems);
        }

        /// <summary>
        /// List<Category> products = JsonConvert.DeserializeObject<List<Category>>(json);
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string OpenFromJson(string name)
        {
            string jsonString = Empty;

            using (StreamReader r = new StreamReader(name))
            {
                jsonString = r.ReadToEnd();
            }
            return jsonString;
        }

        public string UrlToBase64(string url)
        {
            var webClient = new WebClient();
            var img = Empty;
            try
            {
                byte[] imageBytes = webClient.DownloadData(url);
                if (imageBytes.Length > 0)
                {
                    img = Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return img;
        }
    } 
}