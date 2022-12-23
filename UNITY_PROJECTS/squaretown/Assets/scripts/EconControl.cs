using UnityEngine;
using System.Collections.Generic;

public class EconControl : MonoBehaviour {

    public static EconControl singleton;
    public System.Random RNG;
    public int CompanyCount;
    public int InvestorCount;
    public int StartingCash = 100000000;
    public int Counter=1000;
    public List<CompanyInfo> Companies=new List<CompanyInfo> { };
    public List<InvestorInfo> Investors=new List<InvestorInfo> { };
    public int SharesBought=0;
    public int SharesSold=0;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        CreateCompanies();
        CreateInvestors();
        for(int i=0;i<Counter;i++)
        {
            UpdateInvestors();
            UpdatePrices();
        }
        ShowEndStats();
    }


    void CreateCompanies()
    {
        for(int i=0; i<CompanyCount;i++)
        {
            CompanyInfo C = new CompanyInfo(RNG.Next(100, 10000), RNG.Next(1, 6), RNG.Next(100, 501));
            Companies.Add(C);
        }
    }

    void UpdatePrices()
    {
        foreach (CompanyInfo C in Companies)
            C.UpdatePrice();

    }

    void UpdateInvestors()
    {
        foreach (InvestorInfo I in Investors)
            I.DetermineInvestorAction();
    }

    void CreateInvestors()
    {
        for(int i=0; i<InvestorCount;i++)
        {
            InvestorInfo I = new InvestorInfo(StartingCash);
            I.LongPositions = i ==0;
            I.SellChance = RNG.Next(5, 21);
            Investors.Add(I);
        }
    }

   public void PrintMessage(string M)
    {
        print(M);
    }

    void ShowEndStats()
    {
        print(SharesBought);
        print(SharesSold);

        foreach (InvestorInfo I in Investors)
        {
            I.SellAll();
            print(I.Money);
            if (I.Money > Investors[0].Money)
                print("Win");
            else
                print("Lose");
        }
        foreach (CompanyInfo C in Companies)
            print(C.Price);
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        /*
        Counter -= 1;
        if (Counter > 0)
        {
            UpdateInvestors();
            UpdatePrices();
        }
        else if(Counter==0)
        {
            ShowEndStats();
        }
        */
	}
}
