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

    private Piece currentPiece;
    private ushort neighboursCount;
    private List<Tile> neighbours;
    private Vector3 piecePosition;
    private TileGroup tileGroup;

    private void Awake()
    {
        tileGroup = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        neighbours = Neighbours.GetNeighbours(transform);

        piecePosition.x = transform.position.x;
        piecePosition.y = transform.position.y + 0.2f;
        piecePosition.z = transform.position.z;

        if (currentPiece != null)
        {
            currentPiece.transform.position = piecePosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<Tile> GetNeighbours()
    {
        return neighbours;
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

    public void SetGroup(TileGroup group)
    {
        this.tileGroup = group;
    }

    public PieceType GetTileType()
    {
        if(currentPiece == null)
        {
            return PieceType.Empty;
        }

        return currentPiece.GetPieceType();
    }

    public PieceCamp GetTileCamp()
    {
        if(currentPiece == null)
        {
            return PieceCamp.Neutral;
        }

        return currentPiece.GetPieceCamp();
    }

    public bool IsTempleConnected()
    {
        return tileGroup != null;
    }

    public void OnMouseDown()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            RemoveTileFromGroup();
            return;
        }

        PlacePiece();
    }

    public void OnMouseEnter()
    {
        ShowGhost();
    }

    public void OnMouseExit()
    {
        if(currentPiece != null) return;

        board.GetGhost().gameObject.SetActive(false);
    }

    public void ClearTile()
    {
        if (currentPiece == null) return;

        Destroy(currentPiece.gameObject);
        currentPiece = null;
    }

    private void RemoveTileFromGroup()
    {
        if(currentPiece == null || currentPiece.GetPieceType() == PieceType.Temple)
        {
            return;
        }

        ClearTile();

        tileGroup.RemoveMember(this);
    }

    static public bool IsFriendlyNeighbour(Tile neighbour, PieceCamp camp)
    {
        if (neighbour.GetTileGroup() == null)
        {
            return false;
        }

        if (neighbour.GetTileCamp() != camp)
        {
            return false;
        }

        return true;
    }

    private bool CanPlacePiece(PieceCamp camp)
    {
        if (GetTileCamp() != PieceCamp.Neutral)
        {
            return false;
        }

        foreach (var neighbour in neighbours)
        {
            if (IsFriendlyNeighbour(neighbour, camp))
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
            if (IsFriendlyNeighbour(neighbour, camp))
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

        var ghost = board.GetGhost();

        ghost.gameObject.SetActive(true);

        ghost.transform.SetPositionAndRotation(piecePosition, transform.rotation);
    }

    private void PlacePiece()
    {
        if (currentPiece != null) return;

        var candidateGroup = GetGroupForInsert(board.GetPieceCamp());
        if (candidateGroup == null) return;

        currentPiece = board.GetPiece();
        board.GetGhost().gameObject.SetActive(false);

        currentPiece.transform.position = piecePosition;

        candidateGroup.AddMember(this);
    }
}
