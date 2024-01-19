using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalsController : MonoBehaviour
{
    public float speed;

    private bool directionDefined;
    private Vector3 direction;

    private float timerPause;

    [Header("Fome e Sede")]
    [SerializeField] private Image hungerBar;
    [SerializeField] private Image thirstBar;

    private float hunger = 100;
    private float thirst = 100;

    private float hungerAtual;
    private float thirstAtual;

    private bool inMoviment = false;
    private GameObject objCocho;

    void Start()
    {
        speed = Random.Range(.2f, 1.2f);
    }

    void Update()
    {
        if (!inMoviment)
            MovimentAnimal();
        else
            Alimentar();

        BarEssencials();
    }

    public void MovimentAnimal()
    {
        if (!directionDefined)
        {
            var Walled = gameObject.GetComponentInParent<WalledGenerator>();

            float x = Random.Range(Walled.direction[0].position.x, Walled.direction[1].position.x);
            float y = Random.Range(Walled.direction[2].position.y, Walled.direction[3].position.y);

            Vector3 direct = new Vector3(x, y, 0);

            RaycastHit2D hit = Physics2D.Raycast(direct, Vector3.forward);

            if (!hit.collider)
            {
                timerPause = Random.Range(1f, 3f);

                direction = new Vector3(x, y, 0);
                directionDefined = true;
            }
        }
        else
        {
            if (transform.position != direction)
            {
                transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);

                if (direction.x > transform.position.x)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (direction.x < transform.position.x)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }

                if (Vector2.Distance(transform.position, direction) <= 0.01f)
                {
                    StartCoroutine(PauseMoviment());
                }
            }
        }
    }

    IEnumerator PauseMoviment()
    {
        yield return new WaitForSeconds(timerPause);
        speed = Random.Range(.2f, 1.2f);
        directionDefined = false;
    }

    public void BarEssencials()
    {
        hunger -= Time.deltaTime / 2;
        thirst -= Time.deltaTime;

        if(hunger != hungerAtual)
        {
            hungerBar.fillAmount = hunger / 100;
            hungerAtual = hunger;

            if(hunger < 90 && !inMoviment && gameObject.GetComponentInParent<WalledGenerator>().cochoComida > 0)
            {
                inMoviment = true;
                speed = Random.Range(.2f, 1.2f);
                objCocho = gameObject.GetComponentInParent<WalledGenerator>().cochoComidaObj;
            }
        }

        if(thirst != thirstAtual)
        {
            thirstBar.fillAmount = thirst / 100;
            thirstAtual = thirst;

            if (thirst < 90 && !inMoviment && gameObject.GetComponentInParent<WalledGenerator>().cochoAgua > 0)
            {
                inMoviment = true;
                objCocho = gameObject.GetComponentInParent<WalledGenerator>().cochoAguaObj;
                speed = Random.Range(.2f, 1.2f);
            }
        }
    }

    public void Alimentar()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector3(objCocho.transform.position.x,objCocho.transform.position.y + .3f), speed * Time.deltaTime);

        if (objCocho.transform.position.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (objCocho.transform.position.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

}
