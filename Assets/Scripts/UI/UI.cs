using Enum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    
    [SerializeField] private GameObject main;
    
    [SerializeField] private GameObject rareItems;
    [SerializeField] private GameObject epicItems;
    [SerializeField] private GameObject legendaryItems;
    
    [SerializeField] private GameObject itemUI;

    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    public GraphicRaycaster Raycaster => raycaster;
    public EventSystem EventSystem => eventSystem;
    public bool IsShowUI
    {
        set => main.SetActive(value);
        get => main.activeSelf;
    }

    public void AddItem(Item item)
    {
        GameObject newItem = null;
        
        switch (item.Data.type)
        {
            case ItemType.Rare:
                newItem = Instantiate(itemUI, rareItems.transform);
                break;
            case ItemType.Epic:
                newItem = Instantiate(itemUI, epicItems.transform);
                break;
            case ItemType.Legendary:
                newItem = Instantiate(itemUI, legendaryItems.transform);
                break;
        }
        
        newItem.GetComponent<ItemUI>().SetItem(item);
    }
}
