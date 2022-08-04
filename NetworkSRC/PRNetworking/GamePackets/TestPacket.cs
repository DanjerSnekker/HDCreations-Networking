using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class TestPacket : GameBasePacket
    {
        public Vector3 objPos;

        public TestPacket()
        {
            objPos = Vector3.zero;
        }

        public TestPacket(Vector3 position, string objID) :
            base(PacketType.None, objID)
        {
            objPos = position;
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            //bruh
            bw.Write(objPos.x);
            bw.Write(objPos.y);
            bw.Write(objPos.z);

            return ms.ToArray();
        }
        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            objPos = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            return this;
        }
    }
}