using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EPayroll_BE.Services.ThirdParty
{
    public class FirebaseCloudMessagingService : IFirebaseCloudMessagingService
    {
        private readonly string firebaseCloudMessagingKey = "";// The Legacy Key Server from FirebaseCloudMessaging
        private readonly string firebaseCloudMessagingUri = "https://fcm.googleapis.com/fcm/send";

        public void SendNotification(Dictionary<string, object> notification)
        {
            string content = JsonConvert.SerializeObject(notification);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + firebaseCloudMessagingKey);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());

                StringContent stringContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

                using (var response = httpClient.PostAsync(firebaseCloudMessagingUri, stringContent).GetAwaiter().GetResult())
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }

    public interface IFirebaseCloudMessagingService
    {
        void SendNotification(Dictionary<string, object> notification);
    }
}
