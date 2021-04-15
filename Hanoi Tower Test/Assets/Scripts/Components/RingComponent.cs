﻿using System.Linq;
using UnityEngine;
using DG.Tweening;

using Mikabrytu.HanoiTower.Events;
using Mikabrytu.HanoiTower.Systems;

namespace Mikabrytu.HanoiTower.Components
{
    public class RingComponent : MonoBehaviour
    {
        public int Size;

        [SerializeField] Transform[] _boundaries;
        [SerializeField] private string _pinTag;
        [SerializeField] private Vector2[] _impulseDirections;
        [SerializeField] private float _impulse;

        private InputSystem inputSystem;
        private MoveSystem moveSystem;
        private Rigidbody2D rigidbody;

        private Vector3 pinPosition;
        private bool isMoving = false;
        private bool stuckOnPin = false;

        #region Unity LifeCycle

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
                    TouchReaction();

                ChangePhysics(true);

                if (stuckOnPin)
                    moveSystem.Move(inputSystem.GetTouchPosition(), Vector3.right, pinPosition);
                else
                    moveSystem.Move(inputSystem.GetTouchPosition());
            } else
            {
                ChangePhysics(false);

                if (isMoving)
                    DropRaction();
            }

            isMoving = inputSystem.IsTouching();
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

        #endregion

        private void ChangePhysics(bool isKinematic)
        {
            rigidbody.isKinematic = isKinematic;
        }

        private void TouchReaction()
        {
            EventManager.Raise(new OnRingMoveEvent(transform));
            transform.DORotate(Vector3.zero, .2f);
        }

        private void DropRaction()
        {
            EventManager.Raise(new OnRingDropEvent());

            if (stuckOnPin)
                ImpulseInvalidPlacement();
        }

        private void ImpulseInvalidPlacement()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 5);
            hits = hits.OrderBy(col => col.distance).ToArray();

            if (hits.Length > 2)
            {
                RingComponent other = hits[2].transform.GetComponent<RingComponent>();

                if (other != null && other.Size < Size)
                {
                    int index = Random.Range(0, _impulseDirections.Length);
                    rigidbody.AddForce(_impulseDirections[index] * _impulse);
                }
            }
        }
    }
}
