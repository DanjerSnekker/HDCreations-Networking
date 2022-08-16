using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace PlayTimePackets
{
    public class LeaveRequestPacket : BasePacket
    {
        public bool isHost;
        public string Name;

        public LeaveRequestPacket()
        {
            isHost = false;
            Name = "";
        }
        public LeaveRequestPacket(string name, bool ishost,Player player) :
            base(PacketType.LeaveLobby, player)
        {
            isHost = ishost;
            Name = name;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(isHost);
            bw.Write(Name);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            isHost = br.ReadBoolean();
            Name = br.ReadString();
            return this;
        }
    }
}

