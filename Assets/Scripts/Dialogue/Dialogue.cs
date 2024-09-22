using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float printingSpeed;
    public int index;
    void Start()
    {

        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(PrintText());

    }

    IEnumerator PrintText()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(printingSpeed);
        }
    }
}
