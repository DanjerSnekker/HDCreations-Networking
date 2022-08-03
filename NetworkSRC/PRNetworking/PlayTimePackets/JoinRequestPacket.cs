using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public  class JoinRequestPacket : BasePacket
    {
        public string RoomName;
        public int RoomCode;
        public string RequestYN; // this is for the request response in case the code inputted was incorrect

        public JoinRequestPacket()
        {
            RoomName = "";
            RoomCode = 0;
            RequestYN = "";
        }
        public JoinRequestPacket(string roomname, int roomcode,string requestyn ,Player player) :
            base(PacketType.JoinRequest, player)
        {
            RoomName = roomname;
            RoomCode = roomcode;
            RequestYN = requestyn;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(RoomName);
            bw.Write(RoomCode);
            bw.Write(RequestYN);
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);
            RoomName = br.ReadString();
            RoomCode = br.ReadInt32();
            RequestYN = br.ReadString();
            return this;
        }
    }
}
