using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;
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
    private float MAX_HEALTH = 100f;
    [SerializeField]
    private float currentHealth;
    private bool isCanTakeDamage = true;
    public LayerMask attackMask;
    public float attackRange = 3;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
        currentHealth = MAX_HEALTH;
        
       
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isDead && Mathf.Abs(rb.velocity.y) < 0.01)
            rb.AddForce(Vector2.up * 500f);


        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 10f;
        else
            moveSpeed = 5f;
        if (Input.GetButtonDown("Fire1") && isCanAttack == true)
        {
            isCanAttack = false;
            anim.SetBool("isAttacking1", true);
            StartCoroutine(ResetAttack());
            Collider2D colInfo = Physics2D.OverlapCircle(transform.position, attackRange, attackMask);
            if (colInfo != null)
            {
                colInfo.GetComponent<BossScript>().TakeDamage(50);

            }



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
    public void TakeDamage(float damage)
    {
        if (isCanTakeDamage)
        {
            currentHealth -= damage;
            StartCoroutine(DamageAnimation());
            isCanTakeDamage= false;
        }
        if (currentHealth <= 0f)
        {
            PlayerScript playerScript = GetComponent<PlayerScript>();
            playerScript.enabled = false;
            anim.SetTrigger("isDying");
        }
   
    }
    IEnumerator DamageAnimation()
    {
        anim.SetBool("isHurting", true);
        yield return new WaitForSeconds(0.75f);
        anim.SetBool("isHurting", false);
        isCanTakeDamage = true;
    }

}



