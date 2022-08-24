using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TPSShooter;
public class PlayerHealth : NetworkBehaviour
{
    private PlayerBehaviour playerBehaviour;
    private Animator animator;
    public float localHealth;
    [SyncVar(hook="OnChangeHealth")]
    public float health;
    public UnityEngine.UI.Image healthFill;
    public float fillAmmout;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        playerBehaviour = this.GetComponent<PlayerBehaviour>();
        if(hasAuthority){
            Destroy(healthFill);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasAuthority){
            CmdSetHealth(localHealth);
        }
        fillAmmout = health/100;
        if(localHealth <= 0){
            playerBehaviour.enabled= false;
        }
    }
    [Command]
    public void CmdSetHealth(float _health){
        health = _health;
    }
    public void OnChangeHealth(float oldV,float newV){
        float perc = newV/100;
        if(healthFill){
            healthFill.fillAmount = perc;
        }
        if(newV <= 0){
           animator.SetTrigger("Died");
        }
    }
    public void TakeDamage(float ammount){
        if(hasAuthority){
            localHealth -= ammount;
            
        }
    }
}
