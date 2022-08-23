using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

using LightDev;

namespace TPSShooter
{
  public class Touchpad : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
  {
    [Header("- Sensitivities -")]
    [SerializeField] private float _sensitivity = 20f;
    [SerializeField] private float _scopeSensitivity = 6f;

    [Header("- Save Load -")]
    public bool _saveData;

    private bool _isPressed;
    private float _horizontalValue;
    private float _verticalValue;

    public float HorizontalValue
    {
      get
      {
        if (_previousHorizontalValue == _horizontalValue)
        {
          return 0;
        }
        else
        {
          _previousHorizontalValue = _horizontalValue;

          return _horizontalValue;
        }
      }
    }

    public float VerticalValue
    {
      get
      {
        if (_previousVerticalValue == _verticalValue)
        {
          return 0;
        }
        else
        {
          _previousVerticalValue = _verticalValue;

          return _verticalValue;
        }
      }
    }

    public bool IsPressed { get { return _isPressed; } }

    private float _currentSensitivity;
    private float _previousHorizontalValue;
    private float _previousVerticalValue;

    private void Awake()
    {
      if (_saveData)
      {
        _sensitivity = SaveLoad.TouchpadSensitivity;
        _scopeSensitivity = SaveLoad.TouchpadAimingSensitivity;
      }
      _currentSensitivity = _sensitivity;

      if (!GetComponent<Image>().raycastTarget)
        Debug.LogError("Touchpad: UI gameObject raycast value has to be true.");

      Events.PlayerAimActivated += OnPlayerAimActivated;
      Events.PlayerAimDeactivated += OnPlayerAimDeactivated;
    }

    private void OnDestroy()
    {
      Events.PlayerAimActivated -= OnPlayerAimActivated;
      Events.PlayerAimDeactivated -= OnPlayerAimDeactivated;
    }

    private void OnPlayerAimActivated()
    {
      _currentSensitivity = _scopeSensitivity;
    }

    private void OnPlayerAimDeactivated()
    {
      _currentSensitivity = _sensitivity;
    }

    #region Interfaces_Implementation

    public void OnDrag(PointerEventData eventData)
    {
      _horizontalValue = eventData.delta.x * 0.0061f * _currentSensitivity;
      _verticalValue = eventData.delta.y * 0.0061f * _currentSensitivity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      _isPressed = false;

      _horizontalValue = 0;
      _verticalValue = 0;
    }

    #endregion
  }
}
