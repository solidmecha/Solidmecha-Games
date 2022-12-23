using UnityEngine;
using System.Collections;
using System;

public class CardControl : MonoBehaviour {

    public enum Type { Econ, Swap, Goal, TimeChanger, Auto, CardGen, EconStart};
    public Type CardType;
    public Action Activate;
    public Action Deactivate;
    public int CoinIndex;
    public int Value;
    public int Cost;
    public int CostIndex;
    public GameControl GC;

    private void OnMouseDown()
    {
        MouseClick();
    }

    public void MouseClick()
    {
        if (!GC.won)
        {
            GC.UpdateClick();
            GetComponent<SpriteRenderer>().color = Color.green;
            if (IsInvoking())
                CancelInvoke();
            Invoke("ChangeColorBack", 0.2f);
            GetComponent<TimeScript>().counter += GC.TimeChange;
            if (GC.CoinAmounts[CostIndex] >= Cost)
            {
                GC.GainCoins(CostIndex, -1 * Cost);
                if (Activate != null)
                    Activate();
                if (GC.FlipBack)
                {
                    GC.TimeChange -= GC.LastTimeChange;
                    GC.FlipBack = false;
                }
            }
        }
    }

    public void ChangeColorBack()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void GainCoins()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().GainCoins(CoinIndex, Value);
    }

    void GainCoinsOnce()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().GainCoins(CoinIndex, Value);
        SwapCard();
    }

    void Victory()
    {
        StartCoroutine(Win());
    }

    IEnumerator Win()
    {
        yield return new WaitForEndOfFrame();
        foreach (CardControl cc in transform.parent.GetComponentsInChildren<CardControl>())
        {
            cc.ChangeColorBack();
            Destroy(cc.GetComponent<BoxCollider2D>());
        }
        GetComponent<SpriteRenderer>().color = Color.green;
        GC.ClickCountText.text = "Won in " + GC.ClickCount.ToString();
        GC.won = true;
        Time.timeScale = 0;
    }

    void ChangeTime()
    {
        GameControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GC.TimeChange -= Value;
        if(CardType.Equals(Type.TimeChanger))
        {
            GC.FlipBack = true;
            GC.LastTimeChange = Value;
        }
    }

    void UnChangeTime()
    {
        GC.TimeChange += Value;
    }

    void NewCard()
    {
        if (GC.CardCount < 14)
        {
            GC.CardCount++;
            if (GC.CardCount == 14)
                GC.Cards.RemoveAt(4);
            GC.GenerateCard(GC.CardPos.transform.GetChild(GC.CardCount-1).position);
        }
    }

    void Void()
    {

    }

    private void OnDestroy()
    {
        Deactivate();
    }

    // Use this for initialization
    void Start () {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        GetComponent<TimeScript>().counter = GC.RNG.Next(5, 16);
        switch (CardType)
        {
            case Type.Econ:
                Activate = GainCoins;
                Cost = 0;            
                int r= GC.RNG.Next(3);
                CoinIndex = r;
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.ColorIndex[r];
                Value = GC.RNG.Next(-20, 51);
                transform.GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = Value.ToString();
                Deactivate = Void;          
                break;
            case Type.Auto:
                 r = GC.RNG.Next(1,4);
                GetComponent<AutoClick>().delay = r;
                transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = r.ToString();
                Activate = Void;
                Deactivate = Void;
                break;
            case Type.Goal:
                Activate = Victory;
                Cost = GC.RNG.Next(1500,3001);
                 r = GC.RNG.Next(3);
                CostIndex = r;
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.ColorIndex[r];
                transform.GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = Cost.ToString();
                Deactivate = Void;
                break;
            case Type.Swap:                
                r = GC.RNG.Next(3);
                CoinIndex = r;
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.ColorIndex[r];
                Value = GC.RNG.Next(11, 51);
                transform.GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = Value.ToString();
                Deactivate = Void;
                Cost = GC.RNG.Next(1,21);
                int c = GC.RNG.Next(3);
                while(c==r)
                    c = GC.RNG.Next(3);
                CostIndex = c;
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = GC.ColorIndex[c];
                transform.GetChild(1).GetChild(2).GetComponent<UnityEngine.UI.Text>().text = Cost.ToString();
                Activate = GainCoins;
                break;
            case Type.CardGen:
                Activate = NewCard;
                Cost = GC.RNG.Next(250, 601);
                r = GC.RNG.Next(3);
                CostIndex = r;
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.ColorIndex[r];
                transform.GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = Cost.ToString();
                Deactivate = Void;
                break;
            case Type.TimeChanger:
                r = GC.RNG.Next(1, 4);
                GC.TimeChange -= r;
                Value = r;
                transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = r.ToString();
                Activate = Void;
                Deactivate = UnChangeTime;
                break;
            case Type.EconStart:
                Activate = GainCoinsOnce;
                GetComponent<TimeScript>().counter = 60;
                Cost = 0;
                Deactivate = Void;
                break;
        }

	}

    public void SwapCard()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().GenerateCard(transform.position);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
