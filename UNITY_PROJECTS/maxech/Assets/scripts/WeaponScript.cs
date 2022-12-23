using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public float[] CD;
    public GameControl.WeaponType weaponType;
    public Vector3 target;
    public bool engaged;
    public int[] Damage;
    public int HeatGen;
    public void Engage(Vector2 mousePos)
    {
        if (CD[0]<=0)
        {
            PlayerControl.singleton.takeDamage(HeatGen, GameControl.DamageType.heat);
            engaged = true;
            GameObject Go=Instantiate(GameControl.singleton.WeaponPrototypes[(int)weaponType]);
            {
                switch(weaponType)
                {
                    
                    case GameControl.WeaponType.Gat:

                        target = ClickPane();
                        transform.LookAt(target);
                        Go.transform.position = transform.position;
                        Go.transform.rotation = transform.rotation;
                        Go.transform.SetParent(transform);
                        Go.GetComponent<FiringScript>().isDot = true;
                        Go.GetComponent<FiringScript>().tick = new float[2] { 0, .5f };
                        Go.GetComponent<FiringScript>().WS = this;
                        break;
                    case GameControl.WeaponType.Nova:
                        Go.transform.position = transform.parent.position;
                        Go.transform.SetParent(transform.parent);
                        Go.GetComponent<FiringScript>().WS = this;
                        break;
                    case GameControl.WeaponType.Beam:
                        target = ClickPane();
                        transform.LookAt(target);
                        Go.transform.position = transform.position;
                        Go.transform.rotation = transform.rotation;
                        Go.transform.SetParent(transform);
                        Go.transform.GetChild(0).GetComponent<FiringScript>().WS = this;
                        break;
                    case GameControl.WeaponType.OrbitalCannon:
                        RaycastHit hit;
                        Go.transform.GetChild(0).GetComponent<FiringScript>().WS = this;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                        {
                            Vector3 p = new Vector3(hit.point.x, 10, hit.point.z);
                            Go.transform.position = p;
                        }
                        else
                        {
                            Destroy(Go);
                        }
                        break;
                    case GameControl.WeaponType.Missle:
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                        {
                            target = hit.point;
                            transform.LookAt(target);
                            Go.transform.position = transform.position;
                            Go.transform.rotation = transform.rotation;
                            Go.transform.GetChild(0).GetComponent<missleFX>().WS = this;
                        }
                        break;
                    case GameControl.WeaponType.Burst:
                        Go.transform.position = transform.position;
                        Go.transform.GetChild(0).GetComponent<FiringScript>().WS = this;
                        Go.transform.GetChild(1).GetComponent<FiringScript > ().WS = this;
                        Go.transform.GetChild(2).GetComponent<FiringScript > ().WS = this;
                        Go.transform.GetChild(3).GetComponent<FiringScript > ().WS = this;
                        break;


                }
            }
            CD[0] = CD[1];
        }

    }


    public void DealDamage(EnemyScript e)
    {
        if(e != null)
            e.takeDamage(GameControl.singleton.RNG.Next(Damage[0]+PlayerControl.singleton.AddedDmg(), Damage[1] + PlayerControl.singleton.AddedDmg()));
    }

    public Vector3 ClickPane()
    {
        Plane turretPlane = new Plane(Vector3.up, transform.position);
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        float d = 0f;
        turretPlane.Raycast(r, out d);
        return r.GetPoint(d);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (CD[0] > 0)
        {
            CD[0] -= Time.deltaTime;
            if (CD[0] <= 0)
            {
                CD[0] = 0;
            }
        }
        if(engaged)
            transform.LookAt(target);
    }
}
