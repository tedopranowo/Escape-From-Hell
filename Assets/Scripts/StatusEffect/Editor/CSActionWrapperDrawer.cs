using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CSActionWrapper))]
public class CSActionWrapperDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //TODO: move this to somewhere called less frequently
        CSActionWrapper actionWrapper = EditorHelper.GetObjectFromSerializedProperty(property) as CSActionWrapper;

        if (actionWrapper == null)
            return;

        actionWrapper.Load();

        if (GUI.Button(position, actionWrapper.ToString()))
        {
            CSActionWrapperWindow window = EditorWindow.GetWindow(typeof(CSActionWrapperWindow)) as CSActionWrapperWindow;

            //Set the window object reference
            window.actionWrapper = actionWrapper;
            window.serializedObject = property.serializedObject;
            window.Show();
        }
    }
}

public class CSActionWrapperWindow : EditorWindow
{
    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    private CSActionWrapper m_actionWrapper;
    private SerializedObject m_serializedObject;

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public CSActionWrapper actionWrapper
    {
        get
        {
            return m_actionWrapper;
        }
        set
        {
            m_actionWrapper = value;
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
        m_actionWrapper.type = (CSActionWrapper.Type)EditorGUILayout.EnumPopup("Event: ", m_actionWrapper.type);

        if (m_actionWrapper.type != CSActionWrapper.Type.kNone)
            EditorHelper.DrawCustomDefaultInspector(actionWrapper.csAction);

        if (GUILayout.Button("Save"))
        {
            m_actionWrapper.Save();
            EditorUtility.SetDirty(m_serializedObject.targetObject);
            AssetDatabase.SaveAssets();
        }
    }
}