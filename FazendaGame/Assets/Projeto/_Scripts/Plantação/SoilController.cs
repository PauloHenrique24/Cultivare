using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoilController : MonoBehaviour
{
    private PlayerController player;

    public float speed;

    [HideInInspector] public int nivel = 0; //0 == cavar,1 == plantar,2 == regar,3 == coletar

    [Header("Button")]
    private GameObject buttonPlant;

    private bool isMov;

    public Sprite defaultSolo;

    [HideInInspector] public bool plantado;
    [HideInInspector] public ItemInv item;
    [HideInInspector] public int indiceSpritePlant;

    [Header("Regar")]
    private int qtdWater = 2;
    private int water;

    private bool isWater;

    private bool PlayerInBox;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        buttonPlant = PlantManager.current.buttonPlant;
    }

    void FixedUpdate()
    {
        if (PlayerInBox)
        {
            buttonPlant.GetComponent<Button>().onClick.RemoveAllListeners();

            switch (nivel)
            {
                case 0:
                    if (InventoryManager.current.handObj.transform.childCount > 0
                        && InventoryManager.current.handObj.transform.GetChild(0).GetComponent<ItemSlot>().item.tool == Tool.shovel)
                    {
                        ButtonStylePlant(PlantManager.current.niveisImgButton[nivel]);
                        buttonPlant.GetComponent<Button>().onClick.AddListener(() => Cavar());
                    }
                    break;

                case 1:
                    ButtonStylePlant(PlantManager.current.niveisImgButton[nivel]);
                    buttonPlant.GetComponent<Button>().onClick.RemoveAllListeners();

                    buttonPlant.GetComponent<Button>().onClick.AddListener(() => Plantar());
                    break;

                case 2:
                    if (InventoryManager.current.handObj.transform.childCount > 0 &&
                        InventoryManager.current.handObj.transform.GetChild(0).GetComponent<ItemSlot>().item.tool == Tool.irrigation &&
                        InventoryManager.current.handObj.transform.GetChild(0).GetComponent<ItemSlot>().life > 0)
                    {
                        ButtonStylePlant(PlantManager.current.niveisImgButton[nivel]);
                        buttonPlant.GetComponent<Button>().onClick.RemoveAllListeners();

                        buttonPlant.GetComponent<Button>().onClick.AddListener(() => Regar());
                    }
                    break;

                case 3:
                    ButtonStylePlant(PlantManager.current.niveisImgButton[nivel]);
                    buttonPlant.GetComponent<Button>().onClick.RemoveAllListeners();

                    buttonPlant.GetComponent<Button>().onClick.AddListener(() => Colher());
                    break;

                default:
                    nivel = 0;
                    break;
            }
            isMov = true;
        }
        else
        {
            if (isMov)
            {
                buttonPlant.SetActive(false);
                isMov = false;
            }
        }

        if (isWater)
        {
            buttonPlant.SetActive(false);
        }

    }

    //nivel 0
    public void Cavar()
    {
        buttonPlant.SetActive(false);
        PlayerController.isMov = true;
        player.GetComponent<Animator>().SetInteger("transition", 2);

        var pos = new Vector2(transform.position.x + .47f, transform.position.y + .1f);

        if (!player.isPlant)
        {
            InventoryManager.current.handObj.transform.GetChild(0).GetComponent<ItemSlot>().life--;
            buttonPlant.GetComponent<Button>().onClick.RemoveAllListeners();
            nivel++;
            player.isPlant = true;
        }

        player.posPlantPlayer = pos;

        var lc = player.transform.localScale;
        lc.x = -1;
        player.transform.localScale = lc;

        StartCoroutine(Soil());
    }

    IEnumerator Soil()
    {
        int soilNum = 0;
        for(int i = 0; i < PlantManager.current.soils.Count; i++)
        {
            GetComponent<SpriteRenderer>().sprite = PlantManager.current.soils[soilNum];

            soilNum++;

            yield return new WaitForSeconds(2f);
        }

        player.isPlant = false;
        PlayerController.isMov = false;

        player.GetComponent<Animator>().SetInteger("transition", 0);

        PlantManager.current.planted = false;

    }

    //nivel 1
    public void Plantar()
    {
        buttonPlant.SetActive(false);

        PlayerController.isMov = true;

        player.GetComponent<Animator>().SetInteger("transition", 3);

        var pos = new Vector2(transform.position.x + .20f, transform.position.y + .08f);

        player.posPlantPlayer = pos;

        player.isPlant = true;

        var lc = player.transform.localScale;
        lc.x = -1;
        player.transform.localScale = lc;

        PlantManager.current.soilObj = gameObject;

        PlantManager.current.plantarUI.SetActive(true);
        PlantManager.current.closePlant.onClick.AddListener(ClosePlant);
    }

    public void ClosePlant()
    {
        PlayerController.isMov = false;

        player.GetComponent<Animator>().SetInteger("transition", 1);

        PlantManager.current.soilObj = null;

        player.isPlant = false;

        PlantManager.current.plantarUI.SetActive(false);
    }

    //nivel 2
    public void Regar()
    {
        buttonPlant.SetActive(false);
        PlayerController.isMov = true;

        InventoryManager.current.handObj.transform.GetChild(0).GetComponent<ItemSlot>().life--;


        player.GetComponent<Animator>().SetInteger("transition", 4);

        var pos = new Vector2(transform.position.x + .47f, transform.position.y + .12f);
        player.isPlant = true;
        player.posPlantPlayer = pos;

        var lc = player.transform.localScale;
        lc.x = -1;
        player.transform.localScale = lc;

        StartCoroutine(Water());
    }

    IEnumerator Water()
    {
        yield return new WaitForSeconds(1.5f);
        water++;

        if (water >= qtdWater && indiceSpritePlant < item.seedGrowth.Count - 1)
        {
            indiceSpritePlant++;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.seedGrowth[indiceSpritePlant];
            water = 0;
        }
        else if (indiceSpritePlant >= item.seedGrowth.Count - 1)
        {
            //Pronto para a colheita
            nivel++;
        }

        player.GetComponent<Animator>().SetInteger("transition", 1);
        player.isPlant = false;
        PlayerController.isMov = false;

        isWater = true;
        yield return new WaitForSeconds(4f);
        isWater = false;
    }

    //nivel 3
    public void Colher()
    {
        //Coleta o item mais a semente e restarta o solo
        int qtdSeed = Random.Range(1, 3);

        var plant = Instantiate(item.plantSeed, transform.position, Quaternion.identity);
        plant.AddComponent<Rigidbody2D>().AddForce(Vector3.up * 4,ForceMode2D.Impulse);

        int posRig = Random.Range(0, 2);

        if (posRig == 1)
            plant.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(.1f, 1f), ForceMode2D.Impulse);
        else
            plant.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Random.Range(.1f, 1f), ForceMode2D.Impulse);

        Destroy(plant.GetComponent<Rigidbody2D>(),.8f);

        for (int i = 0;i < qtdSeed; i++)
        {
            var seed = Instantiate(item.prefab, transform.position, Quaternion.identity);
            seed.AddComponent<Rigidbody2D>().AddForce(Vector3.up * 4, ForceMode2D.Impulse);

            int posRi = Random.Range(0, 2);

            if (posRi == 1)
                seed.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(.1f, 1f), ForceMode2D.Impulse);
            else
                seed.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Random.Range(.1f, 1f), ForceMode2D.Impulse);

            Destroy(seed.GetComponent<Rigidbody2D>(), .8f);
        }

        //Remover Planta
        Destroy(transform.GetChild(0).gameObject);

        buttonPlant.GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<SpriteRenderer>().sprite = defaultSolo;

        item = null;
        plantado = false;

        water = 0;
        isWater = false;
        isMov = false;

        indiceSpritePlant = 0;
        nivel = 0;
    }

    void ButtonStylePlant(Sprite btn)
    {
        buttonPlant.SetActive(true);
        buttonPlant.transform.Find("icone").GetComponent<Image>().sprite = btn;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInBox = false;
        }
    }
}
