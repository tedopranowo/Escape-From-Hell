using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    [SerializeField][Tooltip("The sprite which the camera must capture")]
    private SpriteRenderer m_targetRoom = null;

    public SpriteRenderer targetRoom
    {
        set
        {
            //Disable the previous room
            if (targetRoom != null)
                targetRoom.gameObject.SetActive(false);

            //Set the new target room
            m_targetRoom = value;

            //Enable the new room
            if (targetRoom != null)
                m_targetRoom.gameObject.SetActive(true);
        }
        get
        {
            return m_targetRoom;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Quit if target room is null
        if (m_targetRoom == null)
        // Instead quitting, maybe try finding a new room?
            return;

        LookAt(m_targetRoom);
    }

    private void LookAt(SpriteRenderer targetSprite)
    {
        // Get the sprite size
        // Normally, we want to save the SpriteRenderer in a variable since it is expensive. But, right now I want the editor to update
        // the camera when a new object is placed. This code will be changed (see TODO above)
        Vector2 targetSize = targetSprite.GetComponent<SpriteRenderer>().bounds.size;
        Transform target = targetSprite.transform;

        float screenRatio = (float)Screen.height / Screen.width;
        float worldRatio = targetSize.y / targetSize.x;
        float cameraSize;

        // If the world's height ratio is bigger or equal to the screen height ratio, 
        // use the world height as the minimum limit
        if (worldRatio > screenRatio)
            cameraSize = targetSize.y;
        // If the world's height ratio is equal or less than the screen height ratio, 
        // use the world width as the minimum limit
        else
            cameraSize = targetSize.x * screenRatio;

        // Set the camera orthographic size
        GetComponent<Camera>().orthographicSize = cameraSize / 2;

        // Set the camera x, y to be the same as the target x, y
        if (target)
        {
            Vector3 targetCameraPosition = target.position;
            targetCameraPosition.z = GetComponent<Camera>().transform.position.z;

            GetComponent<Camera>().transform.position = targetCameraPosition;
        }
    }
}