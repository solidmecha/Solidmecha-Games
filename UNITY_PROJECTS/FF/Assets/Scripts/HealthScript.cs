using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public int hp;
    public int maxHp;
    public GameObject HPbar;
    float hpBarScaleValue;

    void Start()
    {
        hpBarScaleValue = HPbar.transform.localScale.y;
    }
    public void dealDmg(int D)
    {
        hp -= D;
        if (hp < 0)
        {
            if(gameObject.CompareTag("ship"))
            {
                UnitScript us = (UnitScript)GetComponent(typeof(UnitScript));
                us.GM.CurrentSelection.Remove(gameObject);
            }
            else if(gameObject.name.Equals("EnemyBase(Clone)"))
            {
                EnemySpawner es = (EnemySpawner)gameObject.GetComponent(typeof(EnemySpawner));
                es.GM.InvokeEnemyBase();
            }
            Destroy(gameObject);
        }
        else if (hp > maxHp)
            hp = maxHp;
        UpdateHPbar();

    }
	
    void UpdateHPbar()
    {
        float r= (float)(maxHp - hp) / maxHp;
        HPbar.transform.localScale = new Vector2(HPbar.transform.localScale.x, hpBarScaleValue * hp /maxHp);
        if(r>0)
            HPbar.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        else
            HPbar.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
    }
}
