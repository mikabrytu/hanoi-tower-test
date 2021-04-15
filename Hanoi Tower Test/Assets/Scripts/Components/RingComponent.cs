﻿using UnityEngine;

using Mikabrytu.HanoiTower.Events;
using Mikabrytu.HanoiTower.Systems;

namespace Mikabrytu.HanoiTower.Components
{
    public class RingComponent : MonoBehaviour
    {
        public int Size;

        [SerializeField] private string _pinTag;
        [SerializeField] Transform[] _boundaries;

        private InputSystem inputSystem;
        private MoveSystem moveSystem;
        private Rigidbody2D rigidbody;

        private Vector3 pinPosition;
        private bool isMoving = false;
        private bool stuckOnPin = false;

        private void Start()
        {
            inputSystem = new InputSystem(transform);
            moveSystem = new MoveSystem(transform, Camera.main);

            moveSystem.SetBoundaries(new float[] {
                _boundaries[0].position.y,
                _boundaries[1].position.x,
                _boundaries[2].position.y,
                _boundaries[3].position.x
            });

            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (inputSystem.IsTouching())
            {
                if (!isMoving)
                    EventManager.Raise(new OnRingMoveEvent(transform));

                ChangePhysics(true);

                if (stuckOnPin)
                    moveSystem.Move(inputSystem.GetTouchPosition(), Vector3.right, pinPosition);
                else
                    moveSystem.Move(inputSystem.GetTouchPosition());
            } else
            {
                if (isMoving)
                    EventManager.Raise(new OnRingDropEvent());

                ChangePhysics(false);
            }

            isMoving = inputSystem.IsTouching();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals(_pinTag))
            {
                stuckOnPin = true;
                pinPosition = collision.transform.position;
                //rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals(_pinTag))
            {
                stuckOnPin = false;
                //rigidbody.constraints = RigidbodyConstraints2D.None;
            }
        }

        private void ChangePhysics(bool isKinematic)
        {
            rigidbody.isKinematic = isKinematic;
        }
    }
}
