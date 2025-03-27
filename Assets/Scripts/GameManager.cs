using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI; // Added for UI Dropdown

public enum Difficulty
{
    Easy,    // 0
    Medium,  // 1
    Hard     // 2
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Difficulty difficulty = Difficulty.Easy;  // Default, overwritten by UI
    public Dropdown difficultyDropdown;  // Assign the UI Dropdown in Inspector
    
    private int width;
    private int height;
    private int numMines;
    private readonly float tileSize = 1f;

    // List to store all tile components
    private List<Tile> tiles = new List<Tile>();

    void Start()
{
    if (difficultyDropdown != null)
    {
        difficulty = (Difficulty)difficultyDropdown.value;
        Debug.Log("Difficulty selected via UI: " + difficulty);
    }
    
    // Create game board based on selected difficulty.
    switch (difficulty)
    {
        case Difficulty.Easy:
            CreateGameBoard(10, 8, 10);
            break;
        case Difficulty.Medium:
            CreateGameBoard(16, 14, 30);
            break;
        case Difficulty.Hard:
            CreateGameBoard(24, 22, 80);
            break;
    }

    ResetGameState();
}

    public void CreateGameBoard(int width, int height, int numMines)
    {
        this.width = width;
        this.height = height;
        this.numMines = numMines;

        // Create the game board
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Debug.Log("Creating tile at " + row + ", " + col);
                Transform tileTransform = Instantiate(tilePrefab, gameHolder);
                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);

