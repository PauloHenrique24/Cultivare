using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    public static PlantManager current;

    [Header("Button")]
    public GameObject buttonPlant;
    public List<Sprite> niveisImgButton;

    [Header("Soils Sprites")]
    public List<Sprite> soils;

    [HideInInspector] public bool planted;

    [Header("Plantar")]
    public GameObject plantarUI;
    public Button closePlant;

    public Button SlotBtn;

    [Header("Btn Plantar")]
    public Button btnPlantar;
    [HideInInspector] public GameObject soilObj;
    
    void Awake()
    {
        if (current == null)
            current = this;

        SlotBtn.onClick.AddListener(() => InventoryManager.current.PlantPositionItemPlanted(SlotBtn.gameObject));

        btnPlantar.onClick.AddListener(PlantarBtn);
    }

    void Update()
    {
        if(SlotBtn.gameObject.transform.childCount > 0)
        {
            btnPlantar.interactable = true;
        }
        else
        {
            btnPlantar.interactable = false;
        }
    }

    public void PlantarBtn()
    {
        if(SlotBtn.transform.childCount > 0 && soilObj != null)
        {
            soilObj.GetComponent<SoilController>().plantado = true;
            soilObj.GetComponent<SoilController>().nivel++;
            soilObj.GetComponent<SoilController>().item = SlotBtn.transform.GetChild(0).GetComponent<ItemSlot>().item;

            GameObject seed = new("semente");
            seed.transform.SetParent(soilObj.transform);
            seed.transform.localPosition = new Vector3(0, -0.0044f, 0);
            seed.AddComponent<SpriteRenderer>().sprite = SlotBtn.transform.GetChild(0).GetComponent<ItemSlot>().item.seedGrowth[0];
            seed.GetComponent<SpriteRenderer>().sortingOrder = 2;


            plantarUI.SetActive(false);

            PlayerController.isMov = false;
            FindFirstObjectByType<PlayerController>().isPlant = false;
            FindFirstObjectByType<PlayerController>().GetComponent<Animator>().SetInteger("transition", 1);

            if (SlotBtn.transform.GetChild(0).Find(InventoryManager.current.selectObj.name))
            {
                InventoryManager.current.selectObj.transform.SetParent(InventoryManager.current.transform);
                InventoryManager.current.selectObj.SetActive(false);
            }

            Destroy(SlotBtn.transform.GetChild(0).gameObject);
            soilObj = null;
        }
    }
}
