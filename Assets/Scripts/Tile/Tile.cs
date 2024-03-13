using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Tile : MonoBehaviour
{
    public Board board;
    public ushort deleteCount = 4;

    private Piece currentPiece;
    private Piece ghost;
    private ushort neighboursCount;
    private Tile[] neighbours;
    private Vector3 piecePosition;
    private TileGroup tileGroup;
    private Boolean initialized = false;

    private void Awake()
    {
        tileGroup = null;
    }

    public void Init()
    {
        Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(initialized) return;

        neighbours = Neighbours.GetNeighbours(transform);
        neighboursCount = 0;
        foreach(var neighbour in neighbours)
        {
            if(neighbour != null)
            {
                neighboursCount++;
            }
        }

        piecePosition.x = transform.position.x;
        piecePosition.y = transform.position.y + 0.2f;
        piecePosition.z = transform.position.z;

        if (currentPiece != null)
        {
            currentPiece.transform.position = piecePosition;
        }

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Tile[] GetNeighbours()
    {
        return neighbours;
    }

    public uint GetNeighboursCount()
    {
        return neighboursCount;
    }

    public Piece GetPiece()
    {
        return currentPiece;
    }

    public void SetPiece(Piece piece)
    {
        if (currentPiece != null)
        {
            return;
        }
       
        currentPiece = Instantiate(piece);
        currentPiece.transform.position = piecePosition;
    }

    public TileGroup GetTileGroup()
    {
        return tileGroup;
    }

    public void SetTileGroup(TileGroup group)
    {
        this.tileGroup = group;
    }

    public PieceClass GetTileClass()
    {
        if(currentPiece == null)
        {
            return PieceClass.Empty;
        }

        return currentPiece.GetPieceClass();
    }

    public PieceCamp GetTileCamp()
    {
        if(currentPiece == null)
        {
            return PieceCamp.Neutral;
        }

        return currentPiece.GetPieceCamp();
    }

    public void OnMouseDown()
    {
        PlacePiece();
    }

    public void OnMouseEnter()
    {
        ShowGhost();
    }

    public void OnMouseExit()
    {
        if (ghost != null)
        {
            ghost.Remove();
            ghost = null;
        }
    }

    public void ClearTile()
    {
        if (currentPiece == null) return;

        currentPiece.Remove();
        currentPiece = null;
    }

    //removes the piece if it's surrounded by ennemies
    public void CheckNeighbours()
    {
        if(currentPiece == null || GetTileClass() == PieceClass.Empty)
        {
            return;
        }

        Dictionary<PieceCamp, ushort> ennemies = new();
        foreach(var neighbour in GetNeighbours())
        {
            if(Neighbours.IsHostileNeighbour(neighbour, GetTileCamp()))
            {
                ennemies.TryGetValue(neighbour.GetTileCamp(), out ushort ennemiesCount);
                if(ennemiesCount + 1 >= deleteCount)
                {
                    RemoveTileFromGroup();
                    break;
                }

                ennemies[neighbour.GetTileCamp()] = (ushort)(ennemiesCount + 1);
            }
        }
    }

    private void RemoveTileFromGroup()
    {
        if(currentPiece == null || currentPiece.GetPieceClass() == PieceClass.Temple)
        {
            return;
        }

        ClearTile();

        tileGroup.RemoveMember(this);
    }

    private bool CanPlacePiece(PieceCamp camp)
    {
        if (GetTileCamp() != PieceCamp.Neutral)
        {
            return false;
        }

        foreach (var neighbour in neighbours)
        {
            if (Neighbours.IsFriendlyNeighbour(neighbour, camp))
            {
                return true;
            }
        }

        return false;
    }

    private TileGroup GetGroupForInsert(PieceCamp camp)
    {
        HashSet<TileGroup> groups = new HashSet<TileGroup>();

        foreach(var neighbour in neighbours)
        {
            if (Neighbours.IsFriendlyNeighbour(neighbour, camp))
            {
                groups.Add(neighbour.GetTileGroup());
            }
        }

        if (groups.Count == 0) return null;

        TileGroup firstGroup = null;

        foreach (var group in groups)
        {
            if (firstGroup == null)
            {
                firstGroup = group;
                continue;
            }

            group.MergeIntoOtherGroup(firstGroup);
        }

        return firstGroup;
    }

    private void ShowGhost()
    {
        if (!CanPlacePiece(board.GetPieceCamp())) return;

        if (ghost == null)
        {
            ghost = board.GetGhost();
        }

        ghost.transform.SetPositionAndRotation(piecePosition, transform.rotation);
    }

    private void PlacePiece()
    {
        if (currentPiece != null) return;

        var candidateGroup = GetGroupForInsert(board.GetPieceCamp());
        if (candidateGroup == null) return;

        currentPiece = board.GetPiece();
        if(ghost != null)
        {
            ghost.Remove();
            ghost = null;
        }

        currentPiece.transform.position = piecePosition;

        candidateGroup.AddMember(this);

        foreach(var neighbour in GetNeighbours())
        {
            if(Neighbours.IsHostileNeighbour(neighbour, GetTileCamp()))
            {
                neighbour.CheckNeighbours();
            }
        }
    }
}
