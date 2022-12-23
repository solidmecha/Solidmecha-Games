using UnityEngine;
using System.Collections;

public class MessagingScript : MonoBehaviour {

    public float MessageDelay;
    public float MessageSpeed;
    public float TurnSpeed;
    public int MessagePerTower;
    int SentMessages;
    public GameObject Message;
    public GameObject TowerTarget;
    float counter;
    bool isTurning;
    float Theta;
    int TowerIndex;
    public Transform TowerRoot;
	// Use this for initialization
	void Start () {
        GameControl GC = TowerRoot.GetComponent<GameControl>();
        TowerIndex = GC.RNG.Next(TowerRoot.childCount);
        MessageDelay = GC.RNG.Next(1, 5);
        TurnSpeed = GC.RNG.Next(2, 6) * 30;
        MessageSpeed = GC.RNG.Next(1, 5);
        MessagePerTower = GC.RNG.Next(1, 5);
        CompleteTurn();
    }

    private void OnEnable()
    {
        if(TowerRoot.childCount>0)
            CompleteTurn();
    }

    public void CompleteTurn()
    {
        isTurning = true;
        TowerTarget = TowerRoot.GetChild(TowerIndex).gameObject;
        Vector2 v = TowerTarget.transform.position - transform.position;
        Theta = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (Theta < 0)
            Theta += 360;
        if (transform.localEulerAngles.z > Theta)
        {

            if (transform.localEulerAngles.z - Theta < 180)
            {
                if (TurnSpeed > 0)
                    TurnSpeed *= -1;
                counter = (transform.localEulerAngles.z - Theta) / TurnSpeed * -1;
            }
            else
            {
                if (TurnSpeed < 0)
                    TurnSpeed *= -1;
                counter = (360 - transform.localEulerAngles.z + Theta) / TurnSpeed;
            }

        }
        else
        {
            if (Theta - transform.localEulerAngles.z < 180)
            {
                if (TurnSpeed < 0)
                    TurnSpeed *= -1;
                counter = (Theta - transform.localEulerAngles.z) / TurnSpeed;
            }
            else
            {
                if (TurnSpeed > 0)
                    TurnSpeed *= -1;
                counter = (360 - Theta + transform.localEulerAngles.z) / TurnSpeed * -1;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	if(isTurning)
        {
            transform.Rotate(new Vector3(0, 0, TurnSpeed * Time.deltaTime));
            counter -= Time.deltaTime;
            if (counter<=0)
            {
                isTurning = false;
                transform.localEulerAngles = new Vector3(0, 0, Theta);
                counter = 0;
            }
        }
    else
        {
            counter += Time.deltaTime;
            if(counter>=MessageDelay)
            {
                GameObject go=Instantiate(Message, transform.position+transform.right, transform.rotation) as GameObject;
                MessageControl MC=go.GetComponent<MessageControl>();
                MC.Target = TowerTarget.transform.position;
                MC.Speed = MessageSpeed;
                counter = 0;
                SentMessages++;
                if(SentMessages==MessagePerTower)
                {
                    SentMessages = 0;
                    TowerIndex++;
                    if (TowerIndex == TowerRoot.childCount)
                        TowerIndex = 0;
                    CompleteTurn();
                }
            }
        }
	}
}
