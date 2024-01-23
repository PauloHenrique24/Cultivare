using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BauManager : MonoBehaviour
{
    public int indice;
    private SlotsBau slotsBau;

    private bool isChestOpen;

    [Header("Itens In Slots")]
    [HideInInspector] public List<ItemSlot> itensSlot;

    private List<ItemInv> itens;
    private List<int> lifes;

    void Start()
    {
        BauController.current.bau = gameObject;
    }

    public void Save()
    {
        slotsBau = ScriptableObject.CreateInstance<SlotsBau>();

        string caminhoDoArquivo = "Assets/Projeto/BauItens/BauItem" + indice + ".asset";

        // Verifica se o ScriptableObject já existe
        slotsBau = AssetDatabase.LoadAssetAtPath<SlotsBau>(caminhoDoArquivo);

        if (slotsBau == null)
        {
            slotsBau = ScriptableObject.CreateInstance<SlotsBau>();
            slotsBau.indice = indice;

            AssetDatabase.CreateAsset(slotsBau, caminhoDoArquivo);
        }
        else
        {
            slotsBau.lifesBau.Clear();
            slotsBau.itensBau.Clear();
            slotsBau.indiceSlot.Clear();
            slotsBau.qtdItens.Clear();
        }

        for (int i = 0;i < itensSlot.Count; i++)
        {
            slotsBau.lifesBau.Add(itensSlot[i].life);
            slotsBau.itensBau.Add(itensSlot[i].item);
            slotsBau.qtdItens.Add(itensSlot[i].qtd);

            for(int j = 0;j < BauController.current.slots.Count; j++)
            {
                if (BauController.current.slots[j].transform.childCount > 0 && BauController.current.slots[j].transform.GetChild(0).GetComponent<ItemSlot>() == itensSlot[i])
                {
                    slotsBau.indiceSlot.Add(j);
                    break;
                }
            }
        }

        AssetDatabase.SaveAssets();
    }
}
