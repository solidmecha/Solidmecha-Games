using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScenarioManager : MonoBehaviour {
    //in case it's been a while set ss=to the method because it returns the correct Scenario script
	public GameObject MainDisplay;
	public Text MainDisplayText;
	public List<GameObject> Options=new List<GameObject>{};
    public List<Vector2> OptionsLocs = new List<Vector2> { };
	public List<ScenarioScript> ScenarioScriptList=new List<ScenarioScript>{};
	public string MainText;
	public GameObject Charcter;
	public GameObject ItemManagerObj;
	public CharacterScript CS;
	public ItemManager IM;
	public int ScenarioCount=5; //CHANGE THIS IN EDITOR!!!
    public GameObject item;
    public GameObject ShopLoc;
    public List<GameObject> shopInventory = new List<GameObject> { };

	// Use this for initialization
	void Start () {

		CS=(CharacterScript)Charcter.GetComponent(typeof(CharacterScript));
		IM=(ItemManager)ItemManagerObj.GetComponent(typeof(ItemManager));

       
        for(int i=0;i<Options.Count;i++)
        {
            OptionsLocs.Add(Options[i].transform.position);
        }
		
		MainDisplayText=MainDisplay.GetComponent<Text>();
        continueToCurse();
		showScenario();
	}

	public void showScenario()
	{
		MainDisplayText.text=MainText;
        int s = ScenarioScriptList.Count;
        for (int i=0;i<s;i++)
		{
            Options[i].transform.position = OptionsLocs[i];
			Options[i].transform.GetChild(0).gameObject.GetComponent<Text>().text=ScenarioScriptList[i].Prompt;
			ButtonScript b=(ButtonScript)Options[i].GetComponent(typeof(ButtonScript));
            b.IM = IM;
			b.removeListeners();
			b.skillMethod=selectChoice;
			b.id=i;
			b.setUpButton();
		}
            int o = Options.Count;
            for (int i = 1; i < o; i++)
            {
                if(s<=i)
                {
                    Options[i].transform.position = transform.position;
                }
            }
        

	}

	public void selectChoice(int id)
	{

        if(IM.DisplayBox != null)
        {
            Destroy(IM.DisplayBox);
        }
        for (int i = 0; i < 3; i++)
        {
            if (CS.Favor[i] + ScenarioScriptList[id].FavorResult[i] >0 && CS.Favor[i] + ScenarioScriptList[id].FavorResult[i] <= 100)
            CS.Favor[i] += ScenarioScriptList[id].FavorResult[i];
            else if (CS.Favor[i] + ScenarioScriptList[id].FavorResult[i] < 0)
            {
                CS.Favor[i] = 0;
            }
            else
                CS.Favor[i] = 100;
        }
        MainText =ScenarioScriptList[id].trueResultText;
		ScenarioScriptList[id].trueResult();
		showScenario();
	}

	public void setMainScenario()
	{
		ScenarioScriptList.Clear();
		MainText="What is next for you?";
		ScenarioScript ss=new ScenarioScript();
		ss.SM=this;
		ss=ss.exploreDungeon();
		ScenarioScript ss1=new ScenarioScript();
		ss1.SM=this;
		ss1=ss1.findSecret();
		ScenarioScriptList.Add(ss);
		ScenarioScriptList.Add(ss1);
	}

	public void setboulderScenario()
	{
		ScenarioScriptList.Clear();
		MainText="You see a large boulder blocking a doorway. Do you dare try to move it?";
		ScenarioScript ss=new ScenarioScript();
		ss.SM=this;
		ss=ss.moveBoulder();
        ScenarioScript ss1 = new ScenarioScript();
        ss1.SM = this;
        ss1 = ss1.mentalBoulder();
        ScenarioScriptList.Add(ss);
        ScenarioScriptList.Add(ss1);
    }

    public void setPendulum()
    {
        ScenarioScriptList.Clear();
        MainText = "Giant pendulums swing nearly scraping the path ahead";
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.faceTank();
        ScenarioScript ss1 = new ScenarioScript();
        ss1.SM = this;
        ss1 = ss1.withSkill();
        ScenarioScriptList.Add(ss);
        ScenarioScriptList.Add(ss1);
    }

    public void setShopKeeper()
    {
        ScenarioScriptList.Clear();
        MainText =  "\"Hello traveler,\" says the merchant, \"Would you like to see my wares?\"";
        ScenarioScript ss = new ScenarioScript();
        ScenarioScript ss1 = new ScenarioScript();
        ScenarioScript ss2 = new ScenarioScript();
        ss.SM = this;
        ss1.SM = this;
        ss2.SM = this;
        ss = ss.showConversation("Sure!", "Here they are:", Buy);
        ss1 = ss1.showConversation("I have something to sell", "What do you have for me?", Sell);
        ss2 = ss2.showConversation("Got any apple fritters!?", "Nah, we're out of apple fritters!", shopKeeperDiaglogue2);
        ScenarioScriptList.Add(ss);
    //    ScenarioScriptList.Add(ss1);
        ScenarioScriptList.Add(ss2);

    }

    public void Buy()
    {
      
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        int r = RNG.Next(4)+1;
        List<FoodScript> foodList = new List<FoodScript> { };
        List<ItemScript> itemList = new List<ItemScript> { };
        GameObject go;
        for (int i=0; i< r; i++)
        {
            if(RNG.Next(2)==0)
            {
                go=IM.cookFood(ShopLoc.transform.GetChild(i).position);
                FoodScript fs=(FoodScript)go.GetComponent(typeof(FoodScript));
                fs.inShop = true;
                foodList.Add(fs);
                shopInventory.Add(go);
            }
            else
            {
                go = IM.createRandomItem(ShopLoc.transform.GetChild(i).position);
                
              ItemScript its =  (ItemScript)go.GetComponent(typeof(ItemScript));
                its.inShop = true;
                itemList.Add(its);
                shopInventory.Add(go);
            }

        }
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Nothing for me today.", cleanUp);
        ScenarioScriptList.Add(ss);

    }

    public void cleanUp()
    {
        if(IM.DisplayBox!=null)
        Destroy(IM.DisplayBox);
        foreach(GameObject g in shopInventory)
        {
            Destroy(g);
        }
        shopInventory.Clear();
        setThankYou();   
    }

    public void setThankYou()
    {
        ScenarioScriptList.Clear();
        MainText = "Thank you, and have the day that you deserve. I do truly mean that. Oftentimes salesman will mindlessly wish one a good day insincerely, but I just let karma sort it out.";
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss=ss.showContinue("Ok, you too!", setMainScenario);
        ScenarioScriptList.Add(ss);
        showScenario();
    }

    public void Sell()
    {
        //Sell stuff here
    }

    public void shopKeeperDiaglogue2()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("Got any bear claws?", "Wait a minute...I'll go check", shopKeeperDiaglogue3);
        ScenarioScriptList.Add(ss);
    }
    public void shopKeeperDiaglogue3()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("Wait.", "No! we're out of bear claws!", shopKeeperDiaglogue4);
        ScenarioScriptList.Add(ss);
    }
    public void shopKeeperDiaglogue4()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("Well in that case what do you have?", "All I got right now is this box of one dozen starving, crazed weasels", shopKeeperDiaglogue5);
        ScenarioScriptList.Add(ss);
    }
    public void shopKeeperDiaglogue5()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("Ok I'll take that.", "Hey, there are weasels on your face", continueToInjury);
        ScenarioScript ss1 = new ScenarioScript();
        ss1.SM = this;
        ss1 = ss1.showConversation("I'll go back to where the towels are oh so fluffy!", "You head back.", continueToMain);
        ScenarioScriptList.Add(ss1);
        ScenarioScriptList.Add(ss);
    }

    public void setHealer()
    {
        MainText="You encounter a friendly monk.";
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("Can you remove a curse?","I can try to banish one. Is there one I should focus on?",setRemoveCurse);
        ScenarioScriptList.Add(ss);
        ScenarioScript ss1 = new ScenarioScript();
        ss1.SM = this;
        ss1 = ss1.showConversation("Medic!!!","I can fix one. Allow me to begin", setInjuryHeal);
        ScenarioScriptList.Add(ss1);
        ScenarioScript ss2 = new ScenarioScript();
        ss2.SM = this;
        //meditation and donation
        // ss2 = ss2.inquireAboutFavor();
        //  ScenarioScriptList.Add(ss2);
        ScenarioScript ss3 = new ScenarioScript();
        ss3.SM = this;
        ss3 = ss3.showContinue("Have a nice day.", setMainScenario);
    }

    public void setRemoveCurse()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("A specific one", "Choose it now", setRemoveCurse2);
        ScenarioScriptList.Add(ss);
        ScenarioScript ss1 = new ScenarioScript();
        ss1.SM = this;
        ss1 = ss1.showConversation("Any will do", "There you are. Thank you for your patronage", setRemoveCurse3);
        ScenarioScriptList.Add(ss1);
    }

    public void setRemoveCurse2()
    {
        IM.healing = true;
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss=ss.showContinue("Nevermind", setMainScenario);
        ScenarioScriptList.Add(ss);
    }

    public void setCurseRemoved()
    {
        ScenarioScriptList.Clear();
        float g = (float)CS.Gold * .5f;
        CS.Gold = (int)g;
        IM.updateUI();
        MainText = "Thank you for your patronage.";
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("You're welcome.", setMainScenario);
        ScenarioScriptList.Add(ss);
        showScenario();
    }

    public void setRemoveCurse3()
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        if (CS.curseList.Count>0)
        {
            int r=RNG.Next(CS.curseList.Count);
            IM.removeCurse(r);
        }
        float g = (float)CS.Gold * 0.75f;
        CS.Gold = (int)g;
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss=ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);

    }

    public void setInjuryHeal()
    {

        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("A specific one!","Okay, which one?", setInjuryHeal2);
        ScenarioScriptList.Add(ss);
        ScenarioScript ss1 = new ScenarioScript();
        ss1.SM = this;
        ss1 = ss1.showConversation("Any of them", "There you go. Thanks for remembering I don't work for free",healInjury3);
        ScenarioScriptList.Add(ss1);
    }

    public void setInjuryHeal2()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("Head","Okay, can't fix how you think though", healHead);
        ScenarioScriptList.Add(ss);
        ScenarioScript ss1 = new ScenarioScript();
        ss1.SM = this;
        ss1 = ss1.showConversation("Neck", "I've seen worse", healNeck);
        ScenarioScriptList.Add(ss1);
        ScenarioScript ss2 = new ScenarioScript();
        ss2.SM = this;
        ss2 = ss2.showConversation("Torso", "Okay, Good as new!", healTorso);
        ScenarioScriptList.Add(ss2);
        ScenarioScript ss3 = new ScenarioScript();
        ss3.SM = this;
        ss3 = ss3.showConversation("Feet", "I heal soles and souls", healFeet);
        ScenarioScriptList.Add(ss3);
        ScenarioScript ss4 = new ScenarioScript();
        ss4.SM = this;
        ss4 = ss4.showConversation("Right Arm", "Okay, good thing I could give you a hand", healRightArm);
        ScenarioScriptList.Add(ss4);
        ScenarioScript ss5 = new ScenarioScript();
        ss5.SM = this;
        ss5 = ss.showConversation("Left arm!", "Okay, this arm is all RIGHT now...hahaha", healLeftArm);
        ScenarioScriptList.Add(ss5);
    }
    public void healTorso()
    {
        ScenarioScriptList.Clear();
        IM.healInjury(0);
        float g = (float)CS.Gold * 0.5f;
        CS.Gold = (int)g;
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }
    public void healFeet()
    {
        ScenarioScriptList.Clear();
        IM.healInjury(1);
        float g = (float)CS.Gold * 0.5f;
        CS.Gold = (int)g;
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }
    public void healHead()
    {
        ScenarioScriptList.Clear();
        IM.healInjury(2);
        float g = (float)CS.Gold * 0.5f;
        CS.Gold = (int)g;
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }
    public void healLeftArm()
    {
        ScenarioScriptList.Clear();
        IM.healInjury(3);
        float g = (float)CS.Gold * 0.5f;
        CS.Gold = (int)g;
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }
    public void healRightArm()
    {
        ScenarioScriptList.Clear();
        IM.healInjury(4);
        float g = (float)CS.Gold * 0.5f;
        CS.Gold = (int)g;
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }
    public void healNeck()
    {
        ScenarioScriptList.Clear();
        IM.healInjury(5);
        float g = (float)CS.Gold * 0.5f;
        CS.Gold = (int)g;
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }
    public void healInjury3()
    {
        ScenarioScriptList.Clear();
        if (CS.Problems[0] > 0)
        {
            List<int> r = new List<int> { };
            for (int i = 0; i < 6; i++)
            {
                if (CS.injuryArrayBools[i])
                {
                    r.Add(i);
                }
            }
            System.Random RNG = new System.Random(ThreadSafeRandom.Next());
            IM.healInjury(r[RNG.Next(r.Count)]);
        }
        ScenarioScriptList.Clear();
        float g = (float)CS.Gold * 0.75f;
        CS.Gold = (int)g;
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }

    public void IDontKnow()
    {
        MainText = "I dont' know...";
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showConversation("Take everything too literally", "End up typing this into the computer program", gfLaughs);
        ScenarioScriptList.Add(ss);
    }
