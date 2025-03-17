using Enum;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemSO : ScriptableObject
{
   public uint id;
   public string name;
   public float weight;
   public ItemType type;
}
