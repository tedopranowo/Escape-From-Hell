using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHoverDescription : MonoBehaviour
{

    [SerializeField]
    Text m_itemNameText = null;
    [SerializeField]
    Text m_itemDescText = null;

    public void ShowDescription(IHoverableDescription description)
    {
        //We don't want to use description != null since it will result in true when
        //the object is destroyed by Unity
        if (description as Object)
        {
            //Set the object title and description
            m_itemNameText.text = description.hoveredTitle;
            m_itemDescText.text = description.hoveredDescription;

            //Set the object to active
            gameObject.SetActive(true);

            //If the object is bottom is out of canvas bound, readjust the position
            ReadjustPosition();
        }
        else
        {
            HideDescription();
        }
    }

    public void HideDescription()
    {
        gameObject.SetActive(false);
    }

    //Readjust this object so that it doesn't went out of canvas
    private void ReadjustPosition()
    {
        RectTransform thisRect = GetComponent<RectTransform>();
        Canvas parentCanvas = GetComponentInParent<Canvas>();

        thisRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0.0f, thisRect.rect.height);
    }
}
