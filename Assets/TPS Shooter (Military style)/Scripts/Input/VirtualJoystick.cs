using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TPSShooter
{

    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {

        [Header("- Joystick images -")]
        [SerializeField] private Image _imgBackground;
        [SerializeField] private Image _imgJoystick;

        private float _backgroundWidth;
        private float _backgroundHalfWidth;

        // minimum image width to start running
        private float _minBackgroundRunWidth;

        private float _fingerPositionX;
        private float _fingerPostionY;

        public float HorizontalValue { get; private set; }
        public float VerticalValue { get; private set; }

        public bool IsRun { get; private set; }

        private void Start()
        {
            if (!_imgBackground.raycastTarget)
                Debug.LogError("VirtualJoystick (ImgBackground): UI gameObject raycastTarger value has to be true.");

            _backgroundWidth = _imgBackground.rectTransform.sizeDelta.x;
            _backgroundHalfWidth = _backgroundWidth / 2;

            _minBackgroundRunWidth = _backgroundWidth / 3;
        }

        public void OnPointerDown(PointerEventData e)
        {
            OnDrag(e);
        }

        public void OnPointerUp(PointerEventData e)
        {
            HorizontalValue = 0;
            VerticalValue = 0;

            IsRun = false;

            _imgJoystick.transform.localPosition = Vector3.zero;
        }

        public void OnDrag(PointerEventData e)
        {
            Vector2 pos;
            // transform to local position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_imgBackground.rectTransform,
                                                                        e.position,
                                                                        e.pressEventCamera,
                                                                        out pos))
            {
                _fingerPositionX = pos.x;
                _fingerPostionY = pos.y;

                IsRun = (_fingerPostionY > _backgroundHalfWidth) && (Mathf.Abs(_fingerPositionX) < _minBackgroundRunWidth);

                if (pos.magnitude > _backgroundHalfWidth)
                {
                    float multiplier = _backgroundHalfWidth / pos.magnitude;
                    _fingerPositionX *= multiplier;
                    _fingerPostionY *= multiplier;
                }

                _imgJoystick.transform.localPosition = new Vector2(_fingerPositionX, _fingerPostionY);

                HorizontalValue = _fingerPositionX / _backgroundHalfWidth;
                VerticalValue = _fingerPostionY / _backgroundHalfWidth;
            }
        }

    }

}