using UnityEngine;
using System.Collections.Generic;
using System;

public class ScenarioScript{
	public ScenarioManager SM;
	public string Prompt;
	public string[] ResultsText=new string[5];//crit fail, fail, neutral, pass, crit pass
	public Action[] Results=new Action[5];
	 public int[] ScoreThresholds=new int[4];
	 public List<int> SpecialCases=new List<int>{};
	 public Action trueResult;
	 public string trueResultText;
    public int[] FavorResult = new int[3] { 0, 0, 0 };
    System.Random RNG = new System.Random(ThreadSafeRandom.Next());

    public ScenarioScript(){}
	public ScenarioScript(string p, string[] rt, Action[] r, int[] st)
	{
		Prompt=p;
		for(int i=0;i<5;i++)
		{
			ResultsText[i]=rt[i];
			Results[i]=r[i];
			if(i<4)
			{
				ScoreThresholds[i]=st[i];
			}
		}
	}

    public void determineThresholds(int p, int s, int d)
    {
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        float primaryMod = (float)RNG.Next(1000, 2000) / 1000;
        float secondaryMod = (float)RNG.Next(500, 1000) / 1000;
        float detractMod = (float)RNG.Next(500, 1500) / 1000;
        if (s == 0)
        { secondaryMod = 0; }
        if (d == 0)
        { detractMod = 0; }
    
        float baseValue = primaryMod * p + secondaryMod * s - detractMod * d;
        //assume 6 items roll 1-10
        float lowComparison = primaryMod * RNG.Next(4, 14) + secondaryMod * RNG.Next(4, 14);
        float midComparison=primaryMod*RNG.Next(15,30)+ secondaryMod * RNG.Next(15, 30);
        float highComparison = primaryMod * RNG.Next(32, 50) + secondaryMod * RNG.Next(32, 50);

        if (baseValue < lowComparison)
        {
            ScoreThresholds[0] = RNG.Next(8, 20);
            ScoreThresholds[1] = RNG.Next(20, 60);
            ScoreThresholds[2] = RNG.Next(50, 85);
            ScoreThresholds[3] = RNG.Next(96, 99);
        }
        else if(baseValue< midComparison)
        {
            ScoreThresholds[0] = RNG.Next(5, 15);
            ScoreThresholds[1] = RNG.Next(20, 40);
            ScoreThresholds[2] = RNG.Next(40, 80);
            ScoreThresholds[3] = RNG.Next(94, 99);
        }
        else if (baseValue < highComparison)
        {
            ScoreThresholds[0] = RNG.Next(3, 11);
            ScoreThresholds[1] = RNG.Next(15, 36);
            ScoreThresholds[2] = RNG.Next(35, 70);
            ScoreThresholds[3] = RNG.Next(92, 99);
        }
        else
        {
            ScoreThresholds[0] = RNG.Next(0, 5);
            ScoreThresholds[1] = RNG.Next(10, 30);
            ScoreThresholds[2] = RNG.Next(30, 60);
            ScoreThresholds[3] = RNG.Next(90, 99);
        }

    }

	public void determineResult() //sets trueResult and trueResultText
	{
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		int r = RNG.Next(100);
            if (r<=ScoreThresholds[0])
			{
			trueResult=Results[0];
			trueResultText=ResultsText[0];
			}
			else if(r<=ScoreThresholds[1])
			{
			trueResult=Results[1];
			trueResultText=ResultsText[1];
			}
			else if(r<=ScoreThresholds[2])
			{
			trueResult=Results[2];
			trueResultText=ResultsText[2];
			}
			else if(r<=ScoreThresholds[3])
			{
			trueResult=Results[3];
			trueResultText=ResultsText[3];
			}
			else
			{
			trueResult=Results[4];
			trueResultText=ResultsText[4];
			}
		}
	
	public ScenarioScript exploreDungeon()
	{
		ScenarioScript ss=new ScenarioScript();
		ss.Prompt="Explore!";
		ss.SM=SM;
        int r = RNG.Next(SM.ScenarioCount);
		
		if(RNG.Next(100)<SM.CS.Stats[11])
		{
			ss.trueResult=SM.setItemFind;
		}
		else
		{
		switch(r)
		{
		case 0:
                    ss.trueResult = SM.setHealer;
                    break;
        case 1:
                    ss.trueResult = SM.setShopKeeper;
                    break;
                case 2:
                    ss.trueResult = SM.setboulderScenario;
                    break;
                case 3:
                    ss.trueResult = SM.setboulderScenario;
                    break;
		default:
                    ss.trueResult = SM.setPendulum;
                    break;
		}
		}
		return ss;
	}

