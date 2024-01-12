using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    [Header("Build Itens")]
    public List<ItemBuild> itensBuild = new List<ItemBuild>();

    [Header("Create Itens")]
    public Transform parentItens;
    public GameObject itemPref;

    [Header("Button")]
    public Button rotBtn;
     
    void Start()
    {
        for(int i = 0; i < itensBuild.Count; i++)
        {
            var it = Instantiate(itemPref,parentItens);
            
            it.GetComponent<BuildItem>().icone.sprite = itensBuild[i].icone;
            it.GetComponent<BuildItem>().name_.text = itensBuild[i].name_;
            it.GetComponent<BuildItem>().coins.text = itensBuild[i].value + " Coins";

            it.GetComponent<BuildItem>().item = itensBuild[i];
        }

        rotBtn.gameObject.SetActive(false);
    }

    public void Close()
    {
        BuildSystem.itemSelecionado = null;
        BuildSystem.highlightTile = null;

        rotBtn.gameObject.SetActive(false);
    }
}
