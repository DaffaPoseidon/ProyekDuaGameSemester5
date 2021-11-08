using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpLenght = 5f;
    [SerializeField] private LayerMask ground; 
    
    private Collider2D coll;

    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //Dari melompat ke jatuh
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        //Dari jatuh ke diam
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
           anim.SetBool("Falling", false); 
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLenght, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }

            else
            {
                facingLeft = false;
            }
        }

        else
        {
            if (transform.position.x < leftCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLenght, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }

            else
            {
                facingLeft = true;
            }
        }
    }
}
