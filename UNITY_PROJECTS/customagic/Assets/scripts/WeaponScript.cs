using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public enum state { drifting, holding, engaged, pattern};
    public state currentState;
    float EngageOffset;
    bool flipped;
   public Vector2 targetPosition;
   public bool hostile;
    public float speed=2.5f;
    public int Durability;
    public int DPS;
    public int[] cost;
    public int InteractCount;
    public int ID;
    public GameObject interactableObj;
    public int WeapIndex;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hostile && currentState != state.engaged)
        {
            if (collision.CompareTag("Player") && currentState == state.holding && collision.GetComponent<PlayerWeaponControl>().Weapons.Count<collision.GetComponent<CharControl>().GemCount)
            {
                WeapIndex = WorldControl.singleton.ActivePlayer.GetComponent<PlayerWeaponControl>().Weapons.Count;
                WorldControl.singleton.ActivePlayer.GetComponent<PlayerWeaponControl>().Weapons.Add(this);
                currentState = state.drifting;
                InvokeRepeating("FindPos", 0, 1f);
                Buff();
            }
            else if (collision.CompareTag("W") && collision.GetComponent<WeaponScript>().hostile && WeapIndex == -1)
            {
                interactableObj = collision.transform.parent.gameObject;
                Engage(collision.transform.position);
                collision.GetComponent<WeaponScript>().Engage(transform.position);
                if (tryInteraction())
                {
                    collision.GetComponent<WeaponScript>().Invoke("DestroyObj", 1.2f);
                    Invoke("SetHolding", 1.2f);
                }
                else
                {
                    collision.GetComponent<WeaponScript>().Invoke("SetHolding", 1.2f);
                    collision.GetComponent<WeaponScript>().InvokeRepeating("FindPos", 1.2f, 2f); //reset enemy movement
                    Invoke("DestroyObj", 1.2f);
                }
            }
            else if (ID == 6 && collision.CompareTag("Spikes") && WeapIndex == -1)
            {
                Engage(collision.transform.position);
                interactableObj = collision.gameObject;
                Invoke("DestroyInteractable", 1.2f);
                Invoke("SetHolding", 1.2f);
                if (!tryInteraction())
                {
                    Invoke("DestroyObj", 1.2f);
                }
            }
        }
        else if (hostile && collision.CompareTag("Player") && !collision.GetComponent<CharControl>().Blinking)
            collision.GetComponent<CharControl>().TakeDmg(5);
    }

    void Buff()
    {
        CharControl C = WorldControl.singleton.ActivePlayer.GetComponent<CharControl>();
        switch(ID)
        {
            case 0:
                C.jumpDelay -= .2f;
                C.jumpforce += .1f;
                break;
            case 1:
                C.exploSize += .5f;
                C.FireDelay -= .25f;
                C.ProjSpeed += .25f;
                break;
            case 2:
                C.Armor++;
                break;
            case 3:
                C.ProjSize +=new Vector2(.1f,.1f);
                C.Dmg += 1;
                break;
            case 4:
                C.Luck++;
                break;
            case 5:
                C.speed[1] += .2f;
                break;
        }
    }

    public void Debuff()
    {
        CharControl C = WorldControl.singleton.ActivePlayer.GetComponent<CharControl>();
        switch (ID)
        {
            case 0:
                C.jumpDelay += .2f;
                C.jumpforce -= .1f;
                break;
            case 1:
                C.exploSize -= .5f;
                C.FireDelay += .25f;
                C.ProjSpeed -= .25f;
                break;
            case 2:
                C.Armor--;
                break;
            case 3:
                C.ProjSize -= new Vector2(.1f, .1f);
                C.Dmg -= 1;
                break;
            case 4:
                C.Luck--;
                break;
            case 5:
                C.speed[1] -= .2f;
                break;
        }
    }

    // Use this for initialization
    void Start () {
        if (!hostile)
        {
            WorldControl.singleton.ActivePlayer.GetComponent<PlayerWeaponControl>().Weapons.Add(this);
            InvokeRepeating("FindPos", 0, 1f);
            Buff();
        }
        else
        {
            InvokeRepeating("FindPos", 0, 2.1f);
        }
	}

    void DestroyInteractable()
    {
        if(interactableObj != null)
            Destroy(interactableObj);
    }

    public void SetHolding()
    {
        GetComponent<Animator>().SetInteger("AnimID", 0);
        WeapIndex = -1;
        currentState = state.holding;
    }

    void DestroyObj()
    {
        Destroy(gameObject);
        if (hostile && WorldControl.singleton.RNG.Next(50) <= 17+WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().Luck)
            DropControl.singleton.RandomDrop(transform.position);
    }

    bool tryInteraction()
    {
        InteractCount++;
        return (WorldControl.singleton.RNG.Next(2, 17)+ WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().Luck > InteractCount);
    }

    private void OnDestroy()
    {
        if(hostile && WeapIndex!=-1)
        {
            interactableObj.GetComponent<CrystalScript>().RemoveAttacker(WeapIndex);
        }
    }

    void FindPos()
    {
        if (!hostile)
            targetPosition = (Vector2)WorldControl.singleton.ActivePlayer.transform.position + new Vector2(WorldControl.singleton.RNG.Next(-2, 3) / 2f, WorldControl.singleton.RNG.Next(3) / 2);
        else
        {
            if (currentState == state.drifting)
                targetPosition = GetComponent<BotScript>().Next();
            else if (currentState == state.pattern)
                targetPosition = GetComponent<PatternMovement>().Next();
        }
    }

   public void Engage(Vector2 target)
    {
        CancelInvoke();
        currentState = state.engaged;
        GetComponent<Animator>().SetInteger("AnimID", 1);
        if (target.x - transform.parent.position.x > 0)
        {
            if(!flipped)
            {
                flipped = true;
                transform.parent.Rotate(new Vector3(0, 180, 0));
            }
            transform.parent.position = new Vector2(target.x + EngageOffset, target.y);
        }
        else
        {
            if (flipped)
            {
                flipped = false;
                transform.parent.Rotate(new Vector3(0, 180, 0));
            }
            transform.parent.position = new Vector2(target.x + EngageOffset * -1, target.y);
        }
    }
	
	// Update is called once per frame
	void Update () {
	if(currentState==state.drifting || currentState==state.pattern)
        {
            transform.parent.position=(Vector2)transform.parent.position+((targetPosition - (Vector2)transform.parent.position) * speed * Time.deltaTime);
        }
	}
}
