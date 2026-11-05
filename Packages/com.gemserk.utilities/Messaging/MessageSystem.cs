using System;
using System.Collections.Generic;

namespace Gemserk.Utilities.Messaging
{
    public static class MessageSystem
    {
        public struct MessageArgs
        {
            public object sender;
            // public object target;
            public string message;
            public string parameter;
        }
        
        public interface IMessageListener
        {
            void OnMessage(MessageArgs messageArgs);
        }
        
        [Serializable]
        public struct MessageInfo
        {
            public string message;
            public string parameter;
        }

        private struct MessageListenerInfo
        {
            public IMessageListener listener;
            public MessageInfo messageInfo;
        }

        private static readonly List<MessageListenerInfo> MessageListeners = new List<MessageListenerInfo>();

        public static void RegisterListener(IMessageListener listener, string message, string parameter)
        {
            MessageListeners.Add(new MessageListenerInfo()
            {
                listener = listener,
                messageInfo = new MessageInfo()
                {
                    message = message,
                    parameter = parameter
                }
            });
        }

        public static void UnregisterListener(IMessageListener listener, string message, string parameter)
        {
            MessageListeners.RemoveAll(info =>
            {
                if (info.listener != listener)
                    return false;

                if (!string.IsNullOrEmpty(message) &&
                    !info.messageInfo.message.Equals(message, StringComparison.InvariantCultureIgnoreCase))
                    return false;

                if (!string.IsNullOrEmpty(parameter) &&
                    !info.messageInfo.parameter.Equals(parameter, StringComparison.InvariantCultureIgnoreCase))
                    return false;

                return true;
            });
        }

        public static void SendMessage(object sender, string message, string parameter = null)
        {
            foreach (var messageListenerInfo in MessageListeners)
            {
                if (!messageListenerInfo.messageInfo.message.Equals(message,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(messageListenerInfo.messageInfo.parameter))
                {
                    messageListenerInfo.listener.OnMessage(new MessageArgs()
                    {
                        sender = sender,
                        message = message,
                        parameter = parameter
                    });
                    continue;
                }

                if (messageListenerInfo.messageInfo.parameter.Equals(parameter,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    messageListenerInfo.listener.OnMessage(new MessageArgs()
                    {
                        sender = sender,
                        message = message,
                        parameter = parameter
                    });
                }
            }
        }
    }
}