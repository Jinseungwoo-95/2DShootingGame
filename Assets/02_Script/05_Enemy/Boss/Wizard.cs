using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Boss
{
    private bool isMove;
    private List<Transform> bullets = new List<Transform>();   // 360도로 퍼졌다가 타겟을 향해 날라가는 불렛 리스트

    private void Update()
    {
        if (!isDeath && !notInRoom)
        {
            dir = target.position - transform.position;
            FlipCheck();

            if (isMove)
                transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    private void FlipCheck()
    {
        if (patternIndex != 3)
        {
            if (dir.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /// <summary>
    ///  패턴 공격
    /// </summary>
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
            case 3:
                StartCoroutine(Pattern4());
                break;
        }
    }

    /// <summary>
    /// 연사 하는 패턴
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern1()
    {
        animator.SetBool("Pattern1", true);
        isMove = true;
        yield return patternDelayTime;

        for (int i = 0; i < patternMaxCnt[patternIndex]; i++)
        {
            GameObject bullet = ObjectPoolingManager.instance.GetBullet(BASIC);
            bullet.transform.position = bulletPoint.position;
            bullet.transform.right = dir + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);

            yield return patternAtkDelayTime[patternIndex];
        }

        animator.SetBool("Pattern1", false);
        yield return patternDelayTime;
        PatternAtk();
    }

    /// <summary>
    ///  원으로 퍼졌다가 타겟으로 날라가는 패턴
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern2()
    {
        animator.SetBool("Pattern23", true);
        isMove = false;
        yield return patternDelayTime;

        for (int i = 0; i < patternMaxCnt[patternIndex]; i++)
        {
            // 360도 방향으로 불렛 발사
            for (int j = 0; j < 360; j += 15)
            {
                GameObject obj = ObjectPoolingManager.instance.GetBullet(BASIC);
                obj.transform.position = bulletPoint.position;
                obj.transform.rotation = Quaternion.Euler(0, 0, j);

                bullets.Add(obj.transform);
            }

            yield return patternDelayTime;

            // 타겟을 목표로 날라가기
            for (int k = 0; k < bullets.Count; k++)
            {
                Vector3 dir = target.transform.position - bullets[k].position;
                bullets[k].right = dir;
            }

            bullets.Clear();

            yield return patternAtkDelayTime[patternIndex];
        }
        animator.SetBool("Pattern23", false);
        yield return patternDelayTime;
        PatternAtk();
    }

    /// <summary>
    /// 360도 퍼지는 불렛 패턴 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern3()
    {
        animator.SetBool("Pattern23", true);
        isMove = true;
        yield return patternDelayTime;

        int numA = 25;
        int numB = 20;
        int bulletNum;   // 불렛 생성 개수
        float gap; // 불렛간의 간격
        float theta;

        // 360도 불렛 생성
        for (int i = 0; i < patternMaxCnt[patternIndex]; i++)
        {
            bulletNum = i % 2 == 0 ? numA : numB;

            gap = 360 / (float)bulletNum;

            for (int j = 0; j < bulletNum; j++)
            {
                theta = gap * j;

                GameObject obj = ObjectPoolingManager.instance.GetBullet(BASIC);
                obj.transform.position = bulletPoint.position;
                obj.transform.rotation = Quaternion.Euler(0, 0, theta);
            }
            yield return patternAtkDelayTime[patternIndex];
        }

        animator.SetBool("Pattern23", false);
        yield return patternDelayTime;
        PatternAtk();
    }

    /// <summary>
    /// 스핀 공격 패턴
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern4()
    {
        animator.SetBool("Pattern4", true);
        isMove = true;
        yield return patternDelayTime;
        Coroutine coroutine = StartCoroutine(SpinCoroutine());

        for (int i = 0; i < patternMaxCnt[patternIndex]; i++)
        {
            GameObject obj = ObjectPoolingManager.instance.GetBullet(BASIC);
            obj.transform.position = bulletPoint.position;
            obj.transform.rotation = bulletPoint.rotation;

            GameObject obj2 = ObjectPoolingManager.instance.GetBullet(BASIC);
            obj2.transform.position = bulletPoint.position;
            obj2.transform.rotation = Quaternion.Euler(-bulletPoint.eulerAngles);

            yield return patternAtkDelayTime[patternIndex];
        }

        StopCoroutine(coroutine);
        animator.SetBool("Pattern4", false);
        yield return patternDelayTime;
        PatternAtk();
    }

    private IEnumerator SpinCoroutine()
    {
        while (true)
        {
            bulletPoint.Rotate(Vector3.forward * 180f * Time.deltaTime);

            yield return null;
        }
    }
}
