using System;
using System.Collections;
using System.Configuration;
using System.IO;
using agsXMPP;
using GmailChatBot.Configuration;
using AIMLbot;

namespace GmailChatBot
{
    class Program
    {
        private static BotConfigSection _botConfiguration = (BotConfigSection)ConfigurationManager.GetSection("botConfig");
        private static XmppClientConnection _xmppConnection = new XmppClientConnection();
        private static AIMLChatBot _bot = new AIMLChatBot();

        static void Main(string[] args)
        {
            // Initialize and open the XMPP chat bot connection.
            SetupXMPP();

            Console.WriteLine("Logging in to Gmail ..");

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Initialize XMPP connection.
        /// </summary>
        private static void SetupXMPP()
        {
            // Open connection to Gmail/GTalk.
            _xmppConnection.Server = _botConfiguration.XMPP.Server;
            _xmppConnection.ConnectServer = _botConfiguration.XMPP.ConnectServer;
            _xmppConnection.Username = _botConfiguration.XMPP.Username;
            _xmppConnection.Password = _botConfiguration.XMPP.Password;

            // If a password was not present in the app.config, ask the console user for it.
            if (_xmppConnection.Password.Length == 0)
            {
                _xmppConnection.Password = CommonManager.GetPasswordFromConsole(_botConfiguration.XMPP.Username);
            }

            _xmppConnection.Open();

            // Setup event handlers.
            _xmppConnection.OnLogin += new ObjectHandler(xmpp_OnLogin);
            _xmppConnection.OnError += new ErrorHandler(xmpp_OnError);
            _xmppConnection.OnMessage += new agsXMPP.protocol.client.MessageHandler(xmpp_OnMessage);
        }

        #region Event Handlers

        private static void xmpp_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            if (!string.IsNullOrEmpty(msg.Body))
            {
                // This is called for any message received.
                Console.WriteLine("Received: " + msg.Body);

                // Get the Gmail username.
                agsXMPP.Jid JID = new Jid(msg.From.Bare);

                // Get the context User from the Gmail username (allows our bot to track conversations per user).
                User user = CommonManager.GetUser(msg.From.Bare, _bot);

                // Let our chat bot respond to the message.
                string response = HandleMessage(msg.Body, user);

                // Setup a response event.
                _xmppConnection.MessageGrabber.Add(JID, new agsXMPP.Collections.BareJidComparer(), new MessageCB(ChatResponseReceived), null);

                // Create a new message.
                agsXMPP.protocol.client.Message newmsg = new agsXMPP.protocol.client.Message();
                newmsg.Type = agsXMPP.protocol.client.MessageType.chat;
                newmsg.To = JID;
                newmsg.Body = response;

                // Send response.
                _xmppConnection.Send(newmsg);
            }
        }

        private static void xmpp_OnError(object sender, Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private static void xmpp_OnLogin(object sender)
        {
            Console.WriteLine("Login succeeded! Listening for connections.");
        }

        private static void ChatResponseReceived(object sender, agsXMPP.protocol.client.Message msg, object data)
        {
            // This is called when a message is received after we've initiated a chat with someone.
            // Do nothing, we'll respond in the OnMessage event.
        }

        #endregion

        /// <summary>
        /// Returns a response from the message received.
        /// The response comes from our AIML chat bot or from our own custom processing.
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="user">User (for context)</param>
        /// <returns>string</returns>
        private static string HandleMessage(string message, User user)
        {
            string output = "";

            if (!string.IsNullOrEmpty(message))
            {
                // Provide custom commands for our chat bot, such as disk space, utility functions, typical IRC bot features, etc.
                if (message.ToUpper().IndexOf("DISK SPACE") != -1)
                {
                    DriveInfo driveInfo = new DriveInfo("C");
                    output = "Available disk space on " + driveInfo.Name + " is " + driveInfo.AvailableFreeSpace + ".";
                }
                else if (message.ToUpper().IndexOf("DISK SIZE") != -1)
                {
                    DriveInfo driveInfo = new DriveInfo("C");
                    output = "The current disk size on " + driveInfo.Name + " is " + driveInfo.TotalSize + ".";
                }
                else
                {
                    // No recognized command. Let our chat bot respond.
                    output = _bot.getOutput(message, user);
                }
            }

            return output;
        }
    }
}
