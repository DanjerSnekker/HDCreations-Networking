using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class UsernamePacket : BasePacket
    {
        public string HostName;
        public string PartnerName;
        public UsernamePacket()
        {
            HostName = "";
            PartnerName = "";
        }
        public UsernamePacket(string hostname, string partnername,Player player) :
            base(PacketType.Usernames, player)
        {
            HostName = hostname;
            PartnerName = partnername;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(HostName);
            bw.Write(PartnerName);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            HostName = br.ReadString();
            PartnerName = br.ReadString();
            return this;
        }
    }
}
