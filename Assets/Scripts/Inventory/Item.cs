using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item
{
    public GameObject itemPrefab;
    public int id;
    public BlockType blockType;
    public Texture2D blockImage;

    public Item(GameObject itemPrefab, int id, BlockType blockType, Texture2D blockImage)
    {
        this.itemPrefab = itemPrefab;
        this.id = id;
        this.blockType = blockType;
        this.blockImage = blockImage;
    }
}

[System.Serializable]
public class BlockItem : Item
{
    public BlockItem(GameObject itemPrefab, int id, string name, BlockType blockType, Texture2D blockImage) : base(itemPrefab, id, blockType, blockImage) 
    {
        
    }
    
}
