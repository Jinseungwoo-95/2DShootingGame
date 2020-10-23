using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; 
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;    // 싱글톤

    [SerializeField] private int sourceCnt; // 오디오소스 생성 개수
    [SerializeField] private AudioSource[] audioSources;    // 0번은 bgm용, 1~ 부터는 effect용

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    public bool effectOn;
    public bool bgmOn;

    [SerializeField] Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 다른 씬으로 갈 때 파괴 안되도록 하기
        }
        else
            Destroy(gameObject);

        effectOn = true;
        bgmOn = true;

        // 오디오클립 딕셔너리 초기화
        for (int i = 0; i < effectSounds.Length; i++)
        {
            audioClipsDic.Add(effectSounds[i].name, effectSounds[i].clip);
        }

        CreateAudioSources();
    }

    private void CreateAudioSources()
    {
        audioSources = new AudioSource[sourceCnt];

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        audioSources[0] = source;

        audioSources[0].clip = bgmSounds[0].clip;
        audioSources[0].Play();

        for (int i = 1; i < sourceCnt; i++)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            audioSources[i] = source;
        }
    }

    public void PlayBgm()
    {
        bgmOn = true;
        audioSources[0].UnPause();
    }

    public void StopBgm()
    {
        bgmOn = false;
        audioSources[0].Pause();
    }

    public void PlaySE(string _name, float _volume = 1f)
    {
        if (effectOn)
        {
            if (audioClipsDic.ContainsKey(_name))
            {
                for (int j = 1; j < audioSources.Length; j++)
                {
                    if (!audioSources[j].isPlaying)
                    {
                        audioSources[j].PlayOneShot(audioClipsDic[_name], _volume);
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                return;
            }
            Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
        }
    }

    public void StopAllSE()
    {
        for (int i = 1; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
        }
    }
}
