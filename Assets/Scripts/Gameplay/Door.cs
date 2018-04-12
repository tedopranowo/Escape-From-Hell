using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{  
    //---------------------------------------------------------------------------------------------
    // Enum
    //---------------------------------------------------------------------------------------------
    public enum Type
    {
        Default,
        Red,
        Green,
        Blue
    }

    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    //---------------------------------------------------------------------------------------------
    // Fields
    //---------------------------------------------------------------------------------------------
    [SerializeField]
    private float m_doorHorizontalTeleport = 5;

    [SerializeField]
    private float m_doorVerticalTeleport = 5;

    [SerializeField]
    private bool m_isLockedDoor;

    [SerializeField]
    private Orientation m_orientation;

    [SerializeField]
    private SpriteRenderer m_curRoom;
    [SerializeField]
    private SpriteRenderer m_nextRoom;

    [SerializeField]
    private Sprite m_unlockedSprite;

    [SerializeField]
    private Sprite m_lockedSprite;

    [SerializeField]
    private Type m_type;

    private SpriteRenderer m_doorSpriteRenderer;

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public SpriteRenderer curRoom { get { return m_curRoom; } set { m_curRoom = value; } }
    public SpriteRenderer nextRoom { get { return m_nextRoom; } set { m_nextRoom = value; } }

    public Type type
    {
        get { return m_type; }
        set
        {
            m_type = value;

            //Set the color of the sprite
            switch (m_type)
            {
                case Type.Red:
                    doorSpriteRenderer.color = Color.red;
                    break;
                case Type.Green:
                    doorSpriteRenderer.color = Color.green;
                    break;
                case Type.Blue:
                    doorSpriteRenderer.color = Color.blue;
                    break;
                case Type.Default:
                    doorSpriteRenderer.color = Color.white;
                    break;
            }
        }
    }

    public Orientation orientation
    {
        set { m_orientation = value; }
        get { return m_orientation; }
    }

    public bool IsLocked
    {
        set
        {
            m_isLockedDoor = value;
            if (m_isLockedDoor)
                doorSpriteRenderer.sprite = m_lockedSprite;
            else
                doorSpriteRenderer.sprite = m_unlockedSprite;

        }
        get { return m_isLockedDoor; }
    }

    public SpriteRenderer doorSpriteRenderer
    {
        get
        {
            if (m_doorSpriteRenderer == null)
            {
                GameObject child = gameObject.transform.GetChild(0).gameObject;
                m_doorSpriteRenderer = child.GetComponent<SpriteRenderer>();
            }
            return m_doorSpriteRenderer;
        }
    }

    //---------------------------------------------------------------------------------------------
    // MonoBehaviours
    //---------------------------------------------------------------------------------------------
    private void Awake()
    {
        type = m_type;
        IsLocked = m_isLockedDoor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the object entering the trigger is not controlled by the player, don't do anything
        if (collision.GetComponent<PlayerController>() == null)
            return;

        //If the door is not locked, teleport the player immediately to the other room
        if (!m_isLockedDoor)
        {
            UseDoor();
        }
        //If the door is locked, check if the player owns the key
        else
        {
            if (type == Door.Type.Red)
            {
                if (Inventory.instance.UseKey(Key.Type.Red))
                {
                    UseDoor();
                }
            }

            else if (type == Door.Type.Green)
            {
                if (Inventory.instance.UseKey(Key.Type.Green))
                {
                    UseDoor();
                }
            }

            else if (type == Door.Type.Blue)
            {
                if (Inventory.instance.UseKey(Key.Type.Blue))
                {
                    UseDoor();
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------
    // Methods
    //---------------------------------------------------------------------------------------------
    private void UseDoor()
    {
        Debug.Log(m_nextRoom.GetInstanceID());

        //If the door is locked, unlock the door
        if (m_isLockedDoor)
        {
            m_isLockedDoor = false;

            GameObject child = gameObject.transform.GetChild(0).gameObject;
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = m_unlockedSprite;
        }

        //Destroy all projectiles
        ProjectileScript[] projectileList = FindObjectsOfType<ProjectileScript>();

        foreach(ProjectileScript projectile in projectileList)
        {
            Destroy(projectile.gameObject);
        }

        //Teleport the player
        Vector3 playerVec = PlayerController.instance.gameObject.transform.position;

        if (m_orientation == Door.Orientation.Horizontal)
        {
            if (gameObject.transform.position.y < PlayerController.instance.gameObject.transform.position.y)
            {
                PlayerController.instance.gameObject.transform.position = new Vector3(playerVec.x, playerVec.y - m_doorVerticalTeleport, playerVec.z);
            }
            else
            {
                PlayerController.instance.gameObject.transform.position = new Vector3(playerVec.x, playerVec.y + m_doorVerticalTeleport, playerVec.z);
            }
        }
        else if (m_orientation == Door.Orientation.Vertical)
        {
            if (gameObject.transform.position.x < PlayerController.instance.gameObject.transform.position.x)
            {
                PlayerController.instance.gameObject.transform.position = new Vector3(playerVec.x - m_doorHorizontalTeleport, playerVec.y, playerVec.z);
            }
            else
            {
                PlayerController.instance.gameObject.transform.position = new Vector3(playerVec.x + m_doorHorizontalTeleport, playerVec.y, playerVec.z);
            }
        }

        //Move the camera to the new room
        FindObjectOfType<CameraManager>().targetRoom = m_nextRoom;

        //Update minimap
        Minimap.MinimapManager minimap = FindObjectOfType<Minimap.MinimapManager>();
        if (minimap)
        {
            minimap.GoToRoom(m_nextRoom.transform);
        }
    }

}
