using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GroceriesAndRestaurantManager : MonoBehaviour
{
    public Data data;

    GameObject canvas;

    FoodMenu menuOpened;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");

        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("HomeScene");
        });
    }

    void Start()
    {
       /* if (AdsManager.Instance != null)
            AdsManager.Instance.IsVideoRewardAvailable();*/
        QualityManager.qualities.Clear();
        EndScreenManager.failed = false;
        StartTutorial();
    }

    void StartTutorial()
    {
        if(!PlayerPrefs.HasKey("TutorialDone")&&!GlobalVariables.tutorialDone)
        {
            Tut1();
        }
    }

    void Tut1()
    {
        canvas.transform.Find("BuildingsScreen/Tutorial/1/ClaimButton").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("BuildingsScreen/Tutorial/1/ClaimButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            GameBalance.Instance.Add(200);
            Tut2();
        });

        canvas.transform.Find("BuildingsScreen/Tutorial").GetComponent<Animator>().Play("Tut1");
    }

    void Tut2()
    {
        canvas.transform.Find("BuildingsScreen/Tutorial/2/GroceriesShop").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("BuildingsScreen/Tutorial/2/GroceriesShop").GetComponent<Button>().onClick.AddListener(delegate
        {
            OpenGroceriesOrAppliancesShop();
            Tut4();
        });

        canvas.transform.Find("BuildingsScreen/Tutorial").GetComponent<Animator>().Play("Tut2");
    }

    void Tut3()
    {

        PlayerPrefs.SetString("TutorialDone", "AA");
        GlobalVariables.tutorialDone = true;
        PlayerPrefs.Save();
        PlayerPrefs.Save();
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveListener(delegate
        {
            Tut3();
        });
        canvas.transform.Find("BuildingsScreen/Tutorial").GetComponent<Animator>().Play("Tut3");
    }

    void Tut4()
    {
        PlayerPrefs.SetString("TutorialDone", "AA");
        PlayerPrefs.Save();

        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            Tut3();
        });
        canvas.transform.Find("BuildingsScreen/Tutorial/4/ClaimButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            canvas.transform.Find("BuildingsScreen/Tutorial").GetComponent<Animator>().Play("New State");
        });
        canvas.transform.Find("BuildingsScreen/Tutorial").GetComponent<Animator>().Play("Tut4");
    }

    public void Tut4Remove()
    {
        canvas.transform.Find("BuildingsScreen/Tutorial").GetComponent<Animator>().Play("New State");
    }

    void LoadMenues()
    {
        GameObject newDish;
        int index = 0;
        Transform menu,content;
        
        if(canvas.transform.Find("MenuPicker/MenusHolder/Content").childCount<data.AllMenus.Count)
        {
            OpenChooseMenuScreen();
            return;
        }

        foreach(FoodMenu m in data.AllMenus)
        {
            menu = canvas.transform.Find("MenuPicker/MenusHolder/Content").GetChild(index).Find("MenuUnlocked/MenuHolder/Scroll View/Viewport/Content");

            for(int i=menu.childCount-1;i>0;i--)
            {
                GameObject.Destroy(menu.GetChild(i).gameObject);
            }

            foreach (Dish d in m.dishes)
            {
                newDish = GameObject.Instantiate(menu.GetChild(0).gameObject);
                newDish.transform.SetParent(menu, false);
                newDish.transform.Find("Icon").GetComponent<Image>().sprite = d.sprite;
                newDish.transform.Find("Name").GetComponent<Text>().text = d.name;
                foreach (Transform t in newDish.transform.Find("StarsHolder"))
                    t.gameObject.SetActive(false);
                for (int i = 0; i < d.Stars; i++)
                    newDish.transform.Find("StarsHolder").GetChild(i).gameObject.SetActive(true);

                if (d.HasIngredients())
                {
                    newDish.transform.Find("IconBg/NotEnoughIngredients").gameObject.SetActive(false);
                    newDish.transform.Find("CheckHolder/Yes").gameObject.SetActive(true);
                    newDish.transform.Find("CheckHolder/No").gameObject.SetActive(false);
                }
                else
                {
                    newDish.transform.Find("IconBg/NotEnoughIngredients").gameObject.SetActive(true);
                    newDish.transform.Find("CheckHolder/Yes").gameObject.SetActive(false);
                    newDish.transform.Find("CheckHolder/No").gameObject.SetActive(true);
                }
                newDish.transform.Find("MaxReward").GetComponent<Text>().text = d.earnings3Star.ToString();

                newDish.GetComponent<Button>().onClick.RemoveAllListeners();
                newDish.GetComponent<Button>().onClick.AddListener(delegate
                {
                    OpenDishInformationScreen(d);
                });
            }

            GameObject.Destroy(menu.GetChild(0).gameObject);
            index++;
        }
    }

    public void OpenChooseMenuScreen()
    {
        if (canvas == null)
            canvas = GameObject.Find("Canvas");
        canvas.transform.Find("BuildingsScreen/Tutorial").gameObject.SetActive(false);
        Timer.Schedule(this, 0.5f, delegate
        {
            canvas.transform.Find("DishInformationScreen").gameObject.SetActive(false);
        });
        canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(true);
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            canvas.transform.Find("MenuPicker").gameObject.SetActive(false);
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("HomeScene");
            });
        });

        Transform content=canvas.transform.Find("MenuPicker/MenusHolder/Content");
        GameObject newMenu;

        for(int i=content.transform.childCount-1;i>0;i--)
        {
            GameObject.Destroy(content.transform.GetChild(i).gameObject);
        }
        PlayerPrefs.SetString(data.AllMenus[0].name, "y");
        PlayerPrefs.Save();
        foreach(FoodMenu m in data.AllMenus)
        {
            newMenu = GameObject.Instantiate(content.GetChild(0).gameObject);
            newMenu.transform.SetParent(content, false);
            Vector3 ancho = newMenu.GetComponent<RectTransform>().anchoredPosition3D;
            ancho = content.transform.parent.Find("RightOUT").GetComponent<RectTransform>().anchoredPosition3D;
            ancho.z = 0f;
            newMenu.GetComponent<RectTransform>().anchoredPosition3D = ancho;
            if (!m.Locked())
            {
                if (PlayerPrefs.HasKey(m.name))
                    newMenu.transform.Find("MenuLocked").gameObject.SetActive(false);
                else
                    newMenu.transform.Find("MenuLocked").gameObject.SetActive(true);
                newMenu.transform.Find("MenuUnlocked/MenuHolder/TitleHolder/Title").GetComponent<Text>().text = m.name;
                //newMenu.transform.Find("Icon").GetComponent<Image>().sprite = m.icon;
                //newMenu.GetComponent<Button>().onClick.RemoveAllListeners();
                //newMenu.transform.Find("Locked").gameObject.SetActive(false);
                //newMenu.GetComponent<Button>().onClick.AddListener(delegate
                //{
                //    OpenMenu(m);
                //});
                if (!PlayerPrefs.HasKey(m.name))
                {
                    newMenu.transform.Find("MenuLocked").gameObject.SetActive(true);
                    newMenu.transform.Find("MenuLocked/Icon").GetComponent<Image>().sprite = m.icon;
                    newMenu.transform.Find("MenuLocked/Text").GetComponent<Text>().text = "Earn " + m.starsRequiredToUnlockNextMenu.ToString() + " stars in " + m.menuToEarnStarsIn.name + " to unlock this menu";
                }
            }
            else
            {
                newMenu.transform.Find("MenuLocked").gameObject.SetActive(true);
                newMenu.transform.Find("MenuLocked/Icon").GetComponent<Image>().sprite = m.icon;
                newMenu.transform.Find("MenuLocked/Text").GetComponent<Text>().text = "Earn " + m.starsRequiredToUnlockNextMenu.ToString() + " stars in " + m.menuToEarnStarsIn.name + " to unlock this menu";
                //newMenu.transform.Find("Locked").gameObject.SetActive(true);
                //newMenu.transform.Find("Locked/Text").GetComponent<Text>().text = "Earn " + m.starsRequiredToUnlockNextMenu.ToString() + " stars in " + m.menuToEarnStarsIn.name + " to unlock this menu";
            }
        }
        GameObject.Destroy(content.GetChild(0).gameObject);

        Timer.Schedule(this, 0.01f, delegate
        {
            LoadMenues();
            canvas.transform.Find("MenuPicker").gameObject.SetActive(true);
        });
    }

    public void OpenChooseMenuScreenFocusLastUnlocked()
    {
        int index = -1;
        foreach(FoodMenu m in data.AllMenus)
        {
            if (!m.Locked())
            {
                index++;
            }
        }

        OpenChooseMenuScreen();
        Timer.Schedule(this, .2f, delegate
        {
            Vector3 posLeft = canvas.transform.Find("MenuPicker/MenusHolder/LeftOUT").GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 posRight = canvas.transform.Find("MenuPicker/MenusHolder/RightOUT").GetComponent<RectTransform>().anchoredPosition3D;
            for (int i = 0; i < index; i++)
            {
                Timer.Schedule(this, 0.1f * i, delegate
                {
                    canvas.transform.Find("MenuPicker/MenusHolder").GetComponent<ItemSnapperOnArrow>().GoRight();
                });
            }
            Timer.Schedule(this,.5f, delegate
            {
                if (!PlayerPrefs.HasKey(data.AllMenus[index].name)&&index!=0)
                {
                    canvas.transform.Find("MenuPicker/MenusHolder/Content").GetChild(index).GetComponent<Animator>().Play("Unlock");
                    PlayerPrefs.SetString(data.AllMenus[index].name, "y");
                    PlayerPrefs.Save();
                }
            });
        });
    }


    public void OpenMenu(FoodMenu menu)
    {
        Debug.Log(menu.name);
        menuOpened = menu;
        canvas.transform.Find("Menu/MenuHolder/TitleHolder/Title").GetComponent<Image>().sprite = menu.titleSprite;

        Transform content = canvas.transform.Find("Menu/MenuHolder/Scroll View/Viewport/Content");
        GameObject newDish;

        for (int i = content.childCount - 1; i > 0; i--)
            GameObject.Destroy(content.GetChild(i).gameObject);

        foreach(Dish d in menu.dishes)
        {
            newDish = GameObject.Instantiate(content.GetChild(0).gameObject);
            newDish.transform.SetParent(content, false);
            newDish.transform.Find("Icon").GetComponent<Image>().sprite = d.sprite;
            newDish.transform.Find("Text").GetComponent<Text>().text = d.name;
            foreach (Transform t in newDish.transform.Find("StarsHolder"))
                    t.gameObject.SetActive(false);
            for (int i = 0; i < d.Stars; i++)
                newDish.transform.Find("StarsHolder").GetChild(i).gameObject.SetActive(true);

            if(d.HasIngredients())
            {
                newDish.transform.Find("CheckHolder/Yes").gameObject.SetActive(true);
                newDish.transform.Find("CheckHolder/No").gameObject.SetActive(false);
            }
            else
            {
                newDish.transform.Find("CheckHolder/Yes").gameObject.SetActive(false);
                newDish.transform.Find("CheckHolder/No").gameObject.SetActive(true);
            }

            newDish.GetComponent<Button>().onClick.RemoveAllListeners();
            newDish.GetComponent<Button>().onClick.AddListener(delegate
            {
                OpenDishInformationScreen(d);
            });
        }

        GameObject.Destroy(content.GetChild(0).gameObject);

        canvas.transform.Find("Menu").gameObject.SetActive(true);
        canvas.transform.Find("DishInformationScreen").gameObject.SetActive(false);

        //back dugme
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            canvas.transform.Find("Menu").gameObject.SetActive(false);
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                canvas.transform.Find("MenuPicker").gameObject.SetActive(false);
            });
        });
    }

    public void OpenDishInformationScreen(Dish dish)
    {
        if (canvas == null)
            canvas = GameObject.Find("Canvas");
        Transform dishInfoHolder = canvas.transform.Find("DishInformationScreen");

        dishInfoHolder.Find("DishHolder/DishIconHolder/Icon").GetComponent<Image>().sprite = dish.sprite;
        dishInfoHolder.Find("DishHolder/TitleHolder/Title").GetComponent<Text>().text = dish.name;
        dishInfoHolder.Find("DishHolder/CostText").GetComponent<Text>().text = dish.Cost().ToString();
        dishInfoHolder.Find("DishHolder/ProfitText").GetComponent<Text>().text =(dish.earnings3Star-dish.Cost()).ToString();

        for (int i = 0; i < 3; i++)
        {
            if (dish.Stars > i)
                dishInfoHolder.Find("DishHolder/RatingHolder").GetChild(i).gameObject.SetActive(true);
            else
                dishInfoHolder.Find("DishHolder/RatingHolder").GetChild(i).gameObject.SetActive(false);
        }

        int counter = 0;
        Transform ingredientHolder;


        if (dish.appliances.Count > 0)
        {
            ingredientHolder = dishInfoHolder.Find("DishHolder/Content").GetChild(counter++);
            ingredientHolder.Find("Name").GetComponent<Text>().text = dish.appliances[0].name;
            ingredientHolder.Find("Icon").GetComponent<Image>().sprite = dish.appliances[0].sprite;
            ingredientHolder.Find("StockAmount").gameObject.SetActive(false);
            ingredientHolder.Find("NeededAmount").gameObject.SetActive(false);
            ingredientHolder.Find("BuyHolder").gameObject.SetActive(false);
            ingredientHolder.Find("HaveEnough").gameObject.SetActive(true);
            if (dish.appliances[0].Locked)
            {
                ingredientHolder.Find("BuyHolder").gameObject.SetActive(true);
                ingredientHolder.Find("HaveEnough").gameObject.SetActive(false);
                ingredientHolder.Find("BuyHolder/CostHolder/Text").GetComponent<Text>().text = dish.appliances[0].cost.ToString();
                Button button = ingredientHolder.Find("BuyHolder/CostHolder").GetComponent<Button>();
                ingredientHolder.Find("BuyHolder/CostHolder").GetComponent<Button>().onClick.RemoveAllListeners();
                ingredientHolder.Find("BuyHolder/CostHolder").GetComponent<Button>().onClick.AddListener(delegate
                {
                    if (GameBalance.Instance.Spend(dish.appliances[0].cost))
                    {
                        if (SoundManager.Instance != null)
                            SoundManager.Instance.Play_BuySound();
                        dish.appliances[0].Locked = false;
                        button.transform.parent.parent.Find("HaveEnough").gameObject.SetActive(true);
                        button.transform.parent.gameObject.SetActive(false);

                        if (dish.HasIngredients())
                        {
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = true;
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().onClick.RemoveAllListeners();
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().onClick.AddListener(delegate
                            {
                                Debug.Log("KRENI DA PRAVIS " + dish.name);
                                GlobalVariables.creationPhaseIndex = 0;
                                GlobalVariables.dishInCreation = dish;
                                dish.RemoveIngredientsNeededForDish();
                                dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = false;
                                QualityManager.qualities.Clear();
                                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(GlobalVariables.dishInCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                            //SceneManager.LoadScene(GlobalVariables.dishInCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                        });

                        }
                        else
                        {
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = false;
                        }
                    }
                    else
                    {
                        canvas.GetComponent<MenuManager>().WatchVideoForCoins();
                    }
                });
            }
        }
        else
        {
            if(dish.appliances.Count==0)
            {
                ingredientHolder = dishInfoHolder.Find("DishHolder/Content").GetChild(counter);
                ingredientHolder.Find("StockAmount").gameObject.SetActive(true);
                ingredientHolder.Find("NeededAmount").gameObject.SetActive(true);
            }

        }

       
        foreach(DishIngredient ing in dish.ingredients)
        {
            ingredientHolder = dishInfoHolder.Find("DishHolder/Content").GetChild(counter++);
            ingredientHolder.gameObject.SetActive(true);
            ingredientHolder.Find("Icon").GetComponent<Image>().sprite = ing.ingredient.sprite;
            ingredientHolder.Find("StockAmount").gameObject.SetActive(true);
            ingredientHolder.Find("StockAmount").GetComponent<Text>().text ="In stock: "+ ing.ingredient.Stock.ToString();
            if (ing.ingredient.Stock > 0)
                ingredientHolder.Find("StockAmount").GetComponent<Text>().color = new Color32(0x83, 0x8f, 0xc7, 0xff);
            else
                ingredientHolder.Find("StockAmount").GetComponent<Text>().color = new Color32(0xff, 0x00, 0x00, 0xff);
            ingredientHolder.Find("NeededAmount/NeededText").GetComponent<Text>().text = "x" + ing.amount.ToString();
            ingredientHolder.Find("Name").GetComponent<Text>().text = ing.ingredient.name;

            if (ing.amount > ing.ingredient.Stock)
            {
                ingredientHolder.Find("HaveEnough").gameObject.SetActive(false);
                ingredientHolder.Find("BuyHolder/CostHolder/Text").GetComponent<Text>().text = (ing.ingredient.cost).ToString();
                ingredientHolder.Find("BuyHolder").gameObject.SetActive(true);
                ingredientHolder.Find("BuyHolder/BuyAmount").GetComponent<Text>().text = ing.ingredient.buyingAmount.ToString();
                ingredientHolder.Find("BuyHolder/CostHolder").GetComponent<Button>().onClick.RemoveAllListeners();
                Text inStock = ingredientHolder.Find("StockAmount").GetComponent<Text>();
                Button button = ingredientHolder.Find("BuyHolder/CostHolder").GetComponent<Button>();

                ingredientHolder.Find("BuyHolder/CostHolder").GetComponent<Button>().onClick.AddListener(delegate
                {
                    if (GameBalance.Instance.Spend(ing.ingredient.cost))
                    {
                        if (SoundManager.Instance != null)
                            SoundManager.Instance.Play_BuySound();
                        ing.ingredient.Stock += ing.ingredient.buyingAmount;
                        inStock.text = "In stock: " + ing.ingredient.Stock.ToString();
                        if (ing.ingredient.Stock >= ing.amount)
                        {
                            button.transform.parent.parent.Find("HaveEnough").gameObject.SetActive(true);
                            button.transform.parent.gameObject.SetActive(false);
                        }

                        if (ing.ingredient.Stock > 0)
                            button.transform.parent.parent.Find("StockAmount").GetComponent<Text>().color = new Color32(0x83, 0x8f, 0xc7, 0xff);
                        else
                            button.transform.parent.parent.Find("StockAmount").GetComponent<Text>().color = new Color32(0xff, 0x00, 0x00, 0xff);

                        if (dish.HasIngredients())
                        {
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = true;
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().onClick.RemoveAllListeners();
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().onClick.AddListener(delegate
                            {
                                Debug.Log("KRENI DA PRAVIS " + dish.name);
                                GlobalVariables.creationPhaseIndex = 0;
                                GlobalVariables.dishInCreation = dish;
                                dish.RemoveIngredientsNeededForDish();
                                dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = false;
                                QualityManager.qualities.Clear();
                                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(GlobalVariables.dishInCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                                //SceneManager.LoadScene(GlobalVariables.dishInCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                            });

                        }
                        else
                        {
                            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = false;
                        }

                    }
                    else
                    {
                        canvas.GetComponent<MenuManager>().WatchVideoForCoins();
                    }
                });
            }
            else
            {
                ingredientHolder.Find("BuyHolder").gameObject.SetActive(false);
                ingredientHolder.Find("HaveEnough").gameObject.SetActive(true);
            }
        }
        while (counter < dishInfoHolder.Find("DishHolder/Content").childCount)
            (dishInfoHolder.Find("DishHolder/Content").GetChild(counter++).gameObject).SetActive(false);

        if (dish.HasIngredients())
        {
            dishInfoHolder.Find("FreeGroceriesVideo").gameObject.SetActive(false);
            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = true;
            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().onClick.RemoveAllListeners();
            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().onClick.AddListener(delegate
            {
                Debug.Log("KRENI DA PRAVIS " + dish.name);
                GlobalVariables.creationPhaseIndex = 0;
                GlobalVariables.dishInCreation = dish;
                dish.RemoveIngredientsNeededForDish();
                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(GlobalVariables.dishInCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                QualityManager.qualities.Clear();
                dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = false;
            });
        }
        else
        {
            dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = false;
            

           /* dishInfoHolder.Find("FreeGroceriesVideo").gameObject.SetActive(true);
            dishInfoHolder.Find("FreeGroceriesVideo").GetComponent<Button>().onClick.RemoveAllListeners();
            dishInfoHolder.Find("FreeGroceriesVideo").GetComponent<Button>().onClick.AddListener(delegate
            {
                if (Advertisements.Instance.IsRewardVideoAvailable())
                {
                    canvas.GetComponent<MenuManager>().ShowPopUpDialog("Watch video ad?", "Watch video to get free groceries?", delegate
                    {
                        //AdsManager.videoRewarded.RemoveAllListeners();
                        //AdsManager.videoRewarded.AddListener(delegate
                        //{
                        //    //GlobalVariables.creationPhaseIndex = 0;
                        //    //GlobalVariables.dishInCreation = dish;
                        //    //canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(GlobalVariables.dishInCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                        //    //QualityManager.qualities.Clear();
                        //    //dishInfoHolder.Find("MakeBtn").GetComponent<Button>().interactable = false;
                            

                        //});
                        dish.AddIngredientsNeededForDish();
                        OpenDishInformationScreen(dish);

                        //AdsManager.Instance.ShowVideoReward();
                        //AdsManager.Instance.IsVideoRewardAvailable();
                    },
                    delegate
                    {
                        canvas.GetComponent<MenuManager>().ClosePopUpMenu(canvas.transform.Find("PopUps/PopUpDialog").gameObject);
                    });
                }
                else
                {
                    canvas.GetComponent<MenuManager>().ShowPopUpMessage("Video not ready", "Please try again later.");
                }
            });*/
            if (dish.HasFoodIngredients())
                dishInfoHolder.Find("FreeGroceriesVideo").gameObject.SetActive(false);
        }
        
        canvas.transform.Find("DishInformationScreen").gameObject.SetActive(true);

        //back dugme
        canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(true);
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate 
        {
            //OpenMenu(menuOpened);
            LoadMenues();
            Timer.Schedule(this, .2f, delegate
              {
                  canvas.transform.Find("DishInformationScreen").gameObject.SetActive(false);
              });
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                canvas.transform.Find("MenuPicker").gameObject.SetActive(false);
                canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
                canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
                canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
                {
                    canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("HomeScene");
                });
            });
        });
    }

    public void OpenGroceriesOrAppliancesShop()
    {
      
        canvas.transform.Find("GroceriesAndAppliancesShopScreen").gameObject.SetActive(true);
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances").gameObject.SetActive(true);

        canvas.transform.Find("MenuPicker").gameObject.SetActive(false);
        canvas.transform.Find("DishInformationScreen").gameObject.SetActive(false);

        canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(true);
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances").gameObject.SetActive(false);
            canvas.transform.Find("GroceriesAndAppliancesShopScreen").gameObject.SetActive(false);
        });

        OpenGroceriesShop();
    }

    public void OpenGroceriesShop()
    {
        if(EventSystem.current.currentSelectedGameObject!=null&&EventSystem.current.currentSelectedGameObject.GetComponent<Button>()!=null)
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            obj.GetComponent<Button>().enabled = false;
            Timer.Schedule(this, 0.2f, delegate
            {
                obj.GetComponent<Button>().enabled = true;
            });
        }


        Transform contentHolder = canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/GroceriesHolder/Groceries/ScrollHolder/Scroll View/Viewport/Content");
        GameObject newGroceryItem;
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/AppliancesHolder").GetComponent<Canvas>().sortingOrder = 3;
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/GroceriesHolder").GetComponent<Canvas>().sortingOrder = 4;

        for (int i = contentHolder.childCount-1; i > 0; i--)
            Destroy(contentHolder.GetChild(i).gameObject);
        

        foreach (Ingredient ing in data.AllIngredients)
        {
            newGroceryItem = GameObject.Instantiate(contentHolder.GetChild(0).gameObject);
            newGroceryItem.transform.SetParent(contentHolder,false);
            newGroceryItem.transform.Find("Icon").GetComponent<Image>().sprite = ing.sprite;
            newGroceryItem.transform.Find("Price").GetComponent<Text>().text = ing.cost.ToString();
            newGroceryItem.transform.Find("BuyingAmount").GetComponent<Text>().text = ing.buyingAmount.ToString();
            newGroceryItem.transform.Find("InStock").GetComponent<Text>().text = "In stock: " + ing.Stock.ToString();
            newGroceryItem.transform.Find("Name").GetComponent<Text>().text = ing.name;
            newGroceryItem.transform.Find("BuyBtn").GetComponent<Button>().onClick.RemoveAllListeners();
            Text inStock = newGroceryItem.transform.Find("InStock").GetComponent<Text>();
            if (ing.Stock > 0)
                inStock.color = new Color32(0x1f, 0x45, 0xa8,0xff);
            else
                inStock.color = new Color32(0xff, 0x00, 0x00, 0xff);

            newGroceryItem.transform.Find("BuyBtn").GetComponent<Button>().onClick.AddListener(delegate
            {
                if(GameBalance.Instance.Spend(ing.cost))
                {
                    if (SoundManager.Instance != null)
                        SoundManager.Instance.Play_BuySound();
                    ing.Stock += ing.buyingAmount;
                    inStock.text = "In stock: " + ing.Stock.ToString();
                    if (SoundManager.Instance != null)
                        SoundManager.Instance.Play_CashRegisterSound();

                    if (ing.Stock > 0)
                        inStock.color = new Color32(0x83, 0x8f, 0xc7, 0xff);
                    else
                        inStock.color = new Color32(0xff, 0x00, 0x00, 0xff);
                }
                else
                {
                    canvas.GetComponent<MenuManager>().WatchVideoForCoins();
                }
            });
        }
        GameObject.Destroy(contentHolder.GetChild(0).gameObject);

        Timer.Schedule(this, 0.05f, delegate
        {
            canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/GroceriesHolder/TitleHolderSelected").gameObject.SetActive(true);
            canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/AppliancesHolder/TitleHolderSelected").gameObject.SetActive(false);
            canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/AppliancesHolder/NotSelected").gameObject.SetActive(true);
            canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/GroceriesHolder/NotSelected").gameObject.SetActive(false);
        });

        canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(true);
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            //canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances").gameObject.SetActive(false);
            //canvas.transform.Find("GroceriesAndAppliancesShopScreen/Groceries").gameObject.SetActive(false);
            //canvas.transform.Find("GroceriesAndAppliancesShopScreen").gameObject.SetActive(false);
            canvas.transform.Find("GroceriesAndAppliancesShopScreen").gameObject.SetActive(false);
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
            canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("HomeScene");
            });

        });
    }


    public void OpenAppliancesShop()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            obj.GetComponent<Button>().enabled = false;
            Timer.Schedule(this, 0.2f, delegate
            {
                obj.GetComponent<Button>().enabled = true;
            });
        }
        canvas.transform.Find("GroceriesAndAppliancesShopScreen").gameObject.SetActive(true);
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/AppliancesHolder").GetComponent<Canvas>().sortingOrder = 4;
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/GroceriesHolder").GetComponent<Canvas>().sortingOrder = 3;

        Transform contentHolder = canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/AppliancesHolder/Appliances/ScrollHolder/Content");
        GameObject newApplianceItem;

        for (int i = contentHolder.childCount-1; i > 0; i--)
            Destroy(contentHolder.GetChild(i).gameObject);

        foreach(Appliance app in data.AllAppliances)
        {
            newApplianceItem = GameObject.Instantiate(contentHolder.GetChild(0).gameObject);
            newApplianceItem.transform.SetParent(contentHolder, false);
            newApplianceItem.transform.Find("Icon").GetComponent<Image>().sprite = app.sprite;
            newApplianceItem.transform.Find("Name").GetComponent<Text>().text = app.name;
            if (!app.Locked)
            {
                newApplianceItem.transform.Find("AlreadyHave").gameObject.SetActive(true);
                newApplianceItem.transform.Find("Price").gameObject.SetActive(false);
                newApplianceItem.transform.Find("BuyBtn").gameObject.SetActive(false);
            }
            else
            {
                newApplianceItem.transform.Find("AlreadyHave").gameObject.SetActive(false);
                newApplianceItem.transform.Find("BuyBtn").gameObject.SetActive(true);
                newApplianceItem.transform.Find("Price").gameObject.SetActive(true);
                newApplianceItem.transform.Find("Price").GetComponent<Text>().text = app.cost.ToString();
                newApplianceItem.transform.Find("Price").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject price = newApplianceItem.transform.Find("Price").gameObject;
                newApplianceItem.transform.Find("BuyBtn").GetComponent<Button>().onClick.AddListener(delegate
                {
                    if(GameBalance.Instance.Spend(app.cost))
                    {
                        if (SoundManager.Instance != null)
                            SoundManager.Instance.Play_BuySound();
                        app.Locked = false;
                        price.gameObject.SetActive(false);
                        price.transform.parent.Find("AlreadyHave").gameObject.SetActive(true);
                        price.transform.parent.Find("BuyBtn").gameObject.SetActive(false);
                        SoundManager.Instance.Play_CashRegisterSound();
                    }
                    else
                    {
                        canvas.GetComponent<MenuManager>().WatchVideoForCoins();
                    }
                });
            }
        }
        GameObject.Destroy(contentHolder.GetChild(0).gameObject);



        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/GroceriesHolder/NotSelected").gameObject.SetActive(true);
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/AppliancesHolder/NotSelected").gameObject.SetActive(false);
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/GroceriesHolder/TitleHolderSelected").gameObject.SetActive(false);
        canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances/AppliancesHolder/TitleHolderSelected").gameObject.SetActive(true);


        canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(true);
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            canvas.transform.Find("GroceriesAndAppliancesShopScreen").gameObject.SetActive(false);

            //canvas.transform.Find("GroceriesAndAppliancesShopScreen/Appliances").gameObject.SetActive(false);
            //canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances").gameObject.SetActive(true);
            //canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.RemoveAllListeners();
            //canvas.transform.Find("TopUI/BackButton").GetComponent<Button>().onClick.AddListener(delegate
            //{
            //    canvas.transform.Find("GroceriesAndAppliancesShopScreen/GroceriesOrAppliances").gameObject.SetActive(false);
            //    canvas.transform.Find("GroceriesAndAppliancesShopScreen").gameObject.SetActive(false);
            //});
        });
    }
}
