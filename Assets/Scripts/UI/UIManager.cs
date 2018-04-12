using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private HealthUI m_healthUI;
    [SerializeField] private StatusEffectUI m_statusEffectUI;

	// Use this for initialization
	void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        //[TODO]: Do not update health every frame
        UpdateHealth();
	}

    public void UpdateHealth()
    {
        m_healthUI.Update();
        m_statusEffectUI.Update();
    }
}

[System.Serializable]
class HealthUI
{
    [SerializeField] private Image m_image;
    [SerializeField] private Text m_curHealthText;
    [SerializeField] private Text m_maxHealthText;

    public void Update()
    {
        //Get the player character
        Character playerCharacter = PlayerController.instance? PlayerController.instance.character : null;

        //Get the character health
        int curHealth = playerCharacter ? playerCharacter.health : 0;
        int maxHealth = playerCharacter ? playerCharacter.maxHealth : 0;

        //Update the health UI
        m_curHealthText.text = curHealth.ToString();
        m_maxHealthText.text = maxHealth.ToString();
        m_image.fillAmount = (float)curHealth / maxHealth;
    }
}

[System.Serializable]
class StatusEffectUI
{
    [SerializeField] private Image[] m_images;

    public void Update()
    {
        Dictionary<string, StatusEffect> statusEffects = PlayerController.instance.character.statusEffects;
        int i = 0;

        //Clear all the images
        foreach(Image image in m_images)
        {
            image.color = Color.clear;
        }

        //Update all status effect images
        foreach(KeyValuePair<string, StatusEffect> kvp in statusEffects)
        {
            m_images[i].sprite = kvp.Value.icon;
            m_images[i].color = Color.white;
            m_images[i].GetComponent<HoverItemInfo>().hoveredData = kvp.Value;
            ++i;
        }
    }
}