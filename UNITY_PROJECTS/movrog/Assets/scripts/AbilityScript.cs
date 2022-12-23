using UnityEngine;
using System.Collections;

public class AbilityScript : MonoBehaviour {

    public float AbilityCD;
    public int ManaCost;
    public int ID;
    public GameObject currentMSG;

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Activate(); });
	}

    public void Activate()
    {
        if (ManaCost <= GameControl.singleton.Mana)
        {
            GameControl.singleton.UpdateMana(-1*ManaCost);
            switch (ID)
            {
                case 0:
                    Heal();
                    break;
                case 1:
                    Speed();
                    break;
                case 2:
                    DmgUp();
                    break;
                case 3:
                    Score();
                    break;
            }
            Invoke("Undo", AbilityCD);
            currentMSG = Instantiate(GameControl.singleton.MessageCanvas, transform.position, Quaternion.identity) as GameObject;
            currentMSG.GetComponent<Countdown>().counter = AbilityCD;
            GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    void Heal()
    {
        foreach (UnitScript u in GameControl.singleton.Units)
        {
            if(u.HP[0]>0)
                u.UpdateHP(400);
        }
    }

    void Undo()
    {
        switch (ID)
        {
            case 0:
                break;
            case 1:
                foreach (UnitScript u in GameControl.singleton.Units)
                    u.speed -= 2.5f;
                break;
            case 2:
                foreach (UnitScript u in GameControl.singleton.Units)
                {
                    u.Dmg[0] -= 6;
                    u.Dmg[1] -= 10;
                }
                break;
            case 3:
                break;
        }
        Destroy(currentMSG);
        GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    void DmgUp()
    {
        foreach (UnitScript u in GameControl.singleton.Units)
        {
            u.Dmg[0] += 6;
            u.Dmg[1] += 10;
        }
    }

    void Speed()
    {
        foreach (UnitScript u in GameControl.singleton.Units)
            u.speed+=2.5f;
    }

    void Score()
    {
        GameControl.singleton.VP[0] += 50;
        GameControl.singleton.VPText[0].GetComponent<UnityEngine.UI.Text>().text = GameControl.singleton.VP[0].ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
