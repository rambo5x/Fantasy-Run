using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutBlack : MonoBehaviour
{

    public Image blackScreen;

    public float waitToFade;

    public float fadeSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitToFade > 0)
        {
            waitToFade -= Time.deltaTime;
        }
        else
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if(blackScreen.color.a == 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
