using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TPSShooter;
public class NetworkPlayer : NetworkBehaviour
{
    public GameObject camRig;
    private PlayerBehaviour playerBehaviour;
    private PlayerVehicleAbility playerVehicleAbility;
    private PlayerMenuWeapon playerMenuWeapon;
    private PlayerAutoshoot playerAutoshoot;
    private PlayerGrenade playerGrenade;
    private PlayerWeaponMiddleWare playerWeaponMiddleWare;
    [SyncVar(hook = "OnChangeWeapon")]
    public int wepID;
    public int oldID;
    void Start()
    {
        playerBehaviour = this.GetComponent<PlayerBehaviour>();
        playerVehicleAbility = this.GetComponent<PlayerVehicleAbility>();
        playerMenuWeapon = this.GetComponent<PlayerMenuWeapon>();
        playerAutoshoot = this.GetComponent<PlayerAutoshoot>();
        playerGrenade = this.GetComponent<PlayerGrenade>();
        playerWeaponMiddleWare = this.GetComponent<PlayerWeaponMiddleWare>();
        if (isLocalPlayer)
        {
            camRig.transform.parent = null;
            Cursor.visible = false;
        }
        else
        {
            Destroy(camRig);
            playerBehaviour.Unsubscribe();
            //playerBehaviour.enabled = false;
            playerAutoshoot.enabled = false;
            playerVehicleAbility.enabled = false;
            playerMenuWeapon.enabled = false;
            playerGrenade.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(wepID != oldID){
            //playerBehaviour.EnableWeapon(wepID);
        }
        oldID = wepID;
        if(!isLocalPlayer){
            return;
        }
        if(NetworkClient.active){
            if(!playerBehaviour){
                return;
            }
            CmdSetWepID(playerWeaponMiddleWare.currentWeaponIndex);
        }
        else{
            print("dum");
        }
        if(Input.GetMouseButton(0)){
            CmdShoot();
        }

        
    }
    [Command]
    public void CmdSetWepID(int id){
        wepID = id;
    }
    public void OnChangeWeapon(int oldV,int newV){
        if(true){
           playerBehaviour.EnableWeapon(newV);
        }
    }
    public void Shoot(){
        if (playerBehaviour.IsAlive == false) return;

      if (playerBehaviour.fireCoroutine != null)
      {
        StopCoroutine(playerBehaviour.fireCoroutine);
      }
      //playerBehaviour.fireCoroutine = StartCoroutine(playerBehaviour.UpdateIsFire());

      if (playerBehaviour.CurrentWeaponBehaviour == null) return;
      if (playerBehaviour.IsThrowingGrenade) return;
      if (playerBehaviour.CurrentWeaponBehaviour.CanShoot == false) return;
      if (playerBehaviour.IsDrivingVehicle) return;

      playerBehaviour.Fire();
    }
    [Command]
    public void CmdShoot(){
        RpcShoot();
    }
    [ClientRpc]
    public void RpcShoot(){
        playerBehaviour.OnFireRequested();
    }
}
