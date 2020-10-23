using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] Button optionBtn;
    [SerializeField] Button exitBtn;

    [SerializeField] string storyName;
    [SerializeField] GameObject title;
    [SerializeField] GameObject option;

    public void StartBtnClick()
    {
        Debug.Log("Start");
        SceneManager.LoadScene(storyName);
    }

    public void OptionBtnClick()
    {
        Debug.Log("Option");
        title.SetActive(false);
        option.SetActive(true);
    }

    public void BackBtnClick()
    {
        title.SetActive(true);
        option.SetActive(false);
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
