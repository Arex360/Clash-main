using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TPSShooter.UI
{
  public class PointerStay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
  {
    public UnityEvent pointerStayEvent;

    private bool _isPointerStay;

    private void Update()
    {
      if (_isPointerStay)
      {
        pointerStayEvent.Invoke();
      }
    }

    public void OnPointerDown(PointerEventData eventData) { _isPointerStay = true; }
    public void OnPointerUp(PointerEventData eventData) { _isPointerStay = false; }
  }
}
