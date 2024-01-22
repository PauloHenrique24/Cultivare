using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArvoreManager : MonoBehaviour
{
    Animator anim;

    private int life;
    public Sprite troncoCortadoImg;
    public GameObject prefWood;

    public GameObject folhas;

    private bool damage;

    void Start()
    {
        anim = GetComponent<Animator>();
        life = Random.Range(3, 6);
    }

    void Update()
    {
        if (life <= 0)
        {
            var pos = new Vector2(transform.Find("Folhas").position.x, transform.Find("Folhas").position.y - .5f);

            GameObject tc = new GameObject("TC");
            
            var troc = Instantiate(tc, pos, Quaternion.identity);
            troc.AddComponent<SpriteRenderer>();

            troc.GetComponent<SpriteRenderer>().sprite = troncoCortadoImg;
            Destroy(troc, 1f);

            //Criar madeira
            GenerateWood(troc);

            Destroy(gameObject);
        }
    }

    void GenerateWood(GameObject troc)
    {
        int qtd = Random.Range(2, 5);

        for (int i = 0; i < qtd; i++)
        {
            var woodItem = Instantiate(prefWood, troc.transform.position, Quaternion.identity);
            woodItem.AddComponent<Rigidbody2D>().AddForce(Vector2.up * 4, ForceMode2D.Impulse);
            int posRig = Random.Range(0, 2);
            if (posRig == 1)
                woodItem.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(.1f,1f), ForceMode2D.Impulse);
            else
                woodItem.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Random.Range(.1f, 1f), ForceMode2D.Impulse);
            
            Destroy(woodItem.GetComponent<Rigidbody2D>(),.8f);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("tool"))
        {
            if (!damage)
            {
                if (InventoryManager.current.toolUsed == Tool.axe)
                {
                    anim.SetTrigger("Axe");
                    life--;
                    Destroy(Instantiate(folhas, transform.Find("Folhas").position, Quaternion.identity),.6f);
                    StartCoroutine(Damage());
                }
            }
        }
    }

    IEnumerator Damage()
    {
        damage = true;
        yield return new WaitForSeconds(.6f);
        damage = false;
    }
}
