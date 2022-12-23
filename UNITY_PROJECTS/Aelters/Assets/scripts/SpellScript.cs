using UnityEngine;
using System.Collections.Generic;

public class SpellScript : MonoBehaviour {

    public int SpellIndex;
    public List<Vector2> AOE;
    public GameObject Spell;
    public GameControl GC;

    public void CastSpell(Vector2 Origin)
    {
        foreach(Vector2 v in AOE)
        {
            RaycastHit2D hit = Physics2D.Raycast(Origin + v, Vector2.zero);
            if (hit)
            {
                Destroy(hit.collider);
                Instantiate(Spell, Origin + v,Quaternion.identity);
                TileScript ts = hit.collider.gameObject.GetComponent<TileScript>();
                if(GC.RNG.Next(100)<ts.Chances[SpellIndex])
                {
                  GC.SpellBook.Add(GC.GenSpell(ts.rewardIndex, 0));
                    Instantiate(GC.Reward, Origin + v, Quaternion.identity);
                }
                else
                {
                    Instantiate(GC.Curse, Origin + v, Quaternion.identity);
                }
            }

        }
        GC.SpellBook.RemoveAt(GC.SpellIndex);
        GC.UpdateSpell();
        Destroy(gameObject);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
