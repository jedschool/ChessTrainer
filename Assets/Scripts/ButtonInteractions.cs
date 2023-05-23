using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonInteractions : MonoBehaviour
{
    [SerializeField] private GameObject moves;
    [SerializeField] private GameObject controller;
    [SerializeField] private Transform incorrect;
    [SerializeField] private Transform savePanel;
    [SerializeField] private Transform selectPanel;
    [SerializeField] private VariationSelectSpawn variationSelectSpawn;

    public static string savedVariation;

    public static List<List<string>> savedVariations = new List<List<string>>();
    public static List<string> variationNames = new List<string>();

    bool training = false;

    public void OnNewGameButtonPressed() {
        incorrect = GameObject.FindGameObjectWithTag("Incorrect").transform;
        foreach(Transform child in incorrect) {
            child.gameObject.SetActive(false);
        }
        controller.GetComponent<Game>().DestroyAll();
        moves.GetComponent<TMP_Text>().text = "";
        controller.GetComponent<Game>().white = true;
        controller.GetComponent<Game>().move = 1;
        controller.GetComponent<Game>().training = this.training;
        MovePlate.moveSet = new List<string>();
        controller.GetComponent<Game>().Start();
    }  
    public void OnSaveVariationButtonPressed() {
        foreach(Transform child in savePanel) {
            child.gameObject.SetActive(true);
        }
    }
    public void OnTrainVariationButtonPressed() {
        foreach(Transform child in selectPanel) {
            child.gameObject.SetActive(true);
        }
        variationSelectSpawn.SpawnVariations();
    }
    public void OnVariationSelected() {
        Debug.Log("Test");
        training = true;
        selectPanel.gameObject.SetActive(false);
        OnNewGameButtonPressed();
        training = false;
    }
    public void OnEndTrainVariationButtonPressed() {
        OnNewGameButtonPressed();
    }
    public void OnEnterVariationButtonPressed() {
        savedVariations.Add(MovePlate.moveSet);
        variationNames.Add(GameObject.FindGameObjectWithTag("NameBox").GetComponent<TMP_InputField>().text);
        Debug.Log("Added " + variationNames[variationNames.Count - 1] + " to slot " + savedVariations.Count);
        savedVariation = moves.GetComponent<TMP_Text>().text;
        GameObject.FindGameObjectWithTag("NameBox").GetComponent<TMP_InputField>().text = "";
        foreach(Transform child in savePanel) {
            child.gameObject.SetActive(false);
        }
    }
    public void OnCancelVariationButtonPressed() {
        foreach(Transform child in savePanel) {
            child.gameObject.SetActive(false);
        }
    }
}