                Tile tile = tileTransform.GetComponent<Tile>();
                tiles.Add(tile);
                tile.gameManager = this;
            }
        }
    }

    private void ResetGameState()
    {
        int tilesCount = tiles.Count;

        // --- Enforce Heuristic: Safe 3x3 Zone in Bottom Left and Top Right Corners ---
        List<int> safeZone = new List<int>();

        // Bottom left safe zone: rows 0 to 2, cols 0 to 2.
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                int index = row * width + col;
                safeZone.Add(index);
            }
        }
        
        // Top right safe zone: rows (height - 3) to (height - 1), cols (width - 3) to (width - 1).
        for (int row = height - 3; row < height; row++)
        {
            for (int col = width - 3; col < width; col++)
            {
                int index = row * width + col;
                safeZone.Add(index);
            }
        }
        
        // Candidate indices: exclude safeZone indices.
        List<int> candidateIndices = new List<int>();
        for (int i = 0; i < tilesCount; i++)
        {
            if (!safeZone.Contains(i))
                candidateIndices.Add(i);
        }

        // Clear any previous mine placements.
        for (int i = 0; i < tilesCount; i++)
        {
            tiles[i].isMine = false;
        }

        // Place mines randomly among candidate indices.
        var initialMinePositions = candidateIndices.OrderBy(x => Random.value)
                                                    .Take(numMines)
                                                    .ToList();
        foreach (int pos in initialMinePositions)
        {
            tiles[pos].isMine = true;
        }
        UpdateMineCounts();
        
        int topRightIndex = (height * width) - 1;
        tiles[topRightIndex].isWinner = true;
        Debug.Log("Winner tile is at " + topRightIndex);

        // Set allowed type-3 tile count based on difficulty.
        int allowedType3 = 0;
        switch (difficulty)
        {
            case Difficulty.Easy:
                allowedType3 = 1;
                break;
            case Difficulty.Medium:
                allowedType3 = 10;
                break;
            case Difficulty.Hard:
                allowedType3 = 20;
                break;
        }

        // --- Monte Carlo / Simulated Annealing Setup ---
        float temperature = 100f;
        float coolingRate = 0.99f;
        int iterations = 0;
        int maxIterations = 10000;

        int currentCost = EvaluateCost(allowedType3);

        while (currentCost > 0 && iterations < maxIterations)
        {
            // Select a random mine from candidateIndices.
            List<int> currentMines = candidateIndices.Where(i => tiles[i].isMine).ToList();
            if (currentMines.Count == 0) break; // Shouldn't happen.
            int mineIndex = currentMines[Random.Range(0, currentMines.Count)];

            // Select a random candidate that is not a mine.
            List<int> currentNonMines = candidateIndices.Where(i => !tiles[i].isMine).ToList();
            if (currentNonMines.Count == 0) break;
            int nonMineIndex = currentNonMines[Random.Range(0, currentNonMines.Count)];

            // Swap: remove mine from mineIndex and add mine to nonMineIndex.
            tiles[mineIndex].isMine = false;
            tiles[nonMineIndex].isMine = true;
            UpdateMineCounts();

            int newCost = EvaluateCost(allowedType3);
            int delta = newCost - currentCost;

            // Accept improvements, or probabilistically accept downs.
            if (delta <= 0 || Random.value < Mathf.Exp(-delta / temperature))
            {
                currentCost = newCost;
            }
            else
            {
                // Revert swap.
                tiles[mineIndex].isMine = true;
                tiles[nonMineIndex].isMine = false;
                UpdateMineCounts();
            }

            temperature *= coolingRate;
            iterations++;
        }

        
        Debug.Log("Monte Carlo finished with cost " + currentCost + " after " + iterations + " iterations.");
    }

    // Update the mine counts for all tiles.
    private void UpdateMineCounts()
    {
        int tilesCount = tiles.Count;
        for (int i = 0; i < tilesCount; i++)
        {
            tiles[i].mineCount = HowManyMines(i);
        }
    }

    // Evaluate the "cost" of the current mine layout based on our heuristics.
    private int EvaluateCost(int allowedType3)
    {
        int cost = 0;
        int tilesCount = tiles.Count;

        // Heuristic 1: Count type 3 tiles.
        int type3Count = 0;
        for (int i = 0; i < tilesCount; i++)
        {
            if (tiles[i].mineCount == 3)
                type3Count++;
        }
        if (type3Count > allowedType3)
            cost += (type3Count - allowedType3) * 10;  // Weight penalty.

        // Heuristic 3: Check contiguous zero regions (using only orthogonal neighbours).
        bool[] visited = new bool[tilesCount];
        for (int i = 0; i < tilesCount; i++)
        {
            if (!visited[i] && tiles[i].mineCount == 0)
            {
                int clusterSize = FloodFillCount(i, visited);
                if (clusterSize > 6)
                    cost += (clusterSize - 6) * 5;  // Weight penalty.
            }
        }
        return cost;
    }

    // Compute the number of adjacent mines (8 directions).
    private int HowManyMines(int index)
    {
        int count = 0;
        foreach (int pos in GetNeighbours(index))
        {
            if (tiles[pos].isMine)
                count++;
        }
        return count;
    }

    // Returns the 8-direction neighbours (including diagonals) for a given tile index.
    private List<int> GetNeighbours(int pos)
    {
        List<int> neighbours = new List<int>();
        int row = pos / width;
        int col = pos % width;

        // North and its diagonals.
        if (row < (height - 1))
        {
            neighbours.Add(pos + width); // north
            if (col > 0)
                neighbours.Add(pos + width - 1); // northwest
            if (col < (width - 1))
                neighbours.Add(pos + width + 1); // northeast
        }
        // West and east.
        if (col > 0)
            neighbours.Add(pos - 1); // west
        if (col < (width - 1))
            neighbours.Add(pos + 1); // east
        // South and its diagonals.
        if (row > 0)
        {
            neighbours.Add(pos - width); // south
            if (col > 0)
                neighbours.Add(pos - width - 1); // southwest
            if (col < (width - 1))
                neighbours.Add(pos - width + 1); // southeast
        }
        return neighbours;
    }

    // Flood fill to count contiguous "0" tiles using orthogonal moves (up, down, left, right).
    private int FloodFillCount(int start, bool[] visited)
    {
        int count = 0;
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(start);
        visited[start] = true;
        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            count++;
            foreach (int neighbor in GetOrthogonalNeighbours(current))
            {
                if (!visited[neighbor] && tiles[neighbor].mineCount == 0)
                {
                    visited[neighbor] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }
        return count;
    }

    // Returns only the 4 orthogonal neighbours (up, down, left, right).
    private List<int> GetOrthogonalNeighbours(int pos)
    {
        List<int> neighbours = new List<int>();
        int row = pos / width;
        int col = pos % width;
        if (row < height - 1)
            neighbours.Add(pos + width); // up
        if (row > 0)
            neighbours.Add(pos - width); // down
        if (col > 0)
            neighbours.Add(pos - 1); // left
        if (col < width - 1)
            neighbours.Add(pos + 1); // right
        return neighbours;
    }

    public void RevealAllTiles()
    {
        foreach (Tile tile in tiles)
        {
            tile.Deactivate();
        }
    }

    public void ClickNeighbours(Tile tile){
        int location = tiles.IndexOf(tile);
        foreach (int pos in GetNeighbours(location)){
            tiles[pos].ClickedTile();
        }
    }

    public void GameOver(){
        RevealAllTiles();
        Debug.Log("Game Over");
    }

    public void WinGame(){
        RevealAllTiles();
        LevelManager.instance.Win();
        gameHolder.gameObject.SetActive(true);
        Debug.Log("You Win!");
    }
}
