using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkPlayer : NetworkBehaviour
{
    public GameObject camRig;
    void Start()
    {
        if (isLocalPlayer)
        {
            camRig.transform.parent = null;
            Cursor.visible = false;
        }
        else
        {
            Destroy(camRig);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
