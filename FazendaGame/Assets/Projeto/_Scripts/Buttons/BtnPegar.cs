using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BtnPegar : MonoBehaviour
{
    [HideInInspector] public ItemInv item;
    [HideInInspector] public GameObject btnEnabled;
    [HideInInspector] public GameObject itemPref;

    [HideInInspector] public bool isColected;

    void Start()
    {
        btnEnabled = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (isColected)
        {
            btnEnabled.SetActive(false);
        }
    }

    public void Pegar()
    {
        if (InventoryManager.current.AddItemInv(item,itemPref.GetComponent<ItemPref>().life))
        {
            itemPref.GetComponent<ItemPref>().isColected = true;
            isColected = true;
            StartCoroutine(Colect());
        }
    }

    public IEnumerator Colect()
    {
        yield return new WaitForSeconds(.3f);
        isColected = false;
    }
}
