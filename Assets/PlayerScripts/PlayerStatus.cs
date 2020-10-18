using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a scriptable object which we turn into an asset. This lets it be permanent over loads with minimal effort and other fun stuff. I need to transfer the movement variables to here.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerStatus", order = 1)]
public class PlayerStatus : ScriptableObject {
    
    public int maximumHP;

    public int currentHp;

    
}
