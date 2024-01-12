using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class AnvilManager : MonoBehaviour
{
    public static AnvilManager current;

    [Header("Slots")]
    public GameObject itemSlot;
    public Transform parentSlot;
    public int qtdSlots;

    [HideInInspector] public List<GameObject> anvilSlots;

    [Header("Receitas")]
    public GameObject itemReceita;
    public Transform parentReceita;
    public List<AnvilItem> receitas;

    public List<Sprite> matsSprites;

    [Header("Saida")]
    public GameObject itemSaida;
    public Transform parentSaida;

    [Header("Interface")]
    public GameObject canvasUI;
    public GameObject buttonOpenAnvil;

    void Awake()
    {
        if(current == null)
            current = this;
    }

    void Start()
    {
        //Slots Fabricar
        for(int i = 0;i < qtdSlots; i++)
        {
            //Criar item Slot
            var item = Instantiate(itemSlot, parentSlot);
            item.GetComponent<Button>().onClick.AddListener(() => InventoryManager.current.OtherPositionItemAnvil(item));

            anvilSlots.Add(item);

            //Aumentar o tamanho do slot
            item.transform.localScale = new Vector3(1.4f, 1.4f);
        }

        //Receitas
        for(int i = 0;i < receitas.Count; i++)
        {
            var itemRec = Instantiate(itemReceita,parentReceita);
            itemRec.transform.Find("icone").GetComponent<Image>().sprite = receitas[i].item.icone;
            itemRec.transform.Find("name_").GetComponent<TextMeshProUGUI>().text = receitas[i].item.name_;

            itemRec.GetComponent<ReceitasItem>().item = receitas[i];
        }
    }

    bool dist = false;

    void Update()
    {
        if(Vector2.Distance(transform.position,FindFirstObjectByType<PlayerController>().transform.position) < .7f)
        {
            if (dist)
            {
                canvasUI.SetActive(true);
                buttonOpenAnvil.SetActive(true);
                dist = false;
            }
        }
        else
        {
            if (!dist)
            {
                canvasUI.SetActive(false);
                buttonOpenAnvil.SetActive(false);
                dist = true;
            }
        }
    }

    public void Open()
    {
        PlayerController.isMov = true;
    }

    public void CriarBtn()
    {
        AnvilItem itemSelected = null;
        bool receita = false;
        if(receitas.Count > 0)
        {
            foreach (var j in receitas)
            {
                for (int i = 0;i < anvilSlots.Count; i++)
                {
                    if (j.materials[i] != -1)
                    {
                        if (anvilSlots[i].transform.childCount > 1)
                        {
                            if (anvilSlots[i].transform.GetChild(1).GetComponent<ItemSlot>().item.id == j.materials[i])
                            {
                                receita = true;
                            }
                            else
                            {
                                receita = false;
                                break;
                            }
                        }
                        else
                        {
                            receita = false;
                            break;
                        }
                    }
                    else if (j.materials[i] == -1 && anvilSlots[i].transform.childCount > 1)
                    {
                        receita = false;
                        break;
                    }
                }

                if (receita)
                {
                    itemSelected = j;
                    break;
                }
            }
        }

        if (receita)
        {
            //Excluir itens dos slots
            //Criar item no slot saida

            foreach(var i in anvilSlots)
            {
                if (i.transform.childCount > 1)
                {
                    if (i.transform.GetChild(1).transform.Find(InventoryManager.current.selectObj.name))
                    {
                        i.transform.GetChild(1).transform.Find(InventoryManager.current.selectObj.name).gameObject.SetActive(false);
                        i.transform.GetChild(1).transform.Find(InventoryManager.current.selectObj.name).transform.SetParent(InventoryManager.current.transform);
                    }

                    Destroy(i.transform.GetChild(1).gameObject);
                }
            }


            var it = Instantiate(itemSaida,parentSaida);
            it.transform.localPosition = new Vector3(0, 0, 0);

            it.GetComponent<ItemSlot>().qtd = 1;
            it.GetComponent<ItemSlot>().qtdTxt.text = "1";
            it.GetComponent<ItemSlot>().name_ = itemSelected.item.name_;
            it.GetComponent<ItemSlot>().description_ = itemSelected.item.description_;

            it.GetComponent<ItemSlot>().icone.gameObject.SetActive(true);
            it.GetComponent<ItemSlot>().icone.sprite = itemSelected.item.icone;

            it.GetComponent<ItemSlot>().item = itemSelected.item;
        }
    }

    public void Close()
    {
        foreach(var i in anvilSlots)
        {
            i.transform.Find("receita").gameObject.SetActive(false);
        }

        PlayerController.isMov = false;
    }

    /* Botão Criar
      public void CriarBtn()
    {
        bool receita = false;
        if(itemSelected != null)
        {
            for(int i = 0;i < anvilSlots.Count; i++)
            {
                if (itemSelected.materials[i] != -1)
                {
                    if (anvilSlots[i].transform.childCount > 1)
                    {
                        if (anvilSlots[i].transform.GetChild(1).GetComponent<ItemSlot>().item.id == itemSelected.materials[i])
                        {
                            receita = true;
                        }
                        else
                        {
                            receita = false;
                            break;
                        }
                    }
                    else {
                        receita = false;
                        break;
                    }
                }
                else if (itemSelected.materials[i] == -1 && anvilSlots[i].transform.childCount > 1)
                {
                    receita = false;
                    break;
                }
            }
        }

        if (receita)
        {
            //Excluir itens dos slots
            //Criar item no slot saida

            foreach(var i in anvilSlots)
            {
                if (i.transform.childCount > 1)
                {
                    if (i.transform.GetChild(1).transform.Find(InventoryManager.current.selectObj.name))
                    {
                        i.transform.GetChild(1).transform.Find(InventoryManager.current.selectObj.name).gameObject.SetActive(false);
                        i.transform.GetChild(1).transform.Find(InventoryManager.current.selectObj.name).transform.SetParent(InventoryManager.current.transform);
                    }

                    Destroy(i.transform.GetChild(1).gameObject);
                }
            }


            var it = Instantiate(itemSaida,parentSaida);
            it.transform.localPosition = new Vector3(0, 0, 0);

            it.GetComponent<ItemSlot>().qtd = 1;
            it.GetComponent<ItemSlot>().qtdTxt.text = "1";
            it.GetComponent<ItemSlot>().name_ = itemSelected.item.name_;
            it.GetComponent<ItemSlot>().description_ = itemSelected.item.description_;

            it.GetComponent<ItemSlot>().icone.gameObject.SetActive(true);
            it.GetComponent<ItemSlot>().icone.sprite = itemSelected.item.icone;

            it.GetComponent<ItemSlot>().item = itemSelected.item;
        }
    }
     */

}
