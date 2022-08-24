using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCellManager : MonoBehaviour
{
   public List<PlayerCell> playerCells;
   int index = 0;
   public void LinkNetworkNetworkPlayer(NetworkTeam plr){
        if(index < playerCells.Count){
            playerCells[index].linkedPlayer = plr;
            index++;
        }
   }
}
