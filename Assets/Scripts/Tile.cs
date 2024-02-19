using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    public Board board;

    private GameObject currentPiece;
    private ushort neighboursCount;
    private List<Tile> neighbours;
    private Vector3 piecePosition;

    // Start is called before the first frame update
    void Start()
    {
        neighbours = GetNeighbours();
        GetDirections();

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

    public GameObject GetPiece()
    {
        return currentPiece;
    }

    public void SetPiece(GameObject piece)
    {
        if (currentPiece == null || currentPiece.name != piece.name)
        {
            currentPiece = Instantiate(piece);
            currentPiece.transform.position = piecePosition;
        }
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
        if(currentPiece != null) return;

        board.GetGhost().SetActive(false);
    }

    private void FixedUpdate()
    {
    }

    private List<Tile> GetNeighbours()
    {
        var neighbours = new List<Tile>();
        var directions = GetDirections();


        foreach (var direction in directions)
        {

            Physics.Raycast(transform.position, direction, out RaycastHit hit, 3f);

            if (
                hit.collider != null &&
                hit.collider.GetComponent<Tile>() != null
            )
            {
                neighbours.Add(hit.collider.GetComponent<Tile>());
            }
        }

        return neighbours;
    }

    private Vector3[] GetDirections()
    {
        var directions = new Vector3[6];

        var topRight = transform.position;
        topRight.x += 0.8f;
        topRight.z += 1.5f;
        directions[0] = transform.position - topRight;

        var bottomRight = transform.position;
        bottomRight.x += 0.8f;
        bottomRight.z -= 1.5f;
        directions[1] = transform.position - bottomRight;

        var bottomLeft = transform.position;
        bottomLeft.x -= 0.8f;
        bottomLeft.z -= 1.5f;
        directions[2] = transform.position - bottomLeft;

        var topLeft = transform.position;
        topLeft.x -= 0.8f;
        topLeft.z += 1.5f;
        directions[3] = transform.position - topLeft;

        var left = transform.position;
        left.x -= 1.5f;
        directions[4] = transform.position - left;

        var right = transform.position;
        right.x += 1.5f;
        directions[5] = transform.position - right;

        return directions;
    }

    private bool CanUserPlacePiece()
    {
        if (currentPiece != null)
        {
            return false;
        }

        var neighboursCount = 0;
        foreach (var neighbour in neighbours)
        {
            if (neighbour.GetPiece() != null)
            {
                neighboursCount++;
            }
        }

        if (neighboursCount > 2)
        {
            return false;
        }

        return true;
    }

    private void ShowGhost()
    {
        if (!CanUserPlacePiece()) return;

        var ghost = board.GetGhost();

        ghost.SetActive(true);

        ghost.transform.SetPositionAndRotation(piecePosition, transform.rotation);
        ghost.SetActive(true);
    }

    private void PlacePiece()
    {
        if (!CanUserPlacePiece()) return;

        currentPiece = board.GetPiece();

        currentPiece.transform.position = piecePosition;
    }

    // TODO: implement this function
    private void AddTempleConnectedObserver(Tile tile)
    {
        // Add the tile in a list and call it once we know if we're connected
    }

    // TODO: implement this function
    private void NotifyTempleConnectedObservers(Tile tile, bool isConnected)
    {
        // keep a record of the neighbours and their connected status

        // if at least one is connected, then this tile is connected too

        // if the connection status changes notify the observers
    }
}
