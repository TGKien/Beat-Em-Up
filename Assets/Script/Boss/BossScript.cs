using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    public GameObject player;
    Rigidbody2D rb;
    bool facingRight = true;
    Vector3 localScale;
    public float speed;
    Animator anim;
    public float attackRange = 3;
    public int attackDamage = 10;
    public LayerMask attackMask;
    private float MAX_HEALTH = 100f;
    [SerializeField]
    private float currentHealth;
    private bool isCanTakeDamage = true;
    private bool isCanAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        anim = GetComponent<Animator>();
        
        currentHealth = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > rb.transform.position.x)
        {
            facingRight = true;
            anim.SetBool("isWalking", true);
        }
        else if (player.transform.position.x < rb.transform.position.x)
        {
            facingRight = false;
            anim.SetBool("isWalking", true);
        }
        transform.Translate(x: speed * Time.deltaTime * (facingRight ? 1 : -1), y: 0, z: 0);

        if (((facingRight) && (localScale.x > 0)) || ((!facingRight) && (localScale.x < 0)))
            localScale.x *= -1;
        if(Vector2.Distance(player.transform.position, rb.transform.position)<= 5 && isCanAttack)
        {
            anim.SetBool("isAttacking1",true);
            isCanAttack = false;
            //Attack();
            StartCoroutine(WaitAttack());
        }
        transform.localScale = localScale;

    }
    IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(1f);
        Collider2D colInfo = Physics2D.OverlapCircle(transform.position, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerScript>().TakeDamage(attackDamage);

        }
        yield return new WaitForSeconds(1f);

        anim.SetBool("isAttacking1", false);
        yield return new WaitForSeconds(5f);
        isCanAttack = true;

    }

    void Attack()
    {
        Collider2D colInfo = Physics2D.OverlapCircle(transform.position, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerScript>().TakeDamage(attackDamage);
            
        }
    }
    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
        if (isCanTakeDamage)
        {
            currentHealth -= damage;
            StartCoroutine(DamageAnimation());
            isCanTakeDamage = false;
        }
        if ( currentHealth <= 0f)
        {
            BossScript bossScript = GetComponent<BossScript>();
            bossScript.enabled = false;
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


