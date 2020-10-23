using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private float health;
    [SerializeField] private float maxHealth;

    private bool isInvincibility = false;

    public GameObject player;
    public SpriteRenderer playeSpriteRenderer;

    [SerializeField] Slider healthSlider;
    [SerializeField] Text coinsValue;
    [SerializeField] Text inventoryCoinTxt;
    private Image bloodScreen;
    // 아이템 사용 관련
    Dictionary<string, int> itemDic;
    [SerializeField] Item[] items;  // 아이템 효과용 리스트

    private int maxStage;
    public int MaxStage { get { return maxStage; } }

    public float coins;

    public bool preparation;    // 현재 준비씬인지 체크용도

    [SerializeField] private int stageLevel;
    public int StageLevel { get { return stageLevel; } set { stageLevel = value; } }

    private bool canMove;
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    private bool openStore;
    public bool OpenStore { get { return openStore; } set { openStore = value; } }

    private bool canShot;
    public bool CanShot { get { return canShot; } set { canShot = value; } }
         

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        maxStage = 3;
        preparation = true;
        itemDic = new Dictionary<string, int>();
        // 아이템 효과 초기화
        for (int i = 0; i < items.Length; i++)
        {
            itemDic.Add(items[i].name, items[i].value);
        }

        stageLevel = 1;
        canMove = true;
        openStore = false;
        canShot = false;

        health = maxHealth;

        Transform statePanel = GameObject.Find("StatePanel").transform;

        player = GameObject.FindGameObjectWithTag("Player");
        playeSpriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        coinsValue = statePanel.GetChild(0).GetComponent<Text>();
        healthSlider = statePanel.GetChild(1).GetComponent<Slider>();
        bloodScreen = GameObject.Find("BloodScreen").GetComponent<Image>();
        inventoryCoinTxt = GameObject.Find("Inventory").transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>();
        SetHealthUI();
        coinsValue.text = "Gold: " + coins.ToString();
        inventoryCoinTxt.text = coins.ToString();
    }

    private void SetHealthUI()
    {
        healthSlider.value = health / maxHealth;
    }

    public void DamagePlayer(float damage)
    {
        if (!isInvincibility)
        {
            isInvincibility = true;
            StartCoroutine(Invincibility());
            health -= damage;
            CameraController.Instance.ShakeTime = 0.2f;
            StartCoroutine(ShowBloodScreen());
            CheckDeath();
            SetHealthUI();
        }
    }

    IEnumerator Invincibility()
    {
        int countTime = 0;
        while (countTime < 10)
        {
            if (countTime % 2 == 0)
                playeSpriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                playeSpriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.2f);
            countTime++;
        }
        playeSpriteRenderer.color = new Color32(255, 255, 255, 255);

        isInvincibility = false;

        yield return null;
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.3f, 0.5f));
        yield return new WaitForSeconds(0.3f);
        bloodScreen.color = Color.clear;
    }

    public void AddCoin(float coin)
    {
        coins += coin;
        coinsValue.text = "Gold: " + coins.ToString();
        inventoryCoinTxt.text = coins.ToString();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            health = 0;
            bloodScreen.color = Color.clear;
            CameraController.Instance.ShakeTime = 0f;
            CanShot = false;
            SetMove(false);
            UIManager.instance.go_end.SetActive(true);
        }
    }

    public void SetMove(bool _value)
    {
        canMove = _value;
        if (_value)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
    }

    public void UseItem(string _itemName)
    {
        health = Mathf.Min(maxHealth, health + itemDic[_itemName]);
        SetHealthUI();
    }
}
