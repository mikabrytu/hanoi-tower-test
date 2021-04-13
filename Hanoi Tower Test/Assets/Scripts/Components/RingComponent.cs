using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mikabrytu.HanoiTower.Systems;

namespace Mikabrytu.HanoiTower.Components
{
    public class RingComponent : MonoBehaviour
    {
        public int Size;

        [SerializeField] private string _pinTag;

        private InputSystem inputSystem;
        private MoveSystem moveSystem;
        private Rigidbody2D rigidbody;

        private Vector3 pinPosition;
        private Vector3 previousPosition;
        private bool stuckOnPin = false;

        private void Start()
        {
            inputSystem = new InputSystem(transform);
            moveSystem = new MoveSystem(transform, Camera.main);

            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (inputSystem.IsTouching())
            {
                ChangePhysics(true);

                if (stuckOnPin)
                    moveSystem.Move(inputSystem.GetTouchPosition(), Vector3.right, pinPosition);
                else
                    moveSystem.Move(inputSystem.GetTouchPosition());
            } else
            {
                ChangePhysics(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals(_pinTag))
            {
                stuckOnPin = true;
                pinPosition = collision.transform.position;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals(_pinTag))
                stuckOnPin = false;
        }

        private void ChangePhysics(bool isKinematic)
        {
            rigidbody.isKinematic = isKinematic;
        }
    }
}
