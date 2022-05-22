using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public GameObject itemPrefab;
    public int id;
    public BlockType blockType;

    public Item(GameObject itemPrefab, int id, BlockType blockType)
    {
        this.itemPrefab = itemPrefab;
        this.id = id;
        this.blockType = blockType;
    }
}

[System.Serializable]
public class BlockItem : Item
{
    public BlockItem(GameObject itemPrefab, int id, string name, BlockType blockType) : base(itemPrefab, id, blockType) 
    {
        
    }
    
}
