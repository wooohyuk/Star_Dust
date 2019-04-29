using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Utility.SingletonMonoBehaviour<CameraManager>
{
    public Camera _camera;
    public Transform walls;
    public float prevSize, nowSize, endSize, minSize, maxSize;

    public override void Init()
    {
        prevSize = _camera.orthographicSize;
        nowSize = _camera.orthographicSize;
        endSize = _camera.orthographicSize;

        minSize = _camera.orthographicSize;
        maxSize = 12f;
    }

    Coroutine sizeUp;
    public void SmoothCameraSizeUp(float rate)
    {
        if (rate > 1f)
            rate = 1f;
        endSize = Mathf.Lerp(minSize, maxSize, rate);

        if (sizeUp != null)
            StopCoroutine(sizeUp);
        sizeUp = StartCoroutine(SmoothCameraSizeUp2());
    }

    Vector3 vec = new Vector3(1.01f, 1f, 0f);
    IEnumerator SmoothCameraSizeUp2()
    {
        while (true)
        {
            prevSize = nowSize;
            nowSize = Mathf.Lerp(nowSize, endSize, Time.deltaTime / 0.5f);
            _camera.orthographicSize = nowSize;

            walls.localScale = vec * (nowSize / minSize);

            yield return null;
        }
    }
}
