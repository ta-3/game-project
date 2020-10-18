using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneBalancer : MonoBehaviour {
    public List<GameObject> rooms;
    public RoomLoadTrigger puzzleExit;
    public RoomLoadTrigger puzzleEnter;
    int room = 0;
    // Use this for initialization
    bool locked = false;
    public bool debug = false;
    CollisionFade cf;
    private void Start()
    {
        cf = GameObject.FindObjectOfType<CollisionFade>();
        if (cf == null) { Debug.Log("Didnt find cf"); }
        if (!debug)
        {
            foreach (GameObject game in rooms)
            {
                game.SetActive(false);
            }
            rooms[0].SetActive(true);
        }
        else
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].activeInHierarchy) { room = i; Debug.Log("room: " + room); }
            }
        }
    }
    
    public void puzzleExitTrigger()
    {
        Debug.Log("Leaving Room!");
        if (!locked)
        {
            switchRoom(+1);
            locked = true;
        }
        
    }
    public void puzzleEntranceTrigger()
    {
     /*   Debug.Log("Entering Room!");
        if (!locked)
        {
            switchRoom(-1);
            locked = true;
        }*/

    }
    public void resetTrigger()
    {
      //  locked = false;
    }
    private void switchRoom(int triggered)
    {
        
        rooms[room].SetActive(false);
        room += triggered;
        room =Mathf.Abs( room % rooms.Count);
        rooms[room % rooms.Count].SetActive(true);
       

        if (room < rooms.Count)
            Debug.Log("Active rooms swapped. Now active: " + rooms[room].name);
        else
            Debug.Log("Past available rooms!");

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {

            if (cf.blind) { return; }
            if (locked)
            {
                locked = false;
                Debug.Log("Swapped activators!");
                puzzleEnter.isExitTrigger = !puzzleEnter.isExitTrigger;
                puzzleExit.isExitTrigger = !puzzleExit.isExitTrigger;
            }
        }
    }

}
