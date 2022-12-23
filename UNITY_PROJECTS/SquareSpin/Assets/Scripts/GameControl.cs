using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public List<GameObject> Levels=new List<GameObject> { };
    public List<GameObject> CurrSceneObjects=new List<GameObject> { };
    public int LevelIndex;
    public Button NextButton;

	// Use this for initialization
	void Start () {
        GameObject go=Instantiate(Levels[0], Vector2.zero, Quaternion.identity) as GameObject;
        CurrSceneObjects.Add(go);
        for(int i=0;i<go.transform.childCount;i++)
        {
            CurrSceneObjects.Add(go.transform.GetChild(i).gameObject);
        }
        NextButton.onClick.AddListener(delegate { NextLevel(); });
    }

    void NextLevel()
    {
        LevelIndex++;
        if (LevelIndex == Levels.Count)
            LevelIndex = 0;
        CleanUp();
        GameObject go = Instantiate(Levels[LevelIndex], Vector2.zero, Quaternion.identity) as GameObject;
        CurrSceneObjects.Add(go);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            CurrSceneObjects.Add(go.transform.GetChild(i).gameObject);
        }

    }

    void Restart()
    {
        CleanUp();
        GameObject go = Instantiate(Levels[LevelIndex], Vector2.zero, Quaternion.identity) as GameObject;
        CurrSceneObjects.Add(go);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            CurrSceneObjects.Add(go.transform.GetChild(i).gameObject);
        }
    }

    void CleanUp()
    {
        foreach (GameObject g in CurrSceneObjects)
            Destroy(g);
        CurrSceneObjects.Clear();
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
	}
}
