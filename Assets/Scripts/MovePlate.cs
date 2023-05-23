using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovePlate : MonoBehaviour
{
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject moves;
    [SerializeField] private Transform incorrect;

    private GameObject reference = null;
    private GameObject castleRook = null;

    public static List<string> moveSet = new List<string>();

    private int matrixX;
    private int matrixY;

    private int originalX;
    private int originalY;
    private int castleResetX;
    private int castleResetY;

    public static int variation = 0;

    public bool attack = false;
    public bool castle = false;
    public int castleDirection; // +1 for short castle, -2 for long castle
    public int rookPosition; // -1 for short castle, +1 for long castle

    public bool training = false;

    public void OnMouseUp() {
        controller = GameObject.FindGameObjectWithTag("GameController");
        incorrect = GameObject.FindGameObjectWithTag("Incorrect").transform;
        bool capture = false;
        originalX = reference.GetComponent<Piece>().GetXBoard();
        originalY = reference.GetComponent<Piece>().GetYBoard();

        if(training) {
            if(attack) {
                capture = true;
            }

            if(castle) {
                if(castleDirection > 0) {
                    if(ButtonInteractions.savedVariations[variation][moveSet.Count - 1] != "0-0") {
                        foreach(Transform child in incorrect) {
                            child.gameObject.SetActive(true);
                        }
                        return;
                    }
                }
                else if(castleDirection < 0) {
                    if(ButtonInteractions.savedVariations[variation][moveSet.Count - 1] != "0-0-0") {
                        foreach(Transform child in incorrect) {
                            child.gameObject.SetActive(true);
                        }
                        return;
                    }
                }
            }
            else {
                Debug.Log(ButtonInteractions.savedVariations[variation - 1][moveSet.Count]);
                if(ButtonInteractions.savedVariations[variation - 1][moveSet.Count] != GetMoveCoordinates(matrixX, matrixY, capture)) {
                    foreach(Transform child in incorrect) {
                        child.gameObject.SetActive(true);
                    }
                    return;
                }
            }
        }


        if(attack) {
            GameObject piece = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            capture = true;
            Destroy(piece);
        }
        if(castle) {
            Game sc = controller.GetComponent<Game>();
            castleRook = sc.GetPosition(matrixX + castleDirection, matrixY);
            controller.GetComponent<Game>().SetPositionEmpty(
                castleRook.GetComponent<Piece>().GetXBoard(),
                castleRook.GetComponent<Piece>().GetYBoard());

            castleRook.GetComponent<Piece>().SetXBoard(matrixX + rookPosition);
            castleRook.GetComponent<Piece>().SetYBoard(matrixY);
            castleRook.GetComponent<Piece>().SetCoords();
            castleRook.GetComponent<Piece>().moved = true;
            controller.GetComponent<Game>().SetPosition(castleRook);
        }
        controller.GetComponent<Game>().SetPositionEmpty(
            reference.GetComponent<Piece>().GetXBoard(),
            reference.GetComponent<Piece>().GetYBoard()
        );
        
        reference.GetComponent<Piece>().SetXBoard(matrixX);
        reference.GetComponent<Piece>().SetYBoard(matrixY);
        reference.GetComponent<Piece>().SetCoords();
        reference.GetComponent<Piece>().moved = true;

        moves = GameObject.FindGameObjectWithTag("Moves");
        string add;
        string prefix;
        string suffix;

        if(castle) {
            if(rookPosition > 0) {
                add = "0-0-0";
            }
            else {
                add = "0-0";
            }
        }
        else {
            add = GetMoveCoordinates(reference.GetComponent<Piece>().GetXBoard(), reference.GetComponent<Piece>().GetYBoard(), capture);
        }

        moveSet.Add(add);

        if(training) {
            if(moveSet.Count % 2 == 1) {
                prefix = (controller.GetComponent<Game>().move++ / 2 + 1) + ". ";
                suffix = "";
            }
            else {
                controller.GetComponent<Game>().move++;
                prefix = " ";
                suffix = "\n";
            }
        }
        else if((moveSet.Count) % 2 == 1) {
            prefix = (controller.GetComponent<Game>().move++ / 2 + 1) + ". ";
            suffix = "";
        }
        else {
            controller.GetComponent<Game>().move++;
            prefix = " ";
            suffix = "\n";
        }

        moves.GetComponent<TMP_Text>().text += (prefix + add + suffix);

        controller.GetComponent<Game>().SetPosition(reference);
        reference.GetComponent<Piece>().DestroyMovePlates();

        foreach(Transform child in incorrect) {
            child.gameObject.SetActive(false);
        }
        controller.GetComponent<Game>().NextTurn();
    }

    public string GetMoveCoordinates(int x, int y, bool capture) {
        string cap = "";
        string p = reference.GetComponent<Piece>().type;
        if(capture) {
            cap = "x";
            if(p == "") {
                p = ((char)(originalX + 65)).ToString().ToLower();
            }
        }
        string file = ((char)(x + 65)).ToString().ToLower();
        return p + cap + file + (y + 1);
    }

    public void SetCoords(int x, int y) {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj) {
        reference = obj;
    }

    public GameObject GetReference() {
        return reference;
    }
}
