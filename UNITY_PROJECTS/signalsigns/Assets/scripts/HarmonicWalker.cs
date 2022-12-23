using UnityEngine;
using System.Collections;

public class HarmonicWalker : MonoBehaviour {

    public GameControl GC;
    public int Index;
    int PositionIndex;
    public float speed;
    Vector2 dir;
    float counter;
    public HarmonicDrawer HD;
	// Use this for initialization
	void Awake() {
    }

    public void Setup()
    {
        transform.position = GC.Shapes[Index][PositionIndex];
        dir = GC.Shapes[Index][PositionIndex + 1] - GC.Shapes[Index][PositionIndex];
    }
	
	// Update is called once per frame
	void Update () {
        if (GC.ShowSign)
        {
            counter += Time.deltaTime;
            if (counter >= 1f / speed)
            {
                PositionIndex += 2;
                if (PositionIndex >= GC.Shapes[Index].Count)
                    PositionIndex = 0;
                transform.position = GC.Shapes[Index][PositionIndex];
                dir = GC.Shapes[Index][PositionIndex + 1] - GC.Shapes[Index][PositionIndex];
                transform.position = (Vector2)transform.position + dir * speed * (counter - 1f / speed);
                counter = counter - 1f / speed;
                HD.StartNewLine();
            }
            else
            {
                transform.position = (Vector2)transform.position + dir * speed * Time.deltaTime;
            }
        }
	}
}
