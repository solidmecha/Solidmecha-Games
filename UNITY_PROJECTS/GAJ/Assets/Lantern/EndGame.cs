using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
	public GameObject button1;
	public GameObject button2;
	public GameObject recapText;
	Text recap;
	// Use this for initialization
	void Start () {
		recap = recapText.GetComponent<Text> ();
		recap.text="Game Over\n"+"Starting stats:\n"+SystemControl.startStats[0].ToString()+" HP\n"+SystemControl.startStats[1].ToString()+ " WP\n";
		recap.text+=SystemControl.startStats[2].ToString()+" St\n"+"Ending Stats:\n"+
			SystemControl.Stats[0].ToString()+" HP\n"+SystemControl.Stats[1].ToString()+ " WP\n";
		recap.text += SystemControl.Stats [2].ToString () + " St\n" + "Choices Made: " + SystemControl.choicesMade.ToString();
		recap.text += "\nYou have advanced the Lantern in the noble quest to bring light\n to the darkness, Will another continue its journey?";
		button1.GetComponent<Button>().onClick.AddListener(delegate{replay();});
		button2.GetComponent<Button>().onClick.AddListener(delegate{exit();});

	}

		void replay()
		{
		GameObject control = GameObject.Find ("Control");
		Destroy(control);
			Application.LoadLevel (0);
		}
		void exit()
		{
			Application.Quit();
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
