using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneration : MonoBehaviour
{
    //public GameObject pathPiece;

    public GameObject[] pathPieces;

    public Transform thresholdPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < thresholdPoint.position.z)
        {
            //copy the pathPiece & move forward
           /* Instantiate(pathPiece, transform.position, transform.rotation);
            transform.position += new Vector3(0f, 0f, 2f);*/

            //Random Path Generation
            int selectPiece = Random.Range(0, pathPieces.Length);
            Instantiate(pathPieces[selectPiece], transform.position, transform.rotation);
            transform.position += new Vector3(0f, 0f, 3.2f);
        }
    }
}
