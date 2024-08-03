using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;

    Animator anim;
    float dirX, moveSpeed = 5f;
    bool isHurting, isDead;
    bool facingRight = true;
    Vector3 localScale;
    private bool isCanAttack = true;
   

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
        
       
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isDead && Mathf.Abs(rb.velocity.y) < 0.01)
            rb.AddForce(Vector2.up * 600f);


        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 10f;
        else
            moveSpeed = 5f;
        if (Input.GetButtonDown("Fire1") && isCanAttack == true)
        {
            isCanAttack = false;
            anim.SetBool("isAttacking1", true);
            StartCoroutine(ResetAttack());
            
        




        }
        SetAnimationState();


        if (!isDead)
            dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
    }




    private IEnumerator ResetAttack()
    {


        yield return new WaitForSeconds(0.550f);
        anim.SetBool("isAttacking1", false);
        isCanAttack = true;
    }

    void FixedUpdate()
    {
        if (!isHurting)
            rb.velocity = new Vector2(dirX, rb.velocity.y);
    }


    void LateUpdate()
    {
        CheckWhereToFace();
    }


    void SetAnimationState()
    {
        Debug.Log(dirX);
        if (dirX == 0)
        {
            anim.SetBool("isWalking", false);
        }


        if (Mathf.Abs(rb.velocity.y) < 0.01)
            anim.SetBool("isJumping", false);
        if (Mathf.Abs(dirX) > 0 && Mathf.Abs(rb.velocity.y) < 0.01)
            anim.SetBool("isWalking", true);


        if (Mathf.Abs(dirX) == 10 && rb.velocity.y <0.01)
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);




        if (Mathf.Abs(rb.velocity.y) > 0.01)
            anim.SetBool("isJumping", true);


    }


    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;


        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;


        transform.localScale = localScale;


    }


}



