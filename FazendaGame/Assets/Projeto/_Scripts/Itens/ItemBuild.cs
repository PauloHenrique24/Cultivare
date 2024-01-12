using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BuildItem", menuName = "Item/Build")]
public class ItemBuild : ScriptableObject
{
    [Header("Item Menu")]
    public string name_;
    public int value;
    public Sprite icone;

    [Space]
    public bool isTilemap;
    public bool isRot;

    [Header("In Scene")]
    public TileBase tile;
    public GameObject buildTile;

    public List<TileBase> tiles;
}
