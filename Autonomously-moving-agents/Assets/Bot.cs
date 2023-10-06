using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour

{
    NavMeshAgent agent;
    public GameObject target;
    Drive ds;
    // Start is called before the first frame update
    void Start()
    {
        this.agent = this.GetComponent<NavMeshAgent>();
        this.ds = this.target.GetComponent<Drive>();
    }

    void Seek(Vector3 location)
    {
        this.agent.SetDestination(location);

    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        this.agent.SetDestination(this.transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = this.target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(this.target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));


        if ((toTarget > 90 && relativeHeading < 20) || this.ds.currentSpeed < 0.01f)
        {
            this.Seek(this.target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (this.agent.speed + this.ds.currentSpeed);
        Vector3 targetPos = this.target.transform.position + this.target.transform.forward * lookAhead;
        this.Seek(targetPos);
    }

    void Evade()
    {
        Vector3 targetDir = this.target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (this.agent.speed + this.ds.currentSpeed);
        Vector3 targetPos = this.target.transform.position + this.target.transform.forward * lookAhead;
        this.Flee(targetPos);
    }

    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        this.Seek(targetWorld);
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            GameObject hidingSpot = World.Instance.GetHidingSpots()[i];
            Vector3 hideDir = hidingSpot.transform.position - this.target.transform.position;
            Vector3 hidePos = hidingSpot.transform.position + hideDir.normalized * 10;
            float distanceToSpot = Vector3.Distance(this.transform.position, hidePos);
            if (distanceToSpot < dist)
            {
                chosenSpot = hidePos;
                dist = distanceToSpot;
            }
        }
        this.Seek(chosenSpot);
    }
    void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenHidingSpot = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            GameObject hidingSpot = World.Instance.GetHidingSpots()[i];
            Vector3 hideDir = hidingSpot.transform.position - this.target.transform.position;
            Vector3 hidePos = hidingSpot.transform.position + hideDir.normalized * 10;
            float distanceToSpot = Vector3.Distance(this.transform.position, hidePos);
            if (distanceToSpot < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenHidingSpot = hidingSpot;
                dist = distanceToSpot;
            }
        }

        Collider hideCol = chosenHidingSpot.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 100.0f;
        hideCol.Raycast(backRay, out info, distance);

        this.Seek(info.point + chosenDir.normalized * 5);
    }

    private bool CanSeeTarget() {
        Vector3 rayToTarget = this.target.transform.position - this.transform.position;
        bool canSee = false;
        if (Physics.Raycast(this.transform.position, rayToTarget, out RaycastHit raycastInfo)) {
            if (raycastInfo.transform.gameObject.CompareTag("cop")) {
                canSee = true;
            }
        }
        return canSee;
    }

    

    // Update is called once per frame
    void Update()
    {
        // this.Seek(this.target.transform.position);
        // this.Flee(this.target.transform.position);
        // this.Pursue();
        // this.Evade();
        // this.Wander();
        // this.Hide();
        if(this.CanSeeTarget()){
        this.CleverHide();
        } else {
            // this.Pursue();
        }
    }
}
