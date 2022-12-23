using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIstuff : MonoBehaviour {

public GameObject Sol;
public GameObject pointRef;
int planetNumber;


public GameObject[] planetCanvasArray=new GameObject[6];
public GameObject[] planetResourceArray=new GameObject[8];
Text[] planetResourceTextArray=new Text[8];

string[] sPopulations=new string[6];
string[] sAtk=new string[6];
string[] sDef=new string[6];
string[] sHappiness=new string[6];
string[] sEnergy=new string[6];
string[] sWealth=new string[6];
string[] sFoodWater=new string[6];
string[] sHealth=new string[6];
private float Xoffset,Yoffset;
private float fi; //fix it based off screen w/h
private float[] OtherOffsetsX=new float[7];
private float[] OtherOffsetsY=new float[7];
float Xpos, Ypos;
GameManager manageGame;



//channel the duck
void IDONTNEEDTHISOnGUI()
{
	GUI.Label(new Rect(Xpos, Ypos,50f,50f), sPopulations[planetNumber]);
	GUI.Label(new Rect(2.65f*Xpos, Ypos,50f,50f), sFoodWater[planetNumber]);
	GUI.Label(new Rect(Xpos, Ypos+.3f*Xoffset*fi,50f,50f), sEnergy[planetNumber]);
	GUI.Label(new Rect(2.65f*Xpos,Ypos+.3f*Xoffset*fi,50f,50f), sHealth[planetNumber]);
	GUI.Label(new Rect(Xpos+(Xoffset/5f)*fi, Ypos+1.4f*(.4f*Xoffset)*fi,50f,50f), sAtk[planetNumber]);
	GUI.Label(new Rect(2.65f*Xpos,Ypos+1.4f*(.4f*Xoffset)*fi,50f,50f), sDef[planetNumber]);
	GUI.Label(new Rect(Xpos, Ypos+2.2f*(.4f*Xoffset)*fi,50f,50f), sHappiness[planetNumber]);
	GUI.Label(new Rect(2.65f*Xpos, Ypos+2.2f*(.4f*Xoffset)*fi,50f,50f), sWealth[planetNumber]);
}
	// Use this for initialization
	void Start () {
		Xoffset=Screen.width/8f;
		Yoffset=Screen.height/13.5f;
		theresProbablyAnEasierWayButThisWorks();
		manageGame=(GameManager) Sol.GetComponent(typeof(GameManager));
		for(int i=0;i<8;i++)
		{
			planetResourceTextArray[i]=planetResourceArray[i].GetComponent<Text>();
		}
	}

	void theresProbablyAnEasierWayButThisWorks()
	{
		Debug.Log(Camera.main.WorldToViewportPoint(pointRef.transform.position));
		Debug.Log(Camera.main.WorldToScreenPoint(pointRef.transform.position));
		pointRef.transform.position=Camera.main.ViewportToWorldPoint(new Vector3(0,1,10)); //THIS IS WHERE THE DATA SHEET GETS MOVED TO UPPER LEFT CORNER
		fi=(Screen.width/Screen.height)/(1024/768);
		Xpos=Camera.main.WorldToViewportPoint(pointRef.transform.position).x+Xoffset;
		Ypos=Camera.main.WorldToViewportPoint(pointRef.transform.position).y+Yoffset;

	}
	
	// Update is called once per frame
	void Update () {
		sPopulations[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[7].ToString());
		sEnergy[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[6].ToString());
		sAtk[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[0].ToString());
		sHappiness[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[3].ToString());
		sFoodWater[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[4].ToString());
		sWealth[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[2].ToString());
		sDef[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[1].ToString());
		sHealth[planetNumber]=(manageGame.planetLookUp(planetNumber).Resources[5].ToString());

		planetResourceTextArray[0].text=sPopulations[planetNumber];
		planetResourceTextArray[1].text=sEnergy[planetNumber];
		planetResourceTextArray[2].text=sAtk[planetNumber];
		planetResourceTextArray[3].text=sHappiness[planetNumber];
		planetResourceTextArray[4].text=sFoodWater[planetNumber];
		planetResourceTextArray[5].text=sWealth[planetNumber];
		planetResourceTextArray[6].text=sDef[planetNumber];
		planetResourceTextArray[7].text=sHealth[planetNumber];
	}
}
