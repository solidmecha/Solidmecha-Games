using UnityEngine;
using UnityEngine.UI;

public class TileScript : MonoBehaviour {

    public int[] Chances = new int[4];//bolt, earth, fire, water
    public int rewardIndex;
    public GameControl GC;

    void OnMouseDown()
    {
        GC.CurrentSpell.CastSpell(transform.position);
    }

	// Use this for initialization
	void Start () {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GC.Icons[rewardIndex];
        if(rewardIndex>1)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
