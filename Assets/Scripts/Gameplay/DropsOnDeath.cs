using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsOnDeath : MonoBehaviour {
    
    //Droppable item struct
    [System.Serializable]
    struct DroppableObject
    {
        public GameObject m_object;

        [Range(0, 100)]
        public float m_dropChance;
    }

    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    [SerializeField] private DroppableObject[] m_droppedObject;

    //---------------------------------------------------------------------------------------------
    // Monobehavior overrides
    //---------------------------------------------------------------------------------------------
    private void OnDestroy()
    {
        //Get the current active room
        SpriteRenderer activeRoom = FindObjectOfType<CameraManager>().targetRoom;

        //Loop for every possible item drop
        foreach(DroppableObject droppableItem in m_droppedObject)
        {
            float randomValue = Random.Range(0.0f, 100.0f);

            //If the random is a success
            if (randomValue < droppableItem.m_dropChance)
            {
                //Instantiate the item
                Instantiate(droppableItem.m_object, transform.position, transform.rotation, activeRoom.transform);
            }
        }
    }
}
