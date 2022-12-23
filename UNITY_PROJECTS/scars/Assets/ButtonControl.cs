using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour {
    public int ID;
    // Use this for initialization
    void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { GameObject.FindGameObjectWithTag("GameController").GetComponent<ProbLifeControl>().HandleButtonClick(ID); });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
