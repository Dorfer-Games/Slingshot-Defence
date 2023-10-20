using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Source.Scripts.System.Util
{
    public class PlayerPrefManager : MonoBehaviour
    {
#if UNITY_EDITOR
        
        [ExecuteInEditMode]
        public void ClearPlayerPref()
        {
            PlayerPrefs.DeleteAll();
        }

#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerPrefManager))]
    class PlayerPrefManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var o = (PlayerPrefManager) target;
            if (o == null) return;

            Undo.RecordObject(o, "PlayerPrefManager");

            if (GUILayout.Button("Clear Player Prefs"))
            {
                o.ClearPlayerPref();
            }
            
        }
    }
#endif
}