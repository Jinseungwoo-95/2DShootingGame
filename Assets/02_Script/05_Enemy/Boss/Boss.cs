using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Boss : MonoBehaviour
{
    protected const string BASIC = "BASIC", FOLLOW = "FOLLOW", BOUND = "BOUND";
    public delegate void BulletClearHandler();
    public static event BulletClearHandler OnBulletClear;   // 보스가 죽을 시 불렛을 없애기 위한 이벤트
    
    public bool notInRoom;

    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform bulletPoint;
    [SerializeField] private int maxHP;
    [SerializeField] protected float moveSpeed;

    [SerializeField] protected int patternIndex;  // 현재 패턴 인덱스
    [SerializeField] protected int[] patternMaxCnt; // 패턴 반복 횟수
    [SerializeField] private float[] patternAtkDelay; // 패턴 공격 딜레이
    [SerializeField] private float patternDelay;  // 패턴 딜레이
    [SerializeField] private Slider healthBar;
    [SerializeField] private string deathSoundName;
    [SerializeField] private GameObject LootDrop;

    private ObjectBossSpawner objectBossSpawner;    // 보스 죽음 시 Portal 생성하기 위한 컴포넌트
    private BoxCollider2D boxCollider;
    protected int patternMaxIndex;
    private float currentHP;
    protected WaitForSeconds patternDelayTime;
    protected Dictionary<int, WaitForSeconds> patternAtkDelayTime;
    protected Vector3 dir; // 타겟 방향 벡터
    protected bool isDeath;

    private void Awake()
    {
        notInRoom = true;
        isDeath = false;
        boxCollider = GetComponent<BoxCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        patternMaxIndex = patternMaxCnt.Length - 1;

        // WaitForSeconds 초기화
        patternDelayTime = new WaitForSeconds(patternDelay);
        patternAtkDelayTime = new Dictionary<int, WaitForSeconds>();
        for (int i = 0; i < patternAtkDelay.Length; i++)
        {
            patternAtkDelayTime.Add(i, new WaitForSeconds(patternAtkDelay[i]));
        }

        GameObject.FindObjectOfType<FadeManager>().FadeIn();
        GameController.instance.CanMove = true;
    }

    private void Start()
    {
        healthBar = GameObject.Find("BossHP").transform.GetChild(0).GetComponent<Slider>();
        objectBossSpawner = FindObjectOfType<ObjectBossSpawner>();
        StartCoroutine(DialogueOffWait());
    }

    IEnumerator DialogueOffWait()
    {
        yield return new WaitUntil(() => DialogueManager.instance.BossAtkStart);

        // hpBar 생기게하기
        healthBar.value = CalculateHealthPercentage();
        healthBar.gameObject.SetActive(true);
        yield return patternDelayTime;

        PatternAtk();
    }

    protected abstract void PatternAtk();

    /// <summary>
    /// 피격 함수(플레이어 불렛 스크립트에서 SendMessage로 호출당함)
    /// </summary>
    /// <param name="damage"></param>
    private void DealDamage(float damage)
    {
        currentHP -= damage;
        healthBar.value = CalculateHealthPercentage();
        CheckDeath();
    }

    float CalculateHealthPercentage()
    {
        return (currentHP / maxHP);
    }

    private void CheckDeath()
    {
        if (currentHP <= 0)
        {
            DialogueManager.instance.BossAtkStart = false;
            isDeath = true;
            boxCollider.enabled = false;
            StopAllCoroutines();
            animator.SetTrigger("isDeath");
            SoundManager.instance.PlaySE(deathSoundName);
            healthBar.gameObject.SetActive(false);
            OnBulletClear?.Invoke();    // OnBulletClear 이벤트가 널이 아닐경우 실행
        }
    }

    /// <summary>
    /// 애니메이션 이벤트로 호출되어 게임오브젝트 파괴
    /// </summary>
    private void EnemyDestroy()
    {
        RoomController.Instance.StartCoroutine(RoomController.Instance.RoomCoroutine());
        objectBossSpawner.BossDeath();
        GameController.instance.StageLevel++;

        Destroy(gameObject);
    }
}
