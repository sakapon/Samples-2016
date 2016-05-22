using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ToastConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // ToastTemplateType enumeration
            // https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.notifications.toasttemplatetype
            // Toast schema
            // https://msdn.microsoft.com/en-us/library/windows/apps/br230849

            // Gets the template.
            //var xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            //var text1 = (XmlElement)xml.GetElementsByTagName("text")[0];
            //text1.InnerText = "The Title";

            var @abstract = "This is the abstract.";
            var detail = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            // My favorites are Alarm4, Alarm10, Call2 and Call6.
            for (int i = 1; i <= 10; i++)
            {
                ShowToast($"Alarm {i}", @abstract, detail, GetAudioSourceForAlarm(i));
                Thread.Sleep(7000);
            }

            for (int i = 1; i <= 10; i++)
            {
                ShowToast($"Call {i}", @abstract, detail, GetAudioSourceForCall(i));
                Thread.Sleep(7000);
            }
        }

        static void ShowToast(string title, string @abstract, string detail, string audioSource)
        {
            var xmlText = $@"<toast><visual><binding template=""ToastText04"">
<text id=""1"">{title}</text>
<text id=""2"">{@abstract}</text>
<text id=""3"">{detail}</text>
</binding></visual>
<audio src=""{audioSource}"" />
</toast>";

            var xml = new XmlDocument();
            xml.LoadXml(xmlText);

            var toast = new ToastNotification(xml);
            var notifier = ToastNotificationManager.CreateToastNotifier("Toast Console");
            notifier.Show(toast);
        }

        static string GetAudioSourceForAlarm(int i) => $"ms-winsoundevent:Notification.Looping.Alarm{(i == 1 ? "" : i.ToString())}";
        static string GetAudioSourceForCall(int i) => $"ms-winsoundevent:Notification.Looping.Call{(i == 1 ? "" : i.ToString())}";
    }
}
