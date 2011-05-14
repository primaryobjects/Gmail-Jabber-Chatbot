using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GmailChatBot.Configuration
{
    /// <summary>
    /// Custom configuration section for Chat bot configuration. Example:
    /// <botConfig>
    ///    <XMPP server="gmail.com" connectServer="talk.google.com" username="your_gmail_username" />
    /// </botConfig>
    /// </summary>
    public class BotConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("XMPP")]
        public XMPPElement XMPP
        {
            get
            {
                return (XMPPElement)base["XMPP"];
            }
        }
    }

    /// <summary>
    /// Custom configuration element XMPP. Example:
    /// </summary>
    public class XMPPElement : ConfigurationElement
    {
        [ConfigurationProperty("server", IsRequired=true)]
        public string Server
        {
            get
            {
                return (string)this["server"];
            }
        }

        [ConfigurationProperty("connectServer", IsRequired = true)]
        public string ConnectServer
        {
            get
            {
                return (string)this["connectServer"];
            }
        }

        /// <summary>
        /// Gmail Username for bot to connect with.
        /// </summary>
        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }
        }

        /// <summary>
        /// Gmail Password for bot to connect with.
        /// </summary>
        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
        }
    }
}
