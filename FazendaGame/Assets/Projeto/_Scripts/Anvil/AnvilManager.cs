using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class AnvilManager : MonoBehaviour
{
    [Header("Receitas")]
    public List<AnvilItem> receitas;

    [Header("Interface")]
    public GameObject canvasUI;
    public GameObject buttonOpenAnvil;

    bool dist = false;

    void Update()
    {
        if(Vector2.Distance(transform.position,FindFirstObjectByType<PlayerController>().transform.position) < .7f)
        {
            if (dist)
            {
                canvasUI.SetActive(true);
                buttonOpenAnvil.SetActive(true);
                buttonOpenAnvil.GetComponent<Button>().onClick.AddListener(Open);
                dist = false;
            }
        }
        else
        {
            if (!dist)
            {
                canvasUI.SetActive(false);
                buttonOpenAnvil.SetActive(false);
                buttonOpenAnvil.GetComponent<Button>().onClick.RemoveListener(Open);
                dist = true;
            }
        }
    }

    public void Open()
    {
        PlayerController.isMov = true;
        AnvilController.current.GenerateReceitas(receitas.ToArray());
        AnvilController.current.criarBtn.onClick.AddListener(CriarBtn);
    }

    public void CriarBtn()
    {
        AnvilItem itemSelected = null;
        bool receita = false;
        if(receitas.Count > 0)
        {
            foreach (var j in receitas)
            {
                for (int i = 0;i < AnvilController.current.anvilSlots.Count; i++)
                {
                    if (j.materials[i] != -1)
                    {
                        if (AnvilController.current.anvilSlots[i].transform.childCount > 1)
                        {
                            if (AnvilController.current.anvilSlots[i].transform.GetChild(1).GetComponent<ItemSlot>().item.id == j.materials[i])
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
                    else if (j.materials[i] == -1 && AnvilController.current.anvilSlots[i].transform.childCount > 1)
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

            foreach(var i in AnvilController.current.anvilSlots)
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


            var it = Instantiate(AnvilController.current.itemSaida, AnvilController.current.parentSaida);
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
