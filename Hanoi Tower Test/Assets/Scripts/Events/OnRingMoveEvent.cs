using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mikabrytu.HanoiTower.Events
{
    public class OnRingMoveEvent : BaseEvent
    {
        public Transform ring;

        public OnRingMoveEvent(Transform ring)
        {
            this.ring = ring;
        }
    }
}
