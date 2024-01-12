using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour
{
    public static WaterManager current;

    [Header("Player Pos")]
    [HideInInspector] public bool isPosPlayer;

    public Button btnWater;

    private bool isremoveList;
    private PlayerController player;

    [HideInInspector] public bool pescando;
    private bool timePesca;
    [HideInInspector] public bool fisgado;

    private bool isRemoveBtn;

    private float timer;
    private float timerPesca;
    private float timerFisgado;

    [Header("Fisgado")]
    public List<GameObject> itensPescaveis;

    void Awake()
    {
        if (current == null)
            current = this;
    }

    void Start()
    {
        player =  FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (isPosPlayer)
        {
            //O Player esta sobre uma plataforma de water
            
            if (pescando)
            {
                if (!isRemoveBtn)
                {
                    btnWater.onClick.RemoveAllListeners();
                    btnWater.onClick.AddListener(CloseFishing);
                    isRemoveBtn = true;
                }
            }
            else
            {
                btnWater.onClick.RemoveAllListeners();
                btnWater.onClick.AddListener(Water);
                isremoveList = false;
            }
        }
        else
        {
            if (!isremoveList)
            {
                btnWater.onClick.RemoveAllListeners();
                isremoveList = true;
            }
        }

        if (pescando && !timePesca && !fisgado)
        {
            timer = Random.Range(1f, 5f);
            timePesca = true;
        }

        if (timePesca)
        {
            timerPesca += Time.deltaTime;

            if (timerPesca >= timer)
            {
                btnWater.onClick.RemoveAllListeners();
                btnWater.onClick.AddListener(Fisgar);

                player.GetComponent<Animator>().SetTrigger("fish");

                fisgado = true;
                timePesca = false;
                timerPesca = 0f;
            }
        }

        if (fisgado)
        {
            timerFisgado += Time.deltaTime;
            if(timerFisgado >= 1.5f)
            {
                isRemoveBtn = false;

                btnWater.onClick.RemoveAllListeners();

                fisgado = false;
                timerFisgado = 0f;
            }
        }
    }

    public void Fisgar()
    {
        CloseFishing();
        StartCoroutine(ObjPesca());
    }
    
    IEnumerator ObjPesca()
    {
        yield return new WaitForSeconds(.7f);
        int indice = Random.Range(0, itensPescaveis.Count);

        var item = Instantiate(itensPescaveis[indice], player.gameObject.transform.position, Quaternion.identity);
        item.AddComponent<Rigidbody2D>().AddForce(Vector3.up * 4, ForceMode2D.Impulse);

        int posRig = Random.Range(0, 2);

        if (posRig == 1)
            item.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(.01f, .3f), ForceMode2D.Impulse);
        else
            item.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Random.Range(.01f, .3f), ForceMode2D.Impulse);

        Destroy(item.GetComponent<Rigidbody2D>(), .8f);
    }

    public void Water()
    {
        if (InventoryManager.current.handObj.transform.childCount > 0)
        {
            var objHand = InventoryManager.current.handObj.transform.GetChild(0).GetComponent<ItemSlot>();
            switch (objHand.item.tool)
            {
                case Tool.irrigation:
                    //Encher Regador
                    if (objHand.life < objHand.item.life)
                    {
                        StartCoroutine(ToFill(objHand));
                    }
                    break;

                case Tool.fishing:
                    //Pesca
                    Pescar();
                    break;
            }
        }
    }

    IEnumerator ToFill(ItemSlot item)
    {
        player.GetComponent<Animator>().SetInteger("transition", 3);
        PlayerController.isMov = true;

        yield return new WaitForSeconds((item.item.life - item.life) * 1.5f);
        PlayerController.isMov = false;

        player.GetComponent<Animator>().SetInteger("transition", 1);

        item.life = item.item.life;
    }


    void Pescar()
    {
        PlayerController.isMov = true;
        player.GetComponent<Animator>().SetInteger("transition", -1);
        player.GetComponent<Animator>().SetInteger("fishing", 1);

        StartCoroutine(Pescando());
    }

    void CloseFishing()
    {
        isRemoveBtn = false;
        timePesca = false;
        pescando = false;
        fisgado = false;

        player.GetComponent<Animator>().SetInteger("fishing", 3);
        StartCoroutine(EndFishing());
    }

    IEnumerator EndFishing()
    {
        yield return new WaitForSeconds(1f);
        player.GetComponent<Animator>().SetInteger("fishing", 0);
        player.GetComponent<Animator>().SetInteger("transition", 1);

        btnWater.onClick.RemoveAllListeners();

        isPosPlayer = true;
        PlayerController.isMov = false;
    }

    IEnumerator Pescando()
    {
        yield return new WaitForSeconds(.5f);
        player.GetComponent<Animator>().SetInteger("fishing", 2);
        pescando = true;
    }
}
