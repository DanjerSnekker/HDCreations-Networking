using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class DisplayLobbiesPacket : BasePacket
    {
        public string Message;
        public DisplayLobbiesPacket()
        {
            Message = "";
        }
        public DisplayLobbiesPacket(string message, Player player) :
            base(PacketType.DisplayLobby, player)
        {
            Message = message;
        }
        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(Message);
            return ms.ToArray();
        }

        public override BasePacket DeSerialize(byte[] buffer)
        {
            return base.DeSerialize(buffer);
            Message = br.ReadString();
        }
    }
}

