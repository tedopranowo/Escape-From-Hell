using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDestroyedEvent : MonoBehaviour {
    #region fields=================================================================================
    [SerializeField] private UnityEvent m_onDeathEvent;
    #endregion

    #region MonoBehaviours=========================================================================
    private void OnDestroy()
    {
        m_onDeathEvent.Invoke();
    }

    #endregion
}
