using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic.Entity;

public class SpaceArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Wall"))
            return;

        Entity entity = other.GetComponent<Entity>();
        if (entity != null)
        {
            entity.WallOutReset(transform);
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("who wall out");
    //    if (other.name.Contains("Stardust"))
    //    {
    //        Debug.Log("who wall out");
    //        other.SendMessage("WallOutReset");
    //    }
    //}
}
