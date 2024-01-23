using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnvilController : MonoBehaviour
{
    public static AnvilController current;

    [Header("Slots")]
    public GameObject itemSlot;
    public Transform parentSlot;
    public int qtdSlots;

    [HideInInspector] public List<GameObject> anvilSlots;

    [Header("Sprites")]
    public List<Sprite> matsSprites;

    [Header("Receitas")]
    public GameObject itemReceita;
    public Transform parentReceita;

    [Header("Buttons")]
    public Button criarBtn;

    [Header("Saida")]
    public GameObject itemSaida;
    public Transform parentSaida;


    private void Awake()
    {
        if (current == null)
            current = this;
    }

    void Start()
    {
        //Fabricar Slots
        for (int i = 0; i < qtdSlots; i++)
        {
            //Criar item Slot
            var item = Instantiate(itemSlot, parentSlot);
            item.GetComponent<Button>().onClick.AddListener(() => InventoryManager.current.OtherPositionItemAnvil(item));

            anvilSlots.Add(item);

            //Aumentar o tamanho do slot
            item.transform.localScale = new Vector3(1.4f, 1.4f);
        }
    }

    public void GenerateReceitas(AnvilItem[] receitas)
    {
        for(int i = 0;i < parentReceita.childCount; i++)
        {
            Destroy(parentReceita.GetChild(i).gameObject);
        }

        for (int i = 0; i < receitas.Length; i++)
        {
            var itemRec = Instantiate(itemReceita, parentReceita);
            itemRec.transform.Find("icone").GetComponent<Image>().sprite = receitas[i].item.icone;
            itemRec.transform.Find("name_").GetComponent<TextMeshProUGUI>().text = receitas[i].item.name_;

            itemRec.GetComponent<ReceitasItem>().item = receitas[i];
        }
    }

    public void Close()
    {
        foreach (var i in anvilSlots)
        {
            i.transform.Find("receita").gameObject.SetActive(false);
        }

        criarBtn.onClick.RemoveAllListeners();

        PlayerController.isMov = false;
    }

    void Update()
    {
        
    }
}
