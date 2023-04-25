using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    public enum MessageType { NoItem, Keys, DoubleItem }
    internal class RandomMessageGenerator
    {
        static Random random1 = new Random();

        const int MesssagesError = 3;

        //Errors Message declareren
        static string[] NoItemError = { "You don't have any Item in your listItems ... Take something with you maybe ..." };
        static string[] KeysError = { "You don't have access to these Keys." };
        static string[] DoubleItemError = { "You have already take this Item in your Items." };

        public static string GetRandomMessage(MessageType Element)
        {
            int E = random1.Next(0, MesssagesError);

            switch (Element)
            {
                case MessageType.Keys: return NoItemError[E];

                case MessageType.NoItem: return KeysError[E];

                case MessageType.DoubleItem: return DoubleItemError[E];

                default: return "//";
            }
        }
    }
}
