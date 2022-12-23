using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CardScript : MonoBehaviour {

    public PlayerScript Controlling_Player;
    public GameManager GM;
    public Sprite MachSprite;
    public List<Sprite> Production_Sprites; //Gears, steam, life, gems
    public bool Activated;
    public bool isMachanation;
    public bool isOnLine;
    public bool isInPlay;
    public List<int[]> Buying_Costs=new List<int[]> { };
    public List<int[]> Activation_Costs=new List<int[]> { }; // [0] for id, [1] for amount
    public Action playAction;
    public int[] Production = new int[2]; // [0] for id, [1] for amount
    public GameObject Cost_Display;
    public GameObject CardBack;
    public int ID;


    void OnMouseDown()
    {
        if (isOnLine)
        {
            if (GM.ActivePlayer.Resources[Buying_Costs[0][0]] >= Buying_Costs[0][1])
            {
                GM.RerollButton.transform.position = new Vector3(10, 10, -11);
                GM.ActivePlayer.Resources[Buying_Costs[0][0]] -= Buying_Costs[0][1];
                GM.ActivePlayer.Graveyard.Add(gameObject);
                transform.SetParent(null);
                transform.position = new Vector3(10, 10, -11);
                GM.UpdateUI();
                Destroy(Cost_Display);
                GM.AssemblyLine[ID] = null;
                Controlling_Player = GM.ActivePlayer;
                isOnLine = false;
            }
        }
        else
        {
            if(!isMachanation)
            {
                //production card play
                Controlling_Player.Hand.Remove(gameObject);
                Controlling_Player.Graveyard.Add(gameObject);
                playAction();
                GM.UpdateUI();
                gameObject.transform.localPosition = new Vector3(10, 10, -11);
                
            }

            else
            {
                //machine play
               if(!isInPlay)
                {
                    for(int i=0;i<5;i++)
                    {
                        if(Controlling_Player.MachinationOpen[i])
                        {
                            transform.position = GM.MachinationSpots[i].transform.position;
                            Controlling_Player.MachinationOpen[i] = false;
                            isInPlay = true;
                            GM.ActivePlayer.Hand.Remove(gameObject);
                            GM.ActivePlayer.Machinations[i] = gameObject;
                            break;
                        }
                    }
                }
               else
                {
                    if(!Activated && Controlling_Player.Resources[Activation_Costs[0][0]] >= Mathf.Abs(Activation_Costs[0][1]))
                    {
                        if (!(Activation_Costs[0][0]==2 && Controlling_Player.Resources[Activation_Costs[0][0]]== Mathf.Abs(Activation_Costs[0][1])))
                        {
                            Controlling_Player.Resources[Activation_Costs[0][0]] -= Mathf.Abs(Activation_Costs[0][1]);
                            playAction();
                            GM.UpdateUI();
                            Activated = true;
                            CardBack = Instantiate(GM.CardBack, transform.position, Quaternion.identity) as GameObject;
                            CardBack.transform.SetParent(transform);
                        }
                    }
                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        if (isMachanation)
        {
            GetComponent<SpriteRenderer>().sprite = MachSprite;
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Production_Sprites[Activation_Costs[0][0]];
            transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = Production_Sprites[Production[0]];
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Activation_Costs[0][1].ToString();
            if (Production[0] != 4)
                transform.GetChild(0).GetChild(3).GetComponent<Text>().text = Production[1].ToString();
            for (int i = 2; i < 6; i++)
            {
                if (i != 4)
                    Destroy(transform.GetChild(i).gameObject);
            }
        }
        else
        {
            //set production sprite
            GameObject go = transform.GetChild(5).gameObject;
            go.GetComponent<SpriteRenderer>().sprite = Production_Sprites[Production[0]];
            if (Production[0] != 4)
                transform.GetChild(0).GetChild(4).GetComponent<Text>().text = Production[1].ToString();
            for (int i = 4; i > 0; i--)
            { Destroy(transform.GetChild(i).gameObject); }
            for (int i = 3; i >= 0; i--)
                Destroy(transform.GetChild(0).GetChild(i).gameObject);
        }

        if(isOnLine)
        {
            Cost_Display = Instantiate(Cost_Display, new Vector2(transform.position.x, transform.position.y - 2), Quaternion.identity) as GameObject;
            Cost_Display.transform.SetParent(transform);
            Cost_Display.GetComponent<SpriteRenderer>().sprite = Production_Sprites[Buying_Costs[0][0]];
            Cost_Display.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Buying_Costs[0][1].ToString();
        }
    }

    public void Produce()
    {
        Controlling_Player.GM.Produce(Production);
    }

    public void Hit()
    {
        GM.HandView.transform.position = new Vector3(10, 10, -11);
        GM.PlayerListObj.transform.position = Vector3.zero;
        GM.UpdateUI();
    }
	// Update is called once per frame
	void Update () {
	
	}
}
