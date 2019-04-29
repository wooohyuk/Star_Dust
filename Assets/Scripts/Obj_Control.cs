using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Control : MonoBehaviour {

    private GameObject target;

   
    void CastRay() 

    {

        target = null;

        Vector2 pos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider != null)
        { 

            Debug.Log (hit.collider.name);  

            target = hit.collider.gameObject; 

        }
    }

    void Update()

    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            CastRay();

            if (target == this.gameObject)
            {  //타겟 오브젝트가 스크립트가 붙은 오브젝트라면

            }

        }

    }
}
