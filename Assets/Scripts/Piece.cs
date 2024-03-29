using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceCamp
{
    Nature, Money, Neutral
}

public enum PieceClass
{
    Empty, Temple, Devotee
}

public class Piece : MonoBehaviour
{
    public PieceCamp pieceCamp;
    public PieceClass pieceClass;

    private float transparency = 0;
    private float maxTransparency = 1;
    private bool isRemoving = false;

    public PieceCamp GetPieceCamp()
    {
        return pieceCamp;
    }

    public PieceClass GetPieceClass() { 
        return pieceClass;
    }

    private void Update()
    {
        if(Removing() || Appearing())
        {
            UpdateTransparency();
        }
    }

    private bool Removing()
    {
        if(!isRemoving)
        {
            return false;
        }

        if (transparency <= 0)
        {
            Destroy(gameObject);
            return false;
        }

        transparency -= (float)(maxTransparency * 3 * Time.deltaTime);

        return true;
    }

    private bool Appearing()
    {
        if (isRemoving || transparency >= maxTransparency)
        {
            return false;
        }

        transparency += (float)(maxTransparency * 3 * Time.deltaTime);

        return true;
    }

    public void Remove()
    {
        isRemoving = true;
    }

    public void MakeGhost()
    {
        maxTransparency = 0.5f;
    }

    void UpdateTransparency()
    {
        var childCount = gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var child = gameObject.transform.GetChild(i);
            
            if (!child.TryGetComponent<Renderer>(out var renderer)) continue;

            var mat = renderer.material;

            if (mat == null) continue;

            var newColor = mat.color;
            newColor.a = transparency;
            mat.SetColor("_Color", newColor);
        }
    }
}
