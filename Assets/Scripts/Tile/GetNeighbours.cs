

using System.Collections.Generic;
using UnityEngine;

class Neighbours
{
    public static List<Tile> GetNeighbours(Transform transform)
    {
        var neighbours = new List<Tile>();
        var directions = GetDirections(transform);
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

    private static Vector3[] GetDirections(Transform transform)
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
}