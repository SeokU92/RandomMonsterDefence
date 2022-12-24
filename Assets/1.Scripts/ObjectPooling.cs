using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectPooling))]
public class ObjectPoolerEditor : Editor
{
    const string INFO = "Ǯ���� ������Ʈ�� ������ �������� \nvoid OnDisable()\n{\n" +
        "    ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� \n" +
        "    CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� \n}";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif

public class ObjectPooling : MonoBehaviour
{
    static ObjectPooling Instance;
    private void Awake()
    {
        Instance = this;
    }
    [Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int amount;
    }

    [SerializeField] Pool[] pools;
    List<GameObject> spawnObjects;
    Dictionary<string, Queue<GameObject>> poolDic;

    public static GameObject SpawnFromPool(string key, Vector3 position) =>  
        Instance._SpawnFromPool(key, position, Quaternion.identity);
    public static GameObject SpawnFromPool(string key, Vector3 position, Quaternion rotation) =>
        Instance._SpawnFromPool(key, position, rotation);
    public static T SpawnFromPool<T>(string key, Vector3 position) where T : Component
    {
        GameObject obj = Instance._SpawnFromPool(key, position, Quaternion.identity);
        if(obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }
    public static T SpawnFromPool<T>(string key, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = Instance._SpawnFromPool(key, position, rotation);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }
    public static List<GameObject> GetAllPools(string key)
    {
        if (!Instance.poolDic.ContainsKey(key))
            throw new Exception($"Pool with key {key} doesn't exist");
        return Instance.spawnObjects.FindAll(x => x.name == key);
    }
    public static List<T> GetAllPools<T>(string key) where T : Component
    {
        List<GameObject> objs = GetAllPools(key);
        if (!objs[0].TryGetComponent(out T component))
            throw new Exception("Component not found");
        return objs.ConvertAll(x => x.GetComponent<T>());
    }
    public static void ReturnToPool(GameObject obj)
    {
        if (!Instance.poolDic.ContainsKey(obj.name))
            throw new Exception($"Pool with key{obj.name} doesn't exist");

        Instance.poolDic[obj.name].Enqueue(obj);
    }
    
    private GameObject _SpawnFromPool(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDic.ContainsKey(key))
            throw new Exception($"Pool with key {key} doesn't exist.");

        //ť�������� ���� �߰�
        Queue<GameObject> poolQueue = poolDic[key];
        if(poolQueue.Count <= 0)
        {
            Pool pool = Array.Find(pools, x => x.key == key);
            var obj = CreateNewObject(pool.key, pool.prefab);
            ArrangePool(obj);
        }
        //ť���� ������ ���
        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }
    private void Start()
    {
        spawnObjects = new List<GameObject>();
        poolDic = new Dictionary<string, Queue<GameObject>>();
        
        //�̸�����
        foreach(Pool pool in pools)
        {
            poolDic.Add(pool.key, new Queue<GameObject>());
            for (int i = 0; i < pool.amount; i++)
            {
                var obj = CreateNewObject(pool.key, pool.prefab);
                ArrangePool(obj);
            }

            ////OnDisable�� ReturnToPool �������ο� �ߺ����� �˻�
            if (poolDic[pool.key].Count <= 0)
                Debug.LogError($"{pool.key}");
            else if (poolDic[pool.key].Count != pool.amount)
                Debug.LogError($"{pool.key}�� ReturnToPool�� �ߺ��˴ϴ�.");
        }
    }
    private GameObject CreateNewObject(string key, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = key;
        obj.SetActive(false); // ��Ȱ��ȭ�� ReturnToPool�� �ϹǷ� Enqueue�� ��
        return obj;
    }
    private void ArrangePool(GameObject obj)
    {
        //�߰��� ������Ʈ ��� ����
        bool isFind = false;
        for(int i = 0; i< transform.childCount; i++)
        {
            if(i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
            else if(transform.GetChild(i).name == obj.name)
                isFind = true;
            else if(isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
        }
    }
}
