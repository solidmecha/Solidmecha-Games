using UnityEngine;
using System.Collections;

public class CrystalScript : MonoBehaviour {
    public float hp;
    public float maxHp;
    public float Gain;
    // Use this for initialization
    void Start () {
	
	}
    public void TakeDamage(float D)
    {
        hp -= D;
        if (hp > maxHp)
            hp = maxHp;
        float p = hp / maxHp;

        transform.GetChild(0).localScale = new Vector3(p, 1, 0);
        if (p < .25f)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        else if (p < .65f)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
        if (hp <= 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveControl>().Loss();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        TakeDamage(Gain * Time.deltaTime);
	}
}
