using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TPSShooter.UI
{
  [RequireComponent(typeof(Image))]
  public class WeaponChooseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public int buttonID;

    [Header("Button Sprites")]
    public Sprite spriteButton;
    public Sprite spriteButtonSelected;

    [Header("Weapon Icon")]
    public Image imageWeapon;

    [Header("Text")]
    public Text textInfo;

    public event Action<int> onSelected;
    public event Action<int> onDeselected;

    protected Image buttonImage;

    protected virtual void Awake()
    {
      buttonImage = GetComponent<Image>();
    }

    public void SetWeaponIcon(Sprite icon)
    {
      imageWeapon.sprite = icon;

      Color color = imageWeapon.color;
      color.a = (icon == null) ? 0 : 1;
      imageWeapon.color = color;
    }

    public void SetInfo(string text)
    {
      textInfo.text = text;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      buttonImage.sprite = spriteButtonSelected;
      onSelected?.Invoke(buttonID);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      buttonImage.sprite = spriteButton;
      onDeselected?.Invoke(buttonID);
    }
  }
}
