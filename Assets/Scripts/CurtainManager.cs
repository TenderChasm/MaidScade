using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CurtainManager : MonoBehaviour
{
    public Tilemap Ceiling { get; set; }
    public Tilemap CurtainOfUnseen { get; set; }
    public HashSet<Vector2Int> RevealedTiles { get; set; }

    public Tile CurtainsUpBlock;
    public Tile CurtainsRightBlock;
    public Tile CurtainsDownBlock;
    public Tile CurtainsLeftBlock;
    public Tile CurtainsWholeBlock;
    public Tile CurtainsUpRightCorner;
    public Tile CurtainsUpLeftCorner;
    public Tile CurtainsDownRightCorner;
    public Tile CurtainsDownLeftCorner;

    public Tile CeilingUpBlock;
    public Tile CeilingRightBlock;
    public Tile CeilingDownBlock;
    public Tile CeilingLeftBlock;
    public Tile CeilingWholeBlock;
    public Tile CeilingUpRightBlock;
    public Tile CeilingUpLeftBlock;
    public Tile CeilingDownRightBlock;
    public Tile CeilingDownLeftBlock;
    public Tile CeilingUpRightCorner;
    public Tile CeilingUpLeftCorner;
    public Tile CeilingDownRightCorner;
    public Tile CeilingDownLeftCorner;

    public Tile CeilingUpArk;
    public Tile CeilingRightArk;
    public Tile CeilingDownArk;
    public Tile CeilingLeftArk;


    public enum DFSDirection
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    // Start is called before the first frame update
    void Start()
    {
        RevealedTiles = new HashSet<Vector2Int>();
        Transform mainGridTransform = GameManager.Hr.MainGrid.transform;
        Ceiling = GameManager.Hr.Ceiling;
        CurtainOfUnseen = GameManager.Hr.CurtainsOfUnseen;  
    }

    public void StartRevealFromWorldPosition(Vector2 position)
    {
        RevealedTiles.Clear();
        RevealTilesDFS((Vector2Int)Ceiling.WorldToCell(position), DFSDirection.Up);
    }

    public void StartHideFromWorldPosition(Vector2 position)
    {
        RevealedTiles.Clear();
        HideTilesDFS((Vector2Int)Ceiling.WorldToCell(position));
    }

    private void RevealTilesDFS(Vector2Int startingPos, DFSDirection direction)
    {
        int x = startingPos.x;
        int y = startingPos.y;

        RevealedTiles.Add(startingPos);
        TileBase currentTile = Ceiling.GetTile((Vector3Int)startingPos);
        if (currentTile != null)
        {
            Tile check1 = (Tile)currentTile;
            if ((currentTile == CeilingUpArk || currentTile == CeilingUpBlock || currentTile == CeilingUpRightCorner || currentTile == CeilingUpLeftCorner)
            && (direction == DFSDirection.Down || direction == DFSDirection.DownRight || direction == DFSDirection.DownLeft))
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsDownBlock);
            else if ((currentTile == CeilingRightArk || currentTile == CeilingRightBlock || currentTile == CeilingUpRightCorner || currentTile == CeilingDownRightCorner)
            && (direction == DFSDirection.Left || direction == DFSDirection.UpLeft || direction == DFSDirection.DownLeft))
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsLeftBlock);
            else if ((currentTile == CeilingDownArk || currentTile == CeilingDownBlock || currentTile == CeilingDownRightCorner || currentTile == CeilingDownLeftCorner)
            && (direction == DFSDirection.Up || direction == DFSDirection.UpRight || direction == DFSDirection.UpLeft))
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsUpBlock);
            else if ((currentTile == CeilingLeftArk || currentTile == CeilingLeftBlock || currentTile == CeilingUpLeftCorner || currentTile == CeilingDownLeftCorner)
            && (direction == DFSDirection.Right || direction == DFSDirection.UpRight || direction == DFSDirection.DownRight))
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsRightBlock);
            else if(currentTile == CeilingUpRightBlock && direction == DFSDirection.DownLeft)
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsDownLeftCorner);
            else if (currentTile == CeilingDownRightBlock && direction == DFSDirection.UpLeft)
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsUpLeftCorner);
            else if (currentTile == CeilingDownLeftBlock && direction == DFSDirection.UpRight)
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsUpRightCorner);
            else if (currentTile == CeilingUpLeftBlock && direction == DFSDirection.DownRight)
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsDownRightCorner);
            else
                CurtainOfUnseen.SetTile((Vector3Int)startingPos, null);

            return;
        }

        CurtainOfUnseen.SetTile((Vector3Int)startingPos, null);

        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x, startingPos.y + 1)))
            RevealTilesDFS(new Vector2Int(startingPos.x, startingPos.y + 1), DFSDirection.Up);
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x + 1, startingPos.y)))
            RevealTilesDFS(new Vector2Int(startingPos.x + 1, startingPos.y), DFSDirection.Right);
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x, startingPos.y - 1)))
            RevealTilesDFS(new Vector2Int(startingPos.x, startingPos.y - 1), DFSDirection.Down);
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x - 1, startingPos.y)))
            RevealTilesDFS(new Vector2Int(startingPos.x - 1, startingPos.y), DFSDirection.Left);
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x + 1, startingPos.y + 1)))
            RevealTilesDFS(new Vector2Int(startingPos.x + 1, startingPos.y + 1), DFSDirection.UpRight);  
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x + 1, startingPos.y - 1)))
            RevealTilesDFS(new Vector2Int(startingPos.x + 1, startingPos.y - 1), DFSDirection.DownRight);
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x - 1, startingPos.y - 1)))
            RevealTilesDFS(new Vector2Int(startingPos.x - 1, startingPos.y - 1), DFSDirection.DownLeft);
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x - 1, startingPos.y + 1)))
            RevealTilesDFS(new Vector2Int(startingPos.x - 1, startingPos.y + 1), DFSDirection.UpLeft);
    }

    private void HideTilesDFS(Vector2Int startingPos)
    {
        RevealedTiles.Add(startingPos);
        TileBase currentTile = Ceiling.GetTile((Vector3Int)startingPos);
        CurtainOfUnseen.SetTile((Vector3Int)startingPos, CurtainsWholeBlock);

        if (currentTile != null)
            return;

        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x, startingPos.y + 1)))
            HideTilesDFS(new Vector2Int(startingPos.x, startingPos.y + 1));
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x + 1, startingPos.y + 1)))
            HideTilesDFS(new Vector2Int(startingPos.x + 1, startingPos.y + 1));
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x + 1, startingPos.y)))
            HideTilesDFS(new Vector2Int(startingPos.x + 1, startingPos.y));
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x + 1, startingPos.y - 1)))
            HideTilesDFS(new Vector2Int(startingPos.x + 1, startingPos.y - 1));
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x, startingPos.y - 1)))
            HideTilesDFS(new Vector2Int(startingPos.x, startingPos.y - 1));
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x - 1, startingPos.y - 1)))
            HideTilesDFS(new Vector2Int(startingPos.x - 1, startingPos.y - 1));
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x - 1, startingPos.y)))
            HideTilesDFS(new Vector2Int(startingPos.x - 1, startingPos.y));
        if (!RevealedTiles.Contains(new Vector2Int(startingPos.x - 1, startingPos.y + 1)))
            HideTilesDFS(new Vector2Int(startingPos.x - 1, startingPos.y + 1));
    }
}
