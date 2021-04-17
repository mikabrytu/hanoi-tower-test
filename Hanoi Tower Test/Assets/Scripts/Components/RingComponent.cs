using System.Linq;
using UnityEngine;
using DG.Tweening;

using Mikabrytu.HanoiTower.Events;
using Mikabrytu.HanoiTower.Systems;

namespace Mikabrytu.HanoiTower.Components
{
    public class RingComponent : MonoBehaviour
    {
        public int Size;

        [Header("Tags")]
        [SerializeField] private string pinTag;
        [SerializeField] private string groundTag;

        [Header("Impulse Properties")]
        [SerializeField] private Vector2[] impulseDirections;
        [SerializeField] private float impulse;

        [Header("General Properties")]
        [SerializeField] private Transform[] boundaries;
        [SerializeField] private float momentumForce;

        [Header("Partiles")]
        [SerializeField] private ParticleSystem fallParticle;
        [SerializeField] private ParticleSystem flyParticle;

        private InputSystem inputSystem;
        private MoveSystem moveSystem;
        private Rigidbody2D rigidbody;
        private Animator animator;

        private Vector3 pinPosition;
        private bool isMoving = false;
        private bool isFixed = false;
        private bool isGrounded = true;
        private bool isFlying = false;

        #region Unity LifeCycle

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            inputSystem = new InputSystem(transform);
            moveSystem = new MoveSystem(rigidbody, Camera.main);

            moveSystem.SetBoundaries(new float[] {
                boundaries[0].position.y,
                boundaries[1].position.x,
                boundaries[2].position.y,
                boundaries[3].position.x
            });
        }

        private void Update()
        {
            if (inputSystem.IsTouching())
            {
                if (!isMoving)
                    TouchReaction();

                ChangePhysics(true);

                if (isFixed)
                    moveSystem.Move(inputSystem.GetTouchPosition(), Vector3.right, pinPosition);
                else
                    moveSystem.Move(inputSystem.GetTouchPosition());
            } else
            {
                ChangePhysics(false);

                if (rigidbody.velocity != Vector2.zero)
                    if (isFixed && !isGrounded && !isFlying)
                        Snap();

                if (isMoving)
                    DropReaction();
            }

            isMoving = inputSystem.IsTouching();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals(pinTag))
            {
                isFixed = true;
                pinPosition = collision.transform.position;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals(pinTag))
                isFixed = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((collision.transform.tag.Equals(groundTag) || collision.transform.tag.Equals(tag)) && !isGrounded)
                Fall();
        }

        #endregion

        private void ChangePhysics(bool isKinematic)
        {
            rigidbody.isKinematic = isKinematic;

            if (!isKinematic && rigidbody.constraints == RigidbodyConstraints2D.FreezePositionX)
                rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        private void Snap()
        {
            Vector2 position = pinPosition;
            position.y = rigidbody.position.y;
            rigidbody.position = position;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        private void TouchReaction()
        {
            EventManager.Raise(new OnRingMoveEvent(transform));

            moveSystem.PrepareMomentum(inputSystem.GetTouchPosition());

            animator.SetTrigger("Touch");
            transform.DORotate(Vector3.zero, .2f);
            isGrounded = false;
        }

        private void DropReaction()
        {
            EventManager.Raise(new OnRingDropEvent());

            if (isFixed)
                ImpulseInvalidPlacement();
            else
                moveSystem.ApplyMomentum(inputSystem.GetTouchPosition(), momentumForce);
        }

        private void Fall()
        {
            EventManager.Raise(new OnRingFallEvent());
            animator.SetTrigger("Fall");
            fallParticle.Play();
            isGrounded = true;
            isFlying = false;
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
                    EventManager.Raise(new OnRingFlyEvent());
                    ChangePhysics(false);
                    isFlying = true;

                    int index = Random.Range(0, impulseDirections.Length);
                    rigidbody.AddForce(impulseDirections[index] * impulse);
                    animator.SetTrigger("Fly");
                    flyParticle.Play();
                }
            }
        }
    }
}
