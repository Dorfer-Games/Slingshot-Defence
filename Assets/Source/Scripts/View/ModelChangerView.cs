using System;
using UnityEngine;

namespace Source.Scripts.View
{
    public class ModelChangerView : MonoBehaviour
    {
       [SerializeField] private GameObject ModelRoot;

        private ModelView[] models;
        private void Awake()
        {
            models = ModelRoot.GetComponentsInChildren<ModelView>(true);
        }

        public void SetModel(int id)
        {
            for (int i = 0; i < models.Length; i++)
            {
                models[i].gameObject.SetActive(i==id);
            }
        }
    }
}