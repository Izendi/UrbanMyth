using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Camera.main);
        //var yRotation = Camera.main.transform.rotation.eulerAngles.y;

        var yRotation = playerCamera.transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
