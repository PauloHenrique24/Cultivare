using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager current;

    [Header("GenerateSlots")]
    public GameObject slots;
    public int qtdSlots = 20;

    public Transform targetSlotSpawn;

    [HideInInspector] public List<GameObject> slotsList = new List<GameObject>();
    [HideInInspector] public List<ItemSlot> itemSlots = new List<ItemSlot>();

    [Header("Item Inventario")]
    public GameObject itemInv;

    [Header("Selected")]
    public GameObject selectObj;
    public GameObject soltarBtn;

    [HideInInspector] public ItemSlot selectItem;

    [Header("Hand")]
    public GameObject handObj;
    public GameObject buttonAtk;

    [HideInInspector] public Tool toolUsed;

    void Awake()
    {
        if (current == null)
            current = this;
    }

    void Start()
    {
        toolUsed = Tool.none;

        GenerateSlots();
    }

    public void GenerateSlots()
    {
        for (int i = 0; i < qtdSlots; i++)
        {
            var slot = Instantiate(slots, targetSlotSpawn);
            slot.GetComponent<Button>().onClick.AddListener(() => OtherPositionItemInventory(slot));

            slotsList.Add(slot);
        }
    }

    public bool AddItemInv(ItemInv item)
    {
        int indice = 0;
        bool creat = false;
        bool ret = false;

        if (itemSlots.Count > 0)
        {
            foreach (var i in itemSlots)
            {
                if (i.item == item && i.qtd < item.qtd && !i.item.notBunch && !i.isAnvil)
                {
                    //Este item já existe no inventario e tem espaço e pode ser agrupado
                    AcrescentarItem(i);
                    creat = true;
                    ret = true;
                    break;
                }
                else
                {
                    creat = false;
                }
            }

            if (!creat)
            {
                bool espace = false;
                //Este item não tem, ou não tem espaço, ou não pode ser agrupado
                foreach (var j in slotsList)
                {
                    if (j.transform.childCount <= 0)
                    {
                        //Espaço livre
                        espace = true;
                        CreateItem(j.transform, item);
                        ret = true;
                        break;
                    }
                    else
                    {
                        espace = false;
                    }
                }

                if (!espace)
                {
                    // O Inventario esta cheio
                    ret = false;
                    print("Cheio");
                }
            }
        }
        else
        {
            CreateItem(slotsList[indice].transform, item);
            ret = true;
        }

        return ret;
    }

    public void AcrescentarItem(ItemSlot slot)
    {
        slot.qtd++;
        slot.qtdTxt.text = slot.qtd.ToString("");
    }

    public void CreateItem(Transform parent, ItemInv item)
    {
        var itemI = Instantiate(itemInv, parent);

        itemI.transform.localPosition = new Vector3(0, 0, 0);

        itemI.GetComponent<ItemSlot>().qtd = 1;

        itemI.GetComponent<ItemSlot>().qtdTxt.text = itemI.GetComponent<ItemSlot>().qtd.ToString("");
        itemI.GetComponent<ItemSlot>().icone.sprite = item.icone;
        itemI.GetComponent<ItemSlot>().icone.gameObject.SetActive(true);
        itemI.GetComponent<ItemSlot>().name_ = item.name_;
        itemI.GetComponent<ItemSlot>().description_ = item.description_;

        itemI.GetComponent<ItemSlot>().item = item;

        //Adiciona o item do inventario a uma lista
        itemSlots.Add(itemI.GetComponent<ItemSlot>());
    }

    public void SelectOnItem(Transform target, ItemSlot item)
    {
        //BtnSoltar

        if (!soltarBtn.activeSelf)
            soltarBtn.SetActive(true);

        foreach (var i in itemSlots)
        {
            i.isSelect = false;
        }

        item.isSelect = true;

        if (!selectObj.activeSelf)
        {
            selectObj.SetActive(true);
        }

        selectObj.transform.localScale = new Vector3(1, 1, 1);

        selectObj.transform.SetParent(target);

        selectObj.transform.position = target.position;

        selectItem = item;
    }

    public void Soltar_Btn()
    {
        if (selectItem != null)
        {
            var player = FindFirstObjectByType<PlayerController>();

            if (player.isFacing)
            {
                //Esquerda
                Instantiate(selectItem.item.prefab, new Vector2(player.transform.position.x - Random.Range(0.2f, 0.9f), player.transform.position.y), Quaternion.identity);
                RemoveItemInventory(selectItem, 1);
            }
            else
            {
                //Direita
                Instantiate(selectItem.item.prefab, new Vector2(player.transform.position.x + Random.Range(0.2f, 0.9f), player.transform.position.y), Quaternion.identity);
                RemoveItemInventory(selectItem, 1);
            }
        }
    }

    void RemoveItemInventory(ItemSlot item, int qtd)
    {
        item.qtd -= qtd;
        item.qtdTxt.text = item.qtd.ToString("");

        if (item.qtd <= 0)
        {
            foreach (var sl in slotsList)
            {
                if (sl.transform.childCount > 0 && sl.transform.GetChild(0).GetComponent<ItemSlot>() == item)
                {
                    
                    sl.transform.GetChild(0).Find("SelectObj").gameObject.SetActive(false);
                    sl.transform.GetChild(0).Find("SelectObj").SetParent(FindFirstObjectByType<InventoryManager>().transform);

                    Destroy(sl.transform.GetChild(0).gameObject);
                    break;
                }
            }

            //Bigorna Aberta
            foreach(var AS in AnvilManager.current.anvilSlots)
            {
                if (AS.transform.childCount > 0 && AS.transform.GetChild(0).GetComponent<ItemSlot>() == item)
                {
                    AS.transform.GetChild(0).Find("SelectObj").gameObject.SetActive(false);
                    AS.transform.GetChild(0).Find("SelectObj").SetParent(FindFirstObjectByType<InventoryManager>().transform);

                    Destroy(AS.transform.GetChild(0).gameObject);
                    break;
                }
            }

            selectObj.SetActive(false);
            itemSlots.Remove(item);
            selectItem = null;
            soltarBtn.SetActive(false);
        }
    }

    public void OtherPositionItemInventory(GameObject obj)
    {
        //Metodo de colocar itens no inventario
        if (selectItem != null)
        {
            bool isAdd = false;

            //Verificar se tem um item chegando e se ele ira agrupar ou criar

            if (selectItem.isAnvil)
            {
                foreach (var i in slotsList)
                {
                    if (i.transform.childCount > 0 && i.transform.GetChild(0).GetComponent<ItemSlot>().item == selectItem.item &&
                        i.transform.GetChild(0).GetComponent<ItemSlot>().qtd < i.transform.GetChild(0).GetComponent<ItemSlot>().item.qtd &&
                        !i.transform.GetChild(0).GetComponent<ItemSlot>().item.notBunch)
                    {
                        //Pode adicionar
                        selectItem.qtd--;
                        selectItem.qtdTxt.text = selectItem.qtd.ToString("");

                        if (selectItem.qtd <= 0)
                        {
                            if (selectItem.transform.Find(selectObj.name))
                            {
                                selectObj.transform.localScale = new Vector3(1, 1, 1);
                                selectObj.transform.SetParent(transform);

                                selectObj.SetActive(false);
                            }

                            Destroy(selectItem.gameObject);
                        }

                        AcrescentarItem(i.transform.GetChild(0).GetComponent<ItemSlot>());

                        isAdd = true;
                        break;
                    }
                    else
                    {
                        isAdd = false;
                    }
                }
            }
            else
            {
                isAdd = false;
            }


            if (!isAdd)
            {
                if (obj.transform.childCount <= 0)
                {
                    selectItem.transform.position = obj.transform.position;
                    selectItem.transform.SetParent(obj.transform);

                    selectObj.transform.localScale = new Vector3(1, 1, 1);

                    selectItem.transform.localScale = new Vector3(1, 1, 1);
                    selectItem.isAnvil = false;
                }
            }

            if (handObj.transform.childCount > 0)
            {
                toolUsed = selectItem.item.tool;
                buttonAtk.SetActive(true);
            }
            else
            {
                toolUsed = Tool.none;
                buttonAtk.SetActive(false);
            }
        }
    }

    public void HandPositionItemInventory()
    {
        if (selectItem != null && selectItem.item.tool != Tool.mats && selectItem.item.tool != Tool.seeds && selectItem.item.tool != Tool.none)
        {
            if(handObj.transform.childCount <= 0)
            {
                selectItem.transform.position = handObj.transform.position;
                selectItem.transform.SetParent(handObj.transform);

                selectObj.transform.localScale = new Vector3(1, 1, 1);
                    
                selectItem.transform.localScale = new Vector3(1, 1, 1);
            }

            if (handObj.transform.childCount > 0)
            {
                toolUsed = selectItem.item.tool;
                buttonAtk.SetActive(true);
            }
            else
            {
                toolUsed = Tool.none;
                buttonAtk.SetActive(false);
            }
        }
    }

    public void OtherPositionItemAnvil(GameObject obj)
    {
        if (selectItem != null && selectItem.item.tool == Tool.mats)
        {
            if (obj.transform.childCount <= 1)
            {
                if (selectItem.GetComponent<ItemSlot>().qtd > 1)
                {
                    //Ciar um novo objeto dentro da bigorna
                    selectItem.GetComponent<ItemSlot>().qtd--;
                    selectItem.GetComponent<ItemSlot>().qtdTxt.text = selectItem.GetComponent<ItemSlot>().qtd.ToString("");

                    selectObj.transform.SetParent(gameObject.transform);

                    var it = Instantiate(selectItem,obj.transform);
                    it.transform.SetSiblingIndex(1);

                    it.GetComponent<ItemSlot>().qtd = 1;
                    it.GetComponent<ItemSlot>().qtdTxt.text = it.GetComponent<ItemSlot>().qtd.ToString("");
                }
                else
                {
                    selectItem.transform.position = obj.transform.position;
                    selectItem.transform.SetParent(obj.transform);
                    selectItem.transform.SetSiblingIndex(1);
                    selectItem.isAnvil = true;

                    if(!selectItem.isAnvil)
                        selectObj.SetActive(false);

                    selectObj.transform.localScale = new Vector3(1, 1, 1);

                    selectItem.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                }
            }
        }
    }

    public void PlantPositionItemPlanted(GameObject obj)
    {
        if(selectItem != null && selectItem.item.tool == Tool.seeds)
        {
            if(obj.transform.childCount <= 0)
            {
                if (selectItem.GetComponent<ItemSlot>().qtd > 1)
                {
                    //Ciar um novo objeto dentro da bigorna
                    selectItem.GetComponent<ItemSlot>().qtd--;
                    selectItem.GetComponent<ItemSlot>().qtdTxt.text = selectItem.GetComponent<ItemSlot>().qtd.ToString("");

                    selectObj.transform.SetParent(gameObject.transform);

                    var it = Instantiate(selectItem, obj.transform);
                    it.transform.SetSiblingIndex(1);

                    it.GetComponent<ItemSlot>().qtd = 1;
                    it.GetComponent<ItemSlot>().qtdTxt.text = it.GetComponent<ItemSlot>().qtd.ToString("");
                }
                else
                {
                    selectItem.transform.position = obj.transform.position;
                    selectItem.transform.SetParent(obj.transform);
                    selectItem.transform.SetSiblingIndex(1);
                    selectItem.isAnvil = true;

                    if (!selectItem.isAnvil)
                        selectObj.SetActive(false);
                }
            }
        }
    }

    public void DesSelect()
    {
        if (selectItem != null)
        {
            foreach (var i in itemSlots)
            {
                i.isSelect = false;
            }

            selectObj.SetActive(false);
            soltarBtn.SetActive(false); 
            selectItem = null;
        }
    }

    /*
     *  foreach(var i in slotsList)
            {
                if (i.transform.childCount > 0)
                {
                    if (i.transform.GetChild(0).GetComponent<ItemSlot>().item == selectItem.item && i.transform.GetChild(0).GetComponent<ItemSlot>() != selectItem)
                    {
                       
                        if (i.transform.GetChild(0).GetComponent<ItemSlot>().qtd < selectItem.item.qtd)
                        {
                            selectItem.isAnvil = false;

                            AcrescentarItem(i.transform.GetChild(0).GetComponent<ItemSlot>());
                        }
                        else
                        {
                            foreach (var j in slotsList)
                            {
                                if (j.transform.childCount <= 0)
                                {
                                    selectItem.isAnvil = false;
                                    CreateItem(j.transform, selectItem.item);
                                    break;
                                }
                            }
                        }

                        selectItem.qtd--;
                        selectItem.qtdTxt.text = selectItem.qtd.ToString("");

                        RemoveItemInventory(selectItem, 1);

                        isAdd = true;
                        break;
                        
                    }
                    else
                    {
                        isAdd = false;
                    }
                }
            } foreach(var i in slotsList)
            {
                if (i.transform.childCount > 0)
                {
                    if (i.transform.GetChild(0).GetComponent<ItemSlot>().item == selectItem.item && i.transform.GetChild(0).GetComponent<ItemSlot>() != selectItem)
                    {
                       
                        if (i.transform.GetChild(0).GetComponent<ItemSlot>().qtd < selectItem.item.qtd)
                        {
                            selectItem.isAnvil = false;

                            AcrescentarItem(i.transform.GetChild(0).GetComponent<ItemSlot>());
                        }
                        else
                        {
                            foreach (var j in slotsList)
                            {
                                if (j.transform.childCount <= 0)
                                {
                                    selectItem.isAnvil = false;
                                    CreateItem(j.transform, selectItem.item);
                                    break;
                                }
                            }
                        }

                        selectItem.qtd--;
                        selectItem.qtdTxt.text = selectItem.qtd.ToString("");

                        RemoveItemInventory(selectItem, 1);

                        isAdd = true;
                        break;
                        
                    }
                    else
                    {
                        isAdd = false;
                    }
                }
            }
     
     */

}

public enum Tool
{
    sword,
    axe,
    pickaxe,
    shovel,
    fishing,
    mats,
    seeds,
    irrigation,
    edible,
    seller,
    none
}
