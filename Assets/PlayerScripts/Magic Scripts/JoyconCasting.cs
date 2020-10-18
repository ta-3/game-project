using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates an asset we can easily use/ manipulate without having to attatch to an object.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/JoyconCasting", order = 1)]
class JoyconCasting : CastingTrigger
{
    [SerializeField]
    private KeyCode leftSwitch;
    [SerializeField]
    private KeyCode rightSwitch;

    private JoyconHand leftHand;
    private JoyconHand rightHand;

    public override int detectCastTrigger(SpellCasting sp)
    {
        CheckForJoyons();
        if (rightHand.GetDpadUp())
        {
            return 1;
        }
        /*
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                int numberPressed = i;
                Debug.Log(numberPressed);
                return numberPressed;
            }
            
        }
        */
        return -1;

    }

    public override int detectCastChange(SpellCasting sp)
    {
        CheckForJoyons();

        return ((int)leftHand.focusElement);
        
           
               
        

        /*if (rightHand.GetDpadLeft()) { return -1; }
        else if (rightHand.GetDpadRight()) { return 1; }
        else
            return 0;*/
    }

    private void CheckForJoyons()
    {
        leftHand = GameObject.Find("LeftHand").GetComponent<JoyconHand>();
        rightHand = GameObject.Find("RightHand").GetComponent<JoyconHand>();
    }
    public override void init()
    {
    }
}

