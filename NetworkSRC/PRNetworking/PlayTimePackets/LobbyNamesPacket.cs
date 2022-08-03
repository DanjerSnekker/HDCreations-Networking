using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class LobbyNamesPacket : BasePacket
    {
         public List<string> LobbyNames;
        //public string LobbyName;
        //public int RoomCode;
        public Player Player;

        public LobbyNamesPacket()
        {
           // LobbyName = "";
        }

        public LobbyNamesPacket(List<string> lobbyNames/*string lobbyNames,int roomcodes*/,Player player) :
            base(PacketType.LobbyName, player)
        {
            LobbyNames = lobbyNames;
            /*LobbyName = lobbyNames;
            RoomCode = roomcodes;*/
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(LobbyNames.Count);

            for (int i = 0; i < LobbyNames.Count; i++)
            {
                bw.Write(LobbyNames[i]);
            }

            //bw.Write(LobbyName);
     
            return ms.ToArray();
        }

        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            int count = br.ReadInt32();

            LobbyNames = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                LobbyNames.Add(br.ReadString());
            }
            
           // LobbyName = br.ReadString();

            return this;
        }
    }
}
