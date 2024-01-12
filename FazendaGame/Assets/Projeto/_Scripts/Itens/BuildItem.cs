using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildItem : MonoBehaviour
{
    public TextMeshProUGUI name_;
    public TextMeshProUGUI coins;
    public Image icone;

    [HideInInspector] public ItemBuild item;

    public void SelectedItemBuild()
    {
        if (item != BuildSystem.itemSelecionado)
        {
            BuildSystem.indice = 0;

            BuildSystem.itemSelecionado = item;
            BuildSystem.highlightTile = item.tile;

            if(item.isRot)
                FindFirstObjectByType<BuildManager>().rotBtn.gameObject.SetActive(true);
            else
                FindFirstObjectByType<BuildManager>().rotBtn.gameObject.SetActive(false);

            FindFirstObjectByType<BuildSystem>().RestartTile();
        }
    }
}
