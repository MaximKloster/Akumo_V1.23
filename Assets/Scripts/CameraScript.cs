using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    // Public unity setter
    [Range(0, 10)]
    public float xOffsetToPlayer = 2f;
    public float followPlayerTime = 10f;

    // Public script variables
    public float MovementSpeed { private get; set; }
    public float XOffsetToPlayer
    {
        get { return xOffsetToPlayer; }
        private set { xOffsetToPlayer = value; }
    }
    public float CameraPositionX { get { return transform.position.x; } }
    public float CameraPositionY { get { return transform.position.y; } }
    
    //float followPlayerPositionY;

    // Update is called once per frame
    void Update()
    {
        // Movement
        transform.Translate(Vector3.right * MovementSpeed * Time.deltaTime);

        // Camera follow player in y direction
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, followPlayerPositionY, 0), Time.deltaTime * followPlayerTime);
    }

    // Set camera to position X
    public void CameraToPosition(Vector3 position)
    {
        transform.position = Vector3.right * (position.x - xOffsetToPlayer);
        //followPlayerPositionY = 0;
    }

    // Set camera follow player in y direction
    public void FollowPlayer(float followPlayerPositionY)
    {
        //this.followPlayerPositionY = followPlayerPositionY;
    }
}
