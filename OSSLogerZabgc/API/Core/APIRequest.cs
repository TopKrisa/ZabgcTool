using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OSSLogerZabgc.API.Core
{
    public class APIRequest<T> where T : class
    {
        readonly string Table;
        readonly string Key;
        public APIRequest(DataTableNames.Tables NameTables, Keys.Api key)
        {
            Table = new DataTableNames(NameTables).DataTableName;
            Key = new Keys(key).Key;
        }

       public async Task<T> GetDataById(int id)
        {
            try
            {

                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(
                 $"https://zabgc.zabedu.ru/api/{Table}/{id}={Key}");
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                Stream stream = response.GetResponseStream();

                string json = new StreamReader(stream).ReadToEnd();

                return JsonConvert.DeserializeObject<T>(json);

            }
            catch (Exception)
            {
                return default;
            }

        }

        public async Task<List<T>> GetData()
        {
            try
            {
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(
                 $"https://zabgc.zabedu.ru/api/{Table}/={Key}");
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string json = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
            }
            catch (Exception)
            {

                return default;
            }
        }
        public async Task<string> DeleteData(int id)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://zabgc.zabedu.ru/api/{Table}/{id}={Key}");
                httpWebRequest.Method = "DELETE";
                HttpWebResponse httpResponse = (HttpWebResponse) await  Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse,httpWebRequest.EndGetResponse,null);
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }

        public async Task<HttpWebResponse> EditData(T ObjectData, int id)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://zabgc.zabedu.ru/api/{Table}/{id}={Key}");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PATCH";
                string Json = JsonConvert.SerializeObject(ObjectData);
                using (StreamWriter streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {
                    await streamWriter.WriteAsync(Json);
                }
                return (HttpWebResponse)httpWebRequest.GetResponse();

            }
            catch (Exception)
            {

                return default;
            }
        }

        public async Task<HttpWebResponse> AddData(T ObjectData)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://zabgc.ru/api/{Table}/={Key}");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                string json = JsonConvert.SerializeObject(ObjectData);
                using (StreamWriter streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {
                    streamWriter.Write(json);
                }
                return (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception)
            {
                return default;
            }
        }


    }
}
