using UnityEngine;

namespace TPSShooter
{
  public class WeaponSway : MonoBehaviour
  {
    [SerializeField] private float _amount = 0.005f;
    [SerializeField] private float _maxAmount = 0.03f;
    [SerializeField] private float _smooth = 6f;

    private bool onceSet;

    private Vector3 _initialPosition;
    private Vector3 _currentVelocity;

    private PlayerBehaviour _playerBehaviour;

    private void Start()
    {
      _playerBehaviour = PlayerBehaviour.GetInstance();

      _initialPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
      if (_playerBehaviour.IsAiming)
      {
        if (onceSet)
          onceSet = false;

        float movementX = -InputController.HorizontalRotation * _amount;
        float movementY = -InputController.VerticalRotation * _amount;
        movementX = Mathf.Clamp(movementX, -_maxAmount, _maxAmount);
        movementY = Mathf.Clamp(movementY, -_maxAmount, _maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition + _initialPosition, ref _currentVelocity, Time.deltaTime * _smooth);
      }
      else
      {
        if (!onceSet)
        {
          onceSet = true;

          transform.localPosition = _initialPosition;
        }
      }
    }
  }
}
