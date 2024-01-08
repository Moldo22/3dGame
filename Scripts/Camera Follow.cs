#region Libraries
using UnityEngine;
using Unity.Netcode;
#endregion

public class CameraFollow : NetworkBehaviour
{
    #region Inputs
    public Transform target;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float rotationSpeed = 5.0f;
    private GameObject ultimulPlayer;
    private float rotationX = 0.0f;
    public float verticalRotationLimit = 80.0f;
    public bool first = false;
    #endregion

    #region Handler
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V)) first = !first;
        if (ultimulPlayer == null)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            if (playerObjects.Length > 0) ultimulPlayer = playerObjects[playerObjects.Length - 1];
        }
        if (ultimulPlayer != null) target.transform.position = ultimulPlayer.transform.position;
        if (target != null && !first)
        {
            
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

          
            transform.position = target.position;
            target.Rotate(0, mouseX, 0);
 
            rotationX += mouseY;
            rotationX = Mathf.Clamp(rotationX, -verticalRotationLimit, verticalRotationLimit);
            Quaternion cameraRotation = Quaternion.Euler(rotationX, target.eulerAngles.y, 0);

          
            Vector3 cameraPosition = target.position - cameraRotation * Vector3.forward * distance;
            cameraPosition.y = target.position.y + height;

            transform.rotation = cameraRotation;
            transform.position = cameraPosition;
        }
        if (target != null && first)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

            rotationX += mouseY;
            rotationX = Mathf.Clamp(rotationX, -verticalRotationLimit, verticalRotationLimit);

            Quaternion cameraRotation = Quaternion.Euler(rotationX, target.eulerAngles.y, 0);
            transform.rotation = cameraRotation;

            Vector3 cameraPosition = target.position;
            /*cameraPosition.y = target.position.y + height;

            transform.position = cameraPosition - transform.forward * distance;
       */ }
    }
    #endregion
}

