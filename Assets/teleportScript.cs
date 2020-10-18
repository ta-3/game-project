using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class teleportScript : MonoBehaviour {

    [SerializeField]
    float minForce = 5f;
    [SerializeField]
    float maxForce =100f;
    [SerializeField]
    float maxChargeTime = 5f;

    float force = 0.0f;

    [SerializeField]
    float timeBetweenPulses = 0.1f;
    float timeFromTrigger = 0.0f;
    [SerializeField]
    SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");

    [SerializeField]
    GameObject playerGo;

    [SerializeField]
    LineRenderer lr;
    
    bool charging = false;

    public GameObject prefab;
    public GameObject prefabInstance;

    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        force = minForce;
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (teleportAction.GetState(SteamVR_Input_Sources.RightHand)&&prefabInstance == null)
        {

            charging = true;
            force += ((maxForce - minForce) / maxChargeTime) * Time.deltaTime;
            force = Mathf.Min(force, maxForce);
            timeFromTrigger += Time.deltaTime * force;
            if (timeFromTrigger * force > timeBetweenPulses)
            {
                HapticFeedback.triggerHaptics(0f, Time.deltaTime, 50f, ((force) / maxForce), SteamVR_Input_Sources.RightHand);
                timeBetweenPulses = 0;
            }
            prefab.transform.position = gameObject.transform.position;
            Vector3[] result = GetTrajectory(prefab.GetComponent<Rigidbody>(), transform.forward * force,0.25f,1.0f);
            lr.enabled = true;
            lr.positionCount = result.Length;
            lr.SetPositions(result);
            lr.useWorldSpace = true;
            
            //   PlotTrajectory(this.transform.position, 4.47f * this.transform.forward, 0.0f, 10.0f);

        }
        else
        {
            if (charging)
            {
                lr.enabled = false;
                prefabInstance= GameObject.Instantiate(prefab);

                prefabInstance.GetComponent<Rigidbody>().AddForce(transform.forward * force,ForceMode.Impulse);
                prefabInstance.transform.position = gameObject.transform.position;

                prefabInstance.GetComponent<teleportBallScript>().go = playerGo;
                charging = false;
                force = minForce;
            //    HapticFeedback.triggerHaptics(0f, 0.1f, 100f, 1, SteamVR_Input_Sources.RightHand);
                audioSource.Play();
              
                timeBetweenPulses = 0;

            }
            
        }
	}
    public static Vector3[] GetTrajectory(Rigidbody rb, Vector3 force, float simulationAccuracy, float trajectoryDuration = 1.0f)
    {

        int points = Mathf.Max(Mathf.RoundToInt(trajectoryDuration / (Time.fixedDeltaTime * simulationAccuracy)));

        Vector3[] positions = new Vector3[points];

 
        positions[0] = rb.transform.position;
        Vector3 gravity = Physics.gravity * Time.fixedDeltaTime;
        float drag = (1 - (rb.drag * Time.fixedDeltaTime));

        Vector3 updatedVelocity = force / rb.mass;
        updatedVelocity = updatedVelocity + (Physics.gravity * Time.fixedDeltaTime);
        updatedVelocity *= drag;

        positions[1] = positions[0] + (gravity);

        for (var i = 1; i < positions.Length - 1; i++)
        {

            updatedVelocity = updatedVelocity + (gravity);
            updatedVelocity *= drag;




            positions[i + 1] = positions[i] + (updatedVelocity * Time.fixedDeltaTime);

        }
        return positions;
    }
}
