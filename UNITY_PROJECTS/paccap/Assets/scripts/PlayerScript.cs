using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public static PlayerScript singleton;
    public float MaxVelocity;
    public float Acceleration;
    public float[] SpeedBonus;
    public int BonusIndex;
    public float BonusTimer;
    public GameObject ShotRay;
    public float[] jumpTime;
    public float jumpforce;
    bool inJump;
    public Vector3 ResetPoint;
    public int Ammo;
    public Text AmText;
    public float RocketJumpTime=-1;
    public Text MessageText;
    public GameObject Explosion;
    public bool inPlay;
    int countdownTimer=3;
    float Timer;
    Vector3 dir;
    public int[] CapCount;
    public int[] TargetCount;

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
        CountDown();
	}

    void CountDown()
    {
        if (countdownTimer > 0)
            ShowMessage(countdownTimer.ToString(), .5f);
        else
        {
            ShowMessage("Go!", .5f);
            inPlay = true;
        }
        countdownTimer--;
        if (countdownTimer >= 0)
            Invoke("CountDown", .5f);
    }

    public void AddAmmo(int A)
    {
        Ammo += A;
        ShowAmmoCount();
    }

    public void ResetPlayer()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = ResetPoint;
    }

    public void ShowMessage(string M, float t)
    {
        MessageText.text = M;
        Invoke("ClearMessage", t);
    }

    public void ShowAmmoCount()
    {
        AmText.text = Ammo.ToString();
        Invoke("ClearAmmoCount", 2.7f);
    }

    public void ClearAmmoCount()
    {
        AmText.text = "";
    }

    public void ClearMessage()
    {
        MessageText.text = "";
    }

    public void CapturePoint()
    {
        CapCount[0]++;
        if (CapCount[0] == CapCount[1])
            ShowRecap();

    }

    void ShowRecap()
    {
        Timer = Mathf.Round(Timer * 100f) / 100;
        string Recap = "Time: " + Timer.ToString() + "\n"
                        + "Targets: " + TargetCount[0].ToString() + "/" + TargetCount[1].ToString()
                        + "\n" + "Press 'E' to begin next Level.";
        ShowMessage(Recap, 999f);
        GameControl.singleton.NextSceneReady = true;
    }

    void Fire()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);
        if(hit.collider != null)
        {
            //Target hit
            if(hit.collider.gameObject.CompareTag("Target"))
            {
                TargetCount[0]++;
                Destroy(hit.collider.gameObject.transform.root.gameObject);
                if (Acceleration < MaxVelocity)
                {
                    Acceleration += SpeedBonus[BonusIndex];
                }
                if (BonusIndex < SpeedBonus.Length - 1)
                {
                    BonusIndex++;
                    BonusTimer = 7f;
                }
            }
            else if(hit.collider.CompareTag("Ground"))
            {
                Instantiate(Explosion, hit.point, Quaternion.identity);
                if(hit.point.y<transform.position.y && (hit.point-transform.position).sqrMagnitude<3.5f)
                {
                    RocketJumpTime = .2f;
                }
            }
        }
        Vector3 v = Camera.main.transform.GetChild(0).position + new Vector3(.25f, -.5f, -.5f);
        Instantiate(ShotRay, v, Quaternion.LookRotation(Camera.main.transform.position+Camera.main.transform.forward*100f-v));
        
    }

    bool isGrounded()
    {
        RaycastHit hit;
        for (int i=0;i<5;i++)
        {
           Physics.Raycast(transform.GetChild(i).position, Vector3.down, out hit, .4f+.1f*Acceleration);
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
                return true;
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () {
        if (inPlay)
        {
            BonusTimer -= Time.deltaTime;
            if (RocketJumpTime >= 0)
                RocketJumpTime -= Time.deltaTime;
            if (BonusTimer <= 0)
            {
                BonusTimer = 7f;
                if (BonusIndex > 0)
                {
                    Acceleration -= SpeedBonus[BonusIndex];
                    BonusIndex--;
                }

            }
            dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                dir += new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            }
            if (Input.GetKey(KeyCode.S))
            {
                dir += (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized*-1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                dir += (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized*-1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                 dir += new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (Ammo > 0)
                {
                    Fire();
                    Ammo--;
                }
                ShowAmmoCount();
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
            {
                if (RocketJumpTime < 0)
                    GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpforce * 3f), ForceMode.Impulse);
                else
                {
                    GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpforce * 7f), ForceMode.Impulse);
                    RocketJumpTime = -1f;
                    ShowMessage("Rocket Jump!", 1f);
                }
                inJump = true;
                jumpTime[0] = 0;
            }
            else if (inJump && (Input.GetKey(KeyCode.Space)) && jumpTime[0] < jumpTime[1])
            {
                jumpTime[0] += Time.deltaTime;
                if (RocketJumpTime < 0)
                    GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpforce * 1.5f));
                else
                {
                    GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpforce * 4f), ForceMode.Impulse);
                    ShowMessage("Rocket Jump!", 1f);
                    RocketJumpTime = -1f;
                }
            }
            else
            {
                inJump = false;
                jumpTime[0] = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (inPlay)
        {
            transform.Translate(Acceleration* Time.deltaTime * dir.normalized);
        }
    }

    private void LateUpdate()
    {
        if (inPlay)
        {
            Timer += Time.deltaTime;
            Camera.main.transform.position = transform.position + Vector3.up * .5f;
            Camera.main.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * GameControl.singleton.Sensitivity);
            Camera.main.transform.Rotate(Vector3.right, -1 * Input.GetAxis("Mouse Y") * GameControl.singleton.Sensitivity);
            Camera.main.transform.eulerAngles = (new Vector3(Camera.main.transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, 0));

            if (Camera.main.transform.eulerAngles.x < -85f && Camera.main.transform.eulerAngles.x > -100f)
                Camera.main.transform.eulerAngles = (new Vector3(-85f, Camera.main.transform.localEulerAngles.y, 0));
            if (Camera.main.transform.eulerAngles.x > 85f && Camera.main.transform.eulerAngles.x < 100)
                Camera.main.transform.eulerAngles = (new Vector3(85f, Camera.main.transform.localEulerAngles.y, 0));

            if (transform.position.y < -75f)
                ResetPlayer();
            if(GameControl.singleton.NextSceneReady && Input.GetKeyDown(KeyCode.E))
            {
                GameControl.singleton.LoadNextScene();
            }
        }
    }
}
