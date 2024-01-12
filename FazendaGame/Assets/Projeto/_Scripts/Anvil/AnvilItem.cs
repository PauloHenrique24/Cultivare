using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Anvil Item",menuName = "Item/bigorna")]
public class AnvilItem : ScriptableObject
{
    [Header("Item Inventario")]
    public ItemInv item;

    [Header("Receita 0==wood 1==iron 2==coal !=none")]
    public int[] materials = new int[9];
}