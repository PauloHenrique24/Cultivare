using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlotsBau",menuName = "Create/SlotBau")]
public class SlotsBau : ScriptableObject
{
    public int indice;
    public List<ItemInv> itensBau;
    public List<int> lifesBau;
    public List<int> indiceSlot;
    public List<int> qtdItens;
}
