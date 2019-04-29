using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Click_Create : MonoBehaviour
{

    public GameObject prefab_fire;
    public GameObject prefab_ice;
    public GameObject prefab_iron;
    public GameObject prefab_jupiter;
    public GameObject prefab_tree;

    public GameObject prefab_a;
    public GameObject prefab_b;
    public GameObject prefab_c;

    Vector3 createPos;
    Vector3 wp;
    Vector2 touchPos;

    private GameObject target;

    int Num = 0;
    
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        for (int i = 0; i < 30; i++)
        {
            Vector3 Apos = new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-5f, 5f),0);
            Vector3 Bpos = new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-5f, 5f), 0);
            Vector3 Cpos = new Vector3(UnityEngine.Random.Range(-9f, 9f), UnityEngine.Random.Range(-5f, 5f), 0);
            GameObject Inst_a = Instantiate(prefab_a, Apos, Quaternion.identity) as GameObject;
            GameObject Inst_b = Instantiate(prefab_b, Bpos, Quaternion.identity) as GameObject;
            GameObject Inst_c = Instantiate(prefab_c, Cpos, Quaternion.identity) as GameObject;
        }
    }

    void CastRay()
    {
        target = null;

        RaycastHit2D hit = Physics2D.Raycast(createPos, Vector2.zero, 0f);
        
        if (hit.collider != null)
        { //히트

            Debug.Log (hit.collider.name);  

            target = hit.collider.gameObject;
            Destroy(target);

        }
        else
        {
            Create();
        }
    }

    void Create()
    {
        // 생성 
        if (Num == 1)
        {
            GameObject Inst_Fire = Instantiate(prefab_fire, createPos, Quaternion.identity) as GameObject;
        }
        if (Num == 2)
        {
            GameObject Inst_Ice = Instantiate(prefab_ice, createPos, Quaternion.identity) as GameObject;
        }
        if (Num == 3)
        {
            GameObject Inst_Iron = Instantiate(prefab_iron, createPos, Quaternion.identity) as GameObject;
        }
        if (Num == 4)
        {
            GameObject Inst_Jupiter = Instantiate(prefab_jupiter, createPos, Quaternion.identity) as GameObject;
        }
        if (Num == 5)
        {
            GameObject Inst_Tree = Instantiate(prefab_tree, createPos, Quaternion.identity) as GameObject;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        Num = UnityEngine.Random.Range(1, 6);

        createPos = new Vector3(transform.position.x, transform.position.y, -10);

        // 마우스 왼쪽 버튼 클릭
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {

            //스크린 좌표를 월드 좌표로 변환
            wp = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

            touchPos = new Vector2(wp.x, wp.y);
            // 오브젝트 위치 갱신 
            createPos = touchPos;

            CastRay();

            }

        }

    }
