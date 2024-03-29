using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public List<BlockItem> itemList;

    public void BreakBlock(BlockType blockType, Vector3 worldPos)
    {
        BlockItem blockItem = itemList.Find(item => item.blockType == blockType);
        if(blockItem != null)
        {
            Instantiate(blockItem.itemPrefab, worldPos, Quaternion.identity, transform.Find("/GroundItems"));
        }
    }

    public BlockItem BlockTypeToItem(BlockType blockType)
    {
        foreach (var item in itemList)
        {
            if (item.blockType == blockType)
                return item;
        }
        return null;
    }
}
