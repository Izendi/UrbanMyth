using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> lines;
    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<string>();
    }
}
