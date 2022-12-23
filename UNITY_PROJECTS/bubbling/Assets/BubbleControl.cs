using UnityEngine;
using System.Collections;

public class BubbleControl : MonoBehaviour {

    public Color[] Colors;
    public int[] LayerIDs;
    public GameObject[] Bubbles;

	// Use this for initialization
	void Start () {
        System.Random R = new System.Random();

        int Count = 13;
        int Count2 = 8;
        for(int i=0;i<Count;i++)
        {
            for (int j = 0; j < Count2; j++)
            {
                int index = R.Next(3);
                //Vector2 pos = new Vector2((float)R.Next(-95, 96) / 10f, (float)R.Next(-45, 46) / 10f);
                Vector2 pos = (Vector2)transform.position+ new Vector2(i,j)*1.25f;
                GameObject go = Instantiate(Bubbles[index], pos, Quaternion.identity) as GameObject;
                BubbleScript B = go.GetComponent<BubbleScript>();
                B.BaseChange = 2;
                B.isIdle = true;
                //B.deltaScale = new Vector2(B.BaseChange, B.BaseChange);
                int colorIndex = R.Next(7);
                go.GetComponent<SpriteRenderer>().color = Colors[colorIndex];
                go.layer = LayerIDs[colorIndex];
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
	}
}
