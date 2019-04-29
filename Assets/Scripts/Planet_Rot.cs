using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Rot : MonoBehaviour
{
    public Vector2 range;
    float rotate_timer;
    public float rotime = 0.1f;

    private float speed = 0f;

    private void Awake()
    {
        int num1 = UnityEngine.Random.Range(0, 2);
        speed = Random.Range(range.x, range.y);
        if (num1 == 0)
        {
            speed = speed * 1;
        }
        if (num1 == 1)
        {
            speed = speed * -1;
        }
    }

	private void Update ()
	{
        rotate_timer += Time.deltaTime;
        if (rotate_timer > rotime)
        {
            transform.Rotate(0, 0, speed);
                rotate_timer = 0;
        }
    }
}
