using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Source.Scripts.System.Util
{
    public class LevelManager : MonoBehaviour
    {
#if UNITY_EDITOR
        public GameObject clear;
        public GameObject replaceTarget;
        public GameObject replacePrefab;
        [Header("mat")]
        public GameObject replaceMatTarget;
        public Material replaceMat1;
        public Material replaceMat2;
      
        
        [ExecuteInEditMode]
        public void Clear()
        {
            var componentsInChildren = clear.GetComponentsInChildren<Collider>(true);
            foreach (var child in componentsInChildren)
            {
                DestroyImmediate(child);
            }
        }
        
        [ExecuteInEditMode]
        public void ReplaceChildren()
        {
            var componentsInChildren = replaceTarget.GetComponentsInChildren<Transform>();
            
            foreach (var child in componentsInChildren)
            {
                if (child.Equals(replaceTarget.transform))
                {
                    continue;
                }
                else
                {
                   var go= (GameObject)PrefabUtility.InstantiatePrefab(replacePrefab);
                   go.transform.position = child.position;
                   go.transform.rotation = child.rotation;
                   go.transform.parent=replaceTarget.transform;
                   go.transform.localScale = child.localScale;
                   DestroyImmediate(child.gameObject);
                }
               
            }
        }
        
        
        [ExecuteInEditMode]
        public void ReplaceChildrenMat()
        {
            var componentsInChildren = replaceMatTarget.GetComponentsInChildren<MeshRenderer>();
            
            foreach (var child in componentsInChildren)
            {
                child.sharedMaterials[0] = replaceMat1;
                child.sharedMaterials[1] = replaceMat2;

            }
        }

#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelManager))]
    class LevelManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var o = (LevelManager) target;
            if (o == null) return;

            Undo.RecordObject(o, "LevelManager");
            
            if (GUILayout.Button("Clear go from colliders"))
            {
                o.Clear();
            }
            
            if (GUILayout.Button("Replace children"))
            {
                o.ReplaceChildren();
            }
            
            if (GUILayout.Button("Replace children mat"))
            {
                o.ReplaceChildrenMat();
            }
        }
    }
#endif
}