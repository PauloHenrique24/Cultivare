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
        
    }
}
