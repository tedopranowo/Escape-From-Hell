using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjectFromEditor : MonoBehaviour {
    [Header("Right click on this script's header and Choose \"Destroy Object\"")]
    [SerializeField]
    bool m_emptyBool = false;
    [ContextMenu("DestroyObject")]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
