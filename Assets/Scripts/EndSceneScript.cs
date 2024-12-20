using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private int nextSceneNo = 13;

    public void Start()
    {
        quitButton.onClick.AddListener(quitGame);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            quitGame();
    }
    public void quitGame()
   {
        SceneManager.LoadScene(nextSceneNo);
   }
}
