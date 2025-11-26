using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;

    [Header("Board Settings")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private float gemSpacing = 1f;

    [Header("Gem Prefabs")]
    [SerializeField] private GameObject[] gemPrefabs;

    [Header("Match Settings")]
    [SerializeField] private int minMatchLength = 3;
    [SerializeField] private int matchPoints = 10;

    private Gem[,] gems;
    private Gem selectedGem;
    private bool isProcessing = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        enabled = false;
    }

    void Start()
    {
        gems = new Gem[width, height];
        InitializeBoard();
    }

    void InitializeBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = GetWorldPosition(x, y);
                SpawnGem(x, y, position);
            }
        }

        StartCoroutine(CheckInitialMatches());
    }

    IEnumerator CheckInitialMatches()
    {
        yield return new WaitForSeconds(0.5f);

        while (HasMatches())
        {
            DestroyMatches();
            yield return new WaitForSeconds(0.3f);
            FillBoard();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnGem(int x, int y, Vector2 position)
    {
        int randomType = Random.Range(0, gemPrefabs.Length);
        GameObject gemObj = Instantiate(gemPrefabs[randomType], position, Quaternion.identity, transform);

        Gem gem = gemObj.GetComponent<Gem>();
        gem.Initialize(x, y, randomType);
        gems[x, y] = gem;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        float xPos = x * gemSpacing - (width - 1) * gemSpacing / 2f;
        float yPos = y * gemSpacing - (height - 1) * gemSpacing / 2f;
        return new Vector2(xPos, yPos);
    }

    void Update()
    {
        if (isProcessing) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Gem clickedGem = GetGemAt(mousePos);

            if (clickedGem != null)
            {
                if (selectedGem == null)
                {
                    selectedGem = clickedGem;
                    HighlightGem(selectedGem, true);
                }
                else
                {
                    if (IsAdjacent(selectedGem, clickedGem))
                    {
                        StartCoroutine(SwapGems(selectedGem, clickedGem));
                    }
                    else
                    {
                        HighlightGem(selectedGem, false);
                        selectedGem = clickedGem;
                        HighlightGem(selectedGem, true);
                    }
                }
            }
        }
    }

    Gem GetGemAt(Vector2 worldPos)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gems[x, y] != null)
                {
                    float distance = Vector2.Distance(gems[x, y].transform.position, worldPos);
                    if (distance < gemSpacing / 2f)
                    {
                        return gems[x, y];
                    }
                }
            }
        }
        return null;
    }

    bool IsAdjacent(Gem gem1, Gem gem2)
    {
        return (Mathf.Abs(gem1.column - gem2.column) == 1 && gem1.row == gem2.row) ||
               (Mathf.Abs(gem1.row - gem2.row) == 1 && gem1.column == gem2.column);
    }

    IEnumerator SwapGems(Gem gem1, Gem gem2)
    {
        isProcessing = true;
        HighlightGem(gem1, false);

        int tempCol = gem1.column;
        int tempRow = gem1.row;

        gems[gem1.column, gem1.row] = gem2;
        gems[gem2.column, gem2.row] = gem1;

        gem1.SetPosition(gem2.column, gem2.row);
        gem2.SetPosition(tempCol, tempRow);

        yield return new WaitForSeconds(0.3f);

        if (HasMatches())
        {
            DestroyMatches();
            yield return new WaitForSeconds(0.3f);
            FillBoard();
            yield return new WaitForSeconds(0.5f);

            while (HasMatches())
            {
                DestroyMatches();
                yield return new WaitForSeconds(0.3f);
                FillBoard();
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            gems[gem2.column, gem2.row] = gem1;
            gems[gem1.column, gem1.row] = gem2;

            gem1.SetPosition(tempCol, tempRow);
            gem2.SetPosition(gem2.column, gem2.row);

            yield return new WaitForSeconds(0.3f);
        }

        selectedGem = null;
        isProcessing = false;
    }

    bool HasMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gems[x, y] != null)
                {
                    if (GetMatchLength(x, y, Vector2.right) >= minMatchLength ||
                        GetMatchLength(x, y, Vector2.up) >= minMatchLength)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    int GetMatchLength(int x, int y, Vector2 direction)
    {
        int type = gems[x, y].type;
        int length = 1;

        int checkX = x + (int)direction.x;
        int checkY = y + (int)direction.y;

        while (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
        {
            if (gems[checkX, checkY] != null && gems[checkX, checkY].type == type)
            {
                length++;
                checkX += (int)direction.x;
                checkY += (int)direction.y;
            }
            else
            {
                break;
            }
        }

        return length;
    }

    void DestroyMatches()
    {
        List<Gem> gemsToDestroy = new List<Gem>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gems[x, y] != null)
                {
                    int horizontalLength = GetMatchLength(x, y, Vector2.right);
                    int verticalLength = GetMatchLength(x, y, Vector2.up);

                    if (horizontalLength >= minMatchLength)
                    {
                        for (int i = 0; i < horizontalLength; i++)
                        {
                            if (!gemsToDestroy.Contains(gems[x + i, y]))
                            {
                                gemsToDestroy.Add(gems[x + i, y]);
                            }
                        }
                    }

                    if (verticalLength >= minMatchLength)
                    {
                        for (int i = 0; i < verticalLength; i++)
                        {
                            if (!gemsToDestroy.Contains(gems[x, y + i]))
                            {
                                gemsToDestroy.Add(gems[x, y + i]);
                            }
                        }
                    }
                }
            }
        }

        foreach (Gem gem in gemsToDestroy)
        {
            gems[gem.column, gem.row] = null;
            Destroy(gem.gameObject);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(gemsToDestroy.Count * matchPoints);
        }
    }

    void FillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gems[x, y] == null)
                {
                    for (int yAbove = y + 1; yAbove < height; yAbove++)
                    {
                        if (gems[x, yAbove] != null)
                        {
                            gems[x, y] = gems[x, yAbove];
                            gems[x, yAbove] = null;
                            gems[x, y].SetPosition(x, y);
                            break;
                        }
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gems[x, y] == null)
                {
                    Vector2 spawnPos = GetWorldPosition(x, height);
                    SpawnGem(x, y, spawnPos);
                    gems[x, y].SetPosition(x, y);
                }
            }
        }
    }

    void HighlightGem(Gem gem, bool highlight)
    {
        if (gem != null)
        {
            SpriteRenderer sr = gem.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = highlight ? Color.yellow : Color.white;
            }
        }
    }
}
