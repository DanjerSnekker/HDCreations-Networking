using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class StartGamePacket : BasePacket
    {
        public string Start;

        public StartGamePacket()
        {
            Start = "";
        }
        public StartGamePacket(string start, Player player) :
            base(PacketType.StartGame, player)
        {
            Start = start;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(Start);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            Start = br.ReadString();
            return this;
        }
    }
}
