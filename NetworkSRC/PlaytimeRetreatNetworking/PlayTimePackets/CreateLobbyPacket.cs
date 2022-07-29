using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class CreateLobbyPacket : BasePacket
    {
        public string Name;

        public CreateLobbyPacket()
        {
            Name = "";
        }
        public CreateLobbyPacket(string name, Player player) :
            base(PacketType.CreateLobby, player)
        {
            Name = name;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(Name);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            Name = br.ReadString();
            return this;
        }
    }
}

