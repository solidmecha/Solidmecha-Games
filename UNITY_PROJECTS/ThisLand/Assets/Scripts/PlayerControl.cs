using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    public float speed;

    public GameObject ControlOBJ;

    GameControl GC;

    public List<GameObject> Buildings = new List<GameObject> { }; //farm, grain, castle, rax, house, mill, temple, barn

    public List<int[]> BuildingCosts = new List<int[]> { };

	// Use this for initialization
	void Start () {
        //cost in coin, stone, wood
        int[] FarmCost = new int[3] { 0,3,6};
        int[] grainCost = new int[3] { 2,0,2};
        int[] castleCost = new int[3] { 9, 9, 0};
        int[] raxCost = new int[3] {4,4,1};
        int[] houseCost = new int[3] { 2, 0, 5 };
        int[] millCost = new int[3] {0, 2,6};
        int[] templeCost = new int[3] { 3, 4, 3 };
        int[] barnCost = new int[3] { 7, 0, 7};
        BuildingCosts.Add(FarmCost);
        BuildingCosts.Add(grainCost);
        BuildingCosts.Add(castleCost);
        BuildingCosts.Add(raxCost);
        BuildingCosts.Add(houseCost);
        BuildingCosts.Add(millCost);
        BuildingCosts.Add(templeCost);
        BuildingCosts.Add(barnCost);
        GC = (GameControl)ControlOBJ.GetComponent(typeof(GameControl));
	}

    void BuildBuilding(int B)
    {   
        Vector2 Pos=transform.position - ControlOBJ.transform.position;
        Pos = new Vector2(Mathf.Round(Pos.x), Mathf.Round(Pos.y));
        if (Pos.x >= 0 && Pos.y >= 0 && Pos.x < GC.width && Pos.y < GC.height)
        {
            if (GC.World[(int)Pos.x][(int)Pos.y].id<6 && GC.World[(int)Pos.x][(int)Pos.y].NaturalRes.Count == 0 && costCheck(B) && GC.World[(int)Pos.x][(int)Pos.y].Building == null)
            {
                GC.ResourceAmount[0] -= BuildingCosts[B][0];
                GC.ResourceAmount[2] -= BuildingCosts[B][1];
                GC.ResourceAmount[3] -= BuildingCosts[B][2];
                GC.World[(int)Pos.x][(int)Pos.y].Building = Instantiate(Buildings[B], Pos + (Vector2)ControlOBJ.transform.position, Quaternion.identity) as GameObject;
                GC.World[(int)Pos.x][(int)Pos.y].Building.transform.SetParent(GC.World[(int)Pos.x][(int)Pos.y].transform);
                GC.UpdateGUI();
            }
        }
    }
	
    bool costCheck(int B)
    {
        if (GC.ResourceAmount[0] >= BuildingCosts[B][0] && GC.ResourceAmount[2] >= BuildingCosts[B][1] && GC.ResourceAmount[3] >= BuildingCosts[B][2])
            return true;
        else
            return false;
    }
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.W))
        { transform.Translate(Vector2.up * speed * Time.deltaTime); }

        if (Input.GetKey(KeyCode.A))
        { transform.Translate(Vector2.left * speed * Time.deltaTime); }

        if (Input.GetKey(KeyCode.S))
        { transform.Translate(Vector2.down * speed * Time.deltaTime); }

        if (Input.GetKey(KeyCode.D))
        { transform.Translate(Vector2.right * speed * Time.deltaTime); }

        if (Input.GetKeyDown(KeyCode.F))
        { BuildBuilding(0); }

        if (Input.GetKeyDown(KeyCode.G))
        { BuildBuilding(1); }

        if (Input.GetKeyDown(KeyCode.C))
        { BuildBuilding(2);}

        if (Input.GetKeyDown(KeyCode.R))
        { BuildBuilding(3); }

        if (Input.GetKeyDown(KeyCode.H))
        { BuildBuilding(4); }

        if (Input.GetKeyDown(KeyCode.M))
        { BuildBuilding(5); }

        if (Input.GetKeyDown(KeyCode.T))
        { BuildBuilding(6); }

        if (Input.GetKeyDown(KeyCode.B))
        { BuildBuilding(7); }

    }
}
