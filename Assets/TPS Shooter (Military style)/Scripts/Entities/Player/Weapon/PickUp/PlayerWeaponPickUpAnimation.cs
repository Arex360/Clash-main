using UnityEngine;

namespace TPSShooter
{
  [RequireComponent(typeof(Animator))]
  [RequireComponent(typeof(PlayerWeaponPickUp))]
  public class PlayerWeaponPickUpAnimation : MonoBehaviour
  {
    private Animator _animator;

    private void Awake()
    {
      _animator = GetComponent<Animator>();

      PlayerWeaponPickUp weaponPick = GetComponent<PlayerWeaponPickUp>();
      weaponPick.OnWeaponPickedUpEvent += OnWeaponPickedUp;
      weaponPick.OnWeaponDroppedEvent += OnWeaponDropped;
    }

    private void OnWeaponPickedUp()
    {
      _animator.enabled = false;
      transform.localScale = Vector3.one;
    }

    private void OnWeaponDropped()
    {
      _animator.enabled = true;
      transform.localScale = new Vector3(2, 2, 2);
    }
  }
}
