using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIMLbot;
using System.Collections;

namespace GmailChatBot
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class CommonManager
    {
        private static Hashtable _userSessions = new Hashtable();

        /// <summary>
        /// Returns a User from cache or creates a new one, based upon their Gmail username, allowing the bot to track conversations per user.
        /// </summary>
        /// <param name="jid">unique string ID (gmail username)</param>
        /// <returns>User</returns>
        public static User GetUser(string jid, Bot bot)
        {
            // Maintain sticky sessions with specific users, allows bot to remember them.
            Hashtable myHash = Hashtable.Synchronized(_userSessions);
            User user = (User)myHash[jid];

            if (user == null)
            {
                user = new User(jid, bot);
                myHash[jid] = user;
            }

            return user;
        }

        /// <summary>
        /// Gets a password from the Console user. Hides typing for basic security.
        /// </summary>
        /// <param name="username">Gmail Username</param>
        /// <returns>Password</returns>
        public static string GetPasswordFromConsole(string username)
        {
            Console.Write("Please enter the Gmail password for " + username + ":");

            // Hide typing.
            ConsoleColor oldFore = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;

            string password = Console.ReadLine();

            // Restore typing.
            Console.ForegroundColor = oldFore;

            return password;
        }
    }
}
