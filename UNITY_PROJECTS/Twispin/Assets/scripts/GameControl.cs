using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    System.Random RNG = new System.Random();
    float counter=1;
    int dir=-1;
    public GameObject CurrentSelected;
    public Color SelectedColor;
    public Material LineMat;
    public GameObject Rotator;
    public GameObject Arm;
    public Transform Root;
	// Use this for initialization
	void Start () {

        int depthCount = RNG.Next(1, 5);
        for(int i=0;i<depthCount; i++)
        {
            int c = RNG.Next(1, 4);
            AddChildRotator(Root);
            for (int j = 0; j < c; j++)
                AddChildRotator(Root.GetChild(i));
           for(int j=0;j<Root.GetChild(i).childCount; j++)
            {
                int innerc = RNG.Next(1, 4);
                for(int k=0;k<innerc;k++)
                    AddChildRotator(Root.GetChild(i).GetChild(j));
            }
        }
	
	}

    void AddChildRotator(Transform T)
    {
        GameObject go = Instantiate(Rotator, T) as GameObject;
        go.GetComponent<SpriteRenderer>().color = new Color(RNG.Next(100, 256) / 255f, RNG.Next(256) / 255f, RNG.Next(256) / 255f);
        go.transform.Rotate(new Vector3(0, 0, RNG.Next(360)));
        go.transform.Translate(transform.right * (float)RNG.Next(90, 300) / 300f);
        go.GetComponent<RotateIt>().speed = RNG.Next(3, 61);
        if (RNG.Next(2) == 1)
            go.GetComponent<RotateIt>().speed *= -1;
    }

    public void SetSelection(GameObject g)
    {
        CurrentSelected = g;
        SelectedColor = g.GetComponent<SpriteRenderer>().color;
    }

    public void ClearSelected()
    {
        CurrentSelected.GetComponent<SpriteRenderer>().color = SelectedColor;
        counter = 1;
        CurrentSelected = null;
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.touchCount == 2)
        {
            if (Time.timeScale > 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
        if(Input.touchCount==4)
        { Application.LoadLevel(0); }

        if(CurrentSelected!=null)
        {
            counter = counter+dir*Time.deltaTime;
            CurrentSelected.GetComponent<SpriteRenderer>().color = Color.Lerp(SelectedColor, Color.white, counter);
            if (counter >= 1)
                dir = -1;
            if (counter <= 0)
                dir = 1;
        }   
	
	}
}
