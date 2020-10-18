using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/KeyboardCasting", order = 1)]
class KeyboardCasting : CastingTrigger
{


    private Vector3 grabbedObjectScale;

    Camera cam;

   
    Transform leftHand;

    Transform rightHand;


    private Transform cursor;
    private bool grabbing = false;
    private Transform grabbedObject = null;



    int startFocus = 0;

    [SerializeField]
    private KeyCode grab;
    [SerializeField]
    private KeyCode leftSwitch;
    [SerializeField]
    private KeyCode rightSwitch;
    private KeyCode[] keyCodes = {

         KeyCode.Alpha0,
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,


     };

    public override void init()
    {

            startFocus = 0;
            grabbing = false;
            GameObject player = GameObject.FindWithTag("Player");
            this.cam = player.GetComponentInChildren<Camera>();

            leftHand = GameObject.FindWithTag("LeftHand").transform;
            rightHand = GameObject.FindWithTag("RightHand").transform;
            cursor = GameObject.FindWithTag("Cursor").transform;
     
        
    }
    public override int detectCastTrigger(SpellCasting sp)
    {
        rightHand.rotation = cam.transform.rotation;
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                int numberPressed = i;
                Debug.Log(numberPressed);
                return numberPressed;
            }

        }
        return -1;
    }

    public override int detectCastChange(SpellCasting sp)
    {
       
        HandleGrabs();

        return startFocus;

    }

    private void HandleGrabs()
    {
        
     
        RaycastHit rae = new RaycastHit();

        Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out rae, 4.0f, int.MaxValue, QueryTriggerInteraction.Ignore);
        if (rae.collider && rae.collider.GetComponent<Renderer>())
        {
            cursor.transform.localScale = new Vector3(.2f, .2f, .2f);
            cursor.SetPositionAndRotation(rae.point, Quaternion.identity);
            //rae.collider.GetComponent<Renderer> ().material = selectionMaterial; 
        }
        else
        {
            RaycastHit extendedRay = new RaycastHit();
            Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out extendedRay, 16.0f, int.MaxValue, QueryTriggerInteraction.Ignore);
            if (extendedRay.collider)
            {
                cursor.transform.localScale = new Vector3(.2f, .2f, .2f);
                cursor.SetPositionAndRotation(extendedRay.point, Quaternion.identity);
            }
            else
            {
                cursor.transform.localScale = new Vector3(0, 0, 0);
                //Material m = (Material) Resources.Load("Fire", 
            }
        }

        if (Input.GetKeyDown(grab))
        {
            if (!grabbing)
            {


                if (rae.collider)
                {
              

                    if (rae.transform.CompareTag("Focus") || (rae.transform.gameObject.GetComponent<FocusMaterial>() != null))
                    {
                   
                        grabbing = true;

                        rae.rigidbody.isKinematic = true;
                        rae.rigidbody.useGravity = false;
                        rae.collider.enabled = false;
                        grabbedObject = rae.transform;
                        grabbedObjectScale = grabbedObject.transform.localScale;
                        grabbedObject.SetParent(leftHand.parent, true);
            
                        grabbedObject.localPosition = new Vector3(0, 0, 0);
                        grabbedObject.localScale = Vector3.Normalize(grabbedObjectScale);

                        FocusMaterial f = grabbedObject.GetComponent<FocusMaterial>();
                        if (f != null)
                        {
                            startFocus = (int)f.getElement();
                            // joyconHandLeft.focusElement = f.element; 
                        }
                        else
                        {
                            startFocus = 0;
                        }
                    }
                }
            }
            else if (grabbedObject)
            {
                grabbing = false;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject.GetComponent<Collider>().enabled = true;
                Vector3 scale = grabbedObject.transform.localScale;
                grabbedObject.GetComponent<Rigidbody>().useGravity = true;
                grabbedObject.transform.SetParent(null);
                grabbedObject.localScale = grabbedObjectScale;

                startFocus = 0;
            }
        }

        // Debug.DrawRay(joyconHandLeft.handTransform.position, -joyconHandLeft.handTransform.forward, Color.green); 
        // Debug.DrawRay(joyconHandRight.handTransform.position, -joyconHandRight.handTransform.forward, Color.green); 
    }
}
