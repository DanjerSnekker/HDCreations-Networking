using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class TriggerPacket : GameBasePacket
    {
        public bool triggerActive;
        public bool PressedActive;

        public TriggerPacket()
        {
            PressedActive = false;
            triggerActive = false;
        }

        public TriggerPacket(string objID, bool pressedactive, bool activeTrigger) :
            base(PacketType.Trigger, objID)
        {
            PressedActive = pressedactive;
            triggerActive = activeTrigger;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(PressedActive);
            bw.Write(triggerActive);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            PressedActive = br.ReadBoolean();
            triggerActive = br.ReadBoolean();

            return this;
        }
    }
}
