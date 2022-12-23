using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatScript : MonoBehaviour {

    public int[] HP;
    public int[] MP;
    public int Armor;
    public int[] LastTurnStats = new int[3];
    public GameObject Canvas;



    public void UpdateHP(int change)
    {
        if(HP[0]==0 && change<0)
            transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
        if (change != 0)
        {
            GameObject g = Instantiate(GameControl.singleton.InfoCanvas, transform.position, Quaternion.identity) as GameObject;
            g.transform.GetChild(0).GetComponent<Text>().text = (-1*change).ToString();
            if (change < 0)
                g.transform.GetChild(0).GetComponent<Text>().color = Color.green;
        }
        HP[0] -= change;
        if(HP[0]<=0)
        {
            HP[0] = 0;
            GameControl.singleton.CheckGameOver();
            transform.GetChild(0).localEulerAngles=new Vector3(0, 0, 90);
        }
        else if(HP[0]>HP[1])
        {
            HP[0] = HP[1];
        }
        Canvas.transform.GetChild(0).GetComponent<Text>().text = HP[0].ToString() + "/" + HP[1].ToString();
    }

    public void UpdateMP(int change)
    {
        if (change != 0)
        {
            GameObject g = Instantiate(GameControl.singleton.InfoCanvas, transform.position, Quaternion.identity) as GameObject;
            g.transform.GetChild(0).GetComponent<Text>().text = (-1 * change).ToString();
            g.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
        }
        MP[0] -= change;

        if (MP[0] > MP[1])
        {
            MP[0] = MP[1];
        }
        if (MP[0] < 0)
            MP[0] = 0;
        Canvas.transform.GetChild(2).GetComponent<Text>().text = MP[0].ToString() + "/" + MP[1].ToString();
    }

    public void UpdateArmor(int change)
    {
        if (change != 0)
        {
            GameObject g = Instantiate(GameControl.singleton.InfoCanvas, transform.position, Quaternion.identity) as GameObject;
            g.transform.GetChild(0).GetComponent<Text>().text = (change).ToString();
            g.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
        }
        Armor += change;
        if (Armor > HP[1])
            Armor = HP[1];
        Canvas.transform.GetChild(1).GetComponent<Text>().text = Armor.ToString();
        if(change<0 && GameControl.singleton.SkillDurationCheck(5))
        {
            GameControl.singleton.MessageText.text = GameControl.singleton.Attacker.name + " is hurt by Retribution!";
            GameControl.singleton.Attacker.GetComponent<StatScript>().UpdateHP(-1*change);
        }
    }

    public void Snapshot()
    {
        LastTurnStats[0] = HP[0];
        LastTurnStats[1] = Armor;
        LastTurnStats[2] = MP[0];
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
