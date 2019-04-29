using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExample : MonoBehaviour {

    private const string voidEventkey = "voidKey";
    private const string intEventKey = "IntKey";
    private void Awake()
    {
        Event.EventManager.Instance.AddEvent(voidEventkey, VoidEvent);
        Event.EventManager.Instance.AddEvent<int>(intEventKey, IntEvent);
    }
	
	// Update is called once per frame
	void Update () {
        Event.EventManager.Instance.InvokeEvent(voidEventkey);
        Event.EventManager.Instance.InvokeEvent(intEventKey, 1);

        // 주의 ADdEvent할때 설정한 파라미터랑 InvokeEvent때 파라미터가 같아야됨
        // 틀리면 에러납니다. (보이드는 보이드만, 인트로 했으면 인트로만 invoke 해야됨)
    }

    private void OnDestroy()
    {
        Event.EventManager.Instance.ClearEvent(voidEventkey);
        Event.EventManager.Instance.ClearEvent(intEventKey);
    }

    private void VoidEvent()
    {
        UnityEngine.Debug.Log("Void Event !");
    }

    private void IntEvent(int value)
    {
        UnityEngine.Debug.Log("Int Event !" + value);
    }
}
