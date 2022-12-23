using UnityEngine;
using System.Collections;

public class AnvilScript : MonoBehaviour {

    bool ready = true;
    float counter;
    int WeapIndex;
    bool playerInRange;
    public bool StormModeActive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
        else if(collision.CompareTag("Spikes"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // Use this for initialization
    void Start () {
        WeapIndex = WorldControl.singleton.RNG.Next(WorldControl.singleton.Weap.Length);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WorldControl.singleton.WeaponSprites[WeapIndex];
	}


    void WeaponGeneration()
    {
      WeaponScript ws= (Instantiate(WorldControl.singleton.Weap[WeapIndex], transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).GetComponent<WeaponScript>();
        ws.ID = WeapIndex;
        ws.WeapIndex = WorldControl.singleton.ActivePlayer.GetComponent<PlayerWeaponControl>().Weapons.Count;
    }
	
	// Update is called once per frame
	void Update () {
	if(!ready)
        {
            counter += Time.deltaTime;
            if(counter>5f)
            {
                ready = true;
                transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                counter = 0;
            }
        }
    if(playerInRange)
        {
            if (ready && Input.GetKey(KeyCode.Q))
            {
                if(StormModeActive)
                {
                    WeapIndex = WorldControl.singleton.RNG.Next(WorldControl.singleton.Weap.Length);
                    WeaponGeneration();
                }
                else if (WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().hasMana(20))
                {
                    if (WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().GemCount > WorldControl.singleton.ActivePlayer.GetComponent<PlayerWeaponControl>().Weapons.Count)
                    {
                        WeaponGeneration();
                        ready = false;
                        WorldControl.singleton.ActivePlayer.GetComponent<CharControl>().ChangeMana(20);
                        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        WorldControl.singleton.ShowMessage("Gather More Gems.", 3f);
                    }
                }
            }
            if (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.Tab))
            {
                WeapIndex = (WeapIndex + 1) % WorldControl.singleton.Weap.Length;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WorldControl.singleton.WeaponSprites[WeapIndex];
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                WeapIndex--;
                if (WeapIndex == -1)
                    WeapIndex = WorldControl.singleton.Weap.Length - 1;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WorldControl.singleton.WeaponSprites[WeapIndex];
            }
        }
	}
}
