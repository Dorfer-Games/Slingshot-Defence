using AYellowpaper.SerializedCollections;
using Source.Scripts.System.Util;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace Source.Scripts.View.Stage
{
    public class Tile : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] private TileType tileType;
        [SerializeField] private bool isRoad=true;
        [SerializeField] private SerializedDictionary<TileType, GameObject> roadModels;
        [SerializeField] private SerializedDictionary<TileType, GameObject> waterModels;
       
        public void SetTileModel()
        {
            if (isRoad)
            {
                foreach (var kv in roadModels)
                {
                    kv.Value.gameObject.SetActive(kv.Key==tileType);
                }
                foreach (var kv in waterModels)
                {
                    kv.Value.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (var kv in waterModels)
                {
                    kv.Value.gameObject.SetActive(kv.Key==tileType);
                }
                foreach (var kv in roadModels)
                {
                    kv.Value.gameObject.SetActive(false);
                }
            }

        }

#endif
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Tile))]
    class TileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var o = (Tile) target;
            if (o == null) return;

            Undo.RecordObject(o, "Tile");

            if (GUILayout.Button("Update tile"))
            {
                o.SetTileModel();
            }
            
        }
    }
#endif
    
}