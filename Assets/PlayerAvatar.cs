using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    public Vector3 position;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkGameManager.instance.localPlayer) return;
        position = NetworkGameManager.instance.localPlayer.transform.position;
        position.y = this.transform.position.y;
        this.transform.LookAt(position);
    }
}
