using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public static GameControl singleton;
    public float Sensitivity;
    int SceneIndex;
    public bool NextSceneReady;
    public int SceneCount;
    public float[] LevelTimes;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
        Sensitivity = 1;
        Cursor.lockState = CursorLockMode.Locked;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
	
	}

    public void LoadNextScene()
    {
        NextSceneReady = false;
        SceneIndex++;
        if (SceneIndex == SceneCount)
            SceneIndex = 0;
        Application.LoadLevel(SceneIndex);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow) && Sensitivity>.1f)
        {
            Sensitivity -= .1f;
            Sensitivity = Mathf.Round(Sensitivity*10)/10f;
            PlayerScript.singleton.ShowMessage(Sensitivity.ToString(), 1f);
            
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) && Sensitivity<5.1f)
        {
            Sensitivity += .1f;
            PlayerScript.singleton.ShowMessage(Sensitivity.ToString(), 1f);
        }
        if(Input.GetKeyDown(KeyCode.P))
            Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(SceneIndex);
        }
	}
}
