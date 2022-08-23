using UnityEngine;

namespace TPSShooter
{
  [RequireComponent(typeof(PlayerBehaviour))]
  public class PlayerMenuWeapon : MonoBehaviour
  {
    private void Start()
    {
      PlayerBehaviour.SelectMenuWeaponUtil util = new PlayerBehaviour.SelectMenuWeaponUtil(GetComponent<PlayerBehaviour>());
      util.SelectMenuWeapon();
    }
  }
}
