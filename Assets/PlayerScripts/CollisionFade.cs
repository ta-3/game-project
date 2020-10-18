using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CollisionFade : MonoBehaviour {

    [SerializeField]
    float distanceToStartFace = 0.2f;

    [SerializeField]
    float distanceToEndFade = 0.1f;
    Color colour = Color.clear;
  
    // Use this for initialization
    public bool blind = false;

    Camera cm;
    public GameObject safeZone;
    int fullMask;
    int partialMask;
    void Start () {
        cm = gameObject.GetComponent<Camera>();
        fullMask = cm.cullingMask;
        partialMask = 1<<LayerMask.NameToLayer("PositionMarker");
        Debug.Log("mask" + partialMask);
	}

    public bool checkTeleport(Vector3 Pos, GameObject go)
    {
        float min = float.MaxValue;
        Vector3 camPos = Pos;
       // camPos.y = this.gameObject.transform.position.y;
        
       Collider[] c =  Physics.OverlapSphere(camPos, distanceToStartFace);
      
        foreach (Collider collider in c)
        {
            if ((!collider.isTrigger)&&(collider.gameObject.tag != "Player") && !collider.gameObject.Equals(go))
            {

                float dist = (collider.ClosestPoint(camPos) - camPos).magnitude;
                if (dist < min) { min = dist; }
            }

        }
        if (min <= distanceToStartFace)
        {
            return false;
        }
        return true;
    }
    void Update () {
        
        float min = float.MaxValue;
        Vector3 camPos = this.gameObject.transform.position;
       Collider[] c =  Physics.OverlapSphere(camPos, distanceToStartFace);
        
        foreach (Collider collider in c)
        {
            if ((!collider.isTrigger)&&(collider.gameObject.tag != "Player"))
            {
                float dist = (collider.ClosestPoint(camPos) - camPos).magnitude;
                if (dist < min) { min = dist; }
            }

        }
        if (blind)
        {
            min = Vector3.Distance(safeZone.transform.position,camPos);
            if (min < distanceToStartFace)
            {
                this.blind = false;
                colour.a = 0;
                cm.cullingMask = fullMask;
                
            }

       }
        else
        {
            if (min < distanceToStartFace)
            {
                if (min < distanceToEndFade)
                {
                    blind = true;

                    colour.a = 0;
                    cm.cullingMask = partialMask;
                    

                }
                else
                {

                    colour.a = 1.0f - ((min - distanceToEndFade) / (distanceToStartFace - distanceToEndFade));
                }
            }
            else
            {
                colour.a = 0;
                safeZone.transform.position = camPos;


            }
            SteamVR_Fade.View(colour, 0);
        }
    }
   

}

