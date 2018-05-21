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
	    /*if (ObjectPool != null && ObjectPool != this) {
	        Destroy(ObjectPool);
	    }
	    else {
	        ObjectPool = this;
	        DontDestroyOnLoad(this); //this instance will persist through scenes
	    }*/


        pooledObjects = new List<GameObject>();
	    foreach (var item in itemsToPool) {
	        PoolItem(item);
	    }
    }

    public void PoolItem(ObjectPoolItem item){
        int tempcount = 0;
        //count how many of the objects we have
        foreach (var po in pooledObjects){
            if (po.name.Equals(item.objectToPool.name + "(Clone)")){
                tempcount++;
            }
        }
        //if its the less than specified for this object, create more until the amount we need
        if (tempcount < item.amountToPool){
            for (int i = 0; i < item.amountToPool - tempcount; i++){
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
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
            if (pooledObjects[i].name == opi.name + "(Clone)"){
                if (!pooledObjects[i].activeInHierarchy){
                    return pooledObjects[i];
                }
            }
        }
        //this code didn't really do anything
        /*
        foreach (var item in itemsToPool){
            if (item.objectToPool.tag == tag){
                if (item.shouldExpand){
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }*/
        return null;
    }
}
