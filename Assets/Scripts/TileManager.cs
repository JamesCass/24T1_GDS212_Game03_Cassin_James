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

    //tile gaps
    private Vector2 offset = new Vector2(1.5f, 1.52f);
    private Vector2 offsetFor15Pairs = new Vector2(1.08f, 1.22f);
    private Vector2 offsetFor20Pairs = new Vector2(1.08f, 1.0f);
    //scale down tiles
    private Vector3 newScaleDown = new Vector3(0.6f, 0.8f, 0.001f);

    private List<Material> materialList = new List<Material>();
    private List<string> texturePathList = new List<string>();
    private Material firstMaterial;
    private string firstTexturePath;

    private void Start()
    {
        LoadMaterials();


        if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E10Pairs)
        {
            SpawnTileMesh(4, 5, StartPosition, offset, false);
            MoveTile(4, 5, StartPosition, offset);
        }

        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E15Pairs)
        {
            SpawnTileMesh(5, 6, StartPosition, offset, false);
            MoveTile(5, 6, StartPosition, offsetFor15Pairs);
        }

        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E20Pairs)
        {
            SpawnTileMesh(5, 8, StartPosition, offset, true);
            MoveTile(5, 8, StartPosition, offsetFor20Pairs);
        }

    }

    private void LoadMaterials()
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetPuzzleThemeTextureDirectoryName();
        var pairNumber = (int)GameSettings.Instance.GetPairNumber();
        const string matBaseName = "Tile";
        var firstMaterialName = "Back";

        for (var index = 1; index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            texturePathList.Add(currentTextureFilePath);
        }

        firstTexturePath = textureFilePath + firstMaterialName;
        firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
    }

    private void Update()
    {

    }

    private void SpawnTileMesh(int rows, int columns, Vector2 Pos, Vector2 offset, bool scaleDown)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempTile = (Tile)Instantiate(TilePrefab, TileSpawnPosition.position, TilePrefab.transform.rotation);

                if (scaleDown)
                {
                    tempTile.transform.localScale = newScaleDown;
                }

                tempTile.name = tempTile.name + 'c' + col + 'r' + row;

                TileList.Add(tempTile);
            }
        }

        ApplyTextures();
    }

    public void ApplyTextures()
    {
        var randomMaterialIndex = Random.Range(0, TileList.Count);
        var AppliedTimes = new int[materialList.Count];

        for (int i = 0; i < materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }

        foreach (var o in TileList)
        {
            var randPrevious = randomMaterialIndex;
            var counter = 0;
            var forceMat = false;

            while (AppliedTimes[randomMaterialIndex] >= 2 || ((randPrevious == randomMaterialIndex) && !forceMat))
            {
                randomMaterialIndex = Random.Range(0, materialList.Count);
                counter++;
                if (counter > 100)
                {
                    for (var j = 0; j < materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            randomMaterialIndex = j;
                            forceMat = true;
                        }
                    }

                    if (forceMat == false)
                        return;
                }
            }

            o.SetFirstMaterial(firstMaterial, firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(materialList[randomMaterialIndex], texturePathList[randomMaterialIndex]);
            AppliedTimes[randomMaterialIndex] += 1;
            forceMat = false;
        }
    }


    private void MoveTile(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
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

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return 0;
        }
    }
}