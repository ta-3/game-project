using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGuide : MonoBehaviour {
    [SerializeField]
    QPointTemplateStore drawTemplate;
    [SerializeField]
    LineRenderer lr;
    [SerializeField]
    Transform Position;

   
    // Use this for initialization
    void Start () {
		if (drawTemplate==null||lr ==null || Position==null)
        {
            Destroy(this);return;
        }
        else
        {
            Vector3[]points = drawTemplate.templatePoints;
         
            foreach (Vector3 p in points)
            {
                lr.positionCount++;
      
                lr.SetPosition(lr.positionCount - 1, p);
                
            }

        }
	}
	

}
