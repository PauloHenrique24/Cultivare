using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPref : MonoBehaviour
{
    public ItemInv item;
    public float speed;
    
    private GameObject player;

    private bool isDisable = false;

    [HideInInspector] public bool isColected;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().gameObject;    
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < .8f)
        {
            FindFirstObjectByType<BtnPegar>().btnEnabled.gameObject.SetActive(true);
            FindFirstObjectByType<BtnPegar>().item = item;
            FindFirstObjectByType<BtnPegar>().itemPref = gameObject;

            isDisable = false;
        }
        else
        {
            if (!isDisable)
            {
                FindFirstObjectByType<BtnPegar>().btnEnabled.gameObject.SetActive(false);
                FindFirstObjectByType<BtnPegar>().item = null;
                FindFirstObjectByType<BtnPegar>().itemPref = null;

                isDisable = true;
            }
        }

        if (isColected)
        {
            GetComponent<Animator>().SetTrigger("coletar");

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < .01f)
            {
                FindFirstObjectByType<BtnPegar>().btnEnabled.gameObject.SetActive(false);
                FindFirstObjectByType<BtnPegar>().item = null;
                FindFirstObjectByType<BtnPegar>().itemPref = null;
                Destroy(gameObject);
            }
        }
    }
}
