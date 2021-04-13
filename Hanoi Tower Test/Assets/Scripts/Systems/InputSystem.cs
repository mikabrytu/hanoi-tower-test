using UnityEngine;

namespace Mikabrytu.HanoiTower.Systems
{
    public class InputSystem
    {
        private enum Platform { Editor, Mobile }

        private Transform transform;

        private Platform platform;
        private Vector3 previousPosition;
        private Vector2 currentPosition;
        private bool isTouching = false;

        public InputSystem(Transform transform) {

#if UNITY_EDITOR
            platform = Platform.Editor;
#endif

#if UNITY_ANDROID
            platform = Platform.Mobile;
#endif

            this.transform = transform;

            currentPosition = Vector2.zero;
        }

        public Vector3 GetPreviousPosition()
        {
            return previousPosition;
        }

        public Vector2 GetTouchPosition()
        {
            Vector2 position = Vector2.zero;

            if (platform == Platform.Editor)
                position = DesktopInput();

            if (platform == Platform.Mobile)
                position = MobileInput();

            return position;
        }

        public bool IsTouching()
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = transform.position;

                Vector3 touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 touch2D = new Vector2(touch.x, touch.y);

                RaycastHit2D hit = Physics2D.Raycast(touch2D, Vector2.zero);
                if (hit.collider != null && hit.transform.name.Equals(transform.name))
                    isTouching = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isTouching = false;
            }

            return isTouching;
        }

        private Vector2 DesktopInput()
        {
            if (isTouching)
            {
                currentPosition = Input.mousePosition;
            }

            return currentPosition;
        }

        private Vector2 MobileInput()
        {
            return Vector2.zero;
        }
    }
}