using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameManager : MonoBehaviour
{
    public int killToWin;
    public static NetworkGameManager instance;
    public string myTeam = "A";
    public NetworkPlayer[] networkPlayers;
    public NetworkTeam[] networkTeams;
    public List<NetworkTeam> teamA;
    public List<NetworkTeam> teamB;
    public NetworkPlayer localPlayer;

    public TeamACell teamACell;
    public TeamBCell teamBCell;
    public Dictionary<string,NetworkTeam> playerStat;
    public TMPro.TextMeshProUGUI msg;
    private void Awake(){
        instance = this;
        playerStat = new Dictionary<string, NetworkTeam>();

    }
    private void Start()
    {
        NetworkPlayer[] _networkPlayers = FindObjectsOfType<NetworkPlayer>();
        foreach(NetworkPlayer plr in _networkPlayers)
        {
            if (plr.hasAuthority)
            {
                localPlayer = plr;
            }
        }
    }
    public void Shoot()
    {
        localPlayer.ShootOnUI();
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
    public void IncreamentKills(string id,int ammount){
        playerStat[id].localKills++;
        CalculateKills();
    }
    public void CalculateKills(){
        int killsA = 0;
        int killsB = 0;
        foreach(NetworkTeam team in teamA){
             killsA += team.kills;
        }
        foreach(NetworkTeam team in teamB){
            killsB += team.kills;
        }
        if(killsA > 0){
            print("Team A on Lead");
            if(killsA >= killToWin){
                foreach(NetworkTeam team in playerStat.Values){
                    team.localKills = 0;
                }
                msg.text = "Team A wins";
                Invoke(nameof(resetMsg),2f);
            }
        }else{
            print("Team B on lead");
            if(killsB >= killToWin){
                foreach(NetworkTeam team in playerStat.Values){
                    team.localKills = 0;
                }
                msg.text = "Team B wins";
                Invoke(nameof(resetMsg),2f);
            }
        }
    }
    public void resetMsg(){
        msg.text = "";
    }
    public void RegisterTeam(){
        networkTeams = GameObject.FindObjectsOfType<NetworkTeam>();
        foreach(NetworkTeam plr in networkTeams){
            if(plr.Team == "A"){
                if(!teamA.Contains(plr)){
                    teamA.Add(plr);
                    if (plr.hasAuthority)
                    {
                        localPlayer = plr.GetComponent<NetworkPlayer>();
                    }
                    teamACell.LinkNetworkNetworkPlayer(plr);
                    playerStat.Add(plr.playerName,plr);
                }
            }else{
                if(!teamB.Contains(plr)){
                    teamB.Add(plr);
                    if (plr.hasAuthority)
                    {
                        localPlayer = plr.GetComponent<NetworkPlayer>();
                    }
                    playerStat.Add(plr.playerName,plr);
                    teamBCell.LinkNetworkNetworkPlayer(plr);
                }
            }
        }
    }
}
