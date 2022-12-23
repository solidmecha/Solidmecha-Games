using UnityEngine;
using System.Collections.Generic;

public class AIscript : MonoBehaviour {

    public float RotSpeed;
    public float speed;
    public GameControl GC;
    List<Vector2> Directions=new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    public int dirIndex;
    int[] dirCheck;
    bool dirSet;
    float counter;

	// Use this for initialization
	void Start () {
	}

    Vector2 ObjectWorldVec()
    {
        Vector2 V = transform.position - GC.transform.position;
        V = new Vector2(Mathf.Round(V.x), Mathf.Round(V.y));
        return V;
    }

   public void SetDirection()
    {
        List<int> dirChoices = new List<int> { 0,1,2,3};
        if (dirIndex == 0 || dirIndex == 2)
        {
            dirChoices.RemoveAt(dirIndex + 1);
        }
        else
        {
            dirChoices.RemoveAt(dirIndex - 1);
        }

        for(int i=dirChoices.Count-1;i>=0;i--)
        {
            if (!(GC.World[(int)ObjectWorldVec().x + (int)Directions[dirChoices[i]].x][(int)ObjectWorldVec().y + (int)Directions[dirChoices[i]].y].GetComponent<SpriteRenderer>().color.Equals(Color.white)))
                dirChoices.RemoveAt(i);
        }
        dirIndex = dirChoices[GC.RNG.Next(dirChoices.Count)];
        dirCheck = new int[] { (int)Directions[dirIndex].x, (int)Directions[dirIndex].y };
        transform.GetChild(0).GetChild(0).GetChild(0).localPosition = 0.125f * Directions[dirIndex];
        transform.position = ObjectWorldVec() + (Vector2)GC.transform.position;
    }

    bool CheckIntersection()
    {
        foreach(int[] ia in GC.IntersectionList)
        {
            if (ia[0] == (int)ObjectWorldVec().x && ia[1] == (int)ObjectWorldVec().y)
                return true;
        }
        return false;
    }


    // Update is called once per frame
    void Update () {
        transform.GetChild(0).Rotate(new Vector3(0,0,RotSpeed*Time.deltaTime));
        transform.GetChild(0).GetChild(0).Rotate(new Vector3(0, 0, -1 * RotSpeed * Time.deltaTime));
        counter += Time.deltaTime;
        if(counter>=3.6f)
        {
            counter = 0;
            dirSet = false;
        }
        if (!dirSet && Vector2.Distance(transform.position, ObjectWorldVec() + (Vector2)GC.transform.position) < .15f && CheckIntersection())
        {
            dirSet = true;
            SetDirection();
        }
        if (GC.World[(int)ObjectWorldVec().x + dirCheck[0]][(int)ObjectWorldVec().y + dirCheck[1]].GetComponent<SpriteRenderer>().color.Equals(Color.white))
            transform.Translate(Directions[dirIndex] * speed * Time.deltaTime);
        else if (Vector2.Distance(transform.position, ObjectWorldVec() + (Vector2)GC.transform.position) > .15f)
            transform.Translate(Directions[dirIndex] * speed * Time.deltaTime);
        else
        {
            if(!CheckIntersection())
                dirSet = false;
            SetDirection();
        }
    }
}
