using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Scriptable object/item")]

 

public class item : ScriptableObject
{

    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public string ItemName;  
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only Ui")]
    public bool stackable = true;
    [Header("Both")]
    public Sprite image;
    public Button RemoveItem;
    public enum ItemType
    {
        Tube,
        Chemicals, 
        Tag,
        Battery,

    }

    public enum ActionType
    {
        Research,
        experiment, 
        Security,
        Nuclear,

    }

}


