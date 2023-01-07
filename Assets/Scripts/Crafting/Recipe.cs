using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe", menuName = "Data/Recipe")]
public class Recipe : ScriptableObject
{
    public BlockType[] blockTypes = new BlockType[4];

    public BlockType result;
}
