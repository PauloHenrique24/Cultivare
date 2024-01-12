using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BtnUse : MonoBehaviour
{
    [Header("Hand")]
    private ItemSlot hand;
    private ItemSlot handUtilize;

    private bool isHand;

    [Header("Icons")]
    public List<Sprite> icons;
    public List<Sprite> pescaSprites;

    public Sprite constructSpr;

    private Image icone;

    private Tool tool;

    [Header("Door")]
    public Sprite doorSprite;
    public static bool doorBtn;

    void Start()
    {
        icone = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        if (!BuildSystem.highlighted)
        {
            if (!doorBtn)
            {
                if (InventoryManager.current.handObj.transform.childCount > 0)
                {
                    var hand = InventoryManager.current.handObj.transform.GetChild(0).GetComponent<ItemSlot>();

                    if (handUtilize != hand)
                    {
                        isHand = false;
                    }

                    if (!isHand)
                    {
                        switch (hand.item.tool)
                        {
                            case Tool.sword:
                                icone.sprite = icons[0];
                                handUtilize = hand;
                                isHand = true;
                                break;

                            case Tool.axe:
                                icone.sprite = icons[1];
                                handUtilize = hand;
                                isHand = true;
                                break;

                            case Tool.fishing:
                                icone.sprite = icons[2];
                                handUtilize = hand;
                                isHand = true;
                                break;

                            case Tool.seller:
                                icone.sprite = icons[4];
                                isHand = true;
                                break;

                            case Tool.irrigation:
                                transform.GetChild(0).gameObject.SetActive(false);
                                handUtilize = hand;
                                isHand = true;
                                break;

                            case Tool.shovel:
                                transform.GetChild(0).gameObject.SetActive(false);
                                handUtilize = hand;
                                isHand = true;
                                break;
                        }

                        tool = hand.item.tool;
                    }
                }

                if (tool == Tool.irrigation && WaterManager.current.isPosPlayer)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    icone.sprite = icons[3];
                }

                if (tool == Tool.fishing)
                {
                    if (WaterManager.current.pescando && !WaterManager.current.fisgado)
                    {
                        //Close
                        icone.sprite = pescaSprites[2];
                    }
                    else if (WaterManager.current.pescando && WaterManager.current.fisgado)
                    {
                        icone.sprite = pescaSprites[1];
                    }
                    else if (!WaterManager.current.pescando && !WaterManager.current.fisgado)
                    {
                        icone.sprite = pescaSprites[0];
                    }
                }
            }
            else
            {
                icone.sprite = doorSprite;
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            icone.sprite = constructSpr;
        }
    }
}
