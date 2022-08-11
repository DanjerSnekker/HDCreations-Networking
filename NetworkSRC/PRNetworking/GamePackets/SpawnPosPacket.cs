using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class SpawnPosPacket : GameBasePacket
    {
        public Guid playerID;
        public Vector3 spawnPos;

        public SpawnPosPacket()
        {
            spawnPos = Vector3.zero;
        }

        public SpawnPosPacket(Guid objID, Vector3 spawnPosition) :
            base(PacketType.PlayerSpawn, objID.ToString())
        {
            //playerType = (PlayerType) type;
            //playerIntType = type;

            spawnPos = spawnPosition;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            //bw.Write((int)playerType);
            //bw.Write((int)playerIntType);      //Might be dumb, but this is so I can access the correct data.

            bw.Write(spawnPos.x);
            bw.Write(spawnPos.y);
            bw.Write(spawnPos.z);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            //playerType = (PlayerType) br.ReadInt32();
            //playerIntType = br.ReadInt32();

            spawnPos = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            return this;
        }
    }
}
