using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject movementInput;
    public GameObject weaponInput;
    public GameObject permnaInput;
    public GameObject weaponInputChoose;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuController.gameStarted)
        {
            canvas.SetActive(true);
            movementInput.SetActive(true);
            weaponInput.SetActive(true);
            permnaInput.SetActive(true);
            //weaponInputChoose.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
            movementInput.SetActive(false);
            weaponInput.SetActive(false);
            permnaInput.SetActive(false);
            //weaponInputChoose.SetActive(false);
        }
    }
}
