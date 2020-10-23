using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    Image image;
    Color color;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void FadeOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(CoFadeOut(_speed));
    }

    IEnumerator CoFadeOut(float _speed)
    {
        color = image.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            image.color = color;

            yield return waitTime;
        }
    }

    public void FadeIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }

    IEnumerator FadeInCoroutine(float _speed)
    {
        color = image.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            image.color = color;

            yield return waitTime;
        }
    }
}
