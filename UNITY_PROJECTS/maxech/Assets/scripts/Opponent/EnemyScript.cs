using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    float[] fireRate=new float[2];
    public int[] FireRateValueRange;
    public int hp;
    public int dmg;
    public GameObject weaponPrefab;

    public void takeDamage(int d)
    {
        if (GameControl.singleton.RNG.Next(100) < PlayerControl.singleton.Crit)
            d *= 2;
        (Instantiate(GameControl.singleton.DmgNum, transform.position+Vector3.up+GameControl.singleton.RandPoint(), Quaternion.identity) as GameObject).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = d.ToString();
        hp -= d;
        PlayerControl.singleton.OnHit();
        if (hp <= 0)
        {
            WaveControl.singleton.ReduceEnemyCount();
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        hp += GameControl.singleton.RNG.Next(10 * GameControl.singleton.CurrentLvl, 2 * 10 * GameControl.singleton.CurrentLvl);
        fireRate[1] = GameControl.singleton.RNG.Next(FireRateValueRange[0], FireRateValueRange[1]) / 100f;
        if(GameControl.singleton.RNG.Next(5)==4)
            gameObject.AddComponent<RotateIt>().speed = GameControl.singleton.RNG.Next(1, 31);
        dmg = GameControl.singleton.RNG.Next(2, 10) * GameControl.singleton.CurrentLvl;
        GetComponent<MoveIt>().move *= (GameControl.singleton.RNG.Next(100, 251) / 100);
    }
    
	// Update is called once per frame
	void Update () {
        fireRate[0] -= Time.deltaTime;
        if(fireRate[0]<=0)
        {
            fireRate[0] = fireRate[1];
            transform.LookAt(PlayerControl.singleton.transform.position);
            transform.Rotate(Vector3.up, GameControl.singleton.RNG.Next(-45, 45));
            (Instantiate(weaponPrefab, transform.position, transform.rotation) as GameObject).GetComponent<evilOrbFx>().ES=this;
            transform.LookAt(WaveControl.singleton.transform.GetChild(GameControl.singleton.RNG.Next(12)));
        }
	}
}
