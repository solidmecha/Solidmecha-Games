using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;
    public float JumpForce;
    public float JumpINC;
    float JumpMaxWatch;
    public float JumpMax;

    public float jumpInputWindow;
    float jumpCounter;
    bool inJumpCount;
    Rigidbody2D RB;

 

    float groundDist;

    void OnCollisionEnter2D(Collision2D coll)
    {

        // coll.gameObject.GetComponent<Rigidbody2D>().velocity= speed * V; ;
        //transform.SetParent(coll.gameObject.transform);
    }
   /* void OnCollisionExit2D(Collision2D coll)
    {

        transform.SetParent(null);
        
    } */

    bool isGrounded()
    {
        RaycastHit2D hit0 = Physics2D.Raycast(transform.GetChild(1).position, Vector2.down, .02f);
        RaycastHit2D hit1 = Physics2D.Raycast(transform.GetChild(0).position, Vector2.down, .02f);

        if (hit0 || hit1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Use this for initialization
    void Start() {
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            RB.AddForce(Vector2.up * JumpForce);
            inJumpCount = true;
        }

        if (inJumpCount)
        {
            jumpCounter += Time.deltaTime;
        }

        if (inJumpCount && (jumpCounter >= jumpInputWindow || !Input.GetKey(KeyCode.Space)))
        {
            inJumpCount = false;
            jumpCounter = 0;
            JumpMaxWatch = 0;
        }
        else if (inJumpCount)
        {
            if (JumpMaxWatch+JumpINC < JumpMax)
            {
                RB.AddForce(Vector2.up * JumpINC);
                JumpMaxWatch += JumpINC;
            }
        }
        
    }
    void FixedUpdate() {

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Vector2.right * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(speed * Vector2.left * Time.deltaTime);
        }
    }
}
