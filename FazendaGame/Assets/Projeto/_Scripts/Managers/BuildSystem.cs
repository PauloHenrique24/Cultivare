using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildSystem : MonoBehaviour
{
    public static TileBase highlightTile;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap tempTilemap;

    private Vector3Int highlightedTilePos;
    public static bool highlighted;

    [Header("Position")]
    public Transform pos;

    public Button useBtn;

    public static ItemBuild itemSelecionado;
    private bool notselect;

    [Header("Build")]
    [SerializeField] private Tilemap buildTilemap;

    public static int indice;

    void Start()
    {
        FindFirstObjectByType<BuildManager>().rotBtn.onClick.AddListener(RotateTile);

        itemSelecionado = null;
        highlighted = false;
    }

    void Update()
    {
        if (itemSelecionado != null)
        {
            HighlightTile();
            notselect = false;
        }
        else
        {
            if (!notselect) {

                FindFirstObjectByType<BuildManager>().rotBtn.gameObject.SetActive(false);

                highlighted = false;
                useBtn.onClick.RemoveListener(Contruir);
                useBtn.gameObject.SetActive(false);

                tempTilemap.SetTile(highlightedTilePos, null);
                notselect = true;
            }
        }
    }

    private Vector3Int GetMouseOnGridPos()
    {
        Vector3 mousePos = pos.position;

        Vector3Int mouseCellPos = mainTilemap.WorldToCell(mousePos);
        mouseCellPos.z = 0;

        return mouseCellPos;
    }

    private void HighlightTile()
    {
        Vector3Int mouseGridPos = GetMouseOnGridPos();
        if(highlightedTilePos != mouseGridPos)
        {
            tempTilemap.SetTile(highlightedTilePos,null);
            TileBase tile = mainTilemap.GetTile(mouseGridPos);

            if (tile)
            {
                RaycastHit2D hit = Physics2D.Raycast(tempTilemap.GetCellCenterLocal(mouseGridPos), Vector3.forward, 1f);

                if(hit.collider == null)
                {
                    tempTilemap.SetTile(mouseGridPos, highlightTile);
                    highlightedTilePos = mouseGridPos;

                    FindFirstObjectByType<BuildManager>().rotBtn.gameObject.SetActive(true);

                    useBtn.onClick.AddListener(Contruir);

                    highlighted = true;
                }
                else
                {
                    FindFirstObjectByType<BuildManager>().rotBtn.gameObject.SetActive(false);

                    useBtn.onClick.RemoveListener(Contruir);
                    useBtn.gameObject.SetActive(false);

                    highlighted = false;
                }
            }
            else
            { 
                FindFirstObjectByType<BuildManager>().rotBtn.gameObject.SetActive(false);

                useBtn.onClick.RemoveListener(Contruir);
                useBtn.gameObject.SetActive(false);

                highlighted = false;
            }
        }
    }

    public void RestartTile()
    {
        Vector3Int mouseGridPos = GetMouseOnGridPos();

        tempTilemap.SetTile(highlightedTilePos, null);
        TileBase tile = mainTilemap.GetTile(mouseGridPos);
        TileBase tileBuild = buildTilemap.GetTile(mouseGridPos);

        if (tile && !tileBuild)
        {
            tempTilemap.SetTile(mouseGridPos, highlightTile);
            highlightedTilePos = mouseGridPos;

            highlighted = true;
        }
        else
        {
            highlighted = false;
        }
    }

    public void RotateTile()
    {
        if (itemSelecionado.tiles.Count > 0 && itemSelecionado.isRot)
        {
            if (indice < itemSelecionado.tiles.Count - 1)
            {
                indice++;
            }
            else
            {
                indice = 0;
            }

            highlightTile = itemSelecionado.tiles[indice];

            RestartTile();
        }
    }

    private bool isContruct;

    public void Contruir()
    {
        if (!isContruct)
        {
            if (PlayerController.Money >= itemSelecionado.value)
            {
                if (itemSelecionado.isTilemap)
                {
                    TileBase tile = buildTilemap.GetTile(highlightedTilePos);
                    if (!tile)
                    {
                        //Contruir apartir de tiles
                        buildTilemap.SetTile(highlightedTilePos, highlightTile);
                        PlayerController.Money -= itemSelecionado.value;
                    }
                }

                if (!itemSelecionado.isTilemap)
                {
                    //Contruir apartir de prefabs
                    var itsele = Instantiate(itemSelecionado.buildTile, buildTilemap.GetCellCenterLocal(highlightedTilePos), Quaternion.identity);

                    if (itemSelecionado.isRot)
                    {
                        itsele.GetComponent<SpriteRenderer>().sprite = GetSpriteAtCell(itemSelecionado.tiles[indice]);
                    }
                }
            }

            if (PlayerController.Money < itemSelecionado.value)
            {
                itemSelecionado = null;
                highlighted = false;
                useBtn.onClick.RemoveListener(Contruir);
                useBtn.gameObject.SetActive(false);
            }

            isContruct = true;
            StartCoroutine(Contruct());
        }
    }

    IEnumerator Contruct()
    {
        yield return new WaitForSeconds(.5f);
        isContruct = false;
    }


    Sprite GetSpriteAtCell(TileBase tile)
    {
        if (tile != null && tile is Tile)
        {
            // Se o Tile for um Tile de Sprite (Tile), você pode obter o sprite
            return ((Tile)tile).sprite;
        }

        return null; // Nenhum sprite encontrado na posição especificada
    }
}
