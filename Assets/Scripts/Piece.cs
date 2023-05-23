using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    private float multiple = 1.2f;
    private float add = -4.2f;

    private bool white;
    public bool moved = true;
    public bool training = false;

    [SerializeField] private Sprite black_king, black_queen, black_rook, black_bishop, black_knight, black_pawn,
    white_king, white_queen, white_rook, white_bishop, white_knight, white_pawn;
    public string type;

    public void Activate() {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch(this.name) {
            case "black_queen": GetComponent<SpriteRenderer>().sprite = black_queen;
            white = false;
            type = "Q";
            break;
            case "black_king": GetComponent<SpriteRenderer>().sprite = black_king;
            white = false;
            type = "K";
            moved = false;
            break;
            case "black_rook": GetComponent<SpriteRenderer>().sprite = black_rook;
            white = false;
            type = "R";
            moved = false;
            break;
            case "black_bishop": GetComponent<SpriteRenderer>().sprite = black_bishop;
            white = false;
            type = "B";
            break;
            case "black_knight": GetComponent<SpriteRenderer>().sprite = black_knight;
            white = false;
            type = "N";
            break;
            case "black_pawn": GetComponent<SpriteRenderer>().sprite = black_pawn;
            white = false;
            type = "";
            break;

            case "white_queen": GetComponent<SpriteRenderer>().sprite = white_queen;
            white = true;
            type = "Q";
            break;
            case "white_king": GetComponent<SpriteRenderer>().sprite = white_king;
            white = true;
            type = "K";
            moved = false;
            break;
            case "white_rook": GetComponent<SpriteRenderer>().sprite = white_rook;
            white = true;
            type = "R";
            moved = false;
            break;
            case "white_bishop": GetComponent<SpriteRenderer>().sprite = white_bishop;
            white = true;
            type = "B";
            break;
            case "white_knight": GetComponent<SpriteRenderer>().sprite = white_knight;
            white = true;
            type = "N";
            break;
            case "white_pawn": GetComponent<SpriteRenderer>().sprite = white_pawn;
            white = true;
            type = "";
            break;
        }
    }

    public void SetCoords() {
        float x = xBoard;
        float y = yBoard;

        x *= multiple;
        y *= multiple;

        x += add;
        y += add;

        transform.position = new Vector3(x, y, 0);
    }

    public int GetXBoard() {
        return xBoard;
    }
    public int GetYBoard() {
        return yBoard;
    }
    public void SetXBoard(int x) {
        xBoard = x;
    }
    public void SetYBoard(int y) {
        yBoard = y;
    }

    private void OnMouseUp() {
       
        if(!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == white) {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates() {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i < movePlates.Length; i++) {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates() {
        switch(this.name) {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                CastleMovePlate(xBoard + 2, yBoard);
                CastleMovePlate(xBoard - 2, yBoard);
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                if(yBoard == 6) {
                    PawnMovePlate(xBoard, yBoard - 2);
                }
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                if(yBoard == 1) {
                    PawnMovePlate(xBoard, yBoard + 2);
                }
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement) {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while(sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null) {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if(sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Piece>().white != white) {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate() {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate() {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y) {
        Game sc = controller.GetComponent<Game>();
        if(sc.PositionOnBoard(x, y)) {
            GameObject cp = sc.GetPosition(x, y);

            if(cp == null) {
                MovePlateSpawn(x, y);
            } 
            else if(cp.GetComponent<Piece>().white != white) {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y) {
        Game sc = controller.GetComponent<Game>();
        if(sc.PositionOnBoard(x, y)) {
            if(sc.GetPosition(x, y) == null) {
                MovePlateSpawn(x, y);
            }
        }

        if(sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null &&
           sc.GetPosition(x + 1, y).GetComponent<Piece>().white != white) {
            MovePlateAttackSpawn(x + 1, y);
        }

        if(sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null &&
           sc.GetPosition(x - 1, y).GetComponent<Piece>().white != white) {
            MovePlateAttackSpawn(x - 1, y);
        }
    }

    public void CastleMovePlate(int x, int y) {
        Game sc = controller.GetComponent<Game>();
        if(!moved) {
            // short castle plate
            if(/*empty lane*/ sc.GetPosition(x, y) == null && sc.GetPosition(x - 1, y) == null && 
            /*corner has rook that hasn't moved*/ sc.GetPosition(x + 1, y) != null && !sc.GetPosition(x + 1, y).GetComponent<Piece>().moved) {
                MovePlateSpawn(x, y, true, 1, -1);
            }
            // long castle plate
            if(/*empty lane*/ sc.GetPosition(x, y) == null && sc.GetPosition(x - 1, y) == null && sc.GetPosition(x + 1, y) == null &&
            /* corner has rook that hasn't moved*/ sc.GetPosition(x - 2, y) != null && !sc.GetPosition(x - 2, y).GetComponent<Piece>().moved) {
                MovePlateSpawn(x, y, true, -2, 1);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY, bool castle = false, int castleDirection = 0, int rookPosition = 0) {
        float x = matrixX;
        float y = matrixY;

        x *= multiple;
        y *= multiple;

        x += add;
        y += add;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = false;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.castle = castle;
        mpScript.castleDirection = castleDirection;
        mpScript.rookPosition = rookPosition;
        if(this.training) {
            mpScript.training = true;
        }
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY) {
        float x = matrixX;
        float y = matrixY;

        x *= multiple;
        y *= multiple;

        x += add;
        y += add;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
        if(this.training) {
            mpScript.training = true;
        }
    }
}
