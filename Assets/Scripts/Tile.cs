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

    // Start is called before the first frame update
    void Start()
    {
        neighbours = GetNeighbours();
        GetDirections();
    }

    // Update is called once per frame
    void Update()
    {
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

    private bool CanUserPlacePiece()
    {
        if(currentPiece != null)
        {
            return false;
        }

        var neighboursCount = 0;
        foreach(var neighbour in neighbours)
        {
            if(neighbour.GetPiece() != null)
            {
                neighboursCount++;
            }
        }

        if(neighboursCount > 2)
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

        var newPosition = new Vector3();
        newPosition.x = transform.position.x;
        newPosition.y = transform.position.y + 0.2f;
        newPosition.z = transform.position.z;

        ghost.transform.SetPositionAndRotation(newPosition, transform.rotation);
        ghost.SetActive(true);
    }

    private void PlacePiece()
    {
        if (!CanUserPlacePiece()) return;

        currentPiece = board.GetPiece();

        var newPosition = new Vector3();
        newPosition.x = transform.position.x;
        newPosition.y = transform.position.y + 0.2f;
        newPosition.z = transform.position.z;

        currentPiece.transform.position = newPosition;
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

    public GameObject GetPiece()
    {
        return currentPiece;
    }

    public void SetPiece(GameObject piece)
    {
        if (currentPiece == null || currentPiece.name != piece.name)
        {
            currentPiece = Instantiate(piece, transform.position, transform.rotation);
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
}
