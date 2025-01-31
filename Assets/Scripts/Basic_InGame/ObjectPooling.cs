using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] Transform poolParent;

    private ObjectPool<GameObject> objectPooling;
    public ObjectPool<GameObject> ObjectPooling_pool => objectPooling;

    [SerializeField] int defaultValue;

    [SerializeField] int maxValue;       

    private void Start()
    {
        objectPooling = new ObjectPool<GameObject>(
            createFunc: Create, // 기본(초반) 생성
            actionOnGet: OutPut_Event, // 꺼낼 때 이벤트
            actionOnRelease: Input_Envent, // 넣을 때 이벤트
           actionOnDestroy: ObjDestroy, // 삭제 시 이벤트
           collectionCheck: false, // 중복?
          defaultCapacity: defaultValue, // 기본 값
           maxSize: maxValue // 최대 값
        );
    }

    // 생성
    public GameObject Create()
    {
        Debug.Log("호출됨");
        return Instantiate(prefab, poolParent);
       
    }

    // 꺼낼 때 이벤트
    public void OutPut_Event(GameObject obj)
    {
        obj.SetActive(true);        
    }

    // 넣을 떄 이벤트
    public void Input_Envent(GameObject obj)
    {
        obj.SetActive(false);
    }

    // 삭제
    public void ObjDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    // 꺼내다
    public GameObject OutPut()
    {
        Debug.Log("호출됨2");
        return objectPooling.Get();
    }

    // 넣다 : Release
    public void Input(GameObject obj)
    {
        objectPooling.Release(obj);


    }
}
