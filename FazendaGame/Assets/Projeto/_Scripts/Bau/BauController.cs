using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BauController : MonoBehaviour
{
    public static BauController current;

    [Header("Slots")]
    public GameObject itemSlot;
    public Transform parentSlot;

    [HideInInspector] public List<GameObject> slots;

    [Space]

    [SerializeField] private int qtdSlots;

    [HideInInspector] public GameObject bau;

    void Awake()
    {
        if (current == null)
            current = this;
    }

    void Start()
    { 
        for(int i = 0; i < qtdSlots; i++)
        {
            var sl = Instantiate(itemSlot, parentSlot);
            slots.Add(sl);
            sl.GetComponent<Button>().onClick.AddListener(() => InventoryManager.current.OtherPositionItemChest(sl.gameObject));
        }
    }

    void Update()
    {
       
    }
}
