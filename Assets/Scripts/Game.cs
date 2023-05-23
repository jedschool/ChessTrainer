using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    
    [SerializeField] private GameObject piece;

    private GameObject[,] positions = new GameObject[8,8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    public bool white = true;
    public int move = 1;
    private bool gameOver = false;
    public bool training = false;
    
    public void Start() {
        playerWhite = new GameObject[] {
            Create("white_rook", 0, 0), Create("white_knight", 1, 0), Create("white_bishop", 2, 0), 
            Create("white_queen", 3, 0), Create("white_king", 4, 0), Create("white_bishop", 5, 0), 
            Create("white_knight", 6, 0), Create("white_rook", 7, 0), Create("white_pawn", 0, 1), 
            Create("white_pawn", 1, 1), Create("white_pawn", 2, 1), Create("white_pawn", 3, 1), 
            Create("white_pawn", 4, 1), Create("white_pawn", 5, 1), Create("white_pawn", 6, 1), 
            Create("white_pawn", 7, 1) 
        };
        playerBlack = new GameObject[] {
            Create("black_rook", 0, 7), Create("black_knight", 1, 7), Create("black_bishop", 2, 7), 
            Create("black_queen", 3, 7), Create("black_king", 4, 7), Create("black_bishop", 5, 7), 
            Create("black_knight", 6, 7), Create("black_rook", 7, 7), Create("black_pawn", 0, 6), 
            Create("black_pawn", 1, 6), Create("black_pawn", 2, 6), Create("black_pawn", 3, 6), 
            Create("black_pawn", 4, 6), Create("black_pawn", 5, 6), Create("black_pawn", 6, 6), 
            Create("black_pawn", 7, 6) 
        };

        for(int i = 0; i < playerBlack.Length; i++) {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public GameObject Create(string name, int x, int y) {
        
        GameObject obj = Instantiate(piece, Vector3.zero, Quaternion.identity);
        Piece p = obj.GetComponent<Piece>();
        p.name = name;
        p.SetXBoard(x);
        p.SetYBoard(y);
        p.Activate();
        p.training = training;

        return obj;
    }

    public void DestroyAll() {
        foreach(GameObject p in playerBlack) {
            Destroy(p);
        }
        foreach(GameObject p in playerWhite) {
            Destroy(p);
        }
    }

    public void SetPosition(GameObject obj) {
        Piece p = obj.GetComponent<Piece>();

        positions[p.GetXBoard(), p.GetYBoard()] = obj;
    }
    public void SetPositionEmpty(int x, int y) {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y) {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y) {
        if(x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) {
            return false;
        }
        else {
            return true;
        }
    }

    public bool GetCurrentPlayer() {
        return white;
    }
    public bool IsGameOver() {
        return gameOver;
    }
    public void NextTurn() {
        white = !white;
    }

    public void Update() {
        if(gameOver == true && Input.GetMouseButtonDown(0)) {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
    }
}
