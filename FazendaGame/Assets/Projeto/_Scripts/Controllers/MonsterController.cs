using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float velocidade;
    [SerializeField] private float distance;

    private PlayerController player;
    private Animator anim;

    private float DistancePlayer;

    [Header("Ataque")]
    private bool timerAtk;

    private bool collisionSword = false;

    [Header("Health")]
    [SerializeField] private GameObject hurtEffect;

    private int life;
    private int lifeMax;

    private bool dead = false;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        anim = GetComponent<Animator>();
        lifeMax = Random.Range(2, 5);
        life = lifeMax;
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            DistancePlayer = Vector2.Distance(transform.position, player.transform.position);

            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            if (DistancePlayer < distance)
            {
                //Correr atras do player;
                if (!timerAtk)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, velocidade * Time.deltaTime);
                    anim.SetBool("walk", true);
                }


                if (transform.position.x < player.transform.position.x)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (transform.position.x > player.transform.position.x)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }

                if (DistancePlayer < .6f && !timerAtk)
                {
                    anim.SetTrigger("atk");
                    timerAtk = true;
                    StartCoroutine(Ataque());
                }
            }
            else
            {
                anim.SetBool("walk", false);
            }
        }

        //Life
        if(life <= 0 && !dead)
        {
            StartCoroutine(GameOver());
            dead = true;
        }
    }

    public void Hit()
    {
        life--;
        anim.SetTrigger("hurt");
        Destroy(Instantiate(hurtEffect,new Vector2(transform.position.x,transform.position.y + .2f),Quaternion.identity),.6f);
    }

    IEnumerator GameOver()
    {
        anim.SetTrigger("dead");
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    IEnumerator Ataque()
    {
        yield return new WaitForSeconds(.8f);
        timerAtk = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tool"))
        {
            if (!collisionSword)
            {
                if (InventoryManager.current.toolUsed == Tool.sword)
                {
                    Hit();
                    StartCoroutine(collSword());
                }
            }
        }
    }

    IEnumerator collSword()
    {
        collisionSword = true;
        yield return new WaitForSeconds(.4f);
        collisionSword = false;
    }
}
