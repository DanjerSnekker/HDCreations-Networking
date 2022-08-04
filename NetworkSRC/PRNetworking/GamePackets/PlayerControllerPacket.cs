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
        public int ownerID { get; set; }

        /*public enum PlayerType
        {
            Unkown = -1,
            Host,
            Partner
        }

        public PlayerType playerType;*/

        public int playerIntType;

        public bool isGrounded;
        public Vector3 velocity;

        public PlayerControllerPacket()
        {
            //playerType = PlayerType.Unkown;
            playerIntType = -1;
            isGrounded = true;
            velocity = Vector3.zero;
        }

        public PlayerControllerPacket(int type, int objID, bool grounded, Vector3 moveVelocity) :
            base(PacketType.PlayerController, objID)
        {
            //playerType = (PlayerType) type;
            playerIntType = type;
            isGrounded = grounded;
            velocity = moveVelocity;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            //bw.Write((int)playerType);
            bw.Write((int)playerIntType);      //Might be dumb, but this is so I can access the correct data.
            bw.Write(isGrounded);
            bw.Write(velocity.x);
            bw.Write(velocity.y);
            bw.Write(velocity.z);
            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            //playerType = (PlayerType) br.ReadInt32();
            playerIntType = br.ReadInt32();
            isGrounded = br.ReadBoolean();
            velocity = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            return this;
        }
    }
}
