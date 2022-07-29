using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class MessagePacket : BasePacket
    {

        public string Message { get; private set; }
        public Player player { get; private set; }

        public MessagePacket()
        {
            Message = "";
        }
        public MessagePacket(string message, Player player) :
            base(PacketType.Message, player)
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
            base.DeSerialize(buffer);

            Message = br.ReadString();

            return this;
        }
    }
}

