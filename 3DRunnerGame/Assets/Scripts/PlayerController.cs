using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager theGM;

    public Rigidbody theRB;

    public float jumpForce;

    public Transform modelHolder;
    public LayerMask whatIsGround;
    public bool onGround;

    public Animator anim;

    private Vector3 startPos;
    private Quaternion startRot;

    public float invincibleTime;
    private float invincibleTimer;

    public AudioManager theAM;

    public GameObject coinEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (theGM.canMove)
        {
            onGround = Physics.OverlapSphere(modelHolder.position, 0.2f, whatIsGround).Length > 0;//test ground collision

            if (onGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Make the player jump
                    theRB.velocity = new Vector3(0f, jumpForce, 0f);

                    theAM.sfxJJump.Play();
                }
            }
        }

        //control invincibility
        if(invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }

        //control animations
        anim.SetBool("walking", theGM.canMove);
        anim.SetBool("onGround", onGround);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hazards")
        {
            if (invincibleTimer <= 0)
            {

                Debug.Log("Hit");
                theGM.HitHazard();

                // theRB.isKinematic = false;

                theRB.constraints = RigidbodyConstraints.None;

                theRB.velocity = new Vector3(Random.Range(GameManager._worldSpeed / 2f, -GameManager._worldSpeed / 2f), 2.5f, -GameManager._worldSpeed / 2f);

                theAM.sfxHit.Play();
            }
        }

        if(other.tag == "Coin")
        {
            theGM.AddCoin();

            Instantiate(coinEffect, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);

            theAM.sfxCoin.Stop();
            theAM.sfxCoin.Play();
        }
    }

    public void ResetPlayer()
    {
        theRB.constraints = RigidbodyConstraints.FreezeRotation;
        transform.rotation = startRot;
        transform.position = startPos;

        invincibleTimer = invincibleTime;
    }
}
