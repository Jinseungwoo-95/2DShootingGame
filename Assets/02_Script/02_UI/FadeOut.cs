using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FadeOut : MonoBehaviour
{
    [SerializeField] Image BGimage;
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] Transform monsters;
    [SerializeField] Animator creditAnimatior;
    Animator animator;

    private void Awake()
    {
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("UI"));
        animator = GetComponent<Animator>();
        Invoke("AnimationStart", 1f);
    }

    void AnimationStart()
    {
        particles = GameObject.FindObjectsOfType<ParticleSystem>();
        animator.SetTrigger("Start");
    }

    void BGFadeOut()
    {
        StartCoroutine(FadeOutColor());
    }

    IEnumerator FadeOutColor()
    {
        // 배경 이미지 알파값 조절
        Color color = BGimage.color;
        while(color.a >= 0.5f)
        {
            color.a -= Time.deltaTime;
            BGimage.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        // 몬스터들 이동
        while (monsters.position.x < 14)
        {
            
            monsters.Translate(Vector3.right * Time.deltaTime * 3f);

            for (int i = 0; i < 3; ++i)
            {
                if (!particles[i].isPlaying)
                    particles[i].Play();
            }

            yield return null;
        }

        // 플레이어 ~~

        // 크레딧
        creditAnimatior.SetTrigger("credit");


    }

}
