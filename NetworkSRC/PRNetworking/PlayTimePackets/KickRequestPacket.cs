using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public  class KickRequestPacket : BasePacket
    {
        public string Request;

        public KickRequestPacket()
        {
            Request = "";
        }
        public KickRequestPacket(string name, Player player) :
            base(PacketType.KickRequest, player)
        {
            Request = name;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(Request);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            Request = br.ReadString();
            return this;
        }
    }
}
