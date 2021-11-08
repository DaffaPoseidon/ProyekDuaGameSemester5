using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Komponen
    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;

    //enum
    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle;
    
    //Fitur tambahan dalam komponen
    [SerializeField] private LayerMask ground;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float runJumpSpeed = 12f;
    [SerializeField] private float hurtForce = 5f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;

    private void Start() {
        //Komponen
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
    }

    private void Update()
    {
        //Pergerakan
        if (state != State.hurt)
        {
            Movement();
        }

        //Animasi
        AnimationState();
        anim.SetInteger("state", (int)state);

    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        //Lari ke kiri
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-runSpeed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //Lari ke kanan
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Melompat
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1.3f, ground);
            if (hit.collider != null)
                Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, runJumpSpeed);
        state = State.jumping;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            PermanentUI.perm.ceri += 1;
            PermanentUI.perm.teksCeri.text = PermanentUI.perm.ceri.ToString();
            cherry.Play();
        }

        if(collision.tag == "Power up")
        {
            Destroy(collision.gameObject);
            runJumpSpeed = 15f;
            runSpeed = 12f;
            GetComponent<SpriteRenderer>().color = Color.black;
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }

            else
            {
                state = State.hurt;
                HandleHealth(); //Kodingan soal nyawa player.

                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);

                }
            }
        }
    }

    private void HandleHealth()
    {
        PermanentUI.perm.health -= 1;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }

        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }

        else
        {
            state = State.idle;
        }
    }

    private void Footstep()
    {
        if (coll.IsTouchingLayers(ground))
        {
            footstep.Play();
        }
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(7);
        GetComponent<SpriteRenderer>().color = Color.white;
        runSpeed = 8f;
        runJumpSpeed = 12f;
    }
}
