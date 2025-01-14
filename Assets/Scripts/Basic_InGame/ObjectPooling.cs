using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] Transform poolParent;

    private ObjectPool<GameObject> objectPooling;

    [SerializeField] int defaultValue;

    [SerializeField] int maxValue;       

    private void Start()
    {
        objectPooling = new ObjectPool<GameObject>(
            createFunc: Create, // �⺻(�ʹ�) ����
            actionOnGet: OutPut_Event, // ���� �� �̺�Ʈ
            actionOnRelease: Input_Envent, // ���� �� �̺�Ʈ
           actionOnDestroy: Destroy, // ���� �� �̺�Ʈ
           collectionCheck: false, // �ߺ�?
          defaultCapacity: defaultValue, // �⺻ ��
           maxSize: maxValue // �ִ� ��
        );
    }

    // ����
    public GameObject Create()
    {
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
    public void Destroy(GameObject obj)
    {
        Destroy(obj.gameObject);
    }

    // ������
    public GameObject OutPut()
    {       
        return objectPooling.Get();
    }

    // �ִ� : Release
    public void Input(GameObject obj)
    {
        objectPooling.Release(obj);


    }
}
