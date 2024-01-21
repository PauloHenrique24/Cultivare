using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalledGenerator : MonoBehaviour
{
    [Header("Start")]
    public int maxAnimals = 5;

    private int animalInWalled = 1;

    [Header("Animal")]
    public AnimalType typeAnimal;

    [Header("Directions")]
    public List<Transform> direction; //xMax, xMin, yMax, yMin

    [Header("Painel de Controle")]
    public Transform targetPainel;

    [Header("Cochos")]
    public GameObject cochoAguaObj;
    public GameObject cochoComidaObj;

    [HideInInspector] public float cochoAgua = 100;
    [HideInInspector] public float cochoComida = 100;

    public List<Sprite> aguaCochoImg;
    public List<Sprite> comidaCochoImg;

    void Start()
    {
        cochoAgua = 100;
        cochoComida = 100;

        for (int i = 0; i < animalInWalled; i++)
        {
            foreach (var a in AnimalsManager.current.animals)
            {
                if (a.type == typeAnimal)
                {
                    Instantiate(a.objPrefab, transform.position, Quaternion.identity, transform);
                    break;
                }
            }
        }
    }

    void Update()
    {
        cochoAgua = Mathf.Clamp(cochoAgua, 0, 100);
        cochoComida = Mathf.Clamp(cochoComida, 0, 100);
    }

    public void ReloadImageCocho(char letra)
    {
        if(letra == 'a')
        {
            switch (cochoAgua)
            {
                case float n when (n > 50):
                    cochoAguaObj.GetComponent<SpriteRenderer>().sprite = aguaCochoImg[0];
                    break;

                case float n when (n > 25 && n <= 50):
                    cochoAguaObj.GetComponent<SpriteRenderer>().sprite = aguaCochoImg[1];
                    break;

                case float n when (n > 0 && n <= 25):
                    cochoAguaObj.GetComponent<SpriteRenderer>().sprite = aguaCochoImg[2];
                    break;

                case float n when (n <= 0):
                    cochoAguaObj.GetComponent<SpriteRenderer>().sprite = aguaCochoImg[3];
                    break;
            }
        }else if(letra == 'c')
        {
            switch (cochoComida)
            {
                case float n when (n > 50):
                    cochoComidaObj.GetComponent<SpriteRenderer>().sprite = comidaCochoImg[0];
                    break;

                case float n when (n > 25 && n <= 50):
                    cochoComidaObj.GetComponent<SpriteRenderer>().sprite = comidaCochoImg[1];
                    break;

                case float n when (n > 0 && n <= 25):
                    cochoComidaObj.GetComponent<SpriteRenderer>().sprite = comidaCochoImg[2];
                    break;

                case float n when (n <= 0):
                    cochoComidaObj.GetComponent<SpriteRenderer>().sprite = comidaCochoImg[3];
                    break;
            }
        }
    }
}
