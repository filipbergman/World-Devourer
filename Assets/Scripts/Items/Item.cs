using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public GameObject itemPrefab;
    public int id;

    public Item(GameObject itemPrefab, int id)
    {
        this.itemPrefab = itemPrefab;
        this.id = id;
    }
}

[System.Serializable]
public class BlockItem : Item
{
    public BlockType blockType;
    public BlockItem(GameObject itemPrefab, int id, string name, BlockType blockType) : base(itemPrefab, id) 
    {
        this.blockType = blockType;
    }
    
}
