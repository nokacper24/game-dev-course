using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour

{
    NavMeshAgent agent;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        this.agent = this.GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 location) {
        this.agent.SetDestination(location);
    }

    void Flee(Vector3 location) {
        Vector3 fleeVector = location - this.transform.position;
        this.agent.SetDestination(this.transform.position - fleeVector);
    }

    // Update is called once per frame
    void Update()
    {
        // this.Seek(this.target.transform.position);
        this.Flee(this.target.transform.position);
    }
}
