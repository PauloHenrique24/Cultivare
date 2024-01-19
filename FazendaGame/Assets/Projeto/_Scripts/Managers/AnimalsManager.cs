using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsManager : MonoBehaviour
{
    public static AnimalsManager current;

    [Header("Animals Type")]
    public List<Animal> animals;


    void Awake()
    {
        if (current == null)
            current = this;

    }
}


[System.Serializable]
public class Animal
{
    public string name;
    public AnimalType type;
    public GameObject objPrefab;

    public Animal(string name, AnimalType type, GameObject objPrefab)
    {
        this.name = name;
        this.type = type;
        this.objPrefab = objPrefab;
    }
}

public enum AnimalType
{
    Chicken,
    Cow,
    Pig,
    Sheep
}
