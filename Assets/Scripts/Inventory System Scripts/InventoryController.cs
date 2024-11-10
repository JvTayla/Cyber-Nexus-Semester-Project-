/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
     public item Item;
    public static InventoryController Instance;

    public InventoryManage inventoryManage;
    

    public void AddItem(item newitem)
    {
        Item = newitem;
    }
    private void Awake()
    {
     
        Instance = this;    
    }

    public void UseItem()
    {
        if (Item == null)
        {
            Debug.LogWarning("No item selected to use.");
            return;
        } 
        if(Item != null) { Debug.LogWarning(" item selected to use.") ; return; } 


        switch (Item.actionType)
        {
            case item.ActionType.Research:
                GameObject gameobj1 = Instantiate(inventoryManage.Testtube, inventoryManage.holdposition.position, Quaternion.identity);
                GameObject testtubeInstance = gameobj1;
                testtubeInstance.transform.SetParent(inventoryManage.holdposition); // Attach to hold position
                Debug.Log("Using item: Research action");
                break;

            case item.ActionType.experiment: // Corrected ActionType reference
                GameObject chemicalsInstance = Instantiate(inventoryManage.Chemicals, inventoryManage.holdposition.position, Quaternion.identity);
                chemicalsInstance.transform.SetParent(inventoryManage.holdposition); // Attach to hold position
                Debug.Log("Using item: Experiment action");
                break;

            default:
                Debug.LogWarning("Action type not recognized.");
                break;
        }
    }
}*/
