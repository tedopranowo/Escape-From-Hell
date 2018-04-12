using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    [SerializeField]
    Transform m_bulletSpawn = null;

    [SerializeField]
    GameObject m_projectile = null;

    [SerializeField]
    AudioClip m_soundEffect = null;

    AudioSource m_audio = null;

    [SerializeField]
    [Range(1, 5)]
    int m_split = 1;

    [SerializeField]
    [Range(1, 120)]
    int m_totalAngle = 120;


    private float m_damage = 0.0f;

    [SerializeField]
    StatusEffect m_statusEffect = null;

    [SerializeField]
    private float m_fireRate = 0.2f;

    [SerializeField]
    [Range(0, 35)]
    private float m_inaccuracy = 0.0f;

    private ObjectPool pool;

    private float m_lastShot = 0;



    public float lastShot
    {
        get { return m_lastShot; }
    }

    public float fireRate
    {
        get { return m_fireRate; }
    }

    public float damage
    {
        set { m_damage = value; }
    }
    // Use this for initialization
    void Awake()
    {
        pool = GetComponent<ObjectPool>();
        m_audio = GetComponent<AudioSource>();
    }

    public void Excecute()
    {
        if (Time.time < m_lastShot + m_fireRate)
        {
            return;
        }
        else
        {
            m_lastShot = Time.time;
        }

        float rotationAngle = 0;
        if (m_split > 1)
        {
            rotationAngle = m_totalAngle / (m_split - 1);
        }


        for (int i = 0; i < m_split; ++i)
        {
            GameObject obj = Instantiate(m_projectile) as GameObject;

            if (obj == null)
                return;


            obj.SetActive(true);
            obj.transform.position = m_bulletSpawn.position;
            if (m_split == 1)
            {
                obj.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, -90 + Random.Range(-m_inaccuracy, m_inaccuracy));
            }
            else
            {
                obj.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, (-m_totalAngle / 2 + rotationAngle * i - 90) + Random.Range(-m_inaccuracy, m_inaccuracy));
            }

            obj.GetComponent<ProjectileScript>().direction = obj.transform.up;
            obj.GetComponent<ProjectileScript>().damage = m_damage;
            obj.GetComponent<ProjectileScript>().statusEffect = m_statusEffect;

            //Play sound effect
            m_audio.PlayOneShot(m_soundEffect, 0.8f);
        }
    }
}
