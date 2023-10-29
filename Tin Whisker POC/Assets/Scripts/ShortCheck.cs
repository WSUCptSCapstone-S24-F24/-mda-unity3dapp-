using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortCheck : MonoBehaviour
{
    

    //private GameObject touchingCylinder = null;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Cylinder"))
    //    {
    //        // If the box isn't already touching a cylinder
    //        if (touchingCylinder == null)
    //        {
    //            touchingCylinder = collision.gameObject;
    //        }
    //        else if (touchingCylinder != collision.gameObject)
    //        {
    //            // This means another cylinder is touching this box before the first one stopped. 
    //            // This might not be the behavior you want. Handle accordingly.
    //        }
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Cylinder") && collision.gameObject == touchingCylinder)
    //    {
    //        CheckIfCylinderBridges(touchingCylinder);
    //        touchingCylinder = null;
    //    }
    //}

    //private void CheckIfCylinderBridges(GameObject cylinder)
    //{
    //    Collider cylinderCollider = cylinder.GetComponent<Collider>();
    //    ContactPoint[] contacts = new ContactPoint[cylinderCollider.contactCount];
    //    cylinderCollider.GetContacts(contacts);

    //    foreach (var contact in contacts)
    //    {
    //        if (contact.otherCollider.CompareTag("Box") && contact.otherCollider.gameObject != this.gameObject)
    //        {
    //            Debug.Log("Cylinder " + cylinder.name + " is bridging Box " + this.gameObject.name + " and Box " + contact.otherCollider.gameObject.name);
               
    //        }
    //    }
    //}
}
