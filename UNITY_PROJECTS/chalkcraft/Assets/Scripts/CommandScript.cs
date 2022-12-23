using UnityEngine;
using System.Collections;

public class CommandScript : MonoBehaviour {

    public enum Command { Go, ccTurn, Turn, Attack }
    public Command command;
    public Transform chalkling;

    private void OnMouseDown()
    {
        if (chalkling == null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            DrawScript ds = GameObject.FindGameObjectWithTag("GameController").GetComponent<DrawScript>();
            ds.OrderGiven = true;
            if (ds.ChalkAmt > 1)
            {
                switch (command)
                {
                    case Command.Attack:
                        Attack();
                        Destroy(transform.parent.gameObject);
                        break;
                    case Command.Go:
                        Go();
                        Destroy(transform.parent.gameObject);
                        break;
                    case Command.Turn:
                        turn();
                        break;
                    case Command.ccTurn:
                        ccTurn();
                        break;
                }
                ds.UpdateChalk(-2);
            }
        }

    }

    public void turn()
    {
        chalkling.Rotate(new Vector3(0, 0, 90));
        if (Mathf.Round(chalkling.localEulerAngles.z) == 180)
            chalkling.GetComponent<SpriteRenderer>().flipY = true;
        else if (Mathf.Round(chalkling.localEulerAngles.z) == 0 && chalkling.GetComponent<SpriteRenderer>().flipY)
            chalkling.GetComponent<SpriteRenderer>().flipY = false;

    }

    public void ccTurn()
    {
        chalkling.Rotate(new Vector3(0, 0, -90));
        if (Mathf.Round(chalkling.localEulerAngles.z) == 180)
            chalkling.GetComponent<SpriteRenderer>().flipY = true;
        else if (Mathf.Round(chalkling.localEulerAngles.z) == 0 && chalkling.GetComponent<SpriteRenderer>().flipY)
            chalkling.GetComponent<SpriteRenderer>().flipY = false;
    }

    public void Go()
    {
        chalkling.GetComponent<ChalklingControl>().currentSpeed = chalkling.GetComponent<ChalklingControl>().Speed;
        chalkling.GetComponent<Animator>().SetBool("Walking", true);
    }

    public void Attack()
    {
        chalkling.GetComponent<ChalklingControl>().currentSpeed = 0;
        chalkling.GetComponent<Animator>().SetBool("Walking", false);      
    }

    // Use this for initialization
    void Start () {
        if (chalkling == null)
            chalkling = transform;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
