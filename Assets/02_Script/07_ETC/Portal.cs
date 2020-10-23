using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] bool goMain;
    GameObject npc;
    [SerializeField] static FadeManager fadeManager;
    private void Awake()
    {
        npc = GameObject.Find("ItemPanel").transform.GetChild(0).gameObject;
        if(fadeManager == null)
            fadeManager = GameObject.FindObjectOfType<FadeManager>();
        
    }

    private void Start()
    {
        if (goMain)
        {
            Invoke("InvokeFade", 1f);
            GameController.instance.CanMove = true;
        }
    }

    void InvokeFade()
    {
        fadeManager.FadeIn();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // 엔딩 크레딧
            if(GameController.instance.StageLevel == GameController.instance.MaxStage)
            {
                SceneManager.LoadScene("Ending");
            }
            // 스테이지로
            else if (goMain)
            {
                StartCoroutine(CoStage());
            }
            // 상점씬으로
            else
            {
                StartCoroutine(CoPreparation());
            }
        }
    }

    IEnumerator CoStage()
    {
        fadeManager.FadeOut();
        GameController.instance.CanMove = false;
        yield return new WaitForSeconds(1f);
        npc.SetActive(false);
        GameController.instance.CanShot = true;
        GameController.instance.preparation = false;
        GameController.instance.player.transform.position = Vector3.zero;
        SceneManager.LoadScene("BasementMain");
    }

    IEnumerator CoPreparation()
    {
        fadeManager.FadeOut();
        GameController.instance.CanMove = false;
        yield return new WaitForSeconds(1f);
        npc.SetActive(true);
        GameController.instance.CanShot = false;
        GameController.instance.preparation = true;
        GameController.instance.player.transform.position = Vector3.zero;
        SceneManager.LoadScene("Preparation");
    }
}
