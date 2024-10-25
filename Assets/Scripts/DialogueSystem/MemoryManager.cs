using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class MemoryManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI MemoryText; // text can be split by sections using separating them with the tag <newsection>

    [SerializeField] 
    private Button NextBtn;
    
    [SerializeField] 
    private Button SkipBtn;

    [SerializeField]
    private Button CloseBtn;

    private List<string> textSections;
    private int currentSectionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        NextBtn.onClick.AddListener(NextSection);
        SkipBtn.onClick.AddListener(SkipMemory);
        CloseBtn.onClick.AddListener(CloseMemory);

        if (!string.IsNullOrEmpty(MemoryText.text))
        {
            textSections = MemoryText.text.Split("<newsection>").ToList();

            if (textSections.Count == 0)
            {
                return;
            }

            if (textSections.Count == 1)
            {
                MemoryText.text = textSections[0];
                NextBtn.enabled = false;
                SkipBtn.enabled = false;
                CloseBtn.enabled = true;
            }
            else
            {
                MemoryText.text = textSections[0];
                NextBtn.enabled = true;
                SkipBtn.enabled = true;
                CloseBtn.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextSection()
    {
        if (currentSectionIndex < textSections.Count - 1)
        {
            currentSectionIndex++;
            MemoryText.text = textSections[currentSectionIndex];
        }

        if (currentSectionIndex == textSections.Count - 1)
        {
            NextBtn.enabled = false;
            SkipBtn.enabled = false;
            CloseBtn.enabled = true;
        }
    }

    public void SkipMemory()
    {
        NextBtn.enabled = false;
        SkipBtn.enabled = false;
        CloseBtn.enabled = true;
    }

    public void CloseMemory()
    {
        gameObject.SetActive(false);
    }
}
