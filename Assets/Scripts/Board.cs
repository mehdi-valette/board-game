using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Piece natureDevotee;
    public Piece natureTemple;

    private Piece ghost;

    // Start is called before the first frame update
    void Start()
    {
        ghost = Instantiate(natureDevotee);
        ChangeAlpha(ghost.gameObject, 0.5f);
        ghost.gameObject.SetActive(false);

        var childCount = gameObject.transform.childCount;
        int childId = Random.Range(0, childCount - 1);

        gameObject
            .transform
            .GetChild(childId)
            .GetComponent<Tile>()
            .SetPiece(natureTemple);
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
        return Instantiate(natureDevotee);
    }
}
