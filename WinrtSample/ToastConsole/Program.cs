using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ToastConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Gets Template.
            //var xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            //var text1 = (XmlElement)xml.GetElementsByTagName("text")[0];
            //text1.InnerText = "The Title";

            var xmlText = @"<toast><visual><binding template=""ToastText04"">
<text id=""1"">The Title</text>
<text id=""2"">This is the abstract.</text>
<text id=""3"">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</text>
</binding></visual></toast>";

            var xml = new XmlDocument();
            xml.LoadXml(xmlText);

            var toast = new ToastNotification(xml);
            var notifier = ToastNotificationManager.CreateToastNotifier("Toast Console");
            notifier.Show(toast);
        }
    }
}
