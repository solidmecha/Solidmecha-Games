using UnityEngine;

public class CharControl : MonoBehaviour {

    public GameObject MMProj;
    public float[] speed;
    public Vector2 ProjSize;
    public float ProjSpeed;
    public float FireDelay;
    public float exploSize;
    public bool canFire;
    public bool running;
    public int[] HP;
    public int[] MP;
    public int Dmg;
    public int PlayerID;
    public bool Blinking;
    float FireCounter;
    public float jumpDelay; //1.25
    public float jumpforce; //5.6, 7.8, 9.5
    float JumpCounter;
    public int[] JumpCharges=new int[2] {2,2};
    public SpriteRenderer[] ReadyIcons;
    int activePortIndex;
    bool PlacingPort;
    public GameObject ConfirmUI;
    public Transform ActiveUI;
    bool ChannelPort;
    float blinkcounter;
    public int GemCount;
    public int Armor;
    public int Luck;
    float[] jumpTime;
    bool inJump;
    public Vector2 Bounds=new Vector2(-7.7f, 58.8f);
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Spikes") && !Blinking)
        {
            TakeDmg(24);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mana") && MP[0] < MP[1])
        {
            ChangeMana(-10);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Hpot") && HP[0] < HP[1])
        {
            TakeDmg(-10);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("gem"))
        {
            GemCount++;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("P"))
            ChannelPort = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("P"))
            ChannelPort = false;
    }

    public void TakeDmg(int D)
    {
        if(HP[0]<=0 && HP[0]-D>0)
        {
            WorldControl.singleton.ShowMessage("Saved.",2f);
        }
        if (D > 0)
        {
            D -= Armor;
            if (D < 0)
                D = 1;
        }
        HP[0] -= D;
        if (D > 0)
            Blinking = true;
        if (HP[0] > HP[1])
            HP[0] = HP[1];
        if (HP[0] < 0)
            HP[0] = 0;
        WorldControl.singleton.UIElements[0].localScale = new Vector2((float)HP[0] / (float)HP[1], WorldControl.singleton.UIElements[0].localScale.y);
        if (HP[0]<=0)
        {
            WorldControl.singleton.GameOver();
        }
    }

    public void ChangeMana(int M)
    {
        MP[0] -= M;
        if (MP[0] > MP[1])
            MP[0] = MP[1];
        if (MP[0] < 0)
            MP[0] = 0;
        WorldControl.singleton.UIElements[1].localScale = new Vector2((float)MP[0] / (float)MP[1], WorldControl.singleton.UIElements[0].localScale.y);
    }
    // Use this for initialization
    void Start () {
        JumpCounter = jumpDelay;
        jumpTime = new float[2] { 0, .5f + (jumpforce - 6.2f) / 7.6f };
	}

    public bool hasMana(int M)
    {
        if (MP[0] >= M)
            return true;
        else
        {
            WorldControl.singleton.ShowMessage("Not enough Mana", 2f);
            return false;
        }
    }

