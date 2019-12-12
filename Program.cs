using programm.Extensions;
using programm.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace programm
{
    public class Program
    {
        static void Main(string[] args)
        {
            string title = string.Empty;
            string body = string.Empty;
            string installId = string.Empty;

            char sendChoise = 'n';

            Console.WriteLine("----- Visual Studio App Center-----");
            Console.WriteLine("Welcome to push notification sender");

            Console.WriteLine("\nPlease enter the Title of  Push notification <Title> ");
            title = Console.ReadLine();

            Console.WriteLine("\nPlease enter the content of  Push notification <Body/Message> ");
            body = Console.ReadLine();

            Console.WriteLine("\nPlease enter the device you want to use <installId>");
            installId = Console.ReadLine();

            Console.WriteLine("Do you want to send it ??  y/n");
            sendChoise = Console.ReadKey().KeyChar;

            if (sendChoise == 'y' || sendChoise == 'Y')
              {
                _ = SendPushNotification(title, body, installId).ContinueWith(async res =>
                  {
                      if (await res)
                      {
                          Console.WriteLine("--------");
                          Console.WriteLine("success");

                      }
                  });
              }
            else
            {
                Console.WriteLine("ok");
            }
            Console.ReadLine();
        }

        private static async Task<bool> SendPushNotification(string title, string body, string installId)
        {
            

            NotificationTarget notificationTarget = new NotificationTarget()
            {
                devices = new List<string>()
                {
                    installId
                },
                type = "devices_target"
            };

            NotificationContent notificationContent = new NotificationContent()
            {
                body = body,
                name = "Test notification",
                title = title,

            };

            pushNotification pushNotification = new pushNotification()
            {
                notification_content = notificationContent,
                notification_target = notificationTarget,
            };

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-API-Token", "f91e1f79bf5eaa0faeee9d67d1a4c56f16ee8e8a");

            StringContent stringContent = pushNotification.AsJson();

            try
            {
                
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("https://api.appcenter.ms/v0.1/apps/michalis_anagnostou-hotmail.com/Notification/push/notifications", stringContent);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                    return true;
                    }
                    else if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                       var json = await httpResponseMessage.Content.ReadAsStringAsync();
                       return false;
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine("there was an error");
                return false;
            }

            return false;
        }
    }
}
