using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerCell : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI kills;
    public NetworkTeam linkedPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(linkedPlayer){
            playerName.text = linkedPlayer.playerName;
            kills.text = $"{linkedPlayer.kills} kills";
        }else{
            playerName.text = "";
            kills.text = "";
        }
    }
}
