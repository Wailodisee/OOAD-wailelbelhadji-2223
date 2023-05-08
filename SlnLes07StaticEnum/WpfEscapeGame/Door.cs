using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfEscapeGame
{
    class Door : Item
    {
        public Room DestinationRoom { get; set; }
        public bool IsOpen { get; set; } = false;
        public Item Key { get; set; }

        public Door(string name, string desc, Room destinationRoom, bool isOpen, Item key) : base(name, desc)
        {
            DestinationRoom = destinationRoom;
            IsOpen = isOpen;
            Key = key;
        }

        public void Open(Item key)
        {
            if (IsOpen)
            {
                Console.WriteLine($"{Name} is already open.");
            }
            else if (key == null || key != Key)
            {
                Console.WriteLine($"You don't have the right key to open {Name}.");
            }
            else
            {
                IsOpen = true;
                Console.WriteLine($"{Name} is now open.");
            }
        }

        public void Close()
        {
            if (IsOpen)
            {
                IsOpen = false;
                Console.WriteLine($"{Name} is now closed.");
            }
            else
            {
                Console.WriteLine($"{Name} is already closed.");
            }
        }
    }
}
