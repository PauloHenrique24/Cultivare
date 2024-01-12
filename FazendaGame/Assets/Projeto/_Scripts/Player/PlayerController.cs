using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    [Header("Movimentação")]
    public float speedNormal;
    private float speedSlow;

    private float speed;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    [HideInInspector] public bool isFacing;

    [Header("Animação")]
    private Animator anim;

    [Header("Inventario")]
    public ItemInv itemTeste;

    [Header("Hand")]
    public GameObject handObj;

    [Header("Atk")]
    private bool isAtk;

    public static bool isMov = false;

    [Header("Plantar")]
    [HideInInspector] public bool isPlant;

    [HideInInspector] public Vector2 posPlantPlayer;

    [Header("Money")]
    public static int Money;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speedSlow = speedNormal / 1.2f;

        isMov = false;
    }

    void Update()
    {
        OnMoviment();

        if (isPlant)
        {
            transform.position = Vector2.MoveTowards(transform.position, posPlantPlayer, 5 * Time.deltaTime);
        }
    }

    void OnMoviment()
    {
        if (!isMov)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            moveDirection = new Vector2(x, y);

            if (x != 0 && y != 0)
            {
                speed = speedSlow;
            }
            else
            {
                speed = speedNormal;
            }

            if (y > 0 && x == 0)
            {
                transform.Find("positionGrid").transform.localPosition = new Vector2(0, 0.64f);
            }
            else if(x != 0)
            {
                transform.Find("positionGrid").transform.localPosition = new Vector2(0.54f, 0);
            }

            if (y < 0 && x == 0)
            {
                transform.Find("positionGrid").transform.localPosition = new Vector2(0, -0.40f);
            }
            else if(x != 0)
            {
                transform.Find("positionGrid").transform.localPosition = new Vector2(0.54f, 0);
            }

            //else
            //{
            //    transform.Find("positionGrid").transform.localPosition = new Vector2(0.54f, 0);
            //}

            rb.velocity = moveDirection * speed;

            Flip(x);
            AnimationMov();
        }
        else
        {
            rb.velocity = new Vector2(0f,0f);
        }
    }

    void AnimationMov()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            anim.SetInteger("transition", 1);
        }
        else
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Flip(float x)
    {
        if (x < 0)
        {
            var pos = transform.localScale;
            pos.x = -1;
            transform.localScale = pos;

            isFacing = true;
        }
        else if (x > 0)
        {
            var pos = transform.localScale;
            pos.x = 1;
            transform.localScale = pos;

            isFacing = false;
        }
    }

    public void Attack()
    {
        if(handObj.transform.childCount > 0)
        {
            if (!isAtk)
            {
                var it = handObj.transform.GetChild(0).GetComponent<ItemSlot>().item;
                switch (it.tool)
                {
                    case Tool.sword:
                        AtkFunc("sword");
                        break;


                    case Tool.axe:
                        AtkFunc("axe");
                        break;

                    case Tool.seller:
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void DestruirBuild()
    {

    }

    void AtkFunc(string animate)
    {
        anim.SetTrigger(animate);

        handObj.transform.GetChild(0).GetComponent<ItemSlot>().life
        -= handObj.transform.GetChild(0).GetComponent<ItemSlot>()
        .item.porcentDamageLife;
       
        StartCoroutine(AtkRestart());
        
    }

    IEnumerator AtkRestart()
    {
        isAtk = true;
        yield return new WaitForSeconds(.7f);
        isAtk = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("waterTarget")) WaterManager.current.isPosPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("waterTarget")) WaterManager.current.isPosPlayer = false;
    }
}
