using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public static class HapticFeedback  {
    public static SteamVR_Action_Vibration hapticFeedback = SteamVR_Actions.default_Haptic;



    public static void triggerHaptics(float startDelay, float duration, float freq, float amp, GameObject go)
    {
        Interactable i = go.GetComponent<Interactable>();
        if (i == null) { return; }
        Hand h = i.attachedToHand;
        if (h == null) { return; }

        triggerHaptics( startDelay,  duration,  freq,  amp, h.handType);
          
         
        
      

    }
    public static void triggerHaptics(float startDelay, float duration, float freq, float amp, SteamVR_Input_Sources source)
    {
        hapticFeedback.Execute(startDelay, duration, freq, amp, source);
    }

}
