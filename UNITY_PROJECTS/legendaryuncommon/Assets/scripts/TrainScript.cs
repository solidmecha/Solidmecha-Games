using UnityEngine;
using System.Collections;

public class TrainScript : MonoBehaviour {

    public GameObject Outlines;
    public string tooltip;

    public void Train()
    {
        if (GameControl.singleton.silver >= 50)
        {
            GameControl.singleton.UpdateSilver(-50);
            GameControl.singleton.ShowPreviews = false;
            GameControl.singleton.CurrentState = GameControl.Gamestate.Train;
            foreach (FadeIn f in Outlines.GetComponentsInChildren<FadeIn>())
                f.fading = true;
            for (int i = 0; i < 4; i++)
                Outlines.transform.GetChild(i).GetComponent<TrainSelect>().tooltip = GameControl.singleton.VirtueNames[i];

            GameControl.singleton.PreviewMsg.text = tooltip;
        }
        else
            GameControl.singleton.PreviewMsg.text = "Not enough silver.";
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
