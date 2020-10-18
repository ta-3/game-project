using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QPointRecogniser", order = 1)]
public class QPointRecogniser : ScriptableObject {
    [SerializeField]
    List<QPointTemplateStore> templateStores;

    [SerializeField]
    QPointTemplateStore storeToUpdate;

    Gesture[] gestureTemplates;

    public void init()
    {
        gestureTemplates = new Gesture[templateStores.Count];
        for (int i = 0; i < templateStores.Count; i++)
        {
            Gesture g = templateStores[i].returnGestures();
            if (g!= null) { gestureTemplates[i] = g; }
        }
    }
    public SpellData recogniseGesture(Vector3[] points, SpellData sd)
    {
        
        if (storeToUpdate!= null)
        {
            storeToUpdate.addTemplate(points);
        }
        else
        {

          
            Point[] array = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                array[i] = new Point(points[i].x, points[i].y, 0);
            }
            Gesture newGesture = new Gesture(array, "");
            float distance = 0;
            string s = QPointCloudRecognizer.Classify(newGesture, gestureTemplates,ref distance);
            Debug.Log(s +":" +distance);
            Shape shape = Shape.None;
            if (distance < 8.0f)
            {
                switch (s)
                {
                    case "Square":
                        shape = Shape.Square;
                        break;
                    case "Triangle":
                        shape = Shape.Triangle;
                        break;
                    case "Equals":
                        shape = Shape.Equals;
                        break;
                    case "Lightningbolt":
                        shape = Shape.Lightningbolt;
                        break;
                    case "Circle":
                        shape = Shape.Circle;
                        break;
                    default:
                        shape = Shape.None;
                        break;
                }
            }
            sd.Shape = shape;
            sd.Scale = newGesture.scale;
            Debug.Log("Scale: " + sd.Scale);
        }
        
        
        return sd;
    }
    

    

}
