using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class StartGamePacket : BasePacket
    {
        public int Gameport;

        public StartGamePacket()
        {
            Gameport = 0;
        }
        public StartGamePacket(int start, Player player) :
            base(PacketType.StartGame, player)
        {
            Gameport = start;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(Gameport);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            Gameport = br.ReadInt32();
            return this;
        }
    }
}
