using UnityEngine;
using System.Collections;

public class GateControl : MonoBehaviour {

    public BitControl.Operation[] Operations;
    public Sprite[] Sprites;
    public int Index;
    public Transform[] Inputs;
    public GameObject Map;
    public Vector2 OutputPos;
    public SpotScript OutputSpot;

    public void NextGate(int i)
    {
        Index += i;
        if (Index == Operations.Length)
            Index = 0;
        GetComponent<SpriteRenderer>().sprite = Sprites[Index];
    }

    public void ResolveOperation()
    {
        MapControl Map_A=null;
        MapControl Map_B=null;
        RaycastHit2D hit = Physics2D.Raycast(OutputPos, Vector2.zero);
        RaycastHit2D hita = Physics2D.Raycast(Inputs[0].position, Vector2.zero);
        RaycastHit2D hitb = Physics2D.Raycast(Inputs[1].position, Vector2.zero);
        if (hita.collider.CompareTag("Map"))
            Map_A = hita.collider.GetComponent<MapControl>();
        if (hitb.collider.CompareTag("Map"))
            Map_B = hitb.collider.GetComponent<MapControl>();
        if (hit.collider == null && Map_A != null && Map_B != null)
        {
            GameObject go = Instantiate(Map, OutputPos, Quaternion.identity) as GameObject;
            MapControl output = go.GetComponent<MapControl>();
            output.Map = new bool[Map_A.Map.Length];
            if ((int)Operations[Index] <= 6)
            {
                for (int i = 0; i < Map_A.Map.Length; i++)
                    output.Map[i] = BitControl.singleton.Combine(Operations[Index], Map_A.Map[i], Map_B.Map[i]);
            }
            else
            {
                bool[] tmp = new bool[16];
                for (int i = 0; i < 16; i++)
                    tmp[i] = Map_A.Map[i];
                for (int i = 0; i < 16; i++)
                    output.Map[i] = tmp[BitControl.singleton.NewIndicies(Operations[Index])[i]];
            }
            output.SetMap();
            output.Position = OutputSpot;
            if (CheckWin(output.Map))
                output.ShowWin();
        }
    }

    public bool CheckWin(bool[] M)
    {
        bool WonRound = true;
        for(int i=0;i<16;i++)
        {
            if (M[i] != PlayerControl.singleton.Target.Map[i])
            {
                WonRound = false;
                break;
            }
        }
        return WonRound;
            
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 
	}
}
