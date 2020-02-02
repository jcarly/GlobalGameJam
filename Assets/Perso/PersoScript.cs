using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersoScript : MonoBehaviour
{
    public float speed = 200f;
    public float jumpForce = 100f;
    public int life = 5;
    public GameObject repairText;
    public TextMesh repairCount;
    public TextMesh lifeCount;
    public int repairLength = 3;
    public int repairLengthDelta = 1;
    public int repairCompletion = 10;
    public int repairBreakMalus = 1;
    public Vector2 hurtForce = new Vector2(50f, 50f);
    public GameObject heart;
    public GameObject progressDisplay;
    public GameObject pressTouch;

    private List<GameObject> lifeDisplay = new List<GameObject>();
    public int repairProgression = 0;
    private int repairTry;
    private bool lookRight = true;
    private Animator anim;
    private Rigidbody2D rb;
    private bool onCible = false;
    private bool repairMode = false;
    private Vector3 originalProgressScale;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        for(int i = 0; i < life; i++)
        {
            lifeDisplay.Add(Instantiate(heart, new Vector3(-7.8f + i, 4.3f, 0f), Quaternion.identity));
        }
        originalProgressScale = progressDisplay.transform.localScale;
        pressTouch.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        repairCount.text = repairProgression + "";
        Vector3 progressScale = originalProgressScale;
        progressScale.x = originalProgressScale.x * repairProgression / repairCompletion;
        progressDisplay.transform.localScale = progressScale;
        lifeCount.text = life + "";
        anim.SetFloat("ZSpeed", Mathf.Abs(rb.velocity.y));

        if (repairMode && Input.anyKeyDown) {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), (string)repairText.GetComponent<RepairScript>().lettersForRepair[0])))
            {
                repairTry--;

                repairText.GetComponent<RepairScript>().lettersForRepair.RemoveAt(0);
                if (repairTry == 0)
                {
                    repairMode = false;
                    repairTry = repairLength + repairProgression * repairLengthDelta;
                    repairText.GetComponent<TextMesh>().text = "";
                    repairProgression++;
                    repairText.GetComponent<Animator>().SetTrigger("Decrease");
                    anim.SetBool("Repair", false);
                }
            }
            else
            {
                ExitRepair();
            }
        }
        else
        {
            float move = Input.GetAxis("Horizontal");
            transform.Translate(Vector2.right * move * speed * Time.deltaTime);
            anim.SetFloat("Speed", Mathf.Abs(move));

            if ((move > 0 && !lookRight) || (move < 0 && lookRight))
            {
                Flip();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Attack1");
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && rb.velocity.y == 0)
            {
                rb.AddForce(new Vector2(0, jumpForce));
                anim.SetTrigger("Jump");
            }
            if (Input.GetKeyDown(KeyCode.R) && onCible)
            {
                repairMode = true;
                anim.SetBool("Repair", true);
                repairText.GetComponent<Animator>().SetTrigger("Grow");
                repairText.GetComponent<RepairScript>().length = repairLength + repairProgression * repairLengthDelta;
                repairTry = repairLength + repairProgression * repairLengthDelta;
                pressTouch.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.tag == "Enemy")
        {
            Vector2 direction = collision.transform.localPosition - transform.localPosition;
            collision.gameObject.GetComponent<EnemyScript>().Dead(direction.normalized.x);
            Destroy(collision.gameObject, 0.5f);
        }
        if (collision.tag == "Cible")
        {
            onCible = true;
            pressTouch.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cible")
        {
            onCible = false;
            pressTouch.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        Debug.Log(collision.collider.tag);
        Vector2 direction =  transform.localPosition - collision.transform.localPosition;
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log(direction.normalized.x);
            Hurt(direction.normalized.x);
        }
    }

    private IEnumerator OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            collision.collider.enabled = false;
            yield return new WaitForSeconds(0.5f);
            collision.collider.enabled = true;
        }
    }

    private void Flip()
    {
        lookRight = !lookRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Hurt(float direction)
    {
        rb.AddForce(new Vector2(direction * hurtForce.x, hurtForce.y));
        if(lookRight && direction > 0)
        {
            Flip();
        }
        anim.SetTrigger("Hurt");
        life--;
        Destroy(lifeDisplay[lifeDisplay.Count - 1]);
        lifeDisplay.RemoveAt(lifeDisplay.Count - 1);
        if (repairMode)
        {
            ExitRepair();
        }
    }

    private void ExitRepair()
    {
        repairMode = false;
        anim.SetBool("Repair", false);
        if (repairProgression > 0)
        {
            repairProgression -= repairBreakMalus;
        }
        
        if (repairText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("GrowText")){
            repairText.GetComponent<Animator>().SetTrigger("Decrease");
        }
    }

    public void Erase()
    {
        Destroy(gameObject);
    }
}
