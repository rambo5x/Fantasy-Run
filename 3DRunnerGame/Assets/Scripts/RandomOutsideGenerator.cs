using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOutsideGenerator : MonoBehaviour
{
    public float timeBetweenObjects;
    private float objectGenCounter;

    public GameObject[] objects;

    public Transform minPoint;
    public Transform maxPoint;

    public GameManager theGM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (theGM.canMove)
        {

            objectGenCounter -= Time.deltaTime;

            if (objectGenCounter <= 0)
            {
                int chosenObject = Random.Range(0, objects.Length);

                //pick a random point for the object to appear

                Vector3 genPoint = new Vector3(Random.Range(minPoint.position.x, maxPoint.position.x), transform.position.y, transform.position.z);

                Instantiate(objects[chosenObject], genPoint, Quaternion.Euler(0f, Random.Range(-45, 45), 0f));

                objectGenCounter = Random.Range(timeBetweenObjects * 0.75f, timeBetweenObjects * 1.25f);

                objectGenCounter = objectGenCounter / theGM.speedMultiplier;
            }
        }
    }
}
