using System;
using AIMLbot;

namespace GmailChatBot
{
    public class AIMLChatBot : Bot
    {
        public AIMLChatBot()
        {
            Initialize();
        }

        /// <summary>
        /// Loads AIML files located in the AIML folder.
        /// </summary>
        private void Initialize()
        {
            loadSettings();
            isAcceptingUserInput = false;
            loadAIMLFromFiles();
            isAcceptingUserInput = true;
        }

        /// <summary>
        /// Given an input string, return an output from the bot.
        /// </summary>
        /// <param name="input">string</param>
        /// <param name="user">User (so conversations can be tracked by the bot, per user)</param>
        /// <returns>string</returns>
        public String getOutput(String input, User user)
        {
            Request r = new Request(input, user, this);
            Result res = Chat(r);

            return (res.Output);
        }
    }
}