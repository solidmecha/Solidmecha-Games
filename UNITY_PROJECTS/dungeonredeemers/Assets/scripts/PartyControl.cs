using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartyControl : MonoBehaviour {

    public static PartyControl singleton;
    public PlayerScript[] Party;
    public System.Random RNG;
    public enum State { Selecting, EnemyTargeting, AllyTargeting, PositionTargeting};
    State CurrentState; 
    public Vector2 selectedPoint;
    public PlayerScript ActivePlayer;
    public GameObject InfoCanvas;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
    }
    // Use this for initialization
    void Start () {
	
	}

    public void ShowMessage(string Msg, Vector2 Location, float duration)
    {
        GameObject g = Instantiate(InfoCanvas, Location, Quaternion.identity) as GameObject;
        g.transform.GetChild(0).GetComponent<Text>().text = Msg;
        g.GetComponent<Lifetime>().Counter = duration;
    }

    void MoveActivePlayer(Vector2 V)
    {
        ActivePlayer.Destination = V;
        ActivePlayer.isMoving = true;
    }
	
	// Update is called once per frame
	void Update () {
	if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider==null)
            {
                if(CurrentState==State.Selecting)
                    MoveActivePlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                else if(CurrentState==State.PositionTargeting)
                    ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            }
            else if(hit.collider.CompareTag("Player"))
            {
                if (CurrentState == State.Selecting && !hit.collider.GetComponent<PlayerScript>().KnockedOut)
                {
                    if (ActivePlayer != null)
                    {
                        ActivePlayer.isActivePlayer = false;
                    }
                    ActivePlayer = hit.collider.GetComponent<PlayerScript>();
                }
                else if(CurrentState==State.AllyTargeting)
                {
                    ActivePlayer.Skills[ActivePlayer.ActiveSkillIndex].Target = hit.collider.gameObject;
                }
            }
        }
    else if(Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null /*&& hit.collider.CompareTag("Monster")*/)
            {
                ActivePlayer.ActiveSkillIndex = 0;
                ActivePlayer.Skills[0].Target = hit.collider.gameObject;
                ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    else if(Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (ActivePlayer.Skills[1].RequiresTarget && hit.collider != null /*&& hit.collider.CompareTag("Monster")*/)
            {
                ActivePlayer.ActiveSkillIndex = 1;
                ActivePlayer.Skills[1].Target = hit.collider.gameObject;
                ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else if(!ActivePlayer.Skills[1].RequiresTarget)
            {
                ActivePlayer.ActiveSkillIndex = 1;
                ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    else if(Input.GetKeyDown(KeyCode.W))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (ActivePlayer.Skills[2].RequiresTarget && hit.collider != null /*&& hit.collider.CompareTag("Monster")*/)
            {
                ActivePlayer.ActiveSkillIndex = 2;
                ActivePlayer.Skills[2].Target = hit.collider.gameObject;
                ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (!ActivePlayer.Skills[2].RequiresTarget)
            {
                ActivePlayer.ActiveSkillIndex = 2;
                ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    else if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (ActivePlayer.Skills[3].RequiresTarget && hit.collider != null /*&& hit.collider.CompareTag("Monster")*/)
            {
                ActivePlayer.ActiveSkillIndex = 3;
                ActivePlayer.Skills[3].Target = hit.collider.gameObject;
                ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (!ActivePlayer.Skills[3].RequiresTarget)
            {
                ActivePlayer.ActiveSkillIndex = 3;
                ActivePlayer.UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
	}
}
