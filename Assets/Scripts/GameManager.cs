using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;  // Needed for LINQ methods like OrderBy

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;

    private int width;
    private int height;
    private int numMines;
    private readonly float tileSize = 0.34f;

    // Declare a list to store tile components
    private List<Tile> tiles = new List<Tile>();

    void Start(){
        CreateGameBoard(9, 9, 10);
        ResetGameState();
    }

    public void CreateGameBoard(int width, int height, int numMines){
        this.width = width;
        this.height = height;
        this.numMines = numMines;
        
        // Create the game board
        for (int row = 0; row < height; row++){
            for (int col = 0; col < width; col++){
                Debug.Log("Creating tile at " + row + ", " + col);
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder;
                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);

                Tile tile = tileTransform.GetComponent<Tile>(); 
                tiles.Add(tile); 
            }
        }
    }

    private void ResetGameState(){
        int tilesCount = tiles.Count;
        int[] minePositions = Enumerable.Range(0, tilesCount)
                                        .OrderBy(x => Random.value)
                                        .Take(numMines)
                                        .ToArray();

        for (int i = 0; i < numMines; i++){
            int pos = minePositions[i];
            tiles[pos].isMine = true;
        }

        for (int i = 0; i < tilesCount; i++){
            tiles[i].mineCount = HowManyMines(i);
        }
    }

    private int HowManyMines(int index){
        int count = 0;
        foreach (int pos in GetNeighbours(index)){
            if(tiles[pos].isMine){
                count++;
            }
        }
        return count;
    }

    private List<int> GetNeighbours(int pos){
        List<int> neighbours = new List<int>();
        int row = pos / width;
        int col = pos % width;
        
        // (0,0) is bottom left
        if (row < (height - 1)){
            // north
            neighbours.Add(pos + width);
            if (col > 0){
                neighbours.Add(pos + width - 1); // northwest
            }
            if (col < (width - 1)){
                neighbours.Add(pos + width + 1); // northeast
            }
        }

        if (col > 0){
            // west
            neighbours.Add(pos - 1);
        }

        if (col < (width - 1)){
            // east
            neighbours.Add(pos + 1);
        }

        if (row > 0){
            // south
            neighbours.Add(pos - width);
            if (col > 0){
                neighbours.Add(pos - width - 1); // southwest
            }
            if (col < (width - 1)){
                neighbours.Add(pos - width + 1); // southeast
            }
        }

        return neighbours;
    }
}
