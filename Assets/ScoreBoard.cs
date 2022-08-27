using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;
    public GameObject body;
    public bool shown;
    void Start()
    {
        instance = this;
        body = this.transform.GetChild(0).transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            shown = !shown;
        }
        body.SetActive(shown);
    }
    public void ToggleShow()
    {
        shown = !shown;
    }
}
