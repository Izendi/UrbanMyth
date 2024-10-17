using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_Spawner : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject parentObject = GameObject.FindGameObjectWithTag("DM");
        foreach (Transform child in parentObject.transform)
        {
            child.gameObject.SetActive(true); // Activate each child GameObject
        }
    }
    // Update is called once per frame
    void Update()
    {
         
        
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            if (sceneBuildIndex == 0)
            {
                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_0_NPC");

                foreach (GameObject obj in objectsWithTag)
                {
                    obj.SetActive(true);
                    // Perform actions with each object, e.g., enable, disable, change properties, etc.
                }

            }
            else
            {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_0_NPC");

                foreach (GameObject obj in objectsWithTag)
                {
                    obj.SetActive(false);
                    // Perform actions with each object, e.g., enable, disable, change properties, etc.
                }
            }

        if (sceneBuildIndex == 1)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_01_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(true);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }

        }
        else
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_01_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(false);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }
        }

        if (sceneBuildIndex == 2)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_02_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(true);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }

        }
        else
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_02_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(false);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }
        }

        if (sceneBuildIndex == 3)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_03_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(true);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }

        }
        else
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_03_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(false);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }
        }

        if (sceneBuildIndex == 4)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_04_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(true);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }

        }
        else
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_04_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(false);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }
        }

        if (sceneBuildIndex == 5)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_05_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(true);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }

        }
        else
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_05_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(false);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }
        }

        if (sceneBuildIndex == 6)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_06_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(true);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }

        }
        else
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("floor_06_NPC");

            foreach (GameObject obj in objectsWithTag)
            {
                obj.SetActive(false);
                // Perform actions with each object, e.g., enable, disable, change properties, etc.
            }
        }

    }
}
