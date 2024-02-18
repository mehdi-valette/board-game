using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public GameObject natureDevotee;

    private GameObject ghost;

    // Start is called before the first frame update
    void Start()
    {
        ghost = Instantiate(natureDevotee);
        ChangeAlpha(ghost, 0.5f);
        ghost.SetActive(false);
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

    public GameObject GetGhost()
    {
        return ghost;
    }

    public GameObject GetPiece()
    {
        return Instantiate(natureDevotee);
    }
}
