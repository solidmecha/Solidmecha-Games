using UnityEngine;
using System.Collections.Generic;

public class SpinControl : MonoBehaviour {

    public GameObject tile;
    List<GameObject> TempGO = new List<GameObject> { };
    System.Random RNG = new System.Random(ThreadSafeRandom.Next());
    int counter;
    bool OnPath;

    // Use this for initialization
    void Start () {
        OnPath = true;
        for(int i=0; i<100;i++)
        {
            for(int j=0;j<100;j++)
            {
                Instantiate(tile, new Vector2(i, j)-new Vector2(50,50), Quaternion.identity);
            }
        }
        TempGO.Add(gameObject);
        TempGO.Add(transform.GetChild(0).gameObject);
        TempGO.Add(transform.GetChild(1).gameObject);
        TempGO.Add(transform.GetChild(2).gameObject);
 
	
	}

    void setRandPath()
    {
            if(RNG.Next(3)==2)
            {
                int r = RNG.Next(4);
                Transform T = transform.root;
                TempGO[r].transform.SetParent(null);
                T.SetParent(TempGO[r].transform);
            }
            if(RNG.Next(2)==0)
            {
                transform.root.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
            }
            else
                transform.root.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z - 90));
        if (counter > 1080)
            OnPath = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(OnPath)
        {
            counter++;
            setRandPath();
        }
	if(Input.GetKeyDown(KeyCode.W))
        {
            transform.root.position += Vector3.up;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.root.position += Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.root.position += Vector3.down;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.root.position += Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.root.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.root.rotation=Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z - 90));
        }
    }
}
