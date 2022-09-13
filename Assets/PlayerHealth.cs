using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TPSShooter;
public class PlayerHealth : NetworkBehaviour
{
    [System.Serializable]
    public class HealthUI
    {
        public GameObject healthHolder;
        public TMPro.TextMeshProUGUI healthUIText;
    }
    private PlayerBehaviour playerBehaviour;
    private Animator animator;
    public float localHealth;
    [SyncVar(hook="OnChangeHealth")]
    public float health;
    public UnityEngine.UI.Image healthFill;
    public float fillAmmout;
    public string lAttacker;
    public Vector3 defaultPosition;
    public bool hasGivenKill;
    public NetworkTeam networkTeam;
    public HealthUI healthUI;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        networkTeam = this.GetComponent<NetworkTeam>();
        defaultPosition = this.transform.position;
        playerBehaviour = this.GetComponent<PlayerBehaviour>();
        if(hasAuthority){
            Destroy(healthFill);
        }
        else
        {
            Destroy(healthUI.healthHolder);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health >0){
            this.GetComponent<Animator>().SetTrigger("respawn");
        }
        if(hasAuthority){
            CmdSetHealth(localHealth);
        }
        
        fillAmmout = health/100;
        if(localHealth <= 0){
            playerBehaviour.enabled= false;
        }else{
            playerBehaviour.enabled = true;
        }
    }
    [Command]
    public void CmdSetHealth(float _health){
        health = _health;
    }
    public void OnChangeHealth(float oldV,float newV){
        float perc = newV/100;
        health = newV;
        if (hasAuthority)
        {
            healthUI.healthUIText.text = newV.ToString();
        }
        if(healthFill){
            healthFill.fillAmount = perc;
        }
        if(newV <= 0){
           animator.SetTrigger("Died");
           if(true){
                //NetworkGameManager.instance.playerStat[lAttacker].SetKill(10);
                Invoke(nameof(Respawn),3f);
           }
        }
    }
    public void TakeDamage(float ammount, string lasthit){
        print("take Damage called");
        if (true){
            print("take Damage called");
            //NetworkGameManager.instance.playerStat[lasthit].Team !
            if(networkTeam.Team != NetworkGameManager.instance.playerStat[lasthit].Team){
                localHealth -= ammount;
                localHealth = Mathf.Clamp(localHealth, 0, 100);
            }else{
                print("Friendly fire disabled");
            }

            print($"I got hit by {lasthit}");
            if(localHealth <= 0 && !hasGivenKill){
              CmdSetKill(lasthit,10);
              hasGivenKill = true;
              //Invoke(nameof(resetGivenKill),6f);
            }
        }
    }
    public void resetGivenKill(){
        hasGivenKill = false;
    }
    public void Respawn(){
        hasGivenKill = false;
        if(NetworkGameManager.instance.myTeam == "A"){
               // PlayerCamera.instance.getDestroy();    
               // CmdReSpawn(1);
               this.GetComponent<Animator>().SetTrigger("respawn");
               localHealth = 100;
               CmdSetHealth(100);
               this.transform.position = defaultPosition;

        }else{
            PlayerCamera.instance.getDestroy();
             this.GetComponent<Animator>().SetTrigger("respawn");
              this.transform.position = defaultPosition;
              localHealth = 100;
              CmdSetHealth(100);
           // CmdReSpawn(2);
        }
    }
    [Command]
    public void CmdReSpawn(int index){
        GameObject player = Instantiate(FindObjectOfType<NetworkManager>().spawnPrefabs[index]);
        NetworkServer.Spawn(player,connectionToClient);
        NetworkServer.Destroy(this.gameObject);
    }
    [Command]
    public void CmdSetKill(string id,int ammount){
          RpcSetKill(id,ammount);
    }
    [ClientRpc]
    public void RpcSetKill(string id, int ammount){
        //NetworkGameManager.instance.playerStat[id].SetKill(ammount);
        print("I got kill");
        NetworkGameManager.instance.IncreamentKills(id,ammount);
    }
}
