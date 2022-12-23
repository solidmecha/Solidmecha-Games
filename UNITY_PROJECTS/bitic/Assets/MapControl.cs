using UnityEngine;
using System.Collections;

public class MapControl : MonoBehaviour {

    public SpotScript Position;
    public bool[] Map;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish") && !collision.GetComponent<SpotScript>().Taken)
        {
            Position.SetTaken(false);
            Position = collision.GetComponent<SpotScript>();
            Position.SetTaken(true);
        }
    }

    public void SetMap()
    {
        for(int i=0;i<Map.Length;i++)
        {
            if (Map[i])
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
            else
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
    public void RandomMap()
    {
        
        Map = new bool[16];
        for (int i = 0; i < 16; i++)
            Map[i] = BitControl.singleton.RNG.Next(2) == 0;
        SetMap();
    }

    public void StartMap(int M)
    {
        Map = new bool[16];
        for(int i=0;i<16;i++)
        {
            Map[i] = BitControl.singleton.StartMaps(M)[i] == 1;
        }
        SetMap();
    }

    public void ShowWin()
    {
        Destroy(GetComponent<Collider2D>());
        gameObject.AddComponent<TransitionScript>();
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
