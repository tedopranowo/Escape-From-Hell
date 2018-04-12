using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelector : MonoBehaviour {

    [SerializeField]
    private List <CustomButton> m_buttonList;
    public enum Type
    {
        kVertical,
        kHorizontal
    }

    [SerializeField]
    Type m_type;

    int m_currentButtonIndex = 0;
    public int CurrentButtonIndex
    {
        get
        {
            return m_currentButtonIndex;
        }

        set
        {
            m_currentButtonIndex = value;
        }
    }

    public bool IsLocked
    {
        get
        {
            return m_isLocked;
        }

        set
        {
            m_isLocked = value;
        }
    }

    bool m_axisInUse = false;
    private bool m_isLocked;

    void Awake()
    {
        m_currentButtonIndex = 0;
    
        for(int i = 0; i < transform.childCount; i++)
        {
            m_buttonList.Add(transform.GetChild(i).GetComponent<CustomButton>());
            m_buttonList[i].Index = i;
        }
       
    }

    void Start()
    {
        m_buttonList[0].OnSelect();
    }
    void Update()
    {
        if(m_type == Type.kVertical)
        {
            float y = Input.GetAxisRaw("Vertical");

            y = y > 1? 1 : y;

            y = y < -1 ? -1 : y;

            if (y > 0 && !m_axisInUse && !IsLocked)
            {
                if(CurrentButtonIndex > 0)
                {
                    m_buttonList[m_currentButtonIndex].Reset();
                    m_currentButtonIndex--;
                    m_buttonList[m_currentButtonIndex].OnSelect();
                }
                m_axisInUse = true;
            }
            else if (y < 0 && !m_axisInUse && !IsLocked)
            {
                if (CurrentButtonIndex < (m_buttonList.Count - 1))
                {
                    m_buttonList[m_currentButtonIndex].Reset();
                    m_currentButtonIndex++;
                    m_buttonList[m_currentButtonIndex].OnSelect();
                }
                m_axisInUse = true;
            }

            if(y == 0)
            {
                m_axisInUse = false;
            }
        }   


        
        if(Input.GetButtonDown("Submit"))
        {
            m_buttonList[m_currentButtonIndex].OnClick();
        }  
    }

    public void SwitchSelection(int index)
    {
        m_buttonList[m_currentButtonIndex].Reset();
        m_currentButtonIndex = index;
        m_buttonList[m_currentButtonIndex].OnSelect();
    }
}
