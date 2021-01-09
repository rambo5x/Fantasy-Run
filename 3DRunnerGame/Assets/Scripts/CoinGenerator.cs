using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public float timeBetweenCoins;
    private float coinGenCounter;

    public GameObject[] coinGroups;

    public Transform topPos;

    // Start is called before the first frame update
    void Start()
    {
        coinGenCounter = timeBetweenCoins;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager._canMove)
        {

            coinGenCounter -= Time.deltaTime;

            if (coinGenCounter <= 0)
            {
                bool goTop = Random.value > .5f; //pick a random number between 0 and 1

                int chosenCoins = Random.Range(0, coinGroups.Length);

                if (goTop)
                {
                    Instantiate(coinGroups[chosenCoins], topPos.position, transform.rotation);
                }
                else
                {
                    Instantiate(coinGroups[chosenCoins], transform.position, transform.rotation);
                }

                coinGenCounter = Random.Range(timeBetweenCoins * 0.75f, timeBetweenCoins * 1.25f);
            }
        }
    }
}
