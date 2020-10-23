using System.Collections;
using UnityEngine;

public class Dark : Boss
{
    private bool isAttack;

    [SerializeField] private int cnt;   // 한번에 날릴 불렛 개수
    [SerializeField] private float angle;
    [SerializeField] private float force;
    [SerializeField] Animator effectAnim;
    [SerializeField] Animator laserAnim;
    [SerializeField] Transform laserTf;

    private void Update()
    {
        if (!isDeath && !notInRoom) 
        {
            dir = target.position - transform.position;
            SetAnimDir();
        }
    }
    protected override void PatternAtk()
    {
        patternIndex = patternIndex == patternMaxIndex ? 0 : patternIndex + 1;    // 다음 패턴 실행

        switch (patternIndex)
        {
            case 0:
                StartCoroutine(Pattern1());
                break;
            case 1:
                StartCoroutine(Pattern2());
                break;
            case 2:
                StartCoroutine(Pattern3());
                break;
        }
    }

    /// <summary>
    /// 바운드불렛을 cnt 만큼 angle 각도로 쏘는 패턴
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern1()
    {
        animator.SetBool("isAttack", true);
        float theta;
        Quaternion rot;
        for (int i = 0; i < patternMaxCnt[patternIndex]; i++)
        {
            yield return new WaitUntil(() => isAttack);

            float gap = cnt > 1 ? angle / (cnt - 1) : 0;
            float startAngle = bulletPoint.eulerAngles.z - (angle / 2.0f);

            for (int j = 0; j < cnt; j++)
            {
                theta = startAngle + gap * (float)j;
                rot = Quaternion.Euler(0, 0, theta);
                GameObject obj = ObjectPoolingManager.instance.GetBullet(BOUND, BulletName.Dark);
                obj.transform.position = bulletPoint.position;
                obj.transform.rotation = rot;
                obj.GetComponent<Rigidbody2D>().AddForce(obj.transform.right * force);
            }

            isAttack = false;
        }

        animator.SetBool("isAttack", false);
        yield return patternDelayTime;
        PatternAtk();
    }

    /// <summary>
    /// 스핀하면서 불렛 쏘는 패턴
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern2()
    {
        animator.SetBool("isSpin", true);

        for (int i = 0; i < patternMaxCnt[patternIndex]; i++)
        {
            GameObject obj = ObjectPoolingManager.instance.GetBullet(BASIC, BulletName.Dark);
            obj.transform.position = bulletPoint.position;
            obj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

            yield return patternAtkDelayTime[patternIndex];
        }

        animator.SetBool("isSpin", false);
        yield return patternDelayTime;
        PatternAtk();
    }

    private IEnumerator Pattern3()
    {
        animator.SetBool("isAttack", true);
        effectAnim.SetBool("isEffect", true);
        laserAnim.SetBool("isLaser", true);
        StartCoroutine("SpinCoroutine");

        for (int i = 0; i < patternMaxCnt[patternIndex]; i++)
        {
            yield return new WaitUntil(() => isAttack);
            GameObject obj = ObjectPoolingManager.instance.GetBullet(FOLLOW, BulletName.Dark);
            obj.transform.position = bulletPoint.position;
            obj.transform.rotation = bulletPoint.rotation;
            isAttack = false;
        }

        animator.SetBool("isAttack", false);
        effectAnim.SetBool("isEffect", false);
        laserAnim.SetBool("isLaser", false);
        StopCoroutine("SpinCoroutine");
        yield return patternDelayTime;
        PatternAtk();
    }

    private IEnumerator SpinCoroutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            laserTf.Rotate(Vector3.forward * 100f * Time.deltaTime);

            yield return null;
        }
    }

    /// <summary>
    /// 애니메이터 Dir 값 설정
    /// </summary>
    private void SetAnimDir()
    {
        float angle = Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z;

        if (angle < 10 || angle > 350)   // E
        {
            animator.SetFloat("DirX", 1.0f);
            animator.SetFloat("DirY", 0f);
        }
        else if (angle >= 10 && angle < 80)   // NE
        {
            animator.SetFloat("DirX", 1.0f);
            animator.SetFloat("DirY", 1.0f);
        }
        else if(angle >=80 && angle < 100)  // N
        {
            animator.SetFloat("DirX", 0f);
            animator.SetFloat("DirY", 1.0f);
        }
        else if(angle >= 100 && angle < 170)    // NW
        {
            animator.SetFloat("DirX", -1.0f);
            animator.SetFloat("DirY", 1.0f);
        }
        else if(angle >= 170 && angle < 190)    // W
        {
            animator.SetFloat("DirX", -1.0f);
            animator.SetFloat("DirY", 0f);
        }
        else if(angle >= 190 && angle < 260)    // SW
        {
            animator.SetFloat("DirX", -1.0f);
            animator.SetFloat("DirY", -1.0f);
        }
        else if(angle >= 260 && angle < 280)    // S
        {
            animator.SetFloat("DirX", 0f);
            animator.SetFloat("DirY", -1.0f);
        }
        else if(angle >= 280 && angle <= 350)   // SE
        {
            animator.SetFloat("DirX", 1.0f);
            animator.SetFloat("DirY", -1.0f);
        }
    }

    /// <summary>
    /// 애니메이션에서 이벤트로 호출
    /// </summary>
    private void SetAttack()
    {
        bulletPoint.right = target.position - bulletPoint.position;
        isAttack = true;
    }
}
