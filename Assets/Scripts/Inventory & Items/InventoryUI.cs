using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {



    private GameObject [] m_weaponSlots;

    private GameObject[] m_consumableSlots;

    [SerializeField]
    private GameObject [] m_keys;

    [SerializeField]
    private GameObject m_slot;
    [SerializeField]
    private GameObject m_weaponPanel;
    [SerializeField]
    private GameObject m_consumablePanel;
 
    [SerializeField]
    private Image m_reloadIcon;
    [SerializeField]
    private Text m_reloadText;

    public void Init(int weaponListSize, int consumableListSize)
    {

        m_weaponSlots = new GameObject[weaponListSize];
        m_consumableSlots = new GameObject[consumableListSize];

        for(int i = 0; i < weaponListSize; i++)
        {
           GameObject weaponSlot = Instantiate(m_slot, m_weaponPanel.transform) as GameObject;
           m_weaponSlots[i] = weaponSlot;
           m_weaponSlots[i].GetComponent<Image>().color = Color.white;
        }

        for (int i = 0; i < consumableListSize; i++)
        {
            GameObject consumableSlot =  Instantiate(m_slot, m_consumablePanel.transform) as GameObject;
            m_consumableSlots[i] = consumableSlot;
            m_consumableSlots[i].GetComponent<Image>().color = Color.white;
        }
    }

    public void UpdateConsumablesUI(int index, Item item)
    {
        if(item == null)
        {
            m_consumableSlots[index].GetComponent<Image>().color = Color.white;
            m_consumableSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
            m_consumableSlots[index].transform.GetChild(0).GetComponent<HoverItemInfo>().hoveredData = null;
        }
        else
        {   
            m_consumableSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
            m_consumableSlots[index].transform.GetChild(0).GetComponent<HoverItemInfo>().hoveredData = item;
        }
      

    }

    public void UpdateWeaponUI(int index, Item item)
    {
        if (item == null)
        {
            m_weaponSlots[index].GetComponent<Image>().color = Color.white;
            m_weaponSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
            m_weaponSlots[index].transform.GetChild(0).GetComponent<HoverItemInfo>().hoveredData = null;
        }
        else
        {
            m_weaponSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
            m_weaponSlots[index].transform.GetChild(0).GetComponent<HoverItemInfo>().hoveredData = item;
        }

    }

    public void HighlightConsumableUI(int oldIndex, int newIndex)
    {
        m_consumableSlots[oldIndex].GetComponent<Image>().color = Color.white;
        m_consumableSlots[newIndex].GetComponent<Image>().color = Color.green;
    }

    public void HighlightWeaponUI(int oldIndex, int newIndex)
    {
        m_weaponSlots[oldIndex].GetComponent<Image>().color = Color.white;
        m_weaponSlots[newIndex].GetComponent<Image>().color = Color.green;
    }

    public void ShowReloadUI(bool act, float fireRate)
    {
        if(act && fireRate > 0.85f)
        {
            m_reloadIcon.enabled = true;
            m_reloadText.enabled = true;
        }
        else
        {
            m_reloadIcon.enabled = false;
            m_reloadText.enabled = false;
        }
    }

    public void UpdateReloadUI(float reloadMax, float fireRate)
    {
        if (Time.time > reloadMax || fireRate < 0.85f)
        {
            m_reloadIcon.enabled = false;
            m_reloadText.enabled = false;
           
        }
        else
        {
            m_reloadIcon.enabled = true;
            m_reloadText.enabled = true;
            m_reloadIcon.fillAmount = (reloadMax - Time.time) / fireRate;
        }
    }

    public void ShowKeyUI(int num, bool show)
    {
        if (show)
        {
            m_keys[num].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }
        else
        {
            m_keys[num].GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
        }
    }
}


