using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.Scripts.View.Stage
{
    public class NavMeshAgentView : MonoBehaviour
    {
        private void Start()
        {
            var agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(Vector3.zero);
        }
    }
}