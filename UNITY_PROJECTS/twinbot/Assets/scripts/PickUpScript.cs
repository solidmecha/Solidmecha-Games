using UnityEngine;
using System.Collections;

public class PickUpScript : MonoBehaviour {

    public int ID; //speed, HP, Ar, Timing

    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player"))
        {
            PickUp(other.GetComponent<BotScript>());
            Destroy(gameObject);
        }
        else if(other.CompareTag("E"))
        {
            PickUp(other.GetComponent<EnemyScript>());
            Destroy(gameObject);
        }
    }


    void PickUp(BotScript B)
    {
        switch(ID)
        {
            case 0:
                BotControl.singleton.speed[B.ID] += .25f;
                break;
            case 1:
                B.TakeDamage(-1);
                break;
            case 2:
                B.ChangeArmor(-1);
                break;
            case 3:
                if(BotControl.singleton.fireDelay[B.ID]>.5f)
                    BotControl.singleton.fireDelay[B.ID] -= .15f;
                break;
            case 4:
                B.SetImmunity(8f);
                break;
            case 5:
                B.tripleShotOn();
                break;
            case 6:
               GameObject g= Instantiate(EnemyGen.singleton.spinner, B.transform.position, Quaternion.identity) as GameObject;
                g.GetComponent<SpinnerScript>().track = B.transform;
                foreach (SpriteRenderer s in g.GetComponentsInChildren<SpriteRenderer>())
                {
                    s.gameObject.tag = "B";
                    s.color = B.GetComponent<SpriteRenderer>().color;
                }
                break;
            case 7:
                g = Instantiate(EnemyGen.singleton.BoomBlast, B.transform.position, Quaternion.identity) as GameObject;
                g.GetComponent<SpriteRenderer>().color= B.GetComponent<SpriteRenderer>().color;
                break;
        }
    }

    void PickUp(EnemyScript E)
    {
        switch (ID)
        {
            case 0:
                E.speed += .25f;
                break;
            case 1:
            case 2:
                if (E.id < 3)
                {
                    E.id++;
                    E.GetComponent<SpriteRenderer>().sprite = EnemyGen.singleton.Efabs[E.id];
                    E.GetComponent<PolygonCollider2D>().points = EnemyGen.singleton.Colliders[E.id];
                }
                break;
            case 3:
                if(E.FireDelay>.5f)
                    E.FireDelay -= .15f;
                break;
            case 4:
                E.SetImmunity(7f);
                break;
            case 5:
                E.tripleShotOn();
                break;
            case 6:
                GameObject g = Instantiate(EnemyGen.singleton.spinner, E.transform.position, Quaternion.identity) as GameObject;
                g.GetComponent<SpinnerScript>().track = E.transform;
                break;
            case 7:
                g = Instantiate(EnemyGen.singleton.BoomBlast, transform.position, Quaternion.identity) as GameObject;
                g.GetComponent<BoomScript>().hostile = true;
                break;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
