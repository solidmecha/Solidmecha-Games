using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public static PlayerControl singleton;
    public float speed;
    bool moving;
    Vector3 Destination;
    public int shields;
    public int shieldValue;
    public int[] hp;
    public int[] heat;
    public int[] power;
    public int Creds;
    public Text hpText;
    public Text heatText;
    public Text energyText;
    public Text ShieldText;
    public Text PrimaryCD;
    public Text SecondaryCD;
    public ItemScript[] EquippedItems; //core, chasis, thruster, hydraulics, Shields, Primary, Secondary
    public List<ItemScript> Inventory;
    public int HPRegen;
    public int HeatLoss;
    public int LifeOnHit;
    public int HeatOnHit;
    public int PercentHeatAddedDmg;
    public int PercentPowerAddedDmg;
    public int PercentMissingHPaddedDmg;
    public int Crit;
    public WeaponScript PrimaryWS;
    public WeaponScript SecondaryWS;
    public float[] DashDuration;
    public float ShieldCounter;
    public float[] ShieldCD;
    public float[] DashCD;
    public Text DashCDtext;
    public Text ShieldCDtext;
    float perSecondCounter;
    public Text CredsText;
    public GameObject ResetCanvas;
    public float StunTimer;

    public void takeDamage(int value, GameControl.DamageType damageType)
    {
        switch(damageType)
        {
            case GameControl.DamageType.hp:
                if(value>0 && shields>0)
                {
                    shields -= value;
                    if(shields>=0)
                    updateShield();
                }
                hp[0] -= value;
                if (hp[0] > hp[1])
                    hp[0] = hp[1];
                updateHP();
                if(hp[0]<=0)
                {
                    GameControl.singleton.isPlayingLevel = false;
                    Instantiate(ResetCanvas);
                }
                break;
            case GameControl.DamageType.heat:
                heat[0] += value;
                if (heat[0] < 0)
                    heat[0] = 0;
                updateHeat();
                if (heat[0] >= heat[1])
                {
                    StunTimer = 4;
                    GameControl.singleton.isPlayingLevel = false;
                }
                break;
            case GameControl.DamageType.energy:
                power[0] -= value;
                updatePower();
                break;
        }
    }

    public void updateHP()
    {
        hpText.text = hp[0].ToString() + "/" + hp[1].ToString() + " HP";

    }

    public void updateHeat()
    {
        heatText.text = heat[0].ToString() + "/" + heat[1].ToString()+ " Heat";
    }

    public void updatePower()
    {
        energyText.text = power[0].ToString() + "/" + power[1].ToString() + " Power";
    }

    public void updateShield()
    {
        ShieldText.text = shields.ToString() + " Shields";
    }

    public void ApplyShield()
    {
        takeDamage(EquippedItems[(int)GameControl.ItemType.Shields].HeatGen, GameControl.DamageType.heat);
        ShieldCounter = 4;
        shields = shieldValue;
        ShieldCD[0] = ShieldCD[1];
        updateShield();
    }

    public void UpdateCreds()
    {
        CredsText.text = "$" + Creds.ToString();
    }

    public void ApplyDash()
    {
        takeDamage(EquippedItems[(int)GameControl.ItemType.Thruster].HeatGen, GameControl.DamageType.heat);
        speed *= 2;
        DashCD[0] = DashCD[1];
        DashDuration[0] = DashDuration[1];
    }

    public void OnHit()
    {
        takeDamage(-1 * HeatOnHit, GameControl.DamageType.heat);
        takeDamage(-1 * LifeOnHit, GameControl.DamageType.hp);
    }

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
        updateHP();
        updatePower();
        updateHeat();
        updateShield();
        perSecondCounter = 1;
        UpdateCreds();
	}
	
    public int AddedDmg()
    {
        return Mathf.RoundToInt( (PercentHeatAddedDmg / 100f) * (float)heat[0] + (PercentMissingHPaddedDmg/100f) * ((float)hp[1] - (float)hp[0]) + (PercentPowerAddedDmg/100f) * (float)power[0]);
    }

    public void StepForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position.x > 12)
            transform.position = new Vector3(12, transform.position.y, transform.position.z);
        else if(transform.position.x<-12)
            transform.position = new Vector3(-12, transform.position.y, transform.position.z);
        if (transform.position.z > 12)
            transform.position = new Vector3(transform.position.x, transform.position.y, 12);
        else if (transform.position.z < -12)
            transform.position = new Vector3(transform.position.x, transform.position.y, -12);
    }

	// Update is called once per frame
	void Update () {
        if (GameControl.singleton.isPlayingLevel)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                transform.localEulerAngles = new Vector3(0, -45, 0);
                StepForward();
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) { 
                transform.localEulerAngles = new Vector3(0, 45, 0); StepForward();
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) {
                transform.localEulerAngles = new Vector3(0, -135, 0); StepForward();
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) { 
                transform.localEulerAngles = new Vector3(0, 135, 0); StepForward();
            }
            else if (Input.GetKey(KeyCode.W)) { 
                transform.localEulerAngles = Vector3.zero; StepForward();
            }
            else if (Input.GetKey(KeyCode.A)) { 
                transform.localEulerAngles = new Vector3(0, -90, 0); StepForward();
            }
            else if (Input.GetKey(KeyCode.S)) { 
                transform.localEulerAngles = new Vector3(0, -180, 0); StepForward();
            }
            else if (Input.GetKey(KeyCode.D)) { 
                transform.localEulerAngles = new Vector3(0, 90, 0); StepForward();
            }

            if (Input.GetMouseButtonDown(0))
            {
                PrimaryWS.Engage(Input.mousePosition);
                /*
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.collider.CompareTag("floor"))
                {
                    Destination = hit.point;
                    moving = true;
                    transform.LookAt(Destination);
                }
            }

            if (moving)
            {
                float dist = Vector3.Magnitude(Destination - transform.position);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                if (dist <= Vector3.Magnitude(Destination - transform.position))
                {
                    transform.position = Destination;
                    moving = false;
                }
            }
            */
            }

            if (Input.GetMouseButtonDown(1))
            {
                SecondaryWS.Engage(Input.mousePosition);
                //PrimaryWS.Engage(Input.mousePosition);
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                //SecondaryWS.Engage(Input.mousePosition);
                ApplyShield();
                ApplyDash();
            }
            if (Input.GetKeyDown(KeyCode.E) && ShieldCD[0]<=0)
            {
                ApplyShield();
            }
            if(Input.GetKeyDown(KeyCode.Space) && DashCD[0]<=0)
            {
                ApplyDash();
            }

            if(DashDuration[0]>0)
            {
                DashDuration[0] -= Time.deltaTime;
                if (DashDuration[0] <= 0)
                    speed /= 2f;      
            }

            if(ShieldCounter>0)
            {
                ShieldCounter -= Time.deltaTime;
                if(ShieldCounter<=0)
                {
                    shields = 0;
                    updateShield();
                }
            }

            if (DashCD[0] > 0)
            {
                DashCD[0] -= Time.deltaTime;
                DashCDtext.text = Mathf.RoundToInt(DashCD[0]).ToString();
            }
            else
                DashCDtext.text = "Dash Ready";

            if (ShieldCD[0] > 0)
            {
                ShieldCD[0] -= Time.deltaTime;
                ShieldCDtext.text = Mathf.RoundToInt(ShieldCD[0]).ToString();
            }
            else
                ShieldCDtext.text = shieldValue.ToString()+" Shields Ready";

            if (PrimaryWS.CD[0] > 0)
                PrimaryCD.text = (Mathf.RoundToInt(PrimaryWS.CD[0] * 100) / 100f).ToString();
            else
                PrimaryCD.text = "Ready!";
            if (SecondaryWS.CD[0] > 0)
                SecondaryCD.text = (Mathf.RoundToInt(SecondaryWS.CD[0] * 100) / 100f).ToString();
            else
                SecondaryCD.text = "Ready!";
            perSecondCounter -= Time.deltaTime;
            if(perSecondCounter<=0)
            {
                perSecondCounter = 1;
                takeDamage(HPRegen * -1, GameControl.DamageType.hp);
                if(heat[0]<heat[1])
                    takeDamage(HeatLoss, GameControl.DamageType.heat);
            }
        }

        if (StunTimer > 0)
        {
            StunTimer -= Time.deltaTime;
            if (StunTimer <= 0)
            {
                takeDamage(-1 * heat[0] / 2, GameControl.DamageType.heat);
                GameControl.singleton.isPlayingLevel = true;
            }
        }

    }
}
