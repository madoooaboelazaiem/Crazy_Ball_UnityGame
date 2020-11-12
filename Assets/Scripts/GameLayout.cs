using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class GameLayout : MonoBehaviour
{
    public GameObject PrefabRoad;
    public GameObject platform;
    public int roadAmount;
    List<GameObject> pooledObjects;



    public Transform startPos;
    public int newRoad = 0;
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for(int i =0;i<roadAmount;i++){
            GameObject obj = (GameObject) Instantiate(PrefabRoad,new Vector3(transform.position.x,transform.position.y, startPos.position.z+newRoad), Quaternion.identity);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            newRoad+=10;
        }

    }
   
    // Update is called once per frame
    void Update()
    {
        // Vector3 car = GameObject.Find("Car").transform.position;
        // Debug.Log("Current road: "+ car);
        // if ( platform != null ) {
        //     Destroy(platform.gameObject);              // destroy the platform
        // }
        // platform= Instantiate(PrefabRoad,new Vector3(car.x,car.y, car.z+newRoad), Quaternion.identity);

        if(transform.position.z < startPos.position.z){
            newRoad+=10;
            transform.position = new Vector3(transform.position.x,transform.position.y,startPos.position.z);
            GameObject newRoadPre = GetPooledObjects();
            newRoadPre.transform.position = transform.position;
            newRoadPre.SetActive(true);
        }
    }
     public GameObject GetPooledObjects(){
        newRoad+=10;
        for(int i=0;i< pooledObjects.Count;i++){
            if(pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }
        GameObject obj = (GameObject) Instantiate(PrefabRoad,new Vector3(transform.position.x,transform.position.y, startPos.position.z), Quaternion.identity);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
}
