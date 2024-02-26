using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public Tile TilePrefab;
    public Transform TileSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2.15f, 3.62f);

    [Space]
    [Header("EndGameScreen")]
    public GameObject EndGamePanel;
    public GameObject NewBestTimeText;
    public GameObject YourTimeText;
    public GameObject EndTimeText;

    public enum GameState {NoAction, MovingOnPositions, DeletingPuzzles, FlipBack, Checking, GameEnd};

    public enum PuzzleState { PuzzleRotating, CanRotate};

    public enum RevealedState { NoRevealed, OneRevealed, TwoRevealed };

    [HideInInspector]
    public GameState CurrentGameState;
    [HideInInspector]
    public PuzzleState CurrentPuzzleState;
    [HideInInspector]
    public RevealedState PuzzleRevealedNumber;

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

    private int firstRevealedTile;
    private int secondRevealedTile;
    private int revealedTileNumber = 0;
    private int tileToDestroy1;
    private int tileToDestroy2;


    private bool coroutineStarted = false;

    private int pairNumbers;
    private int removedPairs;
    private Timer gameTimer;


    private void Start()
    {
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        PuzzleRevealedNumber = 0;
        revealedTileNumber = 0;
        firstRevealedTile = -1;
        secondRevealedTile = -1;

        removedPairs = 0;
        pairNumbers = (int) GameSettings.Instance.GetPairNumber();

        gameTimer = GameObject.Find("Main Camera").GetComponent<Timer>();

        LoadMaterials();


        if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E10Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnTileMesh(4, 5, StartPosition, offset, false);
            MoveTile(4, 5, StartPosition, offset);
        }

        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E15Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnTileMesh(5, 6, StartPosition, offset, false);
            MoveTile(5, 6, StartPosition, offsetFor15Pairs);
        }

        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E20Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnTileMesh(5, 8, StartPosition, offset, true);
            MoveTile(5, 8, StartPosition, offsetFor20Pairs);
        }

    }

    public void CheckTile()
    {
        CurrentGameState = GameState.Checking;
        revealedTileNumber = 0;
        for (int id = 0; id < TileList.Count; id++)
        {
            if (TileList[id].Revealed && revealedTileNumber < 2)
            {
                if (revealedTileNumber == 0)
                {
                    firstRevealedTile = id;
                    revealedTileNumber++;
                }

                else if (revealedTileNumber == 1)
                {
                    secondRevealedTile = id;
                    revealedTileNumber++;
                }
            }
        }

        if (revealedTileNumber == 2)
        {
            if (TileList[firstRevealedTile].GetIndex() == TileList[secondRevealedTile].GetIndex() && firstRevealedTile != secondRevealedTile)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                tileToDestroy1 = firstRevealedTile;
                tileToDestroy2 = secondRevealedTile;

            }
            else
            {
                CurrentGameState = GameState.FlipBack;
            }
            
        }

        CurrentPuzzleState = TileManager.PuzzleState.CanRotate;

        if (CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;
        }
    }

    private void DestroyTile()
    {
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        TileList[tileToDestroy1].Deactivate();
        TileList[tileToDestroy2].Deactivate();
        revealedTileNumber = 0;
        removedPairs++;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
    }

    private IEnumerator FlipBack()
    {
        coroutineStarted = true;

        yield return new WaitForSeconds(0.5f);

        TileList[firstRevealedTile].FlipBack();
        TileList[secondRevealedTile].FlipBack();

        TileList[firstRevealedTile].Revealed = false;
        TileList[secondRevealedTile].Revealed = false;

        PuzzleRevealedNumber = RevealedState.NoRevealed;
        CurrentGameState = GameState.NoAction;

        coroutineStarted = false;
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
        if (CurrentGameState == GameState.DeletingPuzzles)
        {
            if (CurrentPuzzleState == PuzzleState.CanRotate)
            {
                DestroyTile();
                CheckGameEnd();
            }
        }
        if (CurrentGameState == GameState.FlipBack)
        {
            if (CurrentPuzzleState == PuzzleState.CanRotate && coroutineStarted == false)
            {
                StartCoroutine(FlipBack());
            }
        }

        if (CurrentGameState == GameState.GameEnd)
        {
            if (TileList[firstRevealedTile].gameObject.activeSelf == false && TileList[secondRevealedTile].gameObject.activeSelf == false && EndGamePanel.activeSelf == false)
            {
                ShowEndGameInformation();
            }
        }
    }

    private bool CheckGameEnd()
    {
        if (removedPairs == pairNumbers && CurrentGameState != GameState.GameEnd)
        {
            CurrentGameState = GameState.GameEnd;
            gameTimer.StopTimer();
            Config.PlaceScoreOnBoard(gameTimer.GetCurrentTime());

        }

        return(CurrentGameState == GameState.GameEnd);
    }

    private void ShowEndGameInformation()
    {
        EndGamePanel.SetActive(true);

        if(Config.IsBestScore())
        {
            NewBestTimeText.SetActive(true);
            YourTimeText.SetActive(false);
        }
        else
        {
            NewBestTimeText.SetActive(false);
            YourTimeText.SetActive(true);
        }


        var timer = gameTimer.GetCurrentTime();
        var minutes = Mathf.Floor(timer /  60); 
        var seconds = Mathf.RoundToInt(timer % 60);
        var newText = minutes.ToString("00") + ":" + seconds.ToString("00");
        EndTimeText.GetComponent<Text>().text = newText;
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
        var randomMaterialIndex = Random.Range(0, materialList.Count);
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
            o.SetIntex(randomMaterialIndex);
            o.Revealed = false;
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