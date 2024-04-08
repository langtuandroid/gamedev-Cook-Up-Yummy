using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

/**
* Scene:All
* Object:Canvas
* Description: Skripta zaduzena za hendlovanje(prikaz i sklanjanje svih Menu-ja,njihovo paljenje i gasenje, itd...)
**/
public class MenuManager : MonoBehaviour 
{
	
	public Menu currentMenu;
	Menu currentPopUpMenu;
//	[HideInInspector]
//	public Animator openObject;
	public GameObject[] disabledObjects;
	GameObject ratePopUp, crossPromotionInterstitial;

    static bool justStarted=true;
    public static string lastScene;
	
	void Start () 
	{
        

		if (disabledObjects!=null) {
			for(int i=0;i<disabledObjects.Length;i++)
			{
				disabledObjects[i].SetActive(false);
			}
		}

		
		if(Application.loadedLevelName=="HomeScene")
		{
			
			if(PlayerPrefs.HasKey("alreadyRated"))
			{
				Rate.alreadyRated = PlayerPrefs.GetInt("alreadyRated");
                transform.Find("HomeScreen/RateButtonHolder").gameObject.SetActive(false);
			}
			else
			{
				Rate.alreadyRated = 0;
			}
			
			if(Rate.alreadyRated==0)
			{
				Rate.appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
				Debug.Log("appStartedNumber "+Rate.appStartedNumber);
				
				if(Rate.appStartedNumber>=6)
				{
					Rate.appStartedNumber=0;
					PlayerPrefs.SetInt("appStartedNumber",Rate.appStartedNumber);
					PlayerPrefs.Save();
                    ratePopUp = transform.Find("PopUps/PopUpRate").gameObject;
                    ShowPopUpMenu(ratePopUp);
					
				}
				else
				{
                    if(!justStarted)
                    {
                    }
				}
			}
			else
			{
                if (!justStarted)
                {
                }
            }

            if (PlayerPrefs.HasKey("RateNoThanks"))
                transform.Find("HomeScene/RateUs").gameObject.SetActive(false);

		}

        if (justStarted)
            justStarted = false;

	}
	
	/// <summary>
	/// Funkcija koja pali(aktivira) objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se upali</param>
	public void EnableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (!gameObject.activeSelf) 
			{
				gameObject.SetActive (true);
			}
		}
	}

    public void RateNoThanks()
    {
        PlayerPrefs.SetString("RateNoThanks", "clicked");
        PlayerPrefs.Save();
    }

	/// <summary>
	/// Funkcija koja gasi objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se ugasi</param>
	public void DisableObject(GameObject gameObject)
	{
		Debug.Log("Disable Object");
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}
	
	/// <summary>
	/// F-ja koji poziva ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadScene(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevel (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}
	
	/// <summary>
	/// F-ja koji poziva asihrono ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadSceneAsync(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevelAsync (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}

	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao Menu
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ShowMenu(GameObject menu)
	{
		if (currentMenu != null)
		{
			currentMenu.IsOpen = false;
			currentMenu.gameObject.SetActive(false);
		}
		
		currentMenu = menu.GetComponent<Menu> ();
		menu.gameObject.SetActive (true);
		currentMenu.IsOpen = true;
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao Meni
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void CloseMenu(GameObject menu)
	{
		if (menu != null) 
		{
			menu.GetComponent<Menu> ().IsOpen = false;
			menu.SetActive (false);
		}
	}

	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void ShowPopUpMenu(GameObject menu)
	{
		menu.gameObject.SetActive (true);
		currentPopUpMenu = menu.GetComponent<Menu> ();
		currentPopUpMenu.IsOpen = true;
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao PopUp-a, poziva inace coroutine-u, ima delay zbog animacije odlaska Menu-ja
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ClosePopUpMenu(GameObject menu)
	{
		StartCoroutine("HidePopUp",menu);
	}

	/// <summary>
	/// Couorutine-a za zatvaranje Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje, mora imati na sebi skriptu Menu.</param>
	IEnumerator HidePopUp(GameObject menu)
	{
		menu.GetComponent<Menu> ().IsOpen = false;
		yield return new WaitForSeconds(1.3f);

		menu.SetActive (false);
	}

	/// <summary>
	/// Funkcija za prikaz poruke preko Log-a, prilikom klika na dugme
	/// </summary>
	/// /// <param name="message">poruka koju treba prikazati.</param>
	public void ShowMessage(string message)
	{
		Debug.Log(message);
	}

	/// <summary>
	/// Funkcija koja podesava naslov CustomMessage-a, i ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageCustomMessageText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpMessageTitleText(string messageTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
	}

    public delegate void delegat();
	public void ShowPopUpMessage(string title,string messageText,delegat okaction)
	{
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = title;
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextHeader").GetComponent<Text>().text = messageText;
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ButtonsHolder/ButtonOk").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ButtonsHolder/ButtonOk").GetComponent<Button>().onClick.AddListener(delegate {
            okaction();
        });

		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

    public void ShowPopUpMessage(string title, string messageText)
    {
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = title;
    }

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
	{
		transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
		transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ContentHolder/TextBG/TextHeader").GetComponent<Text>().text=dialogMessageText;
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ButtonsHolder/ButtonYes").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ButtonsHolder/ButtonNo").GetComponent<Button>().onClick.RemoveAllListeners();
        ShowPopUpMenu(transform.Find("PopUps/PopUpDialog").gameObject);
	}

    public void ShowPopUpDialog(string dialogTitleText,string dialogMessageText,UnityAction yes,UnityAction no)
    {
        ShowPopUpDialog(dialogTitleText, dialogMessageText);
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ButtonsHolder/ButtonYes").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ButtonsHolder/ButtonNo").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ButtonsHolder/ButtonYes").GetComponent<Button>().onClick.AddListener(yes);
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ButtonsHolder/ButtonNo").GetComponent<Button>().onClick.AddListener(no);
    }

    public void ShowPopUpDialogNoCoins(string dialogTitleText, string dialogMessageText)
    {
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = dialogTitleText;
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/ContentHolder/TextBG/TextHeader").GetComponent<Text>().text = dialogMessageText;
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/ButtonsHolder/ButtonYes").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/ButtonsHolder/ButtonNo").GetComponent<Button>().onClick.RemoveAllListeners();
        ShowPopUpMenu(transform.Find("PopUps/PopUpDialogNoCoins").gameObject);
    }

    public void ShowPopUpDialogNoCoins(string dialogTitleText, string dialogMessageText, UnityAction yes, UnityAction no)
    {
        ShowPopUpDialogNoCoins(dialogTitleText, dialogMessageText);
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/ButtonYes").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/ButtonNo").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/ButtonYes").GetComponent<Button>().onClick.AddListener(yes);
        transform.Find("PopUps/PopUpDialogNoCoins/AnimationHolder/Body/ButtonNo").GetComponent<Button>().onClick.AddListener(no);
    }

    public void StartLoading()
	{
		transform.Find("LoadingMenu").gameObject.SetActive(true);
		transform.Find("LoadingMenu/AnimationHolder").GetComponent<Animator>().Play("Arriving");
	}

    public void ShowTransition()
    {
        transform.Find("Transition").gameObject.SetActive(true);
        transform.Find("Transition").GetComponent<Animator>().SetTrigger("Show");
    }

    //public void ShowReverseTransition()
    //{
    //    GameObject.Find("Canvas/Transition").GetComponent<Animator>().Play("TransitionReverse");
    //}

    public void LoadSceneWithComingTransition(string sceneName)
    {
        StartCoroutine(LoadSceneWithComingTransitionC(sceneName));
    }

    IEnumerator LoadSceneWithComingTransitionC(string sceneName)
    {
        ShowTransition();
        yield return new WaitForSeconds(1.3f);
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithComingTransitionAndInterstitial(string sceneName)
    {
        StartCoroutine(LoadSceneWithComingTransitionAndInterstitialC(sceneName));
    }

    

    IEnumerator LoadSceneWithComingTransitionAndInterstitialC(string sceneName)
    {
        ShowTransition();
        yield return new WaitForSeconds(1f);
        lastScene = SceneManager.GetActiveScene().name;
		//AdsManager.Instance.ShowInterstitial();
		//Implementation.Instance.ShowInterstitial();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

    //public void PlayTransitionStayOnScene()
    //{
        //ShowTransition();
        //Timer.Schedule(this, .8f, delegate
        //{
            //ShowReverseTransition();
        //});
    //}
    public void ShowVideoNotReadyPopUp()
    {
        ShowPopUpMessage("Video not ready.", "Video not ready. Please try again later.");
       // AdsManager.Instance.IsVideoRewardAvailable();
    }

   
   

   

    public void Play_ButtonClick()
    {
        SoundManager.Instance.Play_ButtonClick();
    }

  


    public void EscapeButton()
    {
        //EscapeButtonManager.Instance.Escape();
    }

    public void BlackTransition()
    {
        transform.Find("TransitionBlack").GetComponent<Animator>().SetTrigger("Show");
    }

    public void WatchVideoForCoins()
    {
/*
        ShowPopUpDialog("Watch video?", "Watch video ad to get 100 coins?", delegate
        {
            if (Advertisements.Instance.IsRewardVideoAvailable())
            {
                //AdsManager.videoRewarded.RemoveAllListeners();
                //AdsManager.videoRewarded.AddListener(delegate
                //{
                   
                //});
				Implementation.Instance.ShowRewardedVideo();
				GameBalance.Instance.Add(100);
				//AdsManager.Instance.ShowVideoReward();
				//AdsManager.Instance.IsVideoRewardAvailable();
			}
            else
                ShowPopUpMessage("Video not ready.", "Please try again later");
        },
        delegate
        {
            ClosePopUpMenu(transform.Find("PopUps/PopUpDialog").gameObject);
        });
        */
    }

    public void Play_CashRegisterCloseSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play_CashRegisterSound();
    }

    public void PlayButtonClick()
    {
        SoundManager.Instance.Play_ButtonClick();
    }

    public void PlayButtonClick2()
    {
        SoundManager.Instance.Play_ButtonClick2();
    }

    public void PlayButtonClick3()
    {
        SoundManager.Instance.Play_ButtonClick3();
    }

    public void OpenPPLink()
    {
    //    Application.OpenURL(AdsManager.Instance.privacyPolicyLink);
    }
}
