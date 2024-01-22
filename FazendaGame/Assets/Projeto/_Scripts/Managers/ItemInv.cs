using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Inventory",menuName = "Item/Inventario")]
public class ItemInv : ScriptableObject
{
    [Header("Textos")]
    public string name_;

    [TextArea]
    public string description_;

    [Header("Itens")]

    [Space]
    public Sprite icone;

    [Space]
    public GameObject prefab;

    [Header("Pode ser agrupado")]
    [Space]
    public bool notBunch;

    [Header("Quantidade Maxima")]
    [Space]
    public int qtd;

    [Header("Vida Tool")]
    [Space]
    public bool islife;
    public int life;
    public float damage;

    [Header("Ferramenta")]
    public Tool tool;
    public Nivel toolNivel;

    [Header("Seed")]
    public List<Sprite> seedGrowth;
    public GameObject plantSeed;
    public bool water;

    [Header("Anvil Id")]
    public int id;

}
