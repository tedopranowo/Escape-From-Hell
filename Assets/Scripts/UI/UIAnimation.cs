using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    int framesPerSecond = 10;

    [SerializeField]
    private Sprite[] m_sprites;

    Image m_image;

    int m_currentIndex = 0;

    [SerializeField]
    float m_frameTime = .3f;

    float m_currentTime = 0;

    private void Awake()
    {
        // m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_image = gameObject.GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_currentTime > m_frameTime)
        {
            //m_spriteRenderer.sprite = m_sprites[m_currentIndex];
            m_image.sprite = m_sprites[m_currentIndex];
            ++m_currentIndex;

            m_currentTime = 0.0f;
            if (m_currentIndex > m_sprites.Length - 1)
            {
                m_currentIndex = 0;
            }

        }
        else
        {
            m_currentTime += Time.deltaTime;
        }

    }
}
