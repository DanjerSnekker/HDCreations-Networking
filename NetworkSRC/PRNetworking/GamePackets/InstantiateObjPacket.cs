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
        public int OwnerID { get; set; }

        public string prefabName { get; set; }

        public InstantiateObjPacket()
        {
            prefabName = "";
        }

        public InstantiateObjPacket(string prefabName, string objID) :
            base(PacketType.Instantiate, objID)
        {
            
            this.prefabName = prefabName;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(prefabName);
            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            prefabName = br.ReadString();
            return this;
        }
    }
}
