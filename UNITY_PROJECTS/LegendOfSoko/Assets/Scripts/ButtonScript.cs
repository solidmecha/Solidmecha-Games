using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(); });
	}

    void LoadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
