#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class StateFactory
{
    //will add a menu item to create a new CharacterState asset file out of selection
    [MenuItem("Assets/Create/AI/Character State")]
    static void CreateNewState()
    {
        CreateAsset<CharacterState>("Selected asset is not a State Object");
    }

    //will add a menu item to create a new CharacterStateTransition asset file out of selection
    [MenuItem("Assets/Create/AI/State Transition")]
    static void CreateNewTransition()
    {
        CreateAsset<CharacterStateTransition>("Selected asset is not a Transition Object");
    }

    //creates a asset file dependant on the file selection and specified class type.
    private static void CreateAsset<NewObjectType>(string assertionLog)
    {
        var newObj = ScriptableObject.CreateInstance(Selection.activeObject.name);

        //make sure the created object is of type that we want
        if (!(newObj is NewObjectType))
        {
            Debug.LogAssertion(assertionLog);
            //bail out.
            return;
        }

        //get the path of the selected file.
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        //cuts the ".cs" from the file path
        path = path.Substring(0, path.Length - 3);

        //creates the asset of the specified type, in the folder that a file has been selected.
        AssetDatabase.CreateAsset(newObj, (path + ".asset"));
        //saves all assets
        AssetDatabase.SaveAssets();
        //selects the newly created file.
        Selection.activeObject = newObj;
    }
}
#endif