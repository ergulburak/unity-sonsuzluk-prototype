#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Script
{
    [CustomEditor (typeof (GenerateInfiniteLook))]
    public class GenerateInfiniteLookEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GenerateInfiniteLook myScript = (GenerateInfiniteLook) target;
            GUIStyle myFoldoutStyle = new GUIStyle (EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 14;
            if (GUILayout.Button ("Seviye Oluştur")) {
                myScript.Generate();
                EditorUtility.DisplayDialog ("Generator", "Yükleme İşlemi Başarılı", "OK");
            }
        }
    }
}
#endif