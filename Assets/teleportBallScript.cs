using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class teleportBallScript : MonoBehaviour {

    public GameObject go;

    private void Start()
    {
        Destroy(this.gameObject,10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.layer == 13)
        {
            go.GetComponent<SpellCasting>().CancelCasting = true;
            CollisionFade cf = GameObject.FindObjectOfType<CollisionFade>();
            Camera c = go.GetComponentInChildren<Camera>();
            Vector3 CamPos = c.gameObject.transform.position;
            Vector3 goPos = this.gameObject.transform.position;
            goPos.y += CamPos.y;
            if (cf.checkTeleport(goPos,this.gameObject))
            {


               if (go == null)
                {

                    go = GameObject.Find("Player");
                  //  Debug.Log("Error!");
                }

                //  goPos.x += (c.transform.forward.x * 0.075f); goPos.z += (c.transform.forward.z * 0.075f); goPos.y = this.gameObject.transform.position.y;
                goPos = go.transform.position-CamPos;goPos.y = 0; 
                go.transform.position = this.gameObject.transform.position+goPos;
                Hand left = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<Hand>();
                Hand right = GameObject.FindGameObjectWithTag("RightHand").GetComponent<Hand>();
                if (left.currentAttachedObject != null) { left.currentAttachedObject.transform.position = left.gameObject.transform.position; }
                 if(right.currentAttachedObject != null) { right.currentAttachedObject.transform.position = right.gameObject.transform.position; }
                Destroy(this.gameObject);

            }
            

        }
    }
}

