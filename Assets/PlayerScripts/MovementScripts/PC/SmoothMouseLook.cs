using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SmoothMouseLook : MonoBehaviour
{

    float sensitivityX = 50F;
    float sensitivityY = 50F;

    float minimumX = -360F;
    float maximumX = 360F;

    float minimumY = -60F;
    float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter = 2;

    Quaternion originalRotation;
    Camera c;
    public void Init(float sensitivityX, float sensitivityY, float minX, float maxX, float minY, float maxY)
    {
        c = GetComponentInChildren(typeof(Camera)) as Camera;
        this.sensitivityX = sensitivityX;
        this.sensitivityY = sensitivityY;
        this.minimumX = minX;
        this.maximumX = maxX;
        this.minimumY = minY;
        this.maximumY = maxY;
    }

    void Update()
    {
        rotAverageY = 0f;
        rotAverageX = 0f;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        rotationX += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

         rotArrayY.Add(rotationY);
         rotArrayX.Add(rotationX);

         if (rotArrayY.Count >= frameCounter)
         {
             rotArrayY.RemoveAt(0);
         }
         if (rotArrayX.Count >= frameCounter)
         {
             rotArrayX.RemoveAt(0);
         }

         for (int j = 0; j < rotArrayY.Count; j++)
         {
             rotAverageY += rotArrayY[j];
         }
         for (int i = 0; i < rotArrayX.Count; i++)
         {
             rotAverageX += rotArrayX[i];
         }

         rotAverageY /= rotArrayY.Count;
         rotAverageX /= rotArrayX.Count;
         
         rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
         rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

        Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
         Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);




        


        c.transform.localRotation = originalRotation * yQuaternion;
        transform.localRotation = originalRotation * xQuaternion;




}

void Start()
{
   Cursor.lockState = CursorLockMode.Locked;
   Rigidbody rb = GetComponent<Rigidbody>();
   if (rb)
       rb.freezeRotation = true;
   originalRotation = transform.localRotation;
}

public static float ClampAngle(float angle, float min, float max)
{
   angle = angle % 360;
   if ((angle >= -360F) && (angle <= 360F))
   {
       if (angle < -360F)
       {
           angle += 360F;
       }
       if (angle > 360F)
       {
           angle -= 360F;
       }
   }
   return Mathf.Clamp(angle, min, max);
}
}
 