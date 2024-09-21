using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManage : MonoBehaviour
{
    /*Tile: Unity INventory System
     Author: Solo Game Dev
     Date: 12-16 August 2024
     Code Version: Unity 2022.3.38f1
     Availability :https://www.youtube.com/watch?v=AoD_F1fSFFg&t=614s
     */
    public static InventoryManage Instance;
    public List<item> inventory = new List<item>();

    public Transform itemContent;
    public GameObject itemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnItem(item item)
    {
        inventory.Add(item);
    }

    public void ListItems()
    {
        // No need to clear here; this will be handled by ClearInventoryDisplay() when closing the inventory
        foreach (var item in inventory)
        {
            GameObject obj = Instantiate(itemPrefab, itemContent);
            Image image = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var ItemIcon = image;
            var ItemName = obj.transform.Find("ItemName").GetComponent<Text>();
            Button button = obj.transform.Find("RemoveButton").GetComponent<Button>();
            var RemoveButon = button;
            ItemIcon.sprite = item.image;
            ItemName.text = item.ItemName;
          
        }
    }

    public void ClearInventoryDisplay()
    {
        // Clear the existing content from the itemContent transform
        foreach (Transform child in itemContent)
        {
            Destroy(child.gameObject);
        }
    }
}
