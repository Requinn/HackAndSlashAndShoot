using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}
public class ObjectPooler : MonoBehaviour{
    public static ObjectPooler ObjectPool;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;
    
    
	// Use this for initialization
	void Awake (){
	    ObjectPool = this;
	    pooledObjects = new List<GameObject>();
	    foreach (var item in itemsToPool) {
	        PoolItem(item);
	    }
    }

    public void PoolItem(ObjectPoolItem item){
        for (int i = 0; i < item.amountToPool; i++) {
            GameObject obj = Instantiate(item.objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    //think about this one
    /**
    public void UnPoolItem(ObjectPoolItem item){
        for (int i = 0; i < item.amountToPool; i++){
            Destroy(pooledObjects.Find(item.objectToPool));
        }
    }**/

    //get an inactive object to use
    public GameObject GetPooledObject(GameObject opi){
        for (int i = 0; i < pooledObjects.Count; i++){
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].name == opi.name+"(Clone)") {
                return pooledObjects[i];
            }
        }
        foreach (var item in itemsToPool){
            if (item.objectToPool.tag == tag){
                if (item.shouldExpand){
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
