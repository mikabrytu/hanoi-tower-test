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

        public void Move(Vector3 position, Vector3 fixedAxis, Vector3 fixedPosition)
        {
            position = CastScreenToWorld(position);

            if (fixedAxis.x > 0)
                position.x = fixedPosition.x;

            if (fixedAxis.y > 0)
                position.y = fixedPosition.y;

            if (fixedAxis.z > 0)
                position.z = fixedPosition.z;

            transform.position = position;
        }

        public void Move(Vector3 position)
        {
            position = CastScreenToWorld(position);
            transform.position = position;
        }

        private Vector3 CastScreenToWorld(Vector3 position)
        {
            position = camera.ScreenToWorldPoint(position);
            position.z = 0;

            return position;
        }
    }
}