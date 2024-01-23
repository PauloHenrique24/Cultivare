using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceitasItem : MonoBehaviour
{
    [HideInInspector] public AnvilItem item;

    public void SelectRevenue()
    {
        int j = 0; 
        //Mostrar Receita
        for (int i = 0; i < 9; i++)
        {
            switch (item.materials[i])
            {
                case -1:
                    AnvilController.current.anvilSlots[j].transform.Find("receita").gameObject.SetActive(false);
                    break;

                default:
                    AnvilController.current.anvilSlots[j].transform.Find("receita").gameObject.SetActive(true);

                    AnvilController.current.anvilSlots[j].transform.Find("receita").GetComponent<Image>().sprite = AnvilController.current.matsSprites[item.materials[i]];
                    break;
            }
            j++;
        }
    }
}
