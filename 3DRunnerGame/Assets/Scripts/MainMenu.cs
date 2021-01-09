using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;

    public GameObject mainScreen;
    public GameObject switchingScreen;

    public Transform theCamera;
    public Transform charSwitchHolder;

    private Vector3 camTargetPos;
    private Vector3 mainMenuCamPos;
    public float cameraSpeed;

    public GameObject[] theChars;
    public int currentCharacter;

    public GameObject switchPlayButton;
    public GameObject switchUnlockButton;
    public GameObject switchGetCoinsButton;

    public GameObject modelHolder;
    public GameObject[] models;
    public GameObject defaultChar;

    public int currentCoins;

    public Text coinsText;

    public bool isDefaultChar;

    public GameObject charLockedImage;

    public GameObject adRewardPanel;
    public Text rewardText;

    void Awake()
    {
      //  Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        camTargetPos = theCamera.position;
        mainMenuCamPos = theCamera.position;

        if (!PlayerPrefs.HasKey(theChars[0].name))
        {
            PlayerPrefs.SetInt(theChars[0].name, 1);
        }

        if (PlayerPrefs.HasKey("SelectedChar"))
        {
            defaultChar.SetActive(false);

            //load correct model for character
            for (int i = 0; i < models.Length; i++)
            {
                if (models[i].name == PlayerPrefs.GetString("SelectedChar"))
                {
                    //   defaultChar.SetActive(false);
                    GameObject clone = Instantiate(models[i], modelHolder.transform.position, modelHolder.transform.rotation);
                    clone.transform.parent = modelHolder.transform;
                    Destroy(clone.GetComponent<Rigidbody>());
                    // defaultChar.SetActive(false);
                }

            }

        }
        else
        {
            defaultChar.SetActive(true);
        }

        if (PlayerPrefs.HasKey("CoinsCollected"))
        {
            currentCoins = PlayerPrefs.GetInt("CoinsCollected");
        }
        else
        {
            PlayerPrefs.SetInt("CoinsCollected", 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        theCamera.position = Vector3.Lerp(theCamera.position, camTargetPos, cameraSpeed * Time.deltaTime);//smoother stop

        coinsText.text = "Coins: " + currentCoins;

#if UNITY_EDITOR

        /*  if (Input.GetKeyDown(KeyCode.L))
          {
              for (int i = 1; i < theChars.Length; i++)
              {
                  PlayerPrefs.SetInt(theChars[i].name, 0);
              }
          }

          UnlockedCheck();
          */

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
        }
#endif

        // theCamera.position = Vector3.MoveTowards(theCamera.position, camTargetPos, cameraSpeed * Time.deltaTime);sudden stop
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void ChooseChar()
    {
        mainScreen.SetActive(false);
        switchingScreen.SetActive(true);

        camTargetPos = theCamera.position + new Vector3(0f, charSwitchHolder.position.y, 0f);

        UnlockedCheck();
    }

    public void moveLeft()
    {
        if (currentCharacter > 0)
        {
            camTargetPos += new Vector3(2f, 0f, 0f);

            currentCharacter--;

            UnlockedCheck();
        }
    }

    public void moveRight()
    {
        if (currentCharacter < theChars.Length - 1)
        {

            camTargetPos -= new Vector3(2f, 0f, 0f);

            currentCharacter++;

            UnlockedCheck();

        }
    }

    public void UnlockedCheck()
    {
        if (PlayerPrefs.HasKey(theChars[currentCharacter].name))
        {
            if(PlayerPrefs.GetInt(theChars[currentCharacter].name) == 0)
            {
                switchPlayButton.SetActive(false);

                charLockedImage.SetActive(true);

                if(currentCoins < 400)
                {
                    switchGetCoinsButton.SetActive(true);
                    switchUnlockButton.SetActive(false);
                }
                else
                {
                    switchUnlockButton.SetActive(true);
                    switchGetCoinsButton.SetActive(false);
                }
            }
            else
            {
                switchPlayButton.SetActive(true);

                charLockedImage.SetActive(false);

                switchGetCoinsButton.SetActive(false);
                switchUnlockButton.SetActive(false);
            }

        }
        else
        {
            PlayerPrefs.SetInt(theChars[currentCharacter].name, 0);

            UnlockedCheck();
        }
    }

    public void UnlockChar()
    {
        currentCoins -= 400;

        PlayerPrefs.SetInt(theChars[currentCharacter].name, 1);
        PlayerPrefs.SetInt("CoinsCollected", currentCoins);

        UnlockedCheck();
    }

    public void SelectChar()
    {
        PlayerPrefs.SetString("SelectedChar", theChars[currentCharacter].name);

        PlayGame();
    }

    public void GetCoins()
    {
        ShowRewardedVideo();
    }

    void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            //Reward the player here
            currentCoins += 100;
            PlayerPrefs.SetInt("CoinsCollected", currentCoins);
            rewardText.text = "Thanks for watching. You receive 100 coins!";
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");
            rewardText.text = "You didn't watch the ad. Watch another to earn coins.";
        }
        else if(result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to load");
            rewardText.text = "Unable to show ad. Please try again.";
        }

        adRewardPanel.SetActive(true);
    }

    public void CloseAdPanel()
    {
        adRewardPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
