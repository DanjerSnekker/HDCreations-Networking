using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class InstantiateObjPacket : GameBasePacket
    {
        public string ownershipID;
        public string prefabName;

        public InstantiateObjPacket()
        {
            prefabName = "";
            ownershipID = "";
        }

        public InstantiateObjPacket(string prefabName, string ownerID, string objID) : //Enter either "Player1" OR "Player2" in objID.
            base(PacketType.Instantiate, objID)
        {
            this.prefabName = prefabName;
            ownershipID = ownerID;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(prefabName);
            bw.Write(ownershipID);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            prefabName = br.ReadString();
            ownershipID = br.ReadString();

            return this;
        }
    }
}
