using System.Collections;
using System.Collections.Generic;
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
    public float force;
    private bool timerAtk;

    private bool collisionSword = false;

    private bool ishit;

    [Header("Health")]
    [SerializeField] private GameObject interfaceObj;
    [SerializeField] private Image lifeUI;
    [SerializeField] private GameObject hurtEffect;

    private int life;
    private int lifeMax;

    private int lifeAtual;

    private bool dead = false;

    [Header("Sounds Effects")]
    public AudioClip clipDead;
    public AudioClip clipHit;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        anim = GetComponent<Animator>();
        lifeMax = Random.Range(2, 5);
        life = lifeMax;

        interfaceObj.SetActive(false);

    }

    void FixedUpdate()
    {
        if (!dead)
        {
            DistancePlayer = Vector2.Distance(transform.position, player.transform.position);

            if (!ishit)
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
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        if(lifeAtual != life)
        {
            interfaceObj.SetActive(true);
            lifeUI.fillAmount = (float)life / lifeMax;
            StartCoroutine(LifeEnabled());
            lifeAtual = life;
        }

        //Life
        if(life <= 0 && !dead)
        {
            StartCoroutine(GameOver());
            dead = true;
        }
    }

    IEnumerator LifeEnabled()
    {
        yield return new WaitForSeconds(1f);
        interfaceObj.SetActive(false);
    }

    public void Hit()
    {
        ishit = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 direction = (transform.position - player.transform.position).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        life--;
        anim.SetTrigger("hurt");
        Destroy(Instantiate(hurtEffect,new Vector2(transform.position.x,transform.position.y + .2f),Quaternion.identity),.6f);

        SoundsManager.current.PlaySound(false, clipHit, .2f);

        StartCoroutine(HitEnd());
    }

    IEnumerator HitEnd()
    {
        yield return new WaitForSeconds(.1f);
        ishit = false;
    }

    IEnumerator GameOver()
    {
        anim.SetTrigger("dead");
        GetComponent<BoxCollider2D>().enabled = false;
        SoundsManager.current.PlaySound(false, clipDead, .3f);

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
