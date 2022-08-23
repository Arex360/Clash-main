using UnityEngine;

namespace TPSShooter
{
  [RequireComponent(typeof(BoxCollider))]
  [RequireComponent(typeof(PlayerWeapon))]
  public class PlayerWeaponPickUp : MonoBehaviour
  {
    public Transform parentBone;
    public Vector3 localPosition;
    public Vector3 localEulerAngles;

    private Collider _collider;
    private PlayerWeapon _playerWeapon;

    public event System.Action OnWeaponPickedUpEvent;
    public event System.Action OnWeaponDroppedEvent;

    private void Awake()
    {
      _collider = GetComponent<Collider>();
      _collider.isTrigger = true;

      _playerWeapon = GetComponent<PlayerWeapon>();
    }

    private void OnTriggerEnter(Collider other)
    {
      PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
      if (player)
      {
        PickUpWeapon(player);
      }
    }

    public void PickUpWeapon(PlayerBehaviour playerBehaviour)
    {
      if (!playerBehaviour.CanPickUpWeapon())
      {
        return;
      }

      playerBehaviour.PickUpWeapon(_playerWeapon);

      _collider.enabled = false;

      transform.parent = parentBone;
      transform.localPosition = localPosition;
      transform.localEulerAngles = localEulerAngles;

      _playerWeapon.UpdateParent();

      if (OnWeaponPickedUpEvent != null)
      {
        OnWeaponPickedUpEvent();
      }
    }

    public void DropWeapon()
    {
      transform.position += GetComponentInParent<PlayerBehaviour>().transform.forward * 3f;
      transform.position = new Vector3(transform.position.x, transform.parent.position.y, transform.position.z);
      transform.parent = null;

      _collider.enabled = true;

      if (OnWeaponDroppedEvent != null)
      {
        OnWeaponDroppedEvent();
      }
    }
  }
}
