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
        public string OwnerID { get; set; }

        public string prefabName { get; set; }

        public InstantiateObjPacket()
        {
            prefabName = "";
        }

        public InstantiateObjPacket(string prefabName, string ownerid ,string objID) :
            base(PacketType.Instantiate, objID)
        {
            
            this.prefabName = prefabName;
            this.OwnerID = ownerid;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(OwnerID);
            bw.Write(prefabName);
            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            OwnerID = br.ReadString();
            prefabName = br.ReadString();
            //OwnerID = Guid.Parse(br.ReadString());
            //OwnerID = new Guid (br.ReadString());
            return this;
        }
    }
}
