using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;
    float counter=-10;
    float textCounter;
    int score;
    public float timer;
    public UnityEngine.UI.Text[] TextBoxes;
    System.Random rng;
    public GameBoss GB;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && counter < 0)
        {
            TextBoxes[3].text = "GG. 'R' to Restart";
            GB.GameOver = true;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && counter >= 0)
        {
            string s = "";
            switch (rng.Next(4))
            {
                case 0:
                    s = "Nice!";
                    break;
                case 1:
                    s="Impressive!";
                    break;
                case 2:
                    s="Cool!";
                    break;
                default:
                    s = "Nailed It!";
                    break;
            }
            TextBoxes[1].text = s;
            textCounter = .8f;
        }
         if(collision.gameObject.CompareTag("Finish"))
        {
            Destroy(collision.gameObject);
            score++;
            TextBoxes[2].text = "Collected: " + score.ToString();
        }
    }
    // Use this for initialization
    void Start () {
    rng = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        TextBoxes[0].text = timer.ToString();
        textCounter -= Time.deltaTime;
        if (textCounter <= 0)
            TextBoxes[1].text = "";
	if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.Space) && counter<-10)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            counter = .5f;
        }
        counter -= Time.deltaTime;
        if(counter<=0 && counter>=-.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
        if(counter<-10)
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
    }
}
