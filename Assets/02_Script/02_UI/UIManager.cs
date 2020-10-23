using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private bool MenuActivated = false;

    private GameObject curPanel;
    [SerializeField] private GameObject go_Menu;
    [SerializeField] private GameObject go_option;
    [SerializeField] private GameObject go_Info;
    [SerializeField] private Text effectTxt;
    [SerializeField] private Text bgmTxt;
    [SerializeField] public GameObject go_end;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);

        if (SoundManager.instance.effectOn)
            effectTxt.text = "On";
        else
            effectTxt.text = "Off";

        if (SoundManager.instance.bgmOn)
            bgmTxt.text = "On";
        else
            bgmTxt.text = "Off";
    }

    void Update()
    {
        // Esc키 눌리고 상점이 오픈되어있지 않고 현재패널이 널 일 경우만 메뉴 온/오프 가능
        if (Input.GetKeyDown(KeyCode.Escape) && !GameController.instance.OpenStore && curPanel == null)
        {
            if (MenuActivated == false)
            {
                OpenMenu();
                GameController.instance.SetMove(false);
                GameController.instance.CanShot = false;
                MenuActivated = true;
            }
            else if (MenuActivated == true)
            {
                CloseMenu();
                GameController.instance.SetMove(true);
                if (!GameController.instance.preparation)
                    GameController.instance.CanShot = true;
                MenuActivated = false;
            }
        }
    }


    public void OpenMenu()
    {
        go_Menu.SetActive(true);
      
    }

    public void CloseMenu()
    {
        go_Menu.SetActive(false);
    }

    public void ReplayBtnClick()
    {
        Destroy(gameObject);
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("GameController"));
        Destroy(GameObject.Find("ObjectPoolingManager"));
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    public void RestartBtnClick()
    {
        Destroy(gameObject);
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("GameController"));
        Destroy(GameObject.Find("ObjectPoolingManager"));
        Time.timeScale = 1f;
        SceneManager.LoadScene("Preparation");
    }

    public void OptionBtnClick()
    {
        CloseMenu();
        OpenPanel(go_option);
    }

    private void OpenPanel(GameObject _gameObject)
    {
        curPanel = _gameObject;
        curPanel.SetActive(true);
    }

    public void InfoBtnClick()
    {
        CloseMenu();
        OpenPanel(go_Info);
    }

    public void BackgroundBtnClick()
    {
        string temp = bgmTxt.text;
        if (temp == "On")
        {
            bgmTxt.text = "Off";
            SoundManager.instance.StopBgm();
        }
        else
        {
            bgmTxt.text = "On";
            SoundManager.instance.PlayBgm();
        }
    }

    public void effectSoundClick()
    {
        Debug.Log("effClick");
        string temp = effectTxt.text;
        if (temp == "On")
        {
            effectTxt.text = "Off";
            SoundManager.instance.effectOn = false;
        }
        else
        {
            effectTxt.text = "On";
            SoundManager.instance.effectOn = true;
        }
    }

    public void BackBtnClick()
    {
        curPanel.SetActive(false);
        curPanel = null;
        OpenMenu();
    }


    public void ExitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    // 에디터에서 종료
#else
        Application.Quit(); // 애플리케이션에서 종료
#endif
    }
}
