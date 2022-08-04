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

        public TriggerPacket()
        {
            triggerActive = false;
        }

        public TriggerPacket(string objID, bool activeTrigger) :
            base(PacketType.Trigger, objID)
        {
            triggerActive = activeTrigger;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(triggerActive);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            triggerActive = br.ReadBoolean();

            return this;
        }
    }
}
