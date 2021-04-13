using UnityEngine;
using DG.Tweening;

namespace Mikabrytu.HanoiTower.Systems
{
    public class FollowSystem
    {
        private Camera camera;

        public FollowSystem(Camera camera) {
            this.camera = camera;
        }

        public void ZoomIn(float size, float duration)
        {
            camera.DOOrthoSize(size, duration).SetEase(Ease.OutCirc);
        }

        public void ZoomOut(float size, float duration)
        {
            camera.DOOrthoSize(size, duration).SetEase(Ease.InCirc);
        }
    }
}