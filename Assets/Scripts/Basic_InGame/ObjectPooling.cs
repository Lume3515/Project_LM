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
            createFunc: Create, // �⺻(�ʹ�) ����
            actionOnGet: OutPut_Event, // ���� �� �̺�Ʈ
            actionOnRelease: Input_Envent, // ���� �� �̺�Ʈ
           actionOnDestroy: ObjDestroy, // ���� �� �̺�Ʈ
           collectionCheck: false, // �ߺ�?
          defaultCapacity: defaultValue, // �⺻ ��
           maxSize: maxValue // �ִ� ��
        );
    }

    // ����
    public GameObject Create()
    {
        Debug.Log("ȣ���");
        return Instantiate(prefab, poolParent);
       
    }

    // ���� �� �̺�Ʈ
    public void OutPut_Event(GameObject obj)
    {
        obj.SetActive(true);        
    }

    // ���� �� �̺�Ʈ
    public void Input_Envent(GameObject obj)
    {
        obj.SetActive(false);
    }

    // ����
    public void ObjDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    // ������
    public GameObject OutPut()
    {
        Debug.Log("ȣ���2");
        return objectPooling.Get();
    }

    // �ִ� : Release
    public void Input(GameObject obj)
    {
        objectPooling.Release(obj);


    }
}
