using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
    public Vector2[] Edges;
    public int[] EdgeIndex;
    public int[] RotationIndex;
	// Use this for initialization
	void Start () {
        while(EdgeIndex[0]==EdgeIndex[1])
        {
            EdgeIndex[0] = GardenControl.singleton.RNG.Next(Edges.Length);
            EdgeIndex[1] = GardenControl.singleton.RNG.Next(Edges.Length);
        }
        Instantiate(GardenControl.singleton.Triangle, (Vector2)transform.position+Edges[EdgeIndex[0]], Quaternion.Euler(0,0,RotationIndex[EdgeIndex[0]]*45));
        Instantiate(GardenControl.singleton.Triangle, (Vector2)transform.position + Edges[EdgeIndex[1]], Quaternion.Euler(0, 0, RotationIndex[EdgeIndex[1]] * 45));
    }

    public void HandleCross()
    {
        RaycastHit2D hitSelf= Physics2D.Raycast((Vector2)transform.position, Vector2.zero);
        RaycastHit2D hit0 = Physics2D.Raycast((Vector2)transform.position + Edges[EdgeIndex[0]] * 2, Vector2.zero);
        RaycastHit2D hit1 = Physics2D.Raycast((Vector2)transform.position + Edges[EdgeIndex[1]] * 2, Vector2.zero);
        if(hit0.collider != null & hit1.collider != null)
        {
            if (!hit0.collider.GetComponent<FlowerScript>().isNew && !hit1.collider.GetComponent<FlowerScript>().isNew)
            {
                if (hitSelf.collider != null)
                    hitSelf.collider.GetComponent<FlowerScript>().PickFlower = true;
                GardenControl.singleton.CrossFlowers(hit0.collider.GetComponent<FlowerScript>(), hit1.collider.GetComponent<FlowerScript>(), (Vector2)transform.position+new Vector2(100,100));
                hit0.collider.GetComponent<FlowerScript>().PickFlower = true;
                hit1.collider.GetComponent<FlowerScript>().PickFlower = true;
            }


        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
