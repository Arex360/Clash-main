using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkRootManager : NetworkBehaviour
{
   public NetworkManager manager;
    public GameObject canvas;
    public bool spawned;
    public void Start(){
        if(!isLocalPlayer){
            Destroy(canvas);
            return;
        }
        manager = GameObject.FindObjectOfType<NetworkManager>();
        //spawnA();
    }
   public void Update(){
        if (!isLocalPlayer)
        {
            return;
        }
        if(!spawned){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
       }else{
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
            Mobileinputcontroller.gameStarted = true;
       }
        
   }
   public void SelectTeam(string team){
        if(team == "A"){
            
        }else{
            CmdSpawnPlayerB();
        }
        
   }
   [Command(requiresAuthority = false)]
   public void CmdSpawnPlayerA(){
        GameObject plr = Instantiate(FindObjectOfType<NetworkManager>().spawnPrefabs[1]);
        NetworkGameManager.instance.myTeam = "A";
        NetworkServer.Spawn(plr,connectionToClient);
   }
   [Command(requiresAuthority = false)]

   public void CmdSpawnPlayerB(){
         GameObject plr = Instantiate(FindObjectOfType<NetworkManager>().spawnPrefabs[2]);
         NetworkGameManager.instance.myTeam = "B";
        NetworkServer.Spawn(plr,connectionToClient);
   }


   public void spawnA(){
        CmdSpawnPlayerA();
        spawned = true;
        Destroy(canvas);
   }
   public void spawnB(){
        CmdSpawnPlayerB();
         spawned = true;
        Destroy(canvas);
   }
}
