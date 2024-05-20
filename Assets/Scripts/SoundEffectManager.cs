using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    //tao 1 the hien duy nhat
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;//dam bao rang no la duy nhat
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);//dam bao no se k bi pha huy khi tai scence moi
        }
        else
        {
            Destroy(gameObject);//neu da co 1 the hien thi pha huy doi tuong
        }
    }


    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);//phat am thanh 1 lan
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        //chi can thiet lap su kien, moi khi gia tri thay doi no se goi, k can dat trong update
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChange(); });//goi su kien khi gia tri thay doi vi nguoi dung dieu chinh tren man hinh
    }


    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;//
    }

    public void OnValueChange()
    {
        SetVolume(sfxSlider.value);//vi sfxSlider.value se cho gia tri tu 0f-1f nen co the set volume cho audiosource
    }
}
