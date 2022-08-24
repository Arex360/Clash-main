using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TPSShooter;
public class NetworkPlayer : NetworkBehaviour
{
    private NetworkManager manager;
    public GameObject camRig;
    private PlayerBehaviour playerBehaviour;
    private PlayerVehicleAbility playerVehicleAbility;
    private PlayerMenuWeapon playerMenuWeapon;
    private PlayerAutoshoot playerAutoshoot;
    private PlayerGrenade playerGrenade;
    private PlayerWeaponMiddleWare playerWeaponMiddleWare;
    [SyncVar(hook = "OnChangeWeapon")]
    public int wepID;
    [SyncVar( hook = "OnChangeTeam")]
    public string Team;

    [SyncVar]
    public string playerName;
    [SyncVar]
    public int kills;

    public int oldID;
    public int randomInt;
    public string username;
    void Start()
    {
        username = $"Arex {Random.RandomRange(0,100)}"; 
        if(hasAuthority){
            //CmdRegister(username);
            //CmdSetTeam("A");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        manager = GameObject.FindObjectOfType<NetworkManager>();
        playerBehaviour = this.GetComponent<PlayerBehaviour>();
        playerVehicleAbility = this.GetComponent<PlayerVehicleAbility>();
        playerMenuWeapon = this.GetComponent<PlayerMenuWeapon>();
        playerAutoshoot = this.GetComponent<PlayerAutoshoot>();
        playerGrenade = this.GetComponent<PlayerGrenade>();
        playerWeaponMiddleWare = this.GetComponent<PlayerWeaponMiddleWare>();
        if (hasAuthority)
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
           // playerGrenade.enabled = false;
        }
        if(isServer){
            //CmdSpawnEnemy();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(wepID != oldID){
            //playerBehaviour.EnableWeapon(wepID);
        }
        oldID = wepID;
        if(!hasAuthority){
            return;
        }
        CmdRegister(username);
        CmdSetTeam("A");
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
            Vector3 firepoint = playerBehaviour.FirePoint;
            CmdShoot(firepoint);
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

      //playerBehaviour.Fire();
    }
    [Command]
    public void CmdShoot(Vector3 point){
        RpcShoot(point);
    }
    [ClientRpc]
    public void RpcShoot(Vector3 firepoint){
        
        playerBehaviour.OnFireRequested(firepoint,"Owais");
    }
    [Command]
    public void CmdSpawnEnemy(){
        GameObject enemy = Instantiate(manager.spawnPrefabs[0]);
        NetworkServer.Spawn(enemy,connectionToClient);
    }
    [Command]
    public void CmdRegister(string _name){
        playerName = _name;
        kills = 0;
        RpcRegister();
    }
    [ClientRpc]
    public void RpcRegister(){
        //NetworkGameManager.instance.Register();
    }
    public void OnChangeTeam(string oldV,string newV){
       // NetworkGameManager.instance.RegisterTeam();
    }
    [Command]
    public void CmdSetTeam(string _team){
        Team =_team;
    }
}
