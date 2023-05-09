using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    class Item : Actor
    {
        public bool IsPortable { get; set; }
        public bool IsLocked { get; set; } = false;
        public Item Key { get; set; }
        public Item HiddenItem { get; set; }
        public Room DestinationRoom { get; internal set; }
        public bool IsOpen { get; internal set; }

        public Item(string name, string desc) : base(name, desc) { }

        public Item(string name, string desc, bool isPortable = true) : base(name, desc)
        {
            IsPortable = isPortable;
        }

        public override string ToString()
        {
            return Name;
        }

        internal void Open(Item selectedItem)
        {
            throw new NotImplementedException();
        }
    }
}
