using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror;
public class NetworkTeam : NetworkBehaviour
{
    [SyncVar (hook = "OnChangeName")]
    public string playerName;
    [SyncVar ( hook = "OnChangeTeam")]
    public string Team;
    [SyncVar]
    public int kills;
    public string username;
    void Start(){
        username = $"Arex {Random.RandomRange(0,100)}";
    }
    [Command]
    public void CmdSetData(string name, string _team, int _kills){
        playerName = name;
        Team = _team;
        kills = _kills;
    }
    public void OnChangeName(string oldN,string newN){
         NetworkGameManager.instance.Register();
    }
    public void OnChangeTeam(string oldN, string newN){
        NetworkGameManager.instance.RegisterTeam();
    }
    private void Update(){
        if(!isLocalPlayer){
            return;
        }
        CmdSetData(username,"A",0);
    }
    
}
