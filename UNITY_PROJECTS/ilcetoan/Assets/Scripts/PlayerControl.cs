using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;
    public GameObject Proj;
    public float JumpForce;
    public float JumpINC;
    float JumpMaxWatch;
    public float JumpMax;

    public float jumpInputWindow;
    float jumpCounter;
    bool inJumpCount;
    Rigidbody2D RB;

    // Use this for initialization
    void Start () {
        RB = GetComponent<Rigidbody2D>();
	}

    public void fire(Vector2 dir)
    {
        GameObject go=Instantiate(Proj, transform.GetChild(2).position, Quaternion.identity) as GameObject;
        go.GetComponent<ProjScript>().dir = dir;

    }

    public bool isGrounded()
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
	
	// Update is called once per frame
	void Update () {

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
            if (JumpMaxWatch + JumpINC < JumpMax)
            {
                RB.AddForce(Vector2.up * JumpINC);
                JumpMaxWatch += JumpINC;
            }
        }


        
        if (Input.GetKeyDown(KeyCode.J))
            fire(Vector2.left);
        if (Input.GetKeyDown(KeyCode.L))
            fire(Vector2.right);
        if (Input.GetKeyDown(KeyCode.I))
            fire(Vector2.up);

    }

    private void FixedUpdate()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Vector2.right * speed * Time.deltaTime);
    }
}