	public ScenarioScript findSecret()
	{
		ScenarioScript ss=new ScenarioScript();
		ss.Prompt="Search for a Secret.";
		ss.SM=this.SM;
		System.Random RNG=new System.Random(ThreadSafeRandom.Next());
		if(RNG.Next(100)<SM.CS.Stats[9])
		{
			ss.trueResult=SM.setItemFind;
		}

		else
		{
			ss.trueResultText="Nothing Found";
			ss.trueResult=SM.nothingFound;
		}

		return ss;

	}

//set Boulder Scenario
	public ScenarioScript moveBoulder()
	{
		string text="Of Course!";
		string[] resultTexts=new string[5];
		 resultTexts[0]="The mass lifts but comes right back down on you...";
		 resultTexts[1]="Like Sisyphus before your struggle is futile and the rock rolls back into place";
		 resultTexts[2]="It will not budge.";
		 resultTexts[3]="Straining all your tendons you manage to clear the path";
		 resultTexts[4]="You don't bother moving it. You pass right though it and it crumbles with ease";
		Action[] resultActions=new Action[5];
		resultActions[0]=SM.continueToInjury;
		resultActions[1]=SM.continueToCurse;
		resultActions[2]=SM.continueToMain;
		resultActions[3]=SM.continueToItem;
		resultActions[4]=SM.continueToItem;
		determineThresholds(SM.CS.Stats[2],0,0);
        ScenarioScript ss=new ScenarioScript(text, resultTexts, resultActions, ScoreThresholds);
        ss.SM = SM;
        ss.FavorResult[0] =-2;
        ss.FavorResult[1] =-2;
        ss.FavorResult[2] =1;
        ss.determineResult();
		return ss;
	}

    public ScenarioScript mentalBoulder()
    {
        string text = "Move it with your mind!";
        string[] resultTexts = new string[5];
        resultTexts[0] = "Your head hurts.";
        resultTexts[1] = "The boulder moves, but only in your imagination";
        resultTexts[2] = "You stare at it and nothing really happens.";
        resultTexts[3] = "With some intense concentration, it levitates and is cast aside";
        resultTexts[4] = "On second thought, what boulder?";
        Action[] resultActions = new Action[5];
        resultActions[0] = SM.continueToCurse;
        resultActions[1] = SM.continueToMain;
        resultActions[2] = SM.continueToMain;
        resultActions[3] = SM.continueToItem;
        resultActions[4] = SM.continueToItem;
        determineThresholds(SM.CS.Stats[1], 0, 0);
        ScenarioScript ss = new ScenarioScript(text, resultTexts, resultActions, ScoreThresholds);
        ss.SM = SM;
        ss.FavorResult[0] = -1;
        ss.FavorResult[1] = 2;
        ss.FavorResult[2] = -3;
        ss.determineResult();
        return ss;
    }


    //pendulum
    public ScenarioScript faceTank()
    {
        string text = "Face-tank it.";
        string[] resultTexts = new string[5];
        resultTexts[0] = "BAM! Ouch, why did you do that?";
        resultTexts[1] = "You'll regret this in the morning.";
        resultTexts[2] = "After a few blows you're forced to turn back.";
        resultTexts[3] = "You manage to make it to the other side";
        resultTexts[4] = "You barely notice the tons of weight crashing into you during your leisurely stroll.";
        Action[] resultActions = new Action[5];
        resultActions[0] = SM.continueToInjury;
        resultActions[1] = SM.continueToInjury;
        resultActions[2] = SM.continueToMain;
        resultActions[3] = SM.continueToGold;
        resultActions[4] = SM.continueToItem;
        determineThresholds(SM.CS.Stats[2], 0, SM.CS.Stats[1]);
        ScenarioScript ss = new ScenarioScript(text, resultTexts, resultActions, ScoreThresholds);
        ss.SM = SM;
        ss.FavorResult[0] = 0;
        ss.FavorResult[1] = -5;
        ss.FavorResult[2] = 5;
        ss.determineResult();
        return ss;
    }

    public ScenarioScript withSkill()
    {
        string text = "Cross with finesse!";
        string[] resultTexts = new string[5];
        resultTexts[0] = "That was going well til the 1 ton bob flattened you";
        resultTexts[1] = "You parry back and forth. Mostly back though to where you started";
        resultTexts[2] = "Made it! OOOH Shiny!";
        resultTexts[3] = "Made it! OOOH Shiny!";
        resultTexts[4] = "You make it look easy.";
        Action[] resultActions = new Action[5];
        resultActions[0] = SM.continueToInjury;
        resultActions[1] = SM.continueToMain;
        resultActions[2] = SM.continueToGold;
        resultActions[3] = SM.continueToGold;
        resultActions[4] = SM.continueToItem;
        determineThresholds(SM.CS.Stats[0], 0, SM.CS.Stats[2]);
        ScenarioScript ss = new ScenarioScript(text, resultTexts, resultActions, ScoreThresholds);
        ss.SM = SM;
        ss.FavorResult[0] = 3;
        ss.FavorResult[1] = -1;
        ss.FavorResult[2] = -2;
        ss.determineResult();
        return ss;
    }



    public ScenarioScript showConversation(string s, string r, Action a)
    {
        ScenarioScript ss = new ScenarioScript();
        ss.Prompt = s;
        ss.trueResultText = r;
        ss.trueResult = a;
        return ss;

    }

    public ScenarioScript showContinue(string s, Action a)
	{
		ScenarioScript ss=new ScenarioScript();
		ss.Prompt=s;
		ss.trueResultText="";
		ss.trueResult=a;
		return ss;

	} 
	
}
