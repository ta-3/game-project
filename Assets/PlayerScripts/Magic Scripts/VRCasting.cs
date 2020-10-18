using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public enum Shape
{
    None,
    Circle,
    Square,
    Equals,
    Lightningbolt,
    Triangle,
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VrCasting", order = 1)]
class VRCasting : ScriptableObject
{
    GameObject referencePosition;
    public float pointSeperation = 0.2f;
    public SteamVR_Action_Vibration hapticFeedback;

    public float duration = 0f;
    public float amp = 0.2f;
    public float freq = 100f;
    public float timeBetweenPulses = 1.0f;
    float timeFromPulse = 1.0f;
    private float zModifier = -0.03f;
    //private LineRenderer lrPrefab;
    private List<LineRenderer> trails;
    private List<LineRenderer> activeTrails;
    private List<Vector3> trailsRotation;

    Vector3 rotationTotal;
    Vector3 positionTotal;
    private LineRenderer lr;
    private LineRenderer lrPrefab;
    private bool drawing = false;

    private Hand leftH;
    private Transform attachPoint;
    private Hand rightH;

    private float timeToCast = 1.0f;
    [SerializeField]
    private float timeToCastInitial = 1.0f;
    [SerializeField]
    QPointRecogniser recogniser;

    [SerializeField]
    QPointTemplateStore debug;

    private LineRenderer debugLr;
    private void test()
    {
        if (!lr)
        {
            if (!lrPrefab)
            {
                init();
            }
            lr =(LineRenderer) Instantiate<LineRenderer>(lrPrefab);
        }
        if ((debug != null)&&(debug.templatePoints!=null))
        {
            
            lr.positionCount = debug.templatePoints.Length;
            lr.SetPositions(debug.templatePoints);
        }
        
    }
    public int detectCastChange(SpellCasting sp)
    {
       // test();
        GameObject go = leftH.currentAttachedObject;
        if (go == null||(!go.activeInHierarchy)) { return (int)Element.Air; }
        FocusMaterial f = go.GetComponent<FocusMaterial>();
        if (f != null)
        {
            return (int)f.getElement();
            // joyconHandLeft.focusElement = f.element; 
        }
        else
        {
            return 0;
        }
    }

    public SpellData detectCastTrigger(SpellCasting sp, bool focusChanged)
    {
       focusChanged = focusChanged|| rightH.AttachedObjects.Count != 0;
        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand)&&(rightH.currentAttachedObject ==null))
        {
            drawing = true;
            rotationTotal = new Vector3(0, 0, 0);
            rotationTotal += attachPoint.forward;
            positionTotal = new Vector3(0, 0, 0);
            positionTotal += attachPoint.position;

            lr = Instantiate<LineRenderer>(lrPrefab);
            lr.positionCount = 1;
            lr.SetPosition(0, (new Vector3(attachPoint.position.x, attachPoint.position.y, attachPoint.position.z + zModifier)));
            timeFromPulse = timeBetweenPulses;
          //  HapticFeedback.triggerHaptics(0f, duration, freq, amp, SteamVR_Input_Sources.RightHand);
        } 
        else if (drawing && (!focusChanged) && SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand))
        {
            if (Vector3.Distance(attachPoint.position, lr.GetPosition(lr.positionCount - 1)) > pointSeperation)
            {
                lr.positionCount += 1;
                lr.SetPosition(lr.positionCount - 1, attachPoint.position);
                rotationTotal += attachPoint.forward;
                positionTotal += attachPoint.position;
                HapticFeedback.triggerHaptics(0f, duration, freq, amp, SteamVR_Input_Sources.RightHand);
                if (timeFromPulse <= 0)
                {
                    // rightH.TriggerHapticPulse(0);// HapticFeedback.triggerHaptics(0f, duration, freq, amp, SteamVR_Input_Sources.RightHand);
                    timeFromPulse = timeBetweenPulses;
                }
                else
                {
                    timeFromPulse -= Time.deltaTime;
                }
            }

        }
        else if (drawing && (!focusChanged) && SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            timeFromPulse = timeBetweenPulses;
            timeToCast = timeToCastInitial;
            activeTrails.Add(lr);
        }
        else if (drawing && (!SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand)) && timeToCast > 0)
        {
            timeToCast -= Time.deltaTime;
        }
        else if (drawing && (focusChanged || ((!SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand)) && timeToCast <= 0)))
        {
            if (!drawing) { return null; }
            List<Vector3> points = new List<Vector3>();
            if (focusChanged) { activeTrails.Add(lr); }
            foreach (LineRenderer lineRenderer in activeTrails)
            {
                if (lineRenderer == null) continue;
               Vector3[] lineRendererPoints = new Vector3[lineRenderer.positionCount];//ERROR CAUSING LINES
                lineRenderer.GetPositions(lineRendererPoints);
                points.AddRange(lineRendererPoints);
                Destroy(lineRenderer.gameObject, 4);
            }
           
            drawing = false;

            rotationTotal = rotationTotal / points.Count;
            Plane p = FitToPlane3(points, points.Count);
         

            positionTotal = positionTotal / points.Count;
           

            referencePosition.transform.SetPositionAndRotation(positionTotal, Quaternion.LookRotation(p.normal.normalized));

            // if (!(p.GetSide(rightH.transform.position + Vector3.forward * zModifier))){
            //     p.Flip();
            //}
            //Debug.DrawRay(positionTotal, 100 * p.normal.normalized, Color.red, 100.0f);
            // Debug.DrawRay(positionTotal, 100 * rotationTotal.normalized, Color.black, 100.0f);
            SpellData sd = new SpellData(p.normal.normalized, 1, positionTotal, Shape.None);
           List <Vector3> localPoints = convertToLocal(points, referencePosition, p);
           recogniser.recogniseGesture(localPoints.ToArray(),sd);
          
            return sd;
        }
       
        /*
        if (SteamVR_Actions.default_SpellActive.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Vector2 Position = SteamVR_Actions.default_TouchpadClick.GetAxis(SteamVR_Input_Sources.Any);
            if (Position.y > 0)
            {
                if (Position.x < 0) { return 0; }
                else { return 1; }
            }
            {
                if (Position.x < 0) { return 3; }
                else { return 2; }
            }
        }
        */
        return null;
    }  

    public void ChangeAppearance(Material trailMaterial)
    {  
        for (int i = 0; i < activeTrails.Count; i++)
        {
            //activeTrails[i].startColor = Color.cyan;
            //activeTrails[i].endColor = Color.cyan;
            activeTrails[i].material = trailMaterial;
        }
        activeTrails.Clear();

    }
    
    public int FindMostDistantPoint(List<Vector3> l, int init_i)
    {
        Vector3 startingPoint = l[init_i];
        float dist = float.MinValue;
        int index = 0;

        for (int i = 0; i < l.Count; i++)
        {
            if (i == init_i) { continue; }

            float myDist = Vector3.Distance(startingPoint, l[i]);
            if (dist <= myDist)
            {
                index = i;
                dist = myDist;
            }

        }
        return index;
    }

    public int FindMostDistantPoint(List<Vector3> l, int a_i, int b_i)
    {
        Vector3 a = l[a_i];
        Vector3 b = l[b_i];
        float dist = float.MinValue;
        int index = 0;

        for (int i = 0; i < l.Count; i++)
        {
            if (i == a_i || i == b_i) { continue; }

            float myDist = Vector3.Distance(a, l[i]) + Vector3.Distance(b, l[i]);
            if (dist <= myDist)
            {
                index = i;
                dist = myDist;
            }

        }
        return index;
    }

    public Plane FitToPlane(List<Vector3> l, int iters)
    {
        //return normal
        //float error = float.PositiveInfinity;
    

        Vector3Int store = Vector3Int.zero;

        int init_i = Random.Range(0, l.Count);
        Vector3 startingPoint = l[init_i];

        int a_i = FindMostDistantPoint(l,init_i);
        int b_i = FindMostDistantPoint(l,a_i);
        int c_i = FindMostDistantPoint(l,a_i,b_i);

        Plane p = new Plane (l[a_i], l[b_i], l[c_i]);

        Debug.DrawLine(l[a_i], l[b_i], Color.green,100);
        Debug.DrawLine(l[a_i], l[c_i], Color.green, 100);
        Debug.DrawLine(l[b_i], l[c_i], Color.green, 100);

        Debug.DrawRay((l[a_i]+l[b_i]+l[c_i])/3, p.normal.normalized*100, Color.magenta, 100);
        return p;

    }


    public Plane FitToPlane3(List<Vector3> l, int iters)
    {
        //return normal
        //float error = float.PositiveInfinity;
       

        Plane bestPlane = new Plane();

        Vector3Int store = Vector3Int.zero;
        float error = float.PositiveInfinity;
        for (int i = 0; i < iters; i++)
        {

            int init_i = Random.Range(0, l.Count);
            Vector3 startingPoint = l[init_i];

            int a_i = FindMostDistantPoint(l, init_i);
            int b_i = FindMostDistantPoint(l, a_i);
            int c_i = FindMostDistantPoint(l, a_i, b_i);

            Plane p = new Plane(l[a_i], l[b_i], l[c_i]);

            float e = 0;
            for (int j = 0; j < l.Count - 1; j++)
            {
                if (j == a_i || j == b_i || j == c_i)
                    continue;

                e += p.GetDistanceToPoint(l[j]);
            }
            if (e < error)
            {
                error = e;
                bestPlane = p;
                store = new Vector3Int(a_i, b_i, c_i);
            }
        }

        if (!CheckNormalFacing(bestPlane,positionTotal,rightH.transform.position))
        {
            bestPlane.Flip();
        }
      

        Debug.DrawRay((l[store.x] + l[store.y] + l[store.z]) / 3, bestPlane.normal.normalized * 100, Color.magenta, 100);
        

        Debug.DrawLine(l[store.x], l[store.y], Color.green, 100);
        Debug.DrawLine(l[store.x], l[store.z], Color.green, 100);
        Debug.DrawLine(l[store.y], l[store.z], Color.green, 100);

        
        return bestPlane;

    }

    public bool CheckNormalFacing(Plane plane, Vector3 centrepoint, Vector3 point)
    {
        float normalDistance = (Vector3.Distance(plane.normal.normalized, rotationTotal.normalized));
        float inverseNormalDistance = (Vector3.Distance(-1*plane.normal.normalized, rotationTotal.normalized));
        return (normalDistance < inverseNormalDistance);

    }


    public Plane FitToPlaneRansac(List<Vector3> l, int iters)
    {
        //return normal
        float error = float.PositiveInfinity;
        Plane bestPlane = new Plane();

        Vector3Int store = Vector3Int.zero;

        for (int i = 0; i < iters; i++)
        {
            //Find 3 distinct points
            int a_i = 0;
            int b_i = 0;
            int c_i = 0;

            while (a_i == b_i || a_i == c_i || b_i == c_i)
            {
                a_i = Random.Range(0, l.Count);
                b_i = Random.Range(0, l.Count);
                c_i = Random.Range(0, l.Count);
            }

            Vector3 a = l[a_i];
            Vector3 b = l[b_i];
            Vector3 c = l[c_i];
            //fin

            //Find error (sum of distances to plane)
            Plane p = new Plane(a, b, c);
            float e = 0;
            for (int j = 0; j < l.Count-1; j++)
            {
                if (j == a_i || j == b_i || j == c_i)
                    continue;

                e += p.GetDistanceToPoint(l[j]);
            }

            //If error is best so far, preserve plane
            if (e < error)
            {
                error = e;
                bestPlane = p;
                store = new Vector3Int(a_i, b_i, c_i);
            }
            
        }
        Debug.DrawLine(l[store.x]*5, l[store.y] * 5, Color.green,100);
        Debug.DrawLine(l[store.x] * 5, l[store.z] * 5, Color.green, 100);
        Debug.DrawLine(l[store.z] * 5, l[store.y]*5, Color.green, 100);

        return bestPlane;

    } 
    


    private List<Vector3> convertToLocal( List<Vector3> coordinates_2D, GameObject referencePoint, Plane p)
    {
        //List<Vector3> coordinates_2D = new List<Vector3>();
        for (int i = 0; i < coordinates_2D.Count; i++)
        {
            coordinates_2D[i] = referencePoint.transform.InverseTransformPoint(p.ClosestPointOnPlane(coordinates_2D[i]));       
        }

    

        return coordinates_2D;
    }
    

    public void init()
    {
       referencePosition = new GameObject("Plane Centre");

        drawing = false;
        leftH = ((GameObject)GameObject.FindGameObjectWithTag("LeftHand")).GetComponent<Hand>();
        rightH = leftH.otherHand;
        attachPoint = rightH.transform;

        lrPrefab = Resources.Load<LineRenderer>("RuntimeLoad/MagicTrailRenderer");
        trails = new List<LineRenderer>();
        activeTrails = new List<LineRenderer>();
        recogniser.init();
        timeToCast = timeToCastInitial;
        timeFromPulse = timeBetweenPulses;
    }

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
