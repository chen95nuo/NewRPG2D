using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ObjectPoolTemplate
{
    public GameObject prefab;       
    public float destoryTime;       //自动销毁
    public int catchNumber  ;       //缓存创建(提前创建出N个，保存起来)
}

class ObjectPoolTimer
{
    public int index    ;
    public GameObject go;
    public float timer  ;

    public ObjectPoolTimer(int index, GameObject go, float time)
    {
        this.index = index  ;
        this.go = go        ;
        this.timer = time   ;
    }

    public bool TimerCount()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            return true ;
        }
        return false    ;
    }
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public List<ObjectPoolTemplate> templateList;

    private Dictionary<int, ObjectPoolTemplate> objectTemplateDic = new Dictionary<int, ObjectPoolTemplate>()   ;
    private Dictionary<int, List<GameObject>> objectDictionary = new Dictionary<int, List<GameObject>>()        ;

    private List<ObjectPoolTimer> timerList = new List<ObjectPoolTimer>();
    
    void Awake()
    {
        Instance = this;

        int i = 0;
        foreach (ObjectPoolTemplate template in templateList)
        {
            objectTemplateDic.Add(i, template);
            if (template.catchNumber > 0)
            {
                AdvanceCreate(i, template.catchNumber);
            }
            ++i;
        }
    }

    void Update()
    {
        List<ObjectPoolTimer> deleteList = new List<ObjectPoolTimer>();
        foreach (ObjectPoolTimer poolTimer in timerList)
        {
            if (poolTimer.TimerCount())
            {
                deleteList.Add(poolTimer);
            }
        }
        foreach (ObjectPoolTimer poolTimer in deleteList)
        {
            Destroy(poolTimer.index, poolTimer.go)  ;
            timerList.Remove(poolTimer)             ;
        }
    }

    public void AdvanceCreate(int index, int number = 1)
    {
        List<GameObject> bufferList = new List<GameObject>()                ;
        for (int i = 0; i < number; ++i)
        {
            GameObject go = Create(index, Vector3.zero, Quaternion.identity);
            bufferList.Add(go);
        }
        for (int i = 0; i < bufferList.Count; ++i)
        {
            Destroy(index, bufferList[i]);
        }
    }

    public GameObject Create(int index, Vector3 position, Quaternion rotation)
    {
        if (objectDictionary.ContainsKey(index) == false)
            objectDictionary.Add(index, new List<GameObject>());

        List<GameObject> objectList = objectDictionary[index];

        GameObject go;
        if (objectList.Count > 0)
        {
            go = objectList[0];
            objectList.RemoveAt(0);
        }
        else
        {
            go = GameObject.Instantiate(objectTemplateDic[index].prefab) as GameObject;
        }
        if(go){
        go.SetActive(true)              ;
        go.transform.position = position;
        go.transform.rotation = rotation;
        }
        

        if (objectTemplateDic[index].destoryTime > 0)
        {
            timerList.Add(new ObjectPoolTimer(index, go, objectTemplateDic[index].destoryTime));
        }
        

        return go;
    }

    public void Destroy(int index, GameObject destroyObject)
    {
        if (destroyObject){
            destroyObject.SetActive(false);
            objectDictionary[index].Add(destroyObject);
        }
       
    }
}
