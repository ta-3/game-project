using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Targetting/TargetCursorDirection", order = 1)]
public class TargetOnCursor : TargettingTemplate
{
    
	// Use this for initialization
	void Start ()
    {
        
    }
    public override GameObject getObject()
    {
        GameObject cursor = GameObject.Find("Cursor");
        RaycastHit extendedRay = new RaycastHit();
        GameObject rightHand = GameObject.Find("RightHand");
        Physics.Raycast(rightHand.transform.position,rightHand.transform.forward, out extendedRay, 16.0f, int.MaxValue, QueryTriggerInteraction.Ignore);

        if (extendedRay.collider)
        {
            cursor.transform.localScale = new Vector3(.2f, .2f, .2f);
            cursor.transform.SetPositionAndRotation(extendedRay.point, Quaternion.identity);
            return extendedRay.collider.gameObject;
        }



            return null;
        }



}
