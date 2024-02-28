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

    public PieceCamp GetPieceCamp()
    {
        return pieceCamp;
    }

    public PieceClass GetPieceClass() { 
        return pieceClass;
    }
}
