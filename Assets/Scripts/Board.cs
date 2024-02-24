using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Piece natureDevotee;
    public Piece natureTemple;
    public Piece moneyDevotee;
    public Piece moneyTemple;

    private Piece ghost;
    private Piece activePiece;

    // Start is called before the first frame update
    void Start()
    {
        activePiece = moneyDevotee;

        ghost = Instantiate(activePiece);
        ChangeAlpha(ghost.gameObject, 0.5f);
        ghost.gameObject.SetActive(false);

        var allTile = GetComponentsInChildren<Tile>();
        foreach (var tile in allTile)
        {
            tile.Init();

            if (tile.GetNeighboursCount() == 2)
            {
                var neighbours = tile.GetNeighbours();
                if (neighbours[1] != null && neighbours[2] != null)
                {
                    tile.SetPiece(moneyTemple);
                    new TileGroup(new HashSet<Tile> { tile });
                }
                else if (neighbours[3] != null && neighbours[4] != null)
                {
                    tile.SetPiece(natureTemple);
                    new TileGroup(new HashSet<Tile> { tile });
                }
            }

            if (tile.GetNeighboursCount() == 3)
            {
                var neighbours = tile.GetNeighbours();
                if (neighbours[0] != null && neighbours[1] != null && neighbours[5] != null)
                {
                    tile.SetPiece(natureTemple);
                    new TileGroup(new HashSet<Tile> { tile });
                }
                else if (neighbours[0] != null && neighbours[4] != null && neighbours[5] != null)
                {
                    tile.SetPiece(moneyTemple);
                    new TileGroup(new HashSet<Tile> { tile });
                }
            }
        }

    }

    static void ChangeAlpha(GameObject gameObject, float alpha) {

        var childCount = gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var child = gameObject.transform.GetChild(i);

            var renderer = child.GetComponent<Renderer>();

            if (renderer == null) continue;

            var mat = renderer.material;

            if (mat == null) continue;

            var newColor = mat.color;
            newColor.a = alpha;
            mat.SetColor("_Color", newColor);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    { }

    public Piece GetGhost()
    {
        return ghost;
    }

    public Piece GetPiece()
    {
        return Instantiate(activePiece);
    }

    public PieceCamp GetPieceCamp()
    {
        return activePiece.GetPieceCamp();
    }
}
