using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class PlayerShutDownPacket : BasePacket
    {
        public string NullString;

        public PlayerShutDownPacket()
        {
            NullString = "";
        }
        public PlayerShutDownPacket(string nullstring, Player player) :
            base(PacketType.PlayerShutDown, player)
        {
            NullString = nullstring;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(NullString);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            NullString = br.ReadString();
            return this;
        }
    }
}