    void Fire()
    {
        GameObject go = Instantiate(MMProj, transform.position, Quaternion.identity) as GameObject;
        go.transform.localScale = ProjSize;

        ProjScript ps = go.GetComponent<ProjScript>();
        ps.speed = ProjSpeed;
        ps.Dmg = Dmg;
        if (exploSize > 0)
        {
            ps.hasExplo = true;
            ps.maxSize = exploSize;
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float a = Vector2.Angle(Vector2.right, mousePos - (Vector2)transform.position);       
        if ((mousePos - (Vector2)transform.position).y < 0)
            a *= -1;
        ps.transform.eulerAngles = new Vector3(0, 0, a);
        ps.ID = PlayerID;
        //go.transform.position = go.transform.right * ProjSize.x*.5f + go.transform.position;
    }

    void ResetAnim()
    {
        GetComponent<Animator>().SetInteger("AnimID", -1);
    }
	
	// Update is called once per frame
	void Update () {
        if (JumpCharges[0] < JumpCharges[1])
        {
            JumpCounter -= Time.deltaTime;
        }
            if (Input.GetMouseButton(0))
            {
                if (canFire && hasMana(2))
                {
                    ChangeMana(2);
                    canFire = false;
                    ReadyIcons[0].enabled = false;
                    GetComponent<Animator>().SetInteger("AnimID", 1);
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    GetComponent<SpriteRenderer>().flipX = (mousePos - (Vector2)transform.position).x > 0;
                    Fire();
                    //Invoke("Fire", .65f);
                    Invoke("ResetAnim", .33f);
                }
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if(!IsInvoking())
                    GetComponent<Animator>().SetInteger("AnimID", 2);
            //canFire = false;
            //ReadyIcons[0].enabled = false;
            speed[0] += Time.deltaTime*6f;
            if (speed[0] > speed[1])
                speed[0] = speed[1];
            transform.Translate(speed[0] * Vector2.right * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if(!IsInvoking())
                    GetComponent<Animator>().SetInteger("AnimID", 2);
            speed[0] += Time.deltaTime*6f;
            if (speed[0] > speed[1])
                speed[0] = speed[1];
            transform.Translate(speed[0] * Vector2.left * Time.deltaTime);
                //canFire = false;
                //ReadyIcons[0].enabled = false;
            }
            else
        {
            speed[0] = 0;
        }
            if (!canFire)
            {
                FireCounter += Time.deltaTime;
            ReadyIcons[0].transform.GetChild(0).GetChild(0).localScale = new Vector2(FireCounter / FireDelay, 1);
                if (FireCounter >= FireDelay)
                {
                    FireCounter = 0;
                    canFire = true;
                    ReadyIcons[0].enabled = true;
                }
            }

        if (JumpCounter <= 0)
        {
            JumpCounter = jumpDelay;
            JumpCharges[0]++;
            ReadyIcons[JumpCharges[0]].enabled = true;
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && JumpCharges[0] > 0)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 3.4f), ForceMode2D.Impulse);
            JumpCounter = jumpDelay;
            JumpCharges[0]--;
            ReadyIcons[JumpCharges[0] + 1].enabled = false;
            inJump = true;
            jumpTime[0] = 0;
        }
        else if (inJump && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && jumpTime[0] < jumpTime[1])
        {
            jumpTime[0] += Time.deltaTime;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpforce*1.5f));
        }
        else
        {
            inJump = false;
            jumpTime[0] = 0;
        }
        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && JumpCharges[0]>0)
        {
            int d = -1;
            if (GetComponent<SpriteRenderer>().flipX)
                d = 1;
            transform.Translate(Vector2.right * d * 2.5f);
            if (transform.position.x < Bounds.x)
                transform.position =new Vector2(Bounds.x, transform.position.y);
            else if(transform.position.x>Bounds.y)
                transform.position = new Vector2(Bounds.y, transform.position.y);
            JumpCounter = jumpDelay;
            JumpCharges[0]--;
            ReadyIcons[JumpCharges[0] + 1].enabled = false;
        }
       if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
                GetComponent<Animator>().SetInteger("AnimID", -2);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlacingPort = true;
            ActiveUI=(Instantiate(ConfirmUI, (Vector2)transform.position+Vector2.down*.5f, Quaternion.identity) as GameObject).transform.GetChild(0);
        }
        if(PlacingPort)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                ActiveUI.localScale = (Vector2)ActiveUI.localScale + Vector2.right * Time.deltaTime*.5f;
                if(ActiveUI.localScale.x>=1)
                {
                    PlacingPort = false;
                    if (!ChannelPort)
                    {
                        WorldControl.singleton.ports[activePortIndex].position = transform.position;
                        Destroy(ActiveUI.parent.gameObject);
                        activePortIndex = (activePortIndex + 1) % 2;
                    }
                    else
                    {
                        Destroy(ActiveUI.parent.gameObject);
                        Blinking = true;            
                        if (((Vector2)transform.position - (Vector2)WorldControl.singleton.ports[0].position).sqrMagnitude > ((Vector2)transform.position - (Vector2)WorldControl.singleton.ports[1].position).sqrMagnitude)
                            transform.position = (Vector2)WorldControl.singleton.ports[0].position;
                        else
                            transform.position = (Vector2)WorldControl.singleton.ports[1].position;
                        Camera.main.GetComponent<CameraControl>().Snap(transform.position);
                    }
                    ChannelPort = true;
                }
            }
            else
            {

                PlacingPort = false;
                Destroy(ActiveUI.parent.gameObject);
            }
        }
        if(Blinking)
        {
            blinkcounter += Time.deltaTime;
            if (blinkcounter < .3f || (blinkcounter > .6f && blinkcounter < .9f) || blinkcounter > 1.2f)
                GetComponent<SpriteRenderer>().enabled = false;
            else
                GetComponent<SpriteRenderer>().enabled = true;
            if(blinkcounter>=1.5f)
            {
                Blinking = false;
                blinkcounter = 0;
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
