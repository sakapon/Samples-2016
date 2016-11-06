using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Newtonsoft.Json;

namespace EmotionBotApi
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        static string SubscriptionKey { get; } = ConfigurationManager.AppSettings["SubscriptionKey"];

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
            // In Slack, URLs are qualified by '<' and '>'.
            var text = activity.Text.Trim('<', '>');

            var echoMessage = $"You sent this picture.  \n![]({text})";
            await Reply(connector, activity, echoMessage);

            var mainMessage = await GetEmotionsAsync(text);
            await Reply(connector, activity, mainMessage);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        static async Task<string> GetEmotionsAsync(string text)
        {
            if (!Uri.IsWellFormedUriString(text, UriKind.Absolute)) return "Send the URL of a picture.";

            var emotions = await RecognizeEmotionsAsync(text);
            if (emotions == null) return "The emotions recognition failed.";
            if (emotions.Length == 0) return "Nobody is recognized.";

            var topScore = emotions[0].Scores
                .ToRankedList()
                .First();
            return $"{topScore.Key}: {topScore.Value:P1}";
        }

        static async Task<Emotion[]> RecognizeEmotionsAsync(string imageUrl)
        {
            var client = new EmotionServiceClient(SubscriptionKey);

            try
            {
                return await client.RecognizeAsync(imageUrl);
            }
            catch (Exception)
            {
                return null;
            }
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