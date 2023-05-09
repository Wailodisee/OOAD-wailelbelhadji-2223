using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    class LockableItem : Item
    {
        public bool IsLocked { get; set; } = true;
        public Item Key { get; set; }

        public LockableItem(string name, string desc) : base(name, desc) { }
        public LockableItem(string name, string desc, bool isPortable = true) : base(name, desc, isPortable) { }

        public bool TryUnlock(Item key)
        {
            if (key == Key)
            {
                IsLocked = false;
                return true;
            }
            return false;
        }
    }
}
