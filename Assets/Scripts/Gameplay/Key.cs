using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    public enum Type
    {
        Red,
        Green,
        Blue,
        kCount
    }

    [SerializeField]
    private Sprite[] m_sprites;

    [SerializeField]
    private Type m_color;

    public Type type
    {
       set { m_color = value; }
       get { return m_color; }
    }

    private void Awake()
    {
        Debug.Log("key Spawning");
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch (m_color)
        {
            case Type.Red:
                //spriteRenderer.sprite = Resources.Load("redKey", typeof(Sprite)) as Sprite;
                spriteRenderer.sprite = m_sprites[(int)Type.Red];
                break;
            case Type.Green:
                spriteRenderer.sprite = m_sprites[(int)Type.Green];
                break;
            case Type.Blue:
                spriteRenderer.sprite = m_sprites[(int)Type.Blue];
                break;
        }

    }

    public override void Interact()
    {
        Inventory.instance.AddToInventory(type);
        Destroy(gameObject);
    }


    public void SetType(Type type)
    {
        m_color = type;

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch (m_color)
        {
            case Type.Red:
                spriteRenderer.sprite = m_sprites[(int)Type.Red];
                break;
            case Type.Green:
                spriteRenderer.sprite = m_sprites[(int)Type.Green];
                break;
            case Type.Blue:
                spriteRenderer.sprite = m_sprites[(int)Type.Blue];
                break;
        }
    }
}
