using System;
using System.Diagnostics;

namespace PlayTimePackets
{
    public class Player
    {
        public Guid ID { get; private set; }
        public string Name { get; private set; }

        public Player(string name, bool bol)
        {
            this.Name = name;
            ID = Guid.Empty;
        }
        public Player(string name, Guid Id)
        {
            this.ID = Id;
            this.Name = name;
        }
        public Player(string name)
        {
            ID = Guid.NewGuid();
            Name = name;
        }
    }
}
