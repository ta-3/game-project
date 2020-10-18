using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is responsible for the spell casting behaviour. It triggers the update on the keyboard inputs (which can be hotswapped for whatever we want to use).
public class SpellCasting : MonoBehaviour {

    [SerializeField]
    private List<GameObject> lights;
    private List<Light> myLights;

    [SerializeField]
    Material spellFail;
    private int currentFocus = 0;
    private int prevFocus = 0;
    private bool init = true;
    [SerializeField]
    VRCasting cT;

    [SerializeField]
    List<MagicFocus> Focuses;

    GameObject handPosition;
    [SerializeField]
    GameObject focii;

    private bool cancelCasting = false;
    public bool CancelCasting{get{ return cancelCasting; } set{cancelCasting = value; } }

    // Use this for initialization
    void Awake () {
        currentFocus = 0;

        handPosition = GameObject.Find("Handposition");
        cT.init();
       
          //   focii = Instantiate(Focuses[currentFocus-1].focusItem(),handPosition.transform); 
        
	}
	
	// Update is called once per frame
	void Update () {
      
        bool focusChanged = setFocusChange(cT.detectCastChange(this));

        
            SpellData cast = cT.detectCastTrigger(this, focusChanged||CancelCasting);
             CancelCasting = false;
            if (cast != null)
            {
            if (cast.Shape == Shape.None)
            {
                cT.ChangeAppearance(spellFail);
            }
                if (focusChanged)
                {
                    Focuses[prevFocus ].castMagic(cast);
                    cT.ChangeAppearance(Focuses[prevFocus ].castColour);
                    ChangeLighting(Focuses[prevFocus].lightColour);
                }
                else
                {
                    Focuses[currentFocus ].castMagic(cast);
                    cT.ChangeAppearance(Focuses[currentFocus ].castColour);
                    ChangeLighting(Focuses[currentFocus].lightColour);
              }
            }
        
        
	}

    bool setFocusChange(int lr)
    {
        if (lr == currentFocus)
        {
            return false;
        }
        else
        {
            prevFocus = currentFocus;
            currentFocus = lr;


            // Destroy(focii);
            if (lr != 0)
            {
                //  focii = Instantiate(Focuses[currentFocus-1].focusItem(), handPosition.transform, false);
                //  focii.transform.Rotate(180.0f, 0.0f, 0.0f);
                //   focii.SetActive(true);


            }
            return true;
        }
    }

    void ChangeLighting(Color c)
    {
        myLights = new List<Light>();
        myLights.AddRange(GameObject.FindObjectsOfType<Light>());
        

        for (int i = 0; i < myLights.Count; i++)
        {
            //Light l = lights[i].GetComponent<Light>();

            Light l = myLights[i];
            l.gameObject.GetComponent<Renderer>().material.color = c;
            LightController lc = l.gameObject.GetComponent<LightController>();
            if (lc == null)
            {
                lc = l.gameObject.AddComponent<LightController>();
            }
            lc.Init(c, 0.5f, .05f);
            //l.color = c;
        }
    }


}
