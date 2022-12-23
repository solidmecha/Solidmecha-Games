using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PulseScript : MonoBehaviour {

    public GameObject bullet;
    GameObject player;
    public GameObject ShapeShifter;
    public int id;
    SpriteRenderer SR;
    public float delay;
    float counter;
    public List<Sprite> SpriteList;
    float Durations;
    public int number_Of_Forms;
    public GameObject GoUI;
    public List<string> YesList=new List<string> { };

	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
        player=Instantiate(ShapeShifter) as GameObject;
        PlayerControl PC = (PlayerControl)player.GetComponent(typeof(PlayerControl));
        PC.PS = this;
        switchForm();
	}

    public void GameOver()
    {
        number_Of_Forms-=2;
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        GameObject go = Instantiate(GoUI) as GameObject;
        string form=" form\n";
        if (number_Of_Forms > 1)
            form = " forms\n";
        go.transform.GetChild(0).GetComponent<Text>().text = "You made it through \n"+number_Of_Forms.ToString()+ form+"Can you do better?";
        go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Application.LoadLevel(0); });
        go.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = YesList[RNG.Next(YesList.Count)];
    }


    void pulse()
    {
        for(int i=0;i<36;i++)
        {

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            BulletScript bs = (BulletScript)go.GetComponent(typeof(BulletScript));
            bs.formNumber = number_Of_Forms;
            bs.direction = Vector2.up;
            bs.id = id;
            go.transform.eulerAngles=new Vector3(0, 0, 10 * i);
            go.GetComponent<SpriteRenderer>().sprite = SR.sprite;
        }
    }

    void switchForm()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        number_Of_Forms++;
        int r = RNG.Next(5);      
        SR.sprite = SpriteList[r];
        Durations = 2.25f - .05f * number_Of_Forms;
        if (Durations < .8f)
            Durations = .8f;
        id = r;
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(counter>=Durations)
        {
            counter = 0;
            switchForm();
            pulse();
        }
	
	}
}
