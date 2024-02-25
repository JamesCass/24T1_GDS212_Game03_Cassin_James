using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Tile TilePrefab;
    public Transform TileSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2.15f, 3.62f);

    [HideInInspector]
    public List<Tile> TileList;

    private Vector2 offset = new Vector2(1.5f, 1.52f);
    private void Start()
    {
        SpawnTileMesh(4, 5, StartPosition, offset, false);
        MoveTile(4, 5, StartPosition, offset);
    }

    private void Update()
    {
        
    }

    private void SpawnTileMesh(int rows, int columns, Vector2 Pos, Vector2 offset, bool scaleDown)
    {
        for(int col = 0; col < columns; col++)
        {
            for(int row = 0; row < rows; row++)
            {
                var tempTile = (Tile)Instantiate(TilePrefab, TileSpawnPosition.position, TileSpawnPosition.transform.rotation);

                tempTile.name = tempTile.name + 'c' + col + 'r' + row;

                TileList.Add(tempTile);
            }
        }
    }

    private void MoveTile(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for(var col =0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, TileList[index]));
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, Tile obj)
    {
        var randomDis = 7;

        while(obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return 0;
        }
    }
}
