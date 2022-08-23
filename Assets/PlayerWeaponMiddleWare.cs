using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSShooter;
public class PlayerWeaponMiddleWare : MonoBehaviour
{
    public PlayerBehaviour pb;
    public PlayerWeaponSettings playerWeaponSettings;
    public GameObject enemy;
    public int currentWeaponIndex;
    void Start()
    {
        pb = this.GetComponent<PlayerBehaviour>();
        playerWeaponSettings = pb.weaponSettings;
        Instantiate(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        currentWeaponIndex = playerWeaponSettings.getCurrentWeaponIndex();
    }
}
