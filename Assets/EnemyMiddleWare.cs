using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TPSShooter;
public class EnemyMiddleWare : NetworkManager
{
    public EnemyBehaviour eb;
    void Start()
    {
        eb = this.GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
