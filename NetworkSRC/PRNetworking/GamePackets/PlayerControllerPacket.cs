using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class PlayerControllerPacket : GameBasePacket
    {
        public int playerIntType;

        public Vector3 position;
        /*public Vector3 movement;
        public Vector3 velocity;*/

        public PlayerControllerPacket()
        {
            //playerType = PlayerType.Unkown;
            playerIntType = -1;
            position = Vector3.zero;
            //velocity = Vector3.zero;
        }

        public PlayerControllerPacket(int type, string objID, Vector3 position) :
            base(PacketType.PlayerController, objID)
        {
            //playerType = (PlayerType) type;
            playerIntType = type;

            this.position = position;
            /*movement = move;
            velocity = moveVelocity;*/
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            //bw.Write((int)playerType);
            bw.Write((int)playerIntType);      //Might be dumb, but this is so I can access the correct data.

            bw.Write(position.x);
            bw.Write(position.y);
            bw.Write(position.z);

            /*bw.Write(movement.x);
            bw.Write(movement.y);
            bw.Write(movement.z);

            bw.Write(velocity.x);
            bw.Write(velocity.y);
            bw.Write(velocity.z);*/
            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            //playerType = (PlayerType) br.ReadInt32();
            playerIntType = br.ReadInt32();

            position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            /*movement = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            velocity = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());*/

            return this;
        }
    }
}