public void gfLaughs()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Girlfriend laughs", setMainScenario);
        ScenarioScriptList.Add(ss);
    }

    public void setItemFind()
	{
		ScenarioScriptList.Clear();
        if (!IM.justFound)
        {
            IM.createRandomItem(new Vector2(-2.8f, 1.2f));
            IM.justFound = true;
        }
		MainText="You found an Item!";
		ScenarioScript ss=new ScenarioScript();
		ss.SM=this;
		ss=ss.showContinue("Continue",setMainScenario);
		ScenarioScriptList.Add(ss);
    }

	public void nothingFound()
	{
		ScenarioScriptList.Clear();
		ScenarioScript ss=new ScenarioScript();
		ss.SM=this;
		ss=ss.showContinue("Continue", setMainScenario);
		ScenarioScriptList.Add(ss);
	}

    public void setInjury()
    {
        ScenarioScriptList.Clear();
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        if(RNG.Next(100)<CS.Stats[6])
        {
            MainText = "You avoided injury...this time";
        }
        else
        {
            MainText = "You were injured!";
            IM.Injury();
        }
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }

    public void setCurse()
    {
        ScenarioScriptList.Clear();
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        if (RNG.Next(100) < CS.Stats[7])
        {
            MainText = "You avoided being cursed...this time";
        }
        else
        {
            MainText = "You were cursed!";
            IM.Curse();
        }
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }

    public void setGoldFind()
    {
        ScenarioScriptList.Clear();
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        int r = RNG.Next(50, 500);
        CS.Pay(r*-1);
        MainText = "You found "+ r.ToString()+" gold!";
       
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }

	public void continueToInjury(){
        ScenarioScriptList.Clear();

        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setInjury);
        ScenarioScriptList.Add(ss);
        
    }
	public void continueToCurse()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setCurse);
        ScenarioScriptList.Add(ss);
    }
	public void continueToMain()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setMainScenario);
        ScenarioScriptList.Add(ss);
    }

	public void continueToItem()
	{
		ScenarioScriptList.Clear();
		ScenarioScript ss=new ScenarioScript();
		ss.SM=this;
		ss=ss.showContinue("Continue", setItemFind);
		ScenarioScriptList.Add(ss);
    }

    public void continueToGold()
    {
        ScenarioScriptList.Clear();
        ScenarioScript ss = new ScenarioScript();
        ss.SM = this;
        ss = ss.showContinue("Continue", setGoldFind);
        ScenarioScriptList.Add(ss);
    }

    //need generic methods for finding item, food, gold, getting cursed

    // Update is called once per frame
    void Update () {
	
	}
}
