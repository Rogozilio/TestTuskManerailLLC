using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI weight;

    private Item _item;

    public Item Item => _item;
    public void SetItem(Item item)
    {
        _item = item;
        name.text = item.Data.name;
        weight.text = item.Data.weight.ToString();
    }

    public void RemoveItem()
    {
        if(!_item) return;
        Debug.Log("Des");
        
        _item.MoveToRespawn(() =>
        {
            _item.IsInBackpack = false;
            _item = null;
        });
      
        Destroy(gameObject);
    }
}
