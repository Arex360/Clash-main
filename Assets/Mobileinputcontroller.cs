using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobileinputcontroller : MonoBehaviour
{
    public GameObject weaponUI;
    public GameObject movementUI;
    public GameObject permanUI;
    public GameObject rootUI;
    public GameObject weaponChooseUI;
    public static bool gameStarted;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            //weaponChooseUI.SetActive(true);
            movementUI.SetActive(true);
            permanUI.SetActive(true);
            rootUI.SetActive(true);

        }
        else
        {
            rootUI.SetActive(false);
        }
    }
}
