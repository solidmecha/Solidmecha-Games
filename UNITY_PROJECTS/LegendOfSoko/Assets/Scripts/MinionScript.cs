using UnityEngine;
using System.Collections;

public class MinionScript : MonoBehaviour {

    public float speed;
    public float hp;
    public float maxHp;
    public int ID;
    public int WeaknessID;
    public float Dps;
    float counter;
   

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Finish"))
        {
           collision.collider.GetComponent<CrystalScript>().TakeDamage(Dps*Time.deltaTime);

        }
    }

    // Use this for initialization
    void Start () {
        WaveControl wc = GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveControl>();
        wc.minionCount++;
        ID = wc.minionCount;
        
        int r = wc.RNG.Next(3);
        WeaknessID = r;
        GetComponent<SpriteRenderer>().color = wc.Colors[r];
	}

    public void Buff(float M)
    {
        speed += (.15f*M);
            hp += (25f*M);
        Dps += (2*M);
    }

    public void TakeDamage(float D)
    {
        hp -= D;
        float p = hp / maxHp;

        transform.GetChild(0).localScale=new Vector3(p, 1, 0);
        if (p < .25f)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        else if (p < .65f)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
        if (hp <= 0)
        {
            Destroy(gameObject);
            WaveControl wc= GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveControl>();
            wc.minionCount--;
            if (wc.minionCount == 0)
                wc.Win();
        }
    }
	
	// Update is called once per frame
	void Update () {
         transform.Translate(Vector2.right * speed * Time.deltaTime);
	}
}
