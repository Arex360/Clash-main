using UnityEngine;

namespace TPSShooter
{
  public class HeadBob : MonoBehaviour
  {
    [SerializeField] private float _bobbingSpeed = 12f;
    [SerializeField] private float _bobbingAmount = 0.1f;

    private float _timer;
    private Vector3 _startLocalPosition;

    private PlayerBehaviour _playerBehaviour;

    private void Start()
    {
      _playerBehaviour = PlayerBehaviour.GetInstance();

      _startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
      if (_playerBehaviour.IsAiming && Time.deltaTime != 0)
      {
        HeadBobPlay();
      }
    }

    private void HeadBobPlay()
    {
      float waveSlice = 0.0f;

      Vector3 cSharpConversion = transform.localPosition;

      if (Mathf.Abs(InputController.HorizontalMovement) == 0 && Mathf.Abs(InputController.VerticalMovement) == 0)
      {
        _timer = 0.0f;
      }
      else
      {
        waveSlice = Mathf.Sin(_timer);
        _timer += _bobbingSpeed * Time.deltaTime;
        if (_timer > Mathf.PI * 2)
        {
          _timer = _timer - (Mathf.PI * 2);
        }
      }
      if (waveSlice != 0)
      {
        float translateChange = waveSlice * _bobbingAmount * Time.deltaTime;
        float totalAxes = Mathf.Abs(InputController.HorizontalMovement) + Mathf.Abs(InputController.VerticalMovement);
        totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
        translateChange = totalAxes * translateChange;

        cSharpConversion.y = translateChange + _startLocalPosition.y;
      }
      else
      {
        cSharpConversion.y = _startLocalPosition.y;
      }

      transform.localPosition = cSharpConversion;
    }
  }
}
