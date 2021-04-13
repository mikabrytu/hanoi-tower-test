using UnityEngine;
using DG.Tweening;

using Mikabrytu.HanoiTower.Events;
using Mikabrytu.HanoiTower.Systems;
using System;

namespace Mikabrytu.HanoiTower.Components
{
    public class CameraComponent : MonoBehaviour
    {
        [SerializeField] private float _orthoSize;
        [SerializeField] private float _zoomOutSize;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _followOffset;

        private FollowSystem followSystem;
        private Vector3 originalPosition;

        private void Start()
        {
            followSystem = new FollowSystem(GetComponent<Camera>());

            originalPosition = transform.position;

            EventManager.AddListener<OnRingMoveEvent>(OnRingMove);
            EventManager.AddListener<OnRingDropEvent>(OnRingDrop);
        }

        private void OnRingMove(OnRingMoveEvent e)
        {
            followSystem.ZoomOut(_zoomOutSize, _zoomDuration);
        }

        private void OnRingDrop(OnRingDropEvent e)
        {
            followSystem.ZoomIn(_orthoSize, _zoomDuration);
        }
    }
}
