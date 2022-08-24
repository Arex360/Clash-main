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
       if(!spawned){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
       }else{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        NetworkServer.Spawn(plr,connectionToClient);
   }
   [Command(requiresAuthority = false)]

   public void CmdSpawnPlayerB(){
         GameObject plr = Instantiate(FindObjectOfType<NetworkManager>().spawnPrefabs[2]);
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
