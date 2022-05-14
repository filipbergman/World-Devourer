using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLayerHandler : BlockLayerHandler
{
    public float terrainHeightLimit = 25;

    public static List<Vector3Int> treeLeavesStaticLayout = new List<Vector3Int>()
    {
        new Vector3Int(-2, 0, -2),
        new Vector3Int(-2, 0, -1),
        new Vector3Int(-2, 0, 0),
        new Vector3Int(-2, 0, 1),
        new Vector3Int(-2, 0, 2),
        new Vector3Int(-1, 0, -2),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(-1, 0, 2),
        new Vector3Int(0, 0, -2),
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, 2),
        new Vector3Int(1, 0, -2),
        new Vector3Int(1, 0, -1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 0, 1),
        new Vector3Int(1, 0, 2),
        new Vector3Int(2, 0, -2),
        new Vector3Int(2, 0, -1),
        new Vector3Int(2, 0, 0),
        new Vector3Int(2, 0, 1),
        new Vector3Int(2, 0, 2),
        new Vector3Int(-1, 1, -1),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 1, 1),
        new Vector3Int(0, 1, -1),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 1, 1),
        new Vector3Int(1, 1, -1),
        new Vector3Int(1, 1, 0),
        new Vector3Int(1, 1, 1),
        new Vector3Int(0, 2, 0)
    };

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (chunkData.worldPosition.y < 0)
            return false;

        if(surfaceHeightNoise < terrainHeightLimit 
            && chunkData.treeData.treePositions.Contains(new Vector2Int(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z)))
        {
            Vector3Int chunkCoordinates = new Vector3Int(x, surfaceHeightNoise, z);
            BlockType type = Chunk.GetBlockFromChunkCoordinates(chunkData, chunkCoordinates);

            if (type == BlockType.Grass_Dirt)
            {
                Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.Dirt);
                int treeHeight = 5;
                //int treeHeight = Random.Range(3, 6); // Unity crashes?
                for (int i = 1; i < treeHeight; i++)
                {
                    chunkCoordinates.y = surfaceHeightNoise + i;
                    Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.TreeTrunk);
                }
                foreach (Vector3Int leafPosition in treeLeavesStaticLayout) 
                {
                    chunkData.treeData.treeLeavesSolid.Add(new Vector3Int(x + leafPosition.x, surfaceHeightNoise + treeHeight + leafPosition.y, z + leafPosition.z));
                }
            }
        }
        return false;
    }
}
