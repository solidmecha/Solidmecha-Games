using UnityEngine;
using System.Collections;

public class ObjectScript : MonoBehaviour {

    public GameObject[] objects;
    public Vector2[] Starts;
    public bool[] inUse;
    public int index;
    public float[] FXvalues;
    public bool ParentBaby;
    public bool BabyParent;
    public int ID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Baby"))
        {
            collision.GetComponent<BabyScript>().Room = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Baby"))
        {
            //collision.GetComponent<BabyScript>().Room = null;
        }
    }

    public void SetBaby(BabyScript b)
    {
        b.happyBaby = b.wants[b.wantIndex] == ID;
        b.RoomIndex = index;
        inUse[index] = true;
        b.transform.position = transform.GetChild(index).position;
        if(ParentBaby)
        {
            b.transform.SetParent(objects[index].transform);
            FXscript f = objects[index].AddComponent<FXscript>();
            f.bSpeed = FXvalues[0];
            f.bTime = FXvalues[1];
            f.rSpeed = FXvalues[2];
            f.rTime = FXvalues[3];
        }
        else if(BabyParent)
        {
            b.transform.SetParent(null);
            objects[index].transform.SetParent(b.transform);
            FXscript f = b.gameObject.AddComponent<FXscript>();
            f.bSpeed = FXvalues[0];
            f.bTime = FXvalues[1];
            f.rSpeed = FXvalues[2];
            f.rTime = FXvalues[3];
        }
        else
        {
            b.transform.SetParent(null);
            FXscript f = b.gameObject.AddComponent<FXscript>();
            f.bSpeed = FXvalues[0];
            f.bTime = FXvalues[1];
            f.rSpeed = FXvalues[2];
            f.rTime = FXvalues[3];
        }
        index = -1;
        for(int i=0;i<inUse.Length;i++)
        {
            if (!inUse[i])
                index = i;
        }

    }

    public void PickupBaby(BabyScript b)
    {

        inUse[b.RoomIndex] = false;
        if (BabyParent)
            objects[b.RoomIndex].transform.SetParent(null);
        objects[b.RoomIndex].transform.position = Starts[b.RoomIndex];
        objects[b.RoomIndex].transform.rotation = Quaternion.identity;
        if(ParentBaby)
        {
            Destroy(objects[b.RoomIndex].GetComponent<FXscript>());
        }
        else
        {
            Destroy(b.GetComponent<FXscript>());
        }

        if (index == -1)
        {
            for (int i = 0; i < inUse.Length; i++)
            {
                if (!inUse[i])
                    index = i;
            }
        }
    }

    // Use this for initialization
    void Start () {
        inUse = new bool[transform.childCount];
        Starts = new Vector2[objects.Length];
        for (int i = 0; i < objects.Length; i++)
            Starts[i] = objects[i].transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
