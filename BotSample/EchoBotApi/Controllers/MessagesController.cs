using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace EchoBotApi
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type != ActivityTypes.Message)
                return Request.CreateResponse(HttpStatusCode.OK);
            if (string.IsNullOrWhiteSpace(activity.Text))
                return Request.CreateResponse(HttpStatusCode.OK);

            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            var echoMessage = $"You sent \"{activity.Text}\".";
            await Reply(connector, activity, echoMessage);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        static async Task Reply(ConnectorClient connector, Activity activity, string message)
        {
            var reply = activity.CreateReply(message);
            await connector.Conversations.ReplyToActivityAsync(reply);
        }

        Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}