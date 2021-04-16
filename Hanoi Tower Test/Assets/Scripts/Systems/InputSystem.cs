using UnityEngine;

namespace Mikabrytu.HanoiTower.Systems
{
    public class InputSystem
    {
        private enum Platform { Editor, Mobile }

        private Transform transform;

        private Platform platform;
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
            if (platform == Platform.Editor)
                DesktopIsTouching();

            if (platform == Platform.Mobile)
                MobileIsTouching();

            return isTouching;
        }

        #region Desktop Logic

        private Vector2 DesktopInput()
        {
            if (isTouching)
                currentPosition = Input.mousePosition;

            return currentPosition;
        }

        private bool DesktopIsTouching()
        {
            if (Input.GetMouseButtonDown(0))
                if (CheckIfSelfIsClicked(Input.mousePosition))
                    isTouching = true;

            if (Input.GetMouseButtonUp(0))
                isTouching = false;

            return isTouching;
        }

        #endregion

        #region Mobile Logic

        private Vector2 MobileInput()
        {
            if (isTouching)
                currentPosition = Input.GetTouch(0).position;

            return currentPosition;
        }

        private bool MobileIsTouching()
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    if (CheckIfSelfIsClicked(Input.GetTouch(0).position))
                        isTouching = true;

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    isTouching = false;
            }
            else
                isTouching = false;

            return isTouching;
        }

        #endregion

        private bool CheckIfSelfIsClicked(Vector3 inputPosition)
        {
            Vector3 touch = Camera.main.ScreenToWorldPoint(inputPosition);
            Vector2 touch2D = new Vector2(touch.x, touch.y);

            RaycastHit2D hit = Physics2D.Raycast(touch2D, Vector2.zero);

            return hit.collider != null && hit.transform.name.Equals(transform.name);
        }
    }
}