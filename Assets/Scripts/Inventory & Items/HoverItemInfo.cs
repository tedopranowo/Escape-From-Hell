using UnityEngine;
using UnityEngine.EventSystems;

public interface IHoverableDescription
{
    string hoveredTitle { get; }
    string hoveredDescription { get; }
}


public class HoverItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    private IHoverableDescription m_hoveredDescription = null;

    private ItemHoverDescription m_itemHoverDescription = null;

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public IHoverableDescription hoveredData { set { m_hoveredDescription = value; } }


    //---------------------------------------------------------------------------------------------
    // Monobehavior
    //---------------------------------------------------------------------------------------------
    void Start()
    {
        m_itemHoverDescription = GameObject.Find("ItemDescPanel").GetComponent<ItemHoverDescription>();
    }

    //---------------------------------------------------------------------------------------------
    // IPointerEnterHandler
    //---------------------------------------------------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_itemHoverDescription.ShowDescription(m_hoveredDescription);
        m_itemHoverDescription.transform.position = transform.position;
    }

    //---------------------------------------------------------------------------------------------
    // IPointerExitHandler
    //---------------------------------------------------------------------------------------------
    public void OnPointerExit(PointerEventData eventData)
    {
        m_itemHoverDescription.HideDescription();
    }
}
