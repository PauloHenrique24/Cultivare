using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float velocidade;
    [SerializeField] private float distance;

    private PlayerController player;
    private Animator anim;

    private float DistancePlayer;

    private bool timerAtk;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
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

            if (DistancePlayer < .8f && !timerAtk)
            {
                anim.SetTrigger("atk");
                timerAtk = true;
                StartCoroutine(Ataque());
            }
        }
        else if(DistancePlayer < distance && !timerAtk)
        {
            anim.SetBool("walk", false);
        }
    }

    IEnumerator Ataque()
    {
        yield return new WaitForSeconds(.8f);
        timerAtk = false;
    }
}
