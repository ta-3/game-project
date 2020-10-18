using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionScaling : MonoBehaviour {
    
    float time = 1.0f;
    AudioSource audioS;
    public void setScale(float scale)
    {
        
        time =Mathf.Max(scale*2,1.6f);
        this.transform.localScale = this.transform.localScale * scale;
       ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
        ParticleSystem.Burst bs = ps.emission.GetBurst(0);
        ParticleSystem.MinMaxCurve minMaxCurve = bs.count;
        minMaxCurve.constant *= (1 + scale);
        bs.count = minMaxCurve;
        ps.emission.SetBurst(0, bs);
        this.transform.localScale = this.transform.localScale * scale;
        audioS = AudioManager.reference.playSoundFromObject("FireballImpact", this.gameObject, false);
        
            
    }
    public void Update()
    {
        time -= Time.deltaTime;
        if (time < 0) { StartCoroutine(AudioManager.FadeOut(audioS, 0.5f)); }//audioMaaudio.Stop(); }
    }
}
