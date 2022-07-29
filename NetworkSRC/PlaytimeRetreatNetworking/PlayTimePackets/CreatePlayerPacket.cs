using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class CreatePlayerPacket : BasePacket
    {
        public string Name;
        public Guid Id;

        public CreatePlayerPacket()
        {

        }

        public CreatePlayerPacket(string name, Guid id, Player player) :
            base(PacketType.CreatePlayer, player)
        {
            this.Name = name;
            this.Id = id;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(Name);
            bw.Write(Id.ToString());
            return ms.ToArray();
        }

        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            Name = br.ReadString();
            Id = new Guid(br.ReadString());
            return this;
        }
    }
}

