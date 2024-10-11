using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPickUp : MonoBehaviour
{
    private GameObject collectedObject;
    public float radius = 2f;
    public float distance = 2f;
    public float height = 1f;

    public GameObject GlobalStateManagerObj;
    private GlobalStateManager GSM_script;

    public Camera playerCamera;

    [SerializeField]
    private AudioClip foundCollectibleSound;

    private void Start()
    {
        GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
    }

    private void Update()
    {
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
                
            }
        }
        
    }

}