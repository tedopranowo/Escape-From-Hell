using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CSEventWrapper))]
public class CSEventWrapperDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //TODO: move this to somewhere called less frequently
        CSEventWrapper eventWrapper = EditorHelper.GetObjectFromSerializedProperty(property) as CSEventWrapper;

        //Don't draw anything if the object doesn't exist
        if (eventWrapper == null)
            return;

        eventWrapper.Load();

        if (GUI.Button(position, eventWrapper.ToString()))
        {
            CSEventWrapperWindow window = EditorWindow.GetWindow(typeof(CSEventWrapperWindow)) as CSEventWrapperWindow;

            //Set the window object reference
            window.eventWrapper = eventWrapper;
            window.serializedObject = property.serializedObject;
            window.Show();
        }
    }
}

public class CSEventWrapperWindow : EditorWindow
{
    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    private CSEventWrapper m_eventWrapper;
    private SerializedObject m_serializedObject;

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public CSEventWrapper eventWrapper
    {
        get
        {
            return m_eventWrapper;
        }
        set
        {
            m_eventWrapper = value;
        }
    }

    public SerializedObject serializedObject
    {
        set
        {
            m_serializedObject = value;
        }
        get
        {
            return m_serializedObject;
        }
    }

    //---------------------------------------------------------------------------------------------
    // Unity function override
    //---------------------------------------------------------------------------------------------
    private void OnGUI()
    {
        m_eventWrapper.type = (CSEventWrapper.Type)EditorGUILayout.EnumPopup("Event: ", m_eventWrapper.type);

        if (m_eventWrapper.type != CSEventWrapper.Type.kNone)
            EditorHelper.DrawCustomDefaultInspector(eventWrapper.csEvent);
        
        if (GUILayout.Button("Save"))
        {
            m_eventWrapper.Save();
            EditorUtility.SetDirty(m_serializedObject.targetObject);
            AssetDatabase.SaveAssets();
        }
    }
}