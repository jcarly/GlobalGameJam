using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 3f;
    public GameObject player;
    public Vector2 hurtForce = new Vector2(300f, 300f);
    public AudioClip hitSnd, explodeSnd;
    private Animator anim;
    private Rigidbody2D rb;
    private bool move = true;
    private AudioSource aSource;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        aSource = GetComponent<AudioSource>();
        if(transform.localPosition.x > 0)
        {
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {            
            transform.Translate(Vector2.right * speed * (transform.localScale.x > 0 ? 1 : -1) * Time.deltaTime);
            anim.SetFloat("Speed", Mathf.Abs(speed));
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionDirection = transform.localPosition.x - collision.GetContact(0).point.x;
        if (Mathf.Abs(collisionDirection) > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Dead(float direction)
    {
        move = false;
        rb.AddForce(new Vector2(direction * hurtForce.x, hurtForce.y));
        if (transform.localScale.x > 0 && direction > 0)
        {
            Flip();
        }
        anim.SetTrigger("Dead");
    }

    public void Erase()
    {
        Debug.Log("Destroy Enemy");
        Destroy(this);
    }

    public void Hit()
    {
        aSource.PlayOneShot(hitSnd);
    }
    public void Explode()
    {
        aSource.PlayOneShot(explodeSnd);
    }
}
