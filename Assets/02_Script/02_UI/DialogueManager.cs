using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public Text text;
    public Image rendererSprite;
    public Image rendererDialogueWindow;

    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;
    private List<bool> listIsRight;

    private int count;  // 대화 진행 상황 카운트

    private bool onlyText = false;

    public bool talking = false;    // 대화 중에만 Z키 입력 받기 위한 변수
    private bool keyActivated = false;  // Z키 연타로 인한 이미지 안나오는걸 방지하기 위한 변수

    public Animator animSprite;
    public Animator animDialogoueWindow;

    private bool bossAtkStart;
    public bool BossAtkStart { get { return bossAtkStart; } set { bossAtkStart = value; } }

    // 사운드
    //private AudioManager theAudio;
    //public string typeSound;
    //public string enterSound;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //theAudio = FindObjectOfType<AudioManager>();
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        listIsRight = new List<bool>();
        count = 0;
        text.text = "";
        bossAtkStart = false;
    }

    private void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                keyActivated = false;
                count++;
                text.text = "";

                //theAudio.Play(enterSound);

                // 다 읽은 경우 코루틴 스톱!!
                if (count == listSentences.Count)
                {
                    StopAllCoroutines();
                    ExitDialogue();
                }
                else
                {
                    StopAllCoroutines();

                    if (onlyText)
                        StartCoroutine(StartTextCoroutine());
                    else
                        StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
    }

    public void ShowText(string[] _sentences)
    {
        talking = true;
        onlyText = true;

        for (int i = 0; i < _sentences.Length; i++)
        {
            listSentences.Add(_sentences[i]);
            
        }

        StartCoroutine(StartTextCoroutine());
    }


    public void ShowDialogue(Dialogue _dialogue)
    {
        talking = true;
        onlyText = false;


        for (int i = 0; i < _dialogue.sentences.Length; i++)
        {
            listSentences.Add(_dialogue.sentences[i]);
            listSprites.Add(_dialogue.sprites[i]);
            listDialogueWindows.Add(_dialogue.dialogueWindows[i]);
            listIsRight.Add(_dialogue.isRight[i]);
        }

        animSprite.SetBool("Appear", true);
        animDialogoueWindow.SetBool("Appear", true);

        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartTextCoroutine()
    {
        keyActivated = true;

        for (int i = 0; i < listSentences[count].Length; i++)
        {
            // 한 글자씩 추가하기
            text.text += listSentences[count][i];
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0)
        {
            // 대사바가 변경 될 시 대사바와 캐릭터 변경
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                animSprite.SetBool("IsRight", listIsRight[count]);
                animSprite.SetBool("Change", true);
                animDialogoueWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.sprite = listDialogueWindows[count];
                rendererSprite.sprite = listSprites[count];
                animDialogoueWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
            else
            {
                // 캐릭터의 스프라이트만 변경 될 시
                if (listSprites[count] != listSprites[count - 1])
                {
                    Debug.Log(listIsRight[count]);
                    animSprite.SetBool("IsRight", listIsRight[count]);
                    animSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);
                    rendererSprite.sprite = listSprites[count];
                    animSprite.SetBool("Change", false);
                }
                // 아무 변경 없을 시 잠시 대기
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        else
        {
            rendererDialogueWindow.sprite = listDialogueWindows[count];
            rendererSprite.sprite = listSprites[count];
        }

        keyActivated = true;

        for (int i = 0; i < listSentences[count].Length; i++)
        {
            // 한 글자씩 추가하기
            text.text += listSentences[count][i];
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ExitDialogue()
    {
        GameController.instance.CanMove = true;
        GameController.instance.CanShot = true;
        bossAtkStart = true;
        count = 0;
        text.text = "";
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        listIsRight.Clear();
        animSprite.SetBool("Appear", false);
        animDialogoueWindow.SetBool("Appear", false);

        talking = false;
    }

}
