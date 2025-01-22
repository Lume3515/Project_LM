using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class ObjectArrangement : MonoBehaviour
{
    [MenuItem("ObjectArrangement/In Put")]

    // ** 에디터에서 빈 게임오브젝트에 객체를 생성 후 Map태그를 달아줘야 **OutPut 사용** 가능함 ** \\

    private static void InPut()
    {
        // csv파일 생성? 작성
        using (StreamWriter streamWriter = new StreamWriter($"C:/Users/User/source/Map.csv"))
        {
            // 맵의 크기 만큼 배열 생성
            GameObject[] obj = new GameObject[GameObject.FindGameObjectsWithTag("Map").Length];

            // 맵 찾아오기
            obj = GameObject.FindGameObjectsWithTag("Map");

            // 리스트 초기화
            List<GameObject> mainObj = new List<GameObject>();

            int index = 0;
            // 규격?
            streamWriter.WriteLine("index, name, posX, posY, posZ, rotX, rotY, rotZ, scaleX, scaleY, scaleZ");

            // 메인 오브제 추가
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

            // 값 넣기
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

            Debug.Log("csv 파일 생성완료");

        }
    }


    [MenuItem("ObjectArrangement/Out Put")]

    private static void OutPut()
    {
        // 불러오기
        using (StreamReader streamReader = new StreamReader($"C:/Users/User/source/Map.csv"))
        {
            // 맵 설치할 부모
            Transform parent;

            // 태그로 맵의 부모 가져오기
            parent = GameObject.FindWithTag("Map").transform;

            // 읽기
            string data = streamReader.ReadLine();

            int objNumber_Index = 0;

            while ((data = streamReader.ReadLine()) != null)
            {
                string[] datas = data.Split(',');

                int index = datas[1].IndexOf('_'); // 4

                // 설정(위치, 회전, 크기)
                string resourceName = datas[1].Substring(0, index);
                Vector3 resoucePos = new Vector3(float.Parse(datas[2]), float.Parse(datas[3]), float.Parse(datas[4]));
                Vector3 resouceScale = new Vector3(float.Parse(datas[8]), float.Parse(datas[9]), float.Parse(datas[10]));
                Vector3 resouceRot = new Vector3(float.Parse(datas[5]), float.Parse(datas[6]), float.Parse(datas[7]));

                GameObject obj;

                // 게임매니저라면 인스턴싱 하고 이 반복문 분기
                if (resourceName == "GameManager")
                {

                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Main/" + resourceName));

                    break;
                }
                else
                {
                    // 인스턴싱
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Map/" + resourceName), parent);

                }

                // 상세 설정
                obj.name = $"{resourceName}";
                obj.transform.position = resoucePos;
                obj.transform.rotation = Quaternion.Euler(resouceRot);
                obj.transform.localScale = resouceScale;
                objNumber_Index++;
            }

            // 위와 같음
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
