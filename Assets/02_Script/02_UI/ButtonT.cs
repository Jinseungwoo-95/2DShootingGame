using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonT : MonoBehaviour
{
    private bool isOn;
    [SerializeField] bool isEffect;
    [SerializeField] Text text;

    private void Start()
    {
        if(isEffect)
        {
            if (SoundManager.instance.effectOn)
            {
                isOn = true;
                text.text = "ON";
            }
            else
            {
                isOn = false;
                text.text = "Off";
            }
        }
        else
        {
            if (SoundManager.instance.bgmOn)
            {
                isOn = true;
                text.text = "ON";
            }
            else
            {
                isOn = false;
                text.text = "Off";
            }
        }
    }

    public void BtnClick()
    {
        isOn = !isOn;

        if (isOn)
        {
            if (!isEffect)
                SoundManager.instance.PlayBgm();
            else
                SoundManager.instance.effectOn = true;
            text.text = "ON";
        }
        else
        {
            if (!isEffect)
                SoundManager.instance.StopBgm();
            else
                SoundManager.instance.effectOn = false;
            text.text = "OFF";
        }

    }
}
