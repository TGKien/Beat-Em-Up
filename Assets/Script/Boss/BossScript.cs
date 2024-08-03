using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public GameObject player;
    Rigidbody2D rb;
    bool facingRight = true;
    Vector3 localScale;
    public float speed;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > rb.transform.position.x)
        {
            facingRight = true;
            transform.Translate(x: speed * Time.deltaTime, y: 0, z: 0 );
            anim.SetBool("isWalking", true);
        }
        else if (player.transform.position.x < rb.transform.position.x)
        {
            facingRight = false;
            transform.Translate(x: speed * Time.deltaTime * -1, y: 0, z: 0);
            anim.SetBool("isWalking", true);
        }
        if (((facingRight) && (localScale.x > 0)) || ((!facingRight) && (localScale.x < 0)))
            localScale.x *= -1;

        transform.localScale = localScale;

    }
}
