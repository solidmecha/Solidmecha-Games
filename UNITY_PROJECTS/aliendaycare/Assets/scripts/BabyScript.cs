using UnityEngine;
using System.Collections.Generic;

public class BabyScript : MonoBehaviour {
    public float counter;
    public float timer;
    public float mood;
   public int[] wants;
  public  int wantIndex;
    public float WantCounter;
    public ObjectScript Room;
    public int RoomIndex;
    public bool happyBaby;
    int faceIndex;

    // Use this for initialization
    void Start() {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = DaycareControl.singleton.ClothesColors[DaycareControl.singleton.RNG.Next(DaycareControl.singleton.ClothesColors.Length)];
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = DaycareControl.singleton.ClothesColors[DaycareControl.singleton.RNG.Next(DaycareControl.singleton.ClothesColors.Length)];
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = DaycareControl.singleton.ClothesColors[DaycareControl.singleton.RNG.Next(DaycareControl.singleton.ClothesColors.Length)];
        showWant();
        faceIndex = 1;
    }


   public void addWants()
    {
        List<int> L = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        for (int i = 0; i < wants.Length - 1; i++)
        {
            int a = DaycareControl.singleton.RNG.Next(L.Count);
            wants[i] = L[a];
            L.RemoveAt(a);
        }
        wants[wants.Length - 1] = 11;
    }

    void showThought(Sprite S, float time)
    {
        transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.white;
        transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite = S;
        transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        Invoke("clearThought", time);
    }

    void clearThought()
    {
        transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.clear;
        transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
    }

    void showWant()
    {
        showThought(DaycareControl.singleton.ObjectSprites[wants[wantIndex]], 2.5f);
    }

    void NewFace(int r)
    {
        GetComponent<SpriteRenderer>().sprite = DaycareControl.singleton.BabySprites[r];
    }
	
	// Update is called once per frame
	void Update () {
        if (happyBaby)
        {
            mood += 2*Time.deltaTime;
            if (mood < 45)
                mood = 45;
            DaycareControl.singleton.Score+=5;
        }
        else
        {
            WantCounter -= Time.deltaTime;
            DaycareControl.singleton.Score--;
            mood -= Time.deltaTime;
            if (WantCounter <= 0)
            {
                WantCounter = 10.5f;
                showWant();
            }
        }

        if (mood >= 80)
        {
            if (happyBaby)
            {
                wantIndex++;
                showWant();
                happyBaby = false;
            }
            if (faceIndex != 0)
            {
                showThought(DaycareControl.singleton.Faces[0], 1f);
                faceIndex = 0;
                NewFace(0);
            }
        }
        else if (mood<70&& mood >= 45 && faceIndex !=1)
        {
            showThought(DaycareControl.singleton.Faces[1], 1f);
            faceIndex = 1;
            NewFace(1);
        }
        else if (mood<45 && mood >= 25 && faceIndex != 2)
        {
            showThought(DaycareControl.singleton.Faces[2], 1f);
            faceIndex = 2;
            NewFace(2);
        }
        else if(mood<25 && faceIndex !=3)
        {
            showThought(DaycareControl.singleton.Faces[3], 1f);
            faceIndex = 3;
            NewFace(3);
        }
        if (mood < 0)
            mood = 0;
     
	}
}
