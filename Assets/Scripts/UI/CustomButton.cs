using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public class Scale
{
    [SerializeField]
     float m_hoverScale = 1.20f;

    public float hoverScale
    {
        get
        {
            return m_hoverScale;
        }

        set
        {
            m_hoverScale = value;
        }
    }
}

[System.Serializable]
public class ColorTint
{
    [SerializeField]
    Color m_originalColor = new Color(1, 1, 1, 1);
    [SerializeField]
    Color m_hoverColor = new Color(0, 0, 1, 1);

    public Color originalColor
    {
        get
        {
            return m_originalColor;
        }

        set
        {
            m_originalColor = value;
        }
    }

    public Color hoverColor
    {
        get
        {
            return m_hoverColor;
        }

        set
        {
            m_hoverColor = value;
        }
    }
}

[System.Serializable]
public class SwapSprite
{
    [SerializeField]
    Sprite m_originalSprite = null;
    [SerializeField]
    Sprite m_hoverSprite = null;

    public Sprite originalSprite
    {
        get
        {
            return m_originalSprite;
        }

        set
        {
            m_originalSprite = value;
        }
    }

    public Sprite hoverSprite
    {
        get
        {
            return m_hoverSprite;
        }

        set
        {
            m_hoverSprite = value;
        }
    }
}
[System.Serializable]
public abstract class CustomButton : MonoBehaviour, IPointerEnterHandler,  IPointerExitHandler
{
    public enum Transition
    {
        Scale,
        ColorTint,
        SwapSprite
    }

    enum Type
    {
        Image,
        Text
    }

    public Transition transition;

    public ColorTint m_colorTint;
    public Scale m_scale;
    public SwapSprite m_swapSprite;


    private Button m_button;
    protected ButtonSelector m_owner;
    Type m_type;

    Text m_text;
    Image m_image;


    int m_index;


    public int Index
    {
        get
        {
            return m_index;
        }

        set
        {
            m_index = value;
        }
    }

    protected virtual void Awake() 
    {
        m_text = GetComponent<Text>();
        if (m_text != null)
        {
            m_type = Type.Text;
       
        }
        m_image = GetComponent<Image>();
        if (m_image != null)
        {
            m_type = Type.Image;
        }

        m_owner = transform.parent.GetComponent<ButtonSelector>();
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
       
    }

    public abstract void OnClick();
   
  
    public void OnSelect()
    {
        switch (transition)
        {
            case Transition.Scale:
                transform.localScale = new Vector3(m_scale.hoverScale, m_scale.hoverScale, m_scale.hoverScale);

                break;

            case Transition.ColorTint:

                if (m_type == Type.Text)
                {
                    m_text.color = m_colorTint.hoverColor;
                }
                else
                {
                    m_image.color = m_colorTint.hoverColor;
                }


                break;

            case Transition.SwapSprite:

                break;

        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_owner.SwitchSelection(m_index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void Reset()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (m_type == Type.Text)
        {
            m_text.color = m_colorTint.originalColor;
        }
        else
        {
            m_image.color = m_colorTint.originalColor;
        }
    }
}
