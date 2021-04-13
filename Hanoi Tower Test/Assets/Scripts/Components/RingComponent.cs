using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mikabrytu.HanoiTower.Systems;

namespace Mikabrytu.HanoiTower.Components
{
    public class RingComponent : MonoBehaviour
    {
        private InputSystem inputSystem;
        private MoveSystem moveSystem;

        private void Start()
        {
            inputSystem = new InputSystem(transform, Camera.main);
            moveSystem = new MoveSystem(transform, Camera.main);
        }

        private void Update()
        {
            if (inputSystem.IsTouching())
                moveSystem.Move(inputSystem.GetTouchPosition());
        }
    }
}
