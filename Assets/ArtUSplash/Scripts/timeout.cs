using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class timeout : MonoBehaviour
{
    public float timeoutTime = 90;

    /// <summary>
    /// Allows developers to put any input axes they use in case the game doesn't use mouse and keyboard.
    /// </summary>
    public string[] inputAxes = null;

    private float nextTimeout = -1;

    private Vector3 mouseLastFrame = Vector3.zero;

    private static GameObject instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        nextTimeout = Time.realtimeSinceStartup + timeoutTime;
        mouseLastFrame = Input.mousePosition;
    }

    private void Update()
    {
        Vector3 mouseThisFrame = Input.mousePosition;
        bool didReceiveInput = Input.anyKey || mouseThisFrame != mouseLastFrame;
        if(!didReceiveInput && inputAxes != null)
        {
            for(int i = 0; i < inputAxes.Length; ++i)
            {
                if(Input.GetButton(inputAxes[i]) || Input.GetAxisRaw(inputAxes[i]) != 0)
                {
                    didReceiveInput = true;
                    break;
                }
            }
        }
        
        if(didReceiveInput)
        {
            nextTimeout = Time.realtimeSinceStartup + timeoutTime;
        }
        else if(Time.realtimeSinceStartup > nextTimeout)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        mouseLastFrame = mouseThisFrame;
    }
}
