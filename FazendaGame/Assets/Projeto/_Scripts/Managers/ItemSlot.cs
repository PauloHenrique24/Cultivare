using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI qtdTxt;
    public Image icone;
    public Slider lifeUI;

    [Header("Num")]
    [HideInInspector] public int qtd;
    [HideInInspector] public string name_;
    [HideInInspector] public string description_;

    [HideInInspector] public ItemInv item;

    [HideInInspector] public bool isSelect;

    [HideInInspector] public int life;

    public bool isAnvil;

    void Start()
    {
        if (item.islife)
        {
            lifeUI.gameObject.SetActive(true);
            if(life <= 0)
                life = item.life;

            lifeUI.maxValue = item.life;
        }
        else
        {
            lifeUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (item.islife)
        {
            lifeUI.value = life;

            if (life <= 0 && !item.water)
            {
                InventoryManager.current.buttonAtk.SetActive(false);
                InventoryManager.current.itemSlots.Remove(this);

                InventoryManager.current.selectObj.SetActive(false);
                InventoryManager.current.selectObj.transform.SetParent(InventoryManager.current.gameObject.transform);

                Destroy(gameObject);
            }
        }

        if(qtd <= 0)
        {
            if (transform.Find("SelectObj"))
            {
                transform.Find("SelectObj").gameObject.SetActive(false);
                transform.Find("SelectObj").SetParent(FindFirstObjectByType<InventoryManager>().transform);
            }

            InventoryManager.current.itemSlots.Remove(this);
            Destroy(gameObject);
        }
    }

    public void Select()
    {
        InventoryManager.current.SelectOnItem(transform, this);
    }
}
