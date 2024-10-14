using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    // Update is called once per frame

    void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.FindWithTag("PlayCam").GetComponent<Camera>(); 
            //basicMouseLook_script = PlayCamObj.GetComponent<BasicMouseLook>();
        }
    }

    void Update()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.FindWithTag("PlayCam").GetComponent<Camera>(); 
            //basicMouseLook_script = PlayCamObj.GetComponent<BasicMouseLook>();
        }

        //var yRotation = Camera.main.transform.rotation.eulerAngles.y;

        var yRotation = playerCamera.transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
