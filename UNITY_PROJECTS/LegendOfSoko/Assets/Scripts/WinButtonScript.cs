using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinButtonScript : MonoBehaviour {

    public int NextLevelIndex;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(); });
	}

    void LoadLevel()
    {
        Application.LoadLevel(NextLevelIndex);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
