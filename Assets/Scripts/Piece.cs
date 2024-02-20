using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceCamp
{
    Nature, Neutral
}

public enum PieceType
{
    Empty, Temple, Devotee
}

public class Piece : MonoBehaviour
{
    public PieceCamp pieceCamp;
    public PieceType pieceType;

    public PieceCamp GetPieceCamp()
    {
        return pieceCamp;
    }

    public PieceType GetPieceType() { 
        return pieceType;
    }

    public bool IsPieceEqual(Piece otherPiece)
    {
        return otherPiece != null && otherPiece.pieceType == pieceType && otherPiece.pieceCamp == pieceCamp;
    }
}
