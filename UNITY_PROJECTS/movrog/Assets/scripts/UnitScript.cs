using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour {

    public float[] HP;
    public int UnitIndex;
    public bool PlayerControlled;
    public bool Mob;
    public bool isMoving;
    bool inCombat;
    public float speed;
    public Vector2 Dest;
    Vector2 RespawnPos;
    GameObject currentMSG;
    public PointScript TargetPoint;
    public int Mana;
    public float RespawnTimer;
    public int[] Dmg;

	// Use this for initialization
	void Start () {
        RespawnPos = transform.position;
        if (Mob)
            TargetPoint = transform.parent.GetComponent<PointScript>();
        if (PlayerControlled)
        {
            if (UnitIndex < 9)
                transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = (UnitIndex + 1).ToString();
            else
                transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "0";
        }
	}

    public void UpdateHP(float d)
    {
        HP[0] += d;
        if (HP[0] > HP[1])
            HP[0] = HP[1];
        if(PlayerControlled)
            GameControl.singleton.Canvas.transform.GetChild(UnitIndex).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = ((int)HP[0]).ToString();
    }

    public void StartMove(Vector2 Loc)
    {
        if (PlayerControlled)
        {
            if (transform.childCount == 2)
                Destroy(transform.GetChild(1).gameObject);
            GameObject go = Instantiate(GameControl.singleton.PathPreview, transform.position, Quaternion.identity) as GameObject;
            go.transform.GetChild(0).localScale = new Vector2((Loc - (Vector2)transform.position).magnitude, .35f);
            go.transform.GetChild(0).localPosition = new Vector3((Loc - (Vector2)transform.position).magnitude / 2, 0f, -0.05f);
            go.transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.right, Loc - (Vector2)transform.position));
            if ((Loc - (Vector2)transform.position).y < 0)
                go.transform.eulerAngles = new Vector3(0, 0, -1*Vector2.Angle(Vector2.right, Loc - (Vector2)transform.position));
            go.transform.SetParent(transform);
            go.GetComponent<PathScript>().speed = speed;
        }
        isMoving = true;
        Dest = Loc;
    }

    public void Attack(UnitScript Target)
    {
        if (Target.HP[0] > 0)
        {
            GetComponent<LineRenderer>().SetPositions(new Vector3[2] { transform.position, Target.transform.position });
            Invoke("ResetLine", .1f);
            Target.UpdateHP(-1 * GameControl.singleton.RNG.Next(Dmg[0], Dmg[1] + 1));
            if (Target.Mob && Target.HP[0] <= 0 && PlayerControlled)
                GameControl.singleton.UpdateMana(Target.Mana);
        }
    }
	
    public void ResetLine()
    {
        GetComponent<LineRenderer>().SetPositions(new Vector3[2] { transform.position, transform.position });
    }

    public void StartRespawn()
    {
        HP[0] = HP[1];
        transform.position = new Vector3(-100, -100, -100);
        if (PlayerControlled || Mob)
        {
            currentMSG = Instantiate(GameControl.singleton.MessageCanvas, RespawnPos, Quaternion.identity) as GameObject;
            currentMSG.GetComponent<Countdown>().counter = RespawnTimer;
        }
        Invoke("Respawn", RespawnTimer);
        TargetPoint.Units.Remove(this);
        isMoving = false;
        if (transform.childCount == 2)
            Destroy(transform.GetChild(1).gameObject);
        if(Mob)
            TargetPoint.GetComponent<CampScript>().Mobs.Remove(this);
        else
            TargetPoint = null;
    }

    public void Respawn()
    {
        UpdateHP(0);
        transform.position = RespawnPos;
        Destroy(currentMSG);
        if (Mob)
            TargetPoint.GetComponent<CampScript>().Mobs.Add(this);
    }

    public void SetTargetPoint(PointScript P)
    {
        if(TargetPoint!=null)
        {
            TargetPoint.Units.Remove(this);
        }
        TargetPoint = P;
        StartMove((Vector2)P.transform.position+new Vector2(GameControl.singleton.RNG.Next(-5,5)/5f, GameControl.singleton.RNG.Next(-5, 5) / 5f));
    }

	// Update is called once per frame
	void Update () {
        if (HP[0] <= 0)
            StartRespawn();
	if(isMoving)
        {
            Vector2 dir = Dest - (Vector2)transform.position;
            float dist = dir.sqrMagnitude;
            transform.Translate(dir.normalized * Time.deltaTime * speed);
            if(dist<=(Dest - (Vector2)transform.position).sqrMagnitude)
            {
                transform.position = Dest;
                isMoving = false;
                TargetPoint.Units.Add(this);
            }
            if (HP[0] < HP[1])
                UpdateHP(Time.deltaTime*10);
        }
	}
}
