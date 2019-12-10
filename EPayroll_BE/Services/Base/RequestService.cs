using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EPayroll_BE.Services.Base
{
    public class RequestService : IRequestService
    {
        public TResult Get<TResult>(string uri, string token)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var response = httpClient.GetAsync(new Uri(uri)).GetAwaiter().GetResult())
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<TResult>(content);
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public TResult Post<TResult>(string uri, object dataModel, string token)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(dataModel));
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = httpClient.PostAsync(new Uri(uri), stringContent).GetAwaiter().GetResult())
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<TResult>(content);
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public TResult Put<TResult>(string uri, object dataModel, string token)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(dataModel));
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = httpClient.PutAsync(new Uri(uri), stringContent).GetAwaiter().GetResult())
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<TResult>(content);
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public TResult Delete<TResult>(string uri, string token)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var response = httpClient.DeleteAsync(new Uri(uri)).GetAwaiter().GetResult())
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<TResult>(content);
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }
    }

    public interface IRequestService
    {
        TResult Get<TResult>(string uri, string token);
        TResult Post<TResult>(string uri, object dataModel, string token);
        TResult Put<TResult>(string uri, object dataModel, string token);
        TResult Delete<TResult>(string uri, string token);
    }
}
