using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VariationSelectSpawn : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject panel;
    [SerializeField] private ButtonInteractions buttonInteractions;

    private List<GameObject> buttons = new List<GameObject>();
    
    private const float WIDTH = 390f;
    private const float HEIGHT = 30f;
    private float yPosition = 65f;

    public void SpawnVariations() {
        for (int i = 0; i < ButtonInteractions.variationNames.Count; i++) {
            Debug.Log("Variation");
            GameObject variationButton = Instantiate(button, panel.transform.position + new Vector3(0, yPosition, 0), Quaternion.identity, panel.transform);
            buttons.Add(variationButton);
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate { SetVariation(i); });
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate { buttonInteractions.OnVariationSelected(); });
            foreach(Transform child in variationButton.transform) {
                child.GetComponent<TMP_Text>().text = ButtonInteractions.variationNames[i];
            }
            yPosition -= HEIGHT;
        }
    }

    private void SetVariation(int i) {
        MovePlate.variation = i;
    }
}
