using UnityEngine;

namespace Mikabrytu.HanoiTower.Systems
{
    public class MoveSystem
    {
        private Transform transform;
        private Camera camera;

        public MoveSystem(Transform transform, Camera camera)
        {
            this.transform = transform;
            this.camera = camera;
        }

        public void Move(Vector3 position)
        {
            position = camera.ScreenToWorldPoint(position);
            position.z = 0;

            transform.position = position;
        }
    }
}