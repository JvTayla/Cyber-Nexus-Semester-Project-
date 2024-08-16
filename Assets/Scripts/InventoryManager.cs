using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public Image inventoryItemPrefab;
    public void Additem(item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            Draggleitem draggleitem = slot.GetComponent<Draggleitem>();
            if (draggleitem != null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    public void SpawnNewItem(item item, InventorySlot slot)
    {
        Image newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        Draggleitem draggleitem = newItemGo.GetComponent<Draggleitem>();
        draggleitem.InitialiseItem(item);
    }

}
