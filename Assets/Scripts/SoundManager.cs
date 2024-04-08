using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
  * Scene:All
  * Object:SoundManager
  * Description: Skripta zaduzena za zvuke u apliakciji, njihovo pustanje, gasenje itd...
  **/
public class SoundManager : MonoBehaviour {

	public static int musicOn = 1;
	public static int soundOn = 1;
	public static bool forceTurnOff = false;

	public AudioSource buttonClick;
	public AudioSource buttonClick2;
	public AudioSource buttonClick3;

	public AudioSource idleSound;

    public AudioSource buySound;
    public AudioSource cashRegisterCloseSound;
    public AudioSource knifeCut;
    public AudioSource actionCompletionSound;
    public AudioSource dragIngredientCompletedSound;
    public AudioSource panFrying;
    public AudioSource stoveBakingSound;
    public AudioSource mixingFoodSound;
    public AudioSource doughStretchingSound;
    public AudioSource poowderingSound;
    public AudioSource grateSound;
    public AudioSource eggBreakingSound;
    public AudioSource liquidSound;
    public AudioSource waffleMakerSound;
    public AudioSource milkshakeMixerSound;
    public AudioSource rotisserieOvenSound;
    public AudioSource cirtrusJuicerSound;
    public AudioSource kitchenMixerSound;
    public AudioSource effect;


    static SoundManager instance;

	public static SoundManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
			}

			return instance;
		}
	}

	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);

		if(PlayerPrefs.HasKey("SoundOn"))
		{
			musicOn = PlayerPrefs.GetInt("MusicOn");
			soundOn = PlayerPrefs.GetInt("SoundOn");
		}

        if (soundOn == 0)
            MuteAllSounds();
		Screen.sleepTimeout = SleepTimeout.NeverSleep; 
	}

	public void Play_ButtonClick()
	{
		if(buttonClick != null && soundOn == 1)
			buttonClick.Play();
	}

    public void Play_ButtonClick2()
    {
        if (buttonClick2 != null && soundOn == 1)
            buttonClick.Play();
    }

    public void Play_ButtonClick3()
    {
        if (buttonClick3 != null && soundOn == 1)
            buttonClick3.Play();
    }

	public void Play_IdleMusic()
	{
		if(idleSound != null && musicOn == 1)
			idleSound.Play();
	}

	public void Stop_IdleMusic()
	{
		if(idleSound != null && musicOn == 1)
			idleSound.Stop();
	}

    public void Play_CashRegisterSound()
    {
        if (cashRegisterCloseSound != null && cashRegisterCloseSound.clip!=null &&soundOn==1)
            cashRegisterCloseSound.Play();
    }

    public void Play_BuySound()
    {
        if (buySound != null && buySound.clip != null && soundOn == 1)
            buySound.Play();
    }

    public void Play_KnifeCut()
    {
        if(knifeCut!=null&& knifeCut.clip!=null &&soundOn==1)
        {
            knifeCut.Play();
        }
    }

    public void Play_ActionCompleted()
    {
        if (actionCompletionSound != null && actionCompletionSound.clip != null && soundOn == 1)
            actionCompletionSound.Play();
    }

    public void Play_DragIngredientCompletedSound()
    {
        if (dragIngredientCompletedSound != null && dragIngredientCompletedSound.clip != null && soundOn == 1)
            dragIngredientCompletedSound.Play();
    }

    public void Play_PanFrying()
    {
        if (panFrying != null && panFrying.clip != null && soundOn == 1)
            panFrying.Play();
    }

    public void Play_StoveBaking()
    {
        if (stoveBakingSound != null && stoveBakingSound.clip != null && soundOn == 1)
            stoveBakingSound.Play();
    }

    public void Play_MixingFood()
    {
        if(mixingFoodSound!=null&&mixingFoodSound.clip!=null&&soundOn==1)
        {
            mixingFoodSound.Play();
        }
    }

    public void Play_DoughStretch()
    {
        if (doughStretchingSound != null && doughStretchingSound.clip != null && soundOn == 1)
            doughStretchingSound.Play();
    }

    public void Play_Powdering()
    {
        if (poowderingSound != null && poowderingSound.clip != null && soundOn == 1)
            poowderingSound.Play();
    }

    public void Play_Grate()
    {
        if (grateSound != null && grateSound.clip != null && soundOn == 1)
            grateSound.Play();
    }

    public void Play_EggBreak()
    {
        if (eggBreakingSound != null && eggBreakingSound.clip != null && soundOn == 1)
            eggBreakingSound.Play();
    }

    public void Play_LiquidSound()
    {
        if (liquidSound != null && liquidSound.clip != null && soundOn == 1)
            liquidSound.Play();
    }

    public void Play_WaffleMakerSound()
    {
        if (waffleMakerSound != null && waffleMakerSound.clip != null && soundOn == 1)
            waffleMakerSound.Play();
    }

    public void Play_MilkshakeMixerSoun()
    {
        if (milkshakeMixerSound != null && milkshakeMixerSound.clip != null && soundOn == 1)
            milkshakeMixerSound.Play();
    }

    public void Play_RotisserieOvenSound()
    {
        if (rotisserieOvenSound != null && rotisserieOvenSound.clip != null && soundOn == 1)
            rotisserieOvenSound.Play();
    }

    public void Play_CitrusJuicerSound()
    {
        if (cirtrusJuicerSound != null && cirtrusJuicerSound.clip != null && soundOn == 1)
            cirtrusJuicerSound.Play();
    }
    
    public void Play_KitchenMixerSound()
    {
        if (kitchenMixerSound != null && kitchenMixerSound.clip != null && soundOn == 1)
            kitchenMixerSound.Play();
    }


    public void PlayEffect(AudioClip clip)
    {
        effect.clip = clip;
        effect.Play();
    }

    public void StopEffect()
    {
        effect.Stop();
    }


   


	/// <summary>
	/// Corutine-a koja za odredjeni AudioSource, kroz prosledjeno vreme, utisava AudioSource do 0, gasi taj AudioSource, a zatim vraca pocetni Volume na pocetan kako bi AudioSource mogao opet da se koristi
	/// </summary>
	/// <param name="sound">AudioSource koji treba smanjiti/param>
	/// <param name="time">Vreme za koje treba smanjiti Volume/param>
	IEnumerator FadeOut(AudioSource sound, float time)
	{
		float originalVolume = sound.volume;
		while(sound.volume != 0)
		{
			sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
			yield return null;
		}
		sound.Stop();
		sound.volume = originalVolume;
	}

	/// <summary>
	/// F-ja koja Mute-uje sve zvuke koja su deca SoundManager-a
	/// </summary>
	public void MuteAllSounds()
	{
		foreach (Transform t in transform)
		{
			t.GetComponent<AudioSource>().mute = true;
		}
	}

	/// <summary>
	/// F-ja koja Unmute-uje sve zvuke koja su deca SoundManager-a
	/// </summary>
	public void UnmuteAllSounds()
	{
		foreach (Transform t in transform)
		{
			t.GetComponent<AudioSource>().mute = false;
		}
	}
	
}
