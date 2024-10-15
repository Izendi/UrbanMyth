using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPickUp : MonoBehaviour
{
    public static InventoryPickUp Instance { get; private set; }

    private GameObject collectedObject;
    public float radius = 2f;
    public float distance = 2f;
    public float height = 1f;

    public GameObject GlobalStateManagerObj;
    private GlobalStateManager GSM_script;

    public Camera playerCamera;

    [SerializeField]
    private AudioClip foundCollectibleSound;

    [SerializeField]
    private AudioClip keyItemFound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate
        }
    }
    private void Start()
    {
        GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
    }

    private void Update()
    {

        if (GlobalStateManagerObj == null)
        {

            GlobalStateManagerObj = GameObject.FindWithTag("GSO"); // Reassign the reference
            if (GlobalStateManagerObj == null)
            {
                Debug.LogWarning("PlayerInteract object not found in the scene.");
                return; // Exit early if playerInteract is still null
            }

            GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
        }

        var t = transform;
        var pressedE = Input.GetKeyDown(KeyCode.E);
        
        if (pressedE)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            //var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
            RaycastHit[] hits = Physics.RaycastAll(ray, radius);

            var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "CollectibleItem");

            if (hitIndex != -1)
            {
                SoundManager.instance.PlaySoundEffect(foundCollectibleSound, transform, 1.0f);

                var hitObject = hits[hitIndex].transform.gameObject;
                collectedObject = hitObject;

                string collectedObjectName = collectedObject.name;

                GSM_script.CollectedNote(collectedObjectName);

                Destroy(collectedObject);

                GSM_script.PauseAndDisplayNote(int.Parse(collectedObjectName));
            }

            var hitIndex2 = Array.FindIndex(hits, hit => hit.transform.tag == "KeyItem");

            if (hitIndex2 != -1)
            {
                SoundManager.instance.PlaySoundEffect(keyItemFound, transform, 1.0f);

                var hitObject = hits[hitIndex2].transform.gameObject;
                collectedObject = hitObject;

                string collectedObjectName = collectedObject.name;

                GSM_script.KeyItemCollected(collectedObjectName);

                Destroy(collectedObject);

                GSM_script.PauseAndDisplayKeyItem(collectedObjectName);
            }
        }
        
    }

}