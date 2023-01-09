using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public List<Recipe> recipeList = new List<Recipe>();

    public Transform craftedSlot;

    public void CheckRecipes(List<InventorySlot> craftingSlots)
    {
        // Create a recipe of current inventory crafting slots:
        Recipe tryRecipe = ScriptableObject.CreateInstance<Recipe>();
        for (int i = 0; i < craftingSlots.Count; i++)
        {
            if (craftingSlots[i].item == null)
                tryRecipe.blockTypes[i] = BlockType.Nothing;
            else
                tryRecipe.blockTypes[i] = craftingSlots[i].item.blockType;
        }

        // Try the created recipe against the list of all recipes:
        foreach (var recipe in recipeList)
        {
            int equalityCheck = 0;
            for (int i = 0; i < 4; i++)
            {
                if (recipe.blockTypes[i] == tryRecipe.blockTypes[i])
                    equalityCheck++;
            }
            if (equalityCheck == 4)
            {
                Debug.Log("ITEM FOUND: " + recipe.name);
                FindObjectOfType<InventoryUI>().SpawnCraftedItem(recipe.result);
            }
            else
            {
                FindObjectOfType<InventoryUI>().DespawnCraftedItem();
            }
        }
    }
}
