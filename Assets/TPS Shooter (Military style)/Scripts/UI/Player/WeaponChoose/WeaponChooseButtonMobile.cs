using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TPSShooter.UI
{
  [RequireComponent(typeof(Image))]
  public class WeaponChooseButtonMobile : WeaponChooseButton, IPointerClickHandler
  {
    protected float pressedTime;
    protected bool isPressed;
    protected bool isUsed;

    protected const float timeToDrop = 1;

    public event Action requestWeaponSwap;
    public event Action requestWeaponDrop;

    protected virtual void OnEnable()
    {
      pressedTime = 0;
      isPressed = false;
      isUsed = false;
      buttonImage.sprite = spriteButton;
    }

    protected virtual void Update()
    {
      if (isPressed && !isUsed)
      {
        pressedTime += Time.deltaTime;

        if (pressedTime >= timeToDrop)
        {
          requestWeaponDrop?.Invoke();
          isUsed = true;
        }
      }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);

      isPressed = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
      base.OnPointerExit(eventData);

      pressedTime = 0;
      isPressed = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (isUsed) return;

      isUsed = true;
      requestWeaponSwap?.Invoke();
    }
  }
}
