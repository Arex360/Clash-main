using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameManager : MonoBehaviour
{

    public static NetworkGameManager instance;
    public string myTeam = "A";
    public NetworkPlayer[] networkPlayers;
    public NetworkTeam[] networkTeams;
    public List<NetworkTeam> teamA;
    public List<NetworkTeam> teamB;
    public NetworkPlayer localPlayer;

    public TeamACell teamACell;
    public TeamBCell teamBCell;

    private void Awake(){
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Register(){
        networkPlayers = GameObject.FindObjectsOfType<NetworkPlayer>();
        foreach(NetworkPlayer plr in networkPlayers){
            if(plr.isLocalPlayer){
                localPlayer = plr;
            }
        }
    }

    public void RegisterTeam(){
        networkTeams = GameObject.FindObjectsOfType<NetworkTeam>();
        foreach(NetworkTeam plr in networkTeams){
            if(plr.Team == "A"){
                if(!teamA.Contains(plr)){
                    teamA.Add(plr);
                    teamACell.LinkNetworkNetworkPlayer(plr);
                }
            }else{
                if(!teamB.Contains(plr)){
                    teamB.Add(plr);
                    teamBCell.LinkNetworkNetworkPlayer(plr);
                }
            }
        }
    }
}
