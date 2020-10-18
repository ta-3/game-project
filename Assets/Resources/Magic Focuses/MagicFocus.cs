using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This tiny class lets each focus have different mechanics if we want, if we find focuses need more structure to them we can update this.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Focus", order = 1)]
public class MagicFocus : ScriptableObject
{
    [SerializeField]
    private FocusLimiter limiter;

    [SerializeField]
    public Material castColour;

    [SerializeField]
    private List<SpellTemplate> fociiSpells;

    [SerializeField]
    public Color lightColour;


       [SerializeField]
      private List<Shape> shapes;

        [SerializeField]
        GameObject fociiObject;
        public bool castMagic(SpellData sp)
        {
        for (int i = 0; i < shapes.Count; i++)
        {
            if (shapes[i] == sp.Shape)
            {
                fociiSpells[i].cast(sp);
                return true; 
               
            }
        }
           return false;
        }

        public GameObject focusItem()
        {
            return fociiObject;
        }
    


}
