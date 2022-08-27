using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class NetworkTeam : NetworkBehaviour
{
    [SyncVar (hook = "OnChangeName")]
    public string playerName;
    [SyncVar ( hook = "OnChangeTeam")]
    public string Team;
    [SyncVar]
    public int kills;
    public string username;
    public string team;
    public int localKills;
    public TextMeshProUGUI nameUI;
    void Start(){
        username = StaticData.nick;
        if(hasAuthority){
            Destroy(nameUI.gameObject);
        }
    }
    [Command]
    public void CmdSetData(string name, string _team, int _kills){
        playerName = name;
        Team = _team;
        kills = _kills;
    }
    public void OnChangeName(string oldN,string newN){
         NetworkGameManager.instance.Register();
         if(nameUI){
            nameUI.text = newN;
         }
    }
    public void OnChangeTeam(string oldN, string newN){
        NetworkGameManager.instance.RegisterTeam();
    }
    private void Update(){
        if(!hasAuthority){
            return;
        }
        CmdSetData(username,team,localKills);
       // Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    [Command(requiresAuthority = false)]
    public void CmdSetKills(int kill){
        kills = kill;
    }
    public void SetKill(int kill){
        CmdSetKills(kill);
    }
}
