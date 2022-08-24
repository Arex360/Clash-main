using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    void Start()
    {
        instance = this;
    }

    public void getDestroy(){
        //Destroy(this.gameObject);
    }
}
