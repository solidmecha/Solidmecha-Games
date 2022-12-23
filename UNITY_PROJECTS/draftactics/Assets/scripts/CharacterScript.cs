using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
    public bool PlayerTeam;
    public string CharName;
    public int ClassID;
    public int[] HP;
    public int[] BP;
    public int Evasion;
    public int Armor;
    public int[] Resistance;
    public int[] SkillIDs;
    public int[] Position;
    public int Speed;
    public int Movement;

    private void OnMouseOver()
    {
        //GameControl.singleton.SelectedChar = this;
        GameControl.singleton.ShowStats(this);
    }

    public void TakeDamage(int D)
    {
        if(D != 0)
            (Instantiate(GameControl.singleton.InfoCanvas, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text=D.ToString();
        HP[0] -= D;
        if (HP[0] <= 0)
        {
            if (GetComponent<PassivesScript>().PassiveID == 0 && !GetComponent<PassivesScript>().SingleUse)
            {
                HP[0] = 1;
                GetComponent<PassivesScript>().SingleUse = true;
                GetComponent<PassivesScript>().Description += " (Used)";
            }
            else
            {
                transform.position = new Vector2(100, 100);
                Position[0] = 100;
                Position[1] = 100;
            }
        }
        else if (HP[0] >= HP[1])
            HP[0] = HP[1];
    }

    public void DealDamage(CharacterScript Target)
    {
        int D = GetComponent<SkillScript>().DamageRoll();
        if(GetComponent<PassivesScript>().PassiveID==4)
        {
            int L = GetComponent<SkillScript>().DamageRoll();
            if (L > D)
                D = L;
        }
        if(GetComponent<PassivesScript>().PassiveID == 6 && GameControl.singleton.RNG.Next(10)==4)
        {
            D *= 2;
        }
        if (Target.PlayerTeam == PlayerTeam)
        {
            if (ClassID >= 7)
                D *= -1;
            else
                return;
        }
        else
        {
            if (GameControl.singleton.RNG.Next(100) < Target.Evasion)
            {
                (Instantiate(GameControl.singleton.InfoCanvas, Target.transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Evade!";
                return;
            }
            D = Mathf.RoundToInt(D * (100f - Target.Armor) / 100f);
        }
        Target.TakeDamage(D);
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
