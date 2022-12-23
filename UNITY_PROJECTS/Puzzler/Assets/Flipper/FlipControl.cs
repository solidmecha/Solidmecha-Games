using UnityEngine;
using System.Collections;

public class FlipControl : MonoBehaviour {

    public int Flip_Behavior_ID;

    private void OnMouseDown()
    {
        Flip();
    }

    public void Flip()
    {
        FlipGameController FGC = GameObject.FindGameObjectWithTag("GameController").GetComponent<FlipGameController>();
        switch (Flip_Behavior_ID)
        {
            case 0:              
                for(int i=0;i<FGC.World.Length;i++)
                {
                    FGC.World[i][(int)transform.position.y].transform.Rotate(0, 180, 0);
                }
              break;
            case 1:
                for (int i = 0; i < FGC.World[(int)transform.position.x].Length; i++)
                {
                    FGC.World[(int)transform.root.position.x][i].transform.Rotate(0, 180, 0);
                }
                break;
            case 2:
                transform.root.Rotate(0, 180, 0);
                for (int i = 1; (int)transform.position.x + i < FGC.World.Length && (int)transform.position.y + i < FGC.World[(int)transform.position.x].Length; i++)
                {
                    FGC.World[(int)transform.position.x+i][(int)transform.position.y+i].transform.Rotate(0, 180, 0);
                }
                for (int i = 1; (int)transform.position.x - i>=0 && (int)transform.position.y + i<FGC.World[(int)transform.position.x].Length; i++)
                {
                    FGC.World[(int)transform.position.x - i][(int)transform.position.y + i].transform.Rotate(0, 180, 0);
                }
                for (int i = 1; (int)transform.position.x + i < FGC.World.Length && (int)transform.position.y - i>=0; i++)
                {
                    FGC.World[(int)transform.position.x + i][(int)transform.position.y - i].transform.Rotate(0, 180, 0);
                }
                for (int i = 1; (int)transform.position.x-i >=0 && (int)transform.position.y - i>=0; i++)
                {
                    FGC.World[(int)transform.position.x - i][(int)transform.position.y - i].transform.Rotate(0, 180, 0);
                }
                break;

        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
