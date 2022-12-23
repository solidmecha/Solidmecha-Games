using UnityEngine;
using System.Collections.Generic;

public class InvestorInfo{

    public int Money;
    public int SellChance;
    List<ShareInfo> Portfolio = new List<ShareInfo> { };
    public bool LongPositions=false;

    public struct ShareInfo
     {
       public int CompanyIndex;
       public int SharesCount;
     }

	// Use this for initialization
	void Start () {
	
	}

    public InvestorInfo( int M)
    {
        Money = M;
    }

    void BuyStock(ShareInfo Share)
    {
      //  EconControl.singleton.PrintMessage("Shares Bought: " + Share.SharesCount.ToString() + " at " + EconControl.singleton.Companies[Share.CompanyIndex].Price.ToString());
        EconControl.singleton.SharesBought++;
        Portfolio.Add(Share);
        EconControl.singleton.Companies[Share.CompanyIndex].AvailableShares -= Share.SharesCount;
        Money -= Share.SharesCount*EconControl.singleton.Companies[Share.CompanyIndex].Price;
    }

    void SellStock(ShareInfo Share)
    {
        EconControl.singleton.SharesSold++;
        Portfolio.Remove(Share);
        EconControl.singleton.Companies[Share.CompanyIndex].AvailableShares += Share.SharesCount;
        Money += Share.SharesCount * EconControl.singleton.Companies[Share.CompanyIndex].Price;
    }

    public void SellAll()
    {
        EconControl.singleton.PrintMessage("Shares: " + Portfolio.Count);
        while (Portfolio.Count>0)
        {
            SellStock(Portfolio[0]);
        }
    }

   public void DetermineInvestorAction()
    {
        if (LongPositions && Portfolio.Count == 0)
        {
            for (int i = 0; i < EconControl.singleton.Companies.Count; i++)
            {
                ShareInfo S = new ShareInfo();
                S.CompanyIndex = i;
                S.SharesCount = 1;
                BuyStock(S);
            }
        }
        else if(!LongPositions)
        {
            int R = EconControl.singleton.RNG.Next(EconControl.singleton.CompanyCount);
            if (EconControl.singleton.Companies[R].AvailableShares > 0)
            {
                int C = EconControl.singleton.RNG.Next(1, 10);
                if (Money >= EconControl.singleton.Companies[R].Price * C)
                {
                    ShareInfo S = new ShareInfo();
                    S.CompanyIndex = R;
                    S.SharesCount = C;
                    BuyStock(S);
                }
            }
            if (EconControl.singleton.RNG.Next(SellChance) == 4 && Portfolio.Count > 0)
            {
                SellStock(Portfolio[EconControl.singleton.RNG.Next(Portfolio.Count)]);
            }
        }
      
    }
	
	// Update is called once per frame
	void Update () {
	}
}
