using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Camera miniMapCamera;
    public GameObject Raw;
    public GameObject miniMapWindow;
    bool check = false;
    // Start is called before the first frame update
    void Awake()
    {
        miniMapCamera.enabled = false;
        //Raw = GameObject.Find("MiniMap");
        Raw = GameObject.Find("UI").transform.Find("MiniMap").gameObject;
        miniMapWindow = GameObject.Find("UI").transform.Find("MiniMap").transform.Find("MiniMapWindow").gameObject;
        miniMapWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!check)
            {
                check=true;
                miniMapCamera.enabled = true;
                Raw.SetActive(true);
                miniMapWindow.SetActive(true);
            }
            else
            {
                check = false;
                miniMapCamera.enabled = false;
                Raw.SetActive(false);
                miniMapWindow.SetActive(true);
            }

        }
    }
}
