using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Death,
    Attack
};

public class EnemyController : MonoBehaviour
{
    public EnemyState currentState;
    public bool notInRoom;  // 플레이어가 룸안에 있는지 체크 변수 (false == 플레이어가 방안에 있다)

    [SerializeField] float maxHP;
    [SerializeField] GameObject lootDrop;   // 죽을 시 드랍되는 아이템
    [SerializeField] private float range;   // 탐색 범위
    [SerializeField] private float attackRange; // 공격 범위
    [SerializeField] private float speed;   // 이동 스피드
    [SerializeField] private float coolDown;    // 공격 쿨타임
    [SerializeField] private AttackType attackType; // 공격 타입
    [SerializeField] private Transform bulletPoint; // 불렛 나가는 위치
    
    private float currentHP;
    private Animator animator;
    private Vector3 dir;    // 플레이어를 향한 벡터
    private Transform player;
    private bool coolDownAttack = false;

    // Wander 관련 변수
    private bool chooseDir = false;
    private Vector3 WanderPosition;

    private void Awake()
    {
        currentState = EnemyState.Idle;
        currentHP = maxHP;
        notInRoom = true;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!notInRoom)
        {
            CheckState();
            TakeAction();
        }
    }

    /// <summary>
    /// 상황에 따라 currentState 변경
    /// </summary>
    private void CheckState()
    {
        if (currentState != EnemyState.Death)
        {
            if (IsPlayerInRange(attackRange))
            {
                currentState = EnemyState.Attack;
                animator.SetBool("isMove", false);
            }
            else if (IsPlayerInRange(range))
            {
                currentState = EnemyState.Follow;
                animator.SetBool("isMove", true);
            }
            else
            {
                currentState = EnemyState.Wander;
                animator.SetBool("isMove", true);
            }
        }
    }

    /// <summary>
    /// currentState에 따른 동작
    /// </summary>
    private void TakeAction()
    {
        switch (currentState)
        {
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Attack):
                TryAttack();
                break;
        }
    }

    /// <summary>
    /// 서성거리기
    /// </summary>
    void Wander()
    {
        if(!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position = Vector2.MoveTowards(transform.position, WanderPosition, speed * Time.deltaTime);
    }

    /// <summary>
    /// 플레이어를 향해 이동
    /// </summary>
    void Follow()
    {
        dir = player.position - transform.position;
        FlipCheck(dir.x);

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    /// <summary>
    /// 공격 동작
    /// </summary>
    void TryAttack()
    {
        dir = player.position - transform.position;
        FlipCheck(dir.x);

        if (!coolDownAttack)
        {
            bulletPoint.right = player.position - bulletPoint.position;
            animator.SetTrigger("isAttack");
            attackType.Attack(bulletPoint);
            StartCoroutine(CoolDown());
        }
    }

    /// <summary>
    /// 범위에 플레이어 있는지 체크
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private void FlipCheck(float _x)
    {
        if (_x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /// <summary>
    /// 상태가 Wander일 때 랜덤 위치 설정
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChooseDirection()
    {
        chooseDir = true;

        yield return new WaitForSeconds(Random.Range(0f, 5f));

        WanderPosition = transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);

        FlipCheck(WanderPosition.x - transform.position.x);

        yield return new WaitForSeconds(3f);
        chooseDir = false;
    }

    /// <summary>
    /// 공격 쿨타임 관리
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    /// <summary>
    /// 피격으로 인한 hp 감소
    /// </summary>
    /// <param name="damage"></param>
    private void DealDamage(float damage)
    {
        currentHP -= damage;
        CheckDeath();
    }

    /// <summary>
    ///  Death 체크
    /// </summary>
    public void CheckDeath()
    {
        if (currentHP <= 0)
        {
            currentState = EnemyState.Death;
            animator.SetTrigger("isDeath");
        }
    }

    /// <summary>
    /// 게임오브젝트 파괴(애니메이션 이벤트 호출)
    /// </summary>
    private void EnemyDestroy()
    {
        RoomController.Instance.StartCoroutine(RoomController.Instance.RoomCoroutine());
        Instantiate(lootDrop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
