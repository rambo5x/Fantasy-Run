using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    public bool canMove;
    static public bool _canMove;
    public float worldSpeed;
    static public float _worldSpeed;
   

    public int coinsCollected;

    private bool coinHitThisFrame;

    private bool gameStarted;

    //speeding up
    public float timeToIncreaseSpeed;
    private float increaseSpeedCounter;
    public float speedMultiplier;
    private float targetSpeedMultiplier;
    public float acceleration;
    private float accelerationStore;
    public float speedIncreaseAmount;
    private float worldSpeedStore;

    public GameObject tapMessage;
    public Text coinsText;
    public Text distanceText;
    public Text bestDistanceText;
    private float distanceCovered;
    private float bestDistanceCovered;

    public GameObject deathScreen;
    public Text deathScreenCoins;
    public Text deathScreenDistance;
    public Text deathScreenBestDistance;
    public float deathScreenDelay;

    public string mainMenuName;

    public GameObject notEnoughCoinsScreen;

    public PlayerController thePlayer;

    public GameObject pauseScreen;
    public GameObject pauseBtn;

    public GameObject[] models;

    public GameObject defaultChar;

    public AudioManager theAM;

    public bool isDefaultChar;

    public GameObject adRewardPanel;
    public GameObject adContinuePanel;
    public Text rewardText;
   // public Text continueText;

    public bool isGetMoreCoins;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("CoinsCollected"))
        { 
            coinsCollected = PlayerPrefs.GetInt("CoinsCollected");
        }

        if (PlayerPrefs.HasKey("DistanceCovered"))
        {
            bestDistanceCovered = PlayerPrefs.GetFloat("DistanceCovered");
        }

        increaseSpeedCounter = timeToIncreaseSpeed;

        targetSpeedMultiplier = speedMultiplier;
        worldSpeedStore = worldSpeed;
        accelerationStore = acceleration;

        coinsText.text = "Coins: " + coinsCollected;
        bestDistanceText.text = "Best: " + Mathf.Floor(bestDistanceCovered) + "m";
        distanceText.text = distanceCovered + "m";

        if (PlayerPrefs.HasKey("SelectedChar"))
        {
            defaultChar.SetActive(false);

            //load correct model for character
            for (int i = 0; i < models.Length; i++)
            {
                if (models[i].name == PlayerPrefs.GetString("SelectedChar"))
                {
                    GameObject clone = Instantiate(models[i], thePlayer.modelHolder.position, thePlayer.modelHolder.rotation);
                    clone.transform.parent = thePlayer.modelHolder;
                    Destroy(clone.GetComponent<Rigidbody>());

                    // defaultChar.SetActive(false);
                }
            }
        }
        else
        {
            defaultChar.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _canMove = canMove;
        _worldSpeed = worldSpeed;


       

        if(!gameStarted && Input.GetMouseButtonDown(0))
        {
            canMove = true;
            _canMove = true;

            gameStarted = true;

            tapMessage.SetActive(false);
        }

        //increase speed over time
        if (canMove)
        {
            increaseSpeedCounter -= Time.deltaTime;
            if(increaseSpeedCounter <= 0)
            {
                increaseSpeedCounter = timeToIncreaseSpeed;

                // worldSpeed = worldSpeed * speedMultiplier;

                targetSpeedMultiplier = targetSpeedMultiplier * speedIncreaseAmount;

                timeToIncreaseSpeed = timeToIncreaseSpeed * .97f;
            }
            acceleration = accelerationStore * speedMultiplier;

            speedMultiplier = Mathf.MoveTowards(speedMultiplier, targetSpeedMultiplier, acceleration * Time.deltaTime);
            worldSpeed = worldSpeedStore * speedMultiplier;

            //updating UI
            distanceCovered += Time.deltaTime * worldSpeed;
            distanceText.text = Mathf.Floor(distanceCovered) + "m";

        }
        
        coinHitThisFrame = false;
    }

    public void HitHazard()
    {
        canMove = false;
        _canMove = false;

        PlayerPrefs.SetInt("CoinsCollected", coinsCollected);

        //deathScreen.SetActive(true);
        deathScreenCoins.text = coinsCollected + " coins!";
        deathScreenDistance.text = Mathf.Floor(distanceCovered) + "m!";

        if (distanceCovered <= bestDistanceCovered)
        {
            bestDistanceCovered = PlayerPrefs.GetFloat("DistanceCovered"); ;
        }
        else
        {
            bestDistanceCovered = distanceCovered;
            PlayerPrefs.SetFloat("DistanceCovered", bestDistanceCovered);
        }

        deathScreenBestDistance.text = Mathf.Floor(bestDistanceCovered) + "m!";

        StartCoroutine("ShowDeathScreen");
    }

    public IEnumerator ShowDeathScreen()
    {
        theAM.StopMusic();

        yield return new WaitForSeconds(deathScreenDelay);
        deathScreen.SetActive(true);
        pauseBtn.SetActive(false);
        theAM.gameOverMusic.Play();
    }

    public void AddCoin()
    {
        if (!coinHitThisFrame)
        {
            coinsCollected++;
            coinHitThisFrame = true;

            coinsText.text = "Coins: " + coinsCollected;
        }
    }

    public void ContinueGame()
    {
        isGetMoreCoins = false;
        ShowRewardedVideo();
        pauseBtn.SetActive(true);
     /*   if(coinsCollected >= 100)
        {
            coinsCollected -= 100;

            canMove = true;
            _canMove = true;
            deathScreen.SetActive(false);

            thePlayer.ResetPlayer();

            theAM.StopMusic();
            theAM.gameMusic.Play();
        }
        else
        {
            notEnoughCoinsScreen.SetActive(true);
        }
    */
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMen()
    {
        SceneManager.LoadScene(mainMenuName);

        Time.timeScale = 1f;
    }

    public void CloseNotEnoughCoins()
    {
        notEnoughCoinsScreen.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);

        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1f)
        {
            pauseScreen.SetActive(true);          
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void GetCoins()
    {
        isGetMoreCoins = true;
        ShowRewardedVideo();
    }

    public void ContinueGameBtn()
    {
        adContinuePanel.SetActive(false);

        canMove = true;
        _canMove = true;
        deathScreen.SetActive(false);

        thePlayer.ResetPlayer();

        theAM.StopMusic();
        theAM.gameMusic.Play();
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
            if (isGetMoreCoins)
            {
                Debug.Log("Video completed - Offer a reward to the player");
                //Reward the player here
                coinsCollected += 100;
                PlayerPrefs.SetInt("CoinsCollected", coinsCollected);
                rewardText.text = "Thanks for watching. You receive 100 coins!";
            }
            else
            {
                Debug.Log("Video completed - Offer a reward to the player");
                //Reward the player here
                deathScreen.SetActive(false);
                adContinuePanel.SetActive(true);
            }
        }
        else if (result == ShowResult.Skipped)
        {
            if (isGetMoreCoins)
            {
                Debug.LogWarning("Video was skipped - Do NOT reward the player");
                rewardText.text = "You didn't watch the ad. Watch another to earn more coins.";
            }
            else
            {
                Debug.LogWarning("Video was skipped - Do NOT reward the player");
                rewardText.text = "You didn't watch the ad. Watch another to continue game.";
            }
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to load");
            rewardText.text = "Unable to show ad. Please try again.";
        }

        if (isGetMoreCoins)
        {
            adRewardPanel.SetActive(true);
        }
    }

    public void CloseAdPanel()
    {
        adRewardPanel.SetActive(false);
    }
}
