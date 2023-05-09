using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    class Key : Item
    {
        public LockableItem OpensLock { get; set; }

        public Key(string name, string desc, LockableItem opensLock) : base(name, desc)
        {
            OpensLock = opensLock;
        }
    }
}
