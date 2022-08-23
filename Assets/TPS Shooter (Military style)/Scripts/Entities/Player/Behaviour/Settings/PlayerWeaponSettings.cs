using UnityEngine;

namespace TPSShooter
{
  [System.Serializable]
  public class PlayerWeaponSettings
  {
    public LayerMask shootingLayers;

    public PlayerWeapon CurrentWeapon;
    public PlayerWeapon[] AllWeapons;
    public int getCurrentWeaponIndex()
        {
            int index = 0;
            for(int i = 0; i < AllWeapons.Length; i++)
            {
                if(AllWeapons[i] == CurrentWeapon)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
  }
}
