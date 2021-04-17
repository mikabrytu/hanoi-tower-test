using UnityEngine;

namespace Mikabrytu.HanoiTower.Systems
{
    public class MoveSystem
    {
        private Rigidbody2D rigidbody;
        private Camera camera;
        private Vector3 initialPosition;
        private float initialTime;
        private float[] boundaries;

        public MoveSystem(Rigidbody2D rigidbody, Camera camera)
        {
            this.rigidbody = rigidbody;
            this.camera = camera;
        }

        public void SetBoundaries(float[] boundaries)
        {
            this.boundaries = boundaries;
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

            rigidbody.position = position;
        }

        public void Move(Vector3 position)
        {
            position = CastScreenToWorld(position);

            bool valid = true;
            if (boundaries.Length > 0)
                valid = position.x < boundaries[1] && position.x > boundaries[3] && position.y < boundaries[0] && position.y > boundaries[2];

            if (valid)
                rigidbody.position = position;
        }

        public void PrepareMomentum(Vector3 position)
        {
            initialPosition = position;
            initialTime = Time.time;
        }

        public void ApplyMomentum(Vector3 position, float force)
        {
            Vector3 finalPosition = position;
            float finalTime = Time.time;

            float interval = finalTime - initialTime;
            Vector3 direction = initialPosition - finalPosition;

            rigidbody.AddForce(-direction / interval * force);
        }

        private Vector3 CastScreenToWorld(Vector3 position)
        {
            position = camera.ScreenToWorldPoint(position);
            position.z = 0;

            return position;
        }
    }
}