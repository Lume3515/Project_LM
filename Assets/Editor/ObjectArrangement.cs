using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class ObjectArrangement : MonoBehaviour
{
    [MenuItem("ObjectArrangement/In Put")]

    // ** �����Ϳ��� �� ���ӿ�����Ʈ�� ��ü�� ���� �� Map�±׸� �޾���� **OutPut ���** ������ ** \\

    private static void InPut()
    {
        // csv���� ����? �ۼ�
        using (StreamWriter streamWriter = new StreamWriter($"C:/Users/User/source/Map.csv"))
        {
            // ���� ũ�� ��ŭ �迭 ����
            GameObject[] obj = new GameObject[GameObject.FindGameObjectsWithTag("Map").Length];

            // �� ã�ƿ���
            obj = GameObject.FindGameObjectsWithTag("Map");

            // ����Ʈ �ʱ�ȭ
            List<GameObject> mainObj = new List<GameObject>();

            int index = 0;
            // �԰�?
            streamWriter.WriteLine("index, name, posX, posY, posZ, rotX, rotY, rotZ, scaleX, scaleY, scaleZ");

            // ���� ������ �߰�
            mainObj.Add(GameObject.FindWithTag("GameManager"));
            mainObj.Add(GameObject.FindWithTag("Player"));
            mainObj.Add(GameObject.FindWithTag("Object Pooling"));
            mainObj.Add(GameObject.FindWithTag("Destroy Zone"));
            mainObj.Add(GameObject.FindWithTag("EventSystem"));
            mainObj.Add(GameObject.FindWithTag("Canvas"));
            mainObj.Add(GameObject.FindWithTag("Camera parent"));
            mainObj.Add(GameObject.FindWithTag("Directional Light"));
            mainObj.Add(GameObject.FindWithTag("SpawnPos"));
            mainObj.Add(GameObject.FindWithTag("PlayerHP(World Space Canvas)"));

            // �� �ֱ�
            foreach (GameObject item in obj)
            {
                streamWriter.WriteLine($"{index},{item.name}_{index},{item.transform.position.x},{item.transform.position.y},{item.transform.position.z},{item.transform.eulerAngles.x},{item.transform.eulerAngles.y},{item.transform.eulerAngles.z},{item.transform.localScale.x},{item.transform.localScale.y},{item.transform.localScale.z}");
                index++;
            }

            foreach (GameObject item in mainObj)
            {
                streamWriter.WriteLine($"{index},{item.name}_{index},{item.transform.position.x},{item.transform.position.y},{item.transform.position.z},{item.transform.eulerAngles.x},{item.transform.eulerAngles.y},{item.transform.eulerAngles.z},{item.transform.localScale.x},{item.transform.localScale.y},{item.transform.localScale.z}");
                index++;
            }

            Debug.Log("csv ���� �����Ϸ�");

        }
    }


    [MenuItem("ObjectArrangement/Out Put")]

    private static void OutPut()
    {
        // �ҷ�����
        using (StreamReader streamReader = new StreamReader($"C:/Users/User/source/Map.csv"))
        {
            // �� ��ġ�� �θ�
            Transform parent;

            // �±׷� ���� �θ� ��������
            parent = GameObject.FindWithTag("Map").transform;

            // �б�
            string data = streamReader.ReadLine();

            int objNumber_Index = 0;

            while ((data = streamReader.ReadLine()) != null)
            {
                string[] datas = data.Split(',');

                int index = datas[1].IndexOf('_'); // 4

                // ����(��ġ, ȸ��, ũ��)
                string resourceName = datas[1].Substring(0, index);
                Vector3 resoucePos = new Vector3(float.Parse(datas[2]), float.Parse(datas[3]), float.Parse(datas[4]));
                Vector3 resouceScale = new Vector3(float.Parse(datas[8]), float.Parse(datas[9]), float.Parse(datas[10]));
                Vector3 resouceRot = new Vector3(float.Parse(datas[5]), float.Parse(datas[6]), float.Parse(datas[7]));

                GameObject obj;

                // ���ӸŴ������ �ν��Ͻ� �ϰ� �� �ݺ��� �б�
                if (resourceName == "GameManager")
                {

                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Main/" + resourceName));

                    break;
                }
                else
                {
                    // �ν��Ͻ�
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Map/" + resourceName), parent);

                }

                // �� ����
                obj.name = $"{resourceName}";
                obj.transform.position = resoucePos;
                obj.transform.rotation = Quaternion.Euler(resouceRot);
                obj.transform.localScale = resouceScale;
                objNumber_Index++;
            }

            // ���� ����
            while ((data = streamReader.ReadLine()) != null)
            {
                string[] datas = data.Split(',');

                int index = datas[1].IndexOf('_'); // 4

                string resourceName = datas[1].Substring(0, index);
                Vector3 resoucePos = new Vector3(float.Parse(datas[2]), float.Parse(datas[3]), float.Parse(datas[4]));
                Vector3 resouceScale = new Vector3(float.Parse(datas[8]), float.Parse(datas[9]), float.Parse(datas[10]));
                Vector3 resouceRot = new Vector3(float.Parse(datas[5]), float.Parse(datas[6]), float.Parse(datas[7]));
               
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Main/" + resourceName));

                obj.name = $"{resourceName}";
                obj.transform.position = resoucePos;
                obj.transform.rotation = Quaternion.Euler(resouceRot);
                obj.transform.localScale = resouceScale;
                objNumber_Index++;
            }
        }
    }
}
