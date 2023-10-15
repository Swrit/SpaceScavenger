using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    
    public static ObjectPoolManager Instance { private set; get; }

    private Dictionary<int, List<GameObject>> activePool = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> inactivePool = new Dictionary<int, List<GameObject>>();

    private Dictionary<GameObject, List<Transform>> holders = new Dictionary<GameObject, List<Transform>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public GameObject RequestObject(GameObject prefab)
    {
        if (prefab == null) return null;

        GameObject returnObject = GetObject(prefab);
        returnObject.SetActive(true);
        return returnObject;
    }

    public GameObject RequestObjectAt(GameObject prefab, Vector3 position)
    {
        if (prefab == null) return null;

        GameObject returnObject = GetObject(prefab);
        returnObject.transform.position = position;
        returnObject.SetActive(true);
        return returnObject;
    }

    private GameObject GetObject(GameObject prefab)
    {
        int prefabID = prefab.GetInstanceID();

        CreateDictionaryKey(prefabID);

        int inactivePoolCount = inactivePool[prefabID].Count;

        //Debug.Log(prefabID + " inactive amount: " + inactivePoolCount);
        GameObject returnObject;
        if (inactivePoolCount > 0)
        {
            returnObject = inactivePool[prefabID][inactivePoolCount - 1];
            inactivePool[prefabID].Remove(returnObject);
            activePool[prefabID].Add(returnObject);
        }
        else
        {
            returnObject = Instantiate(prefab);
            activePool[prefabID].Add(returnObject);
        }
        returnObject.transform.SetParent(null);
        //Debug.Log("Returning " + returnObject.GetInstanceID());
        return returnObject;
    }
    
    public void Deactivate(GameObject objectToDeactivate)
    {
        if (objectToDeactivate == null) return;

        ResetObject(objectToDeactivate);
        objectToDeactivate.SetActive(false);
        objectToDeactivate.transform.SetParent(this.transform);
        int prefabID = GetPrefabKey(objectToDeactivate);

        if (prefabID == 0)
        {
            Destroy(objectToDeactivate);
            return;
        }

        activePool[prefabID].Remove(objectToDeactivate);
        inactivePool[prefabID].Add(objectToDeactivate);
    }

    private void CreateDictionaryKey(int prefabID)
    {
        if (!activePool.ContainsKey(prefabID)) activePool[prefabID] = new List<GameObject>();
        if (!inactivePool.ContainsKey(prefabID)) inactivePool[prefabID] = new List<GameObject>();
    }

    private void ResetObject(GameObject objectToReset)
    {
        I_ObjectReset[] resets = objectToReset.GetComponentsInChildren<I_ObjectReset>();
        foreach (I_ObjectReset ior in resets) ior.ResetObject();
    }

    private int GetPrefabKey(GameObject objectToFind)
    {
        foreach (KeyValuePair<int, List<GameObject>> entry in activePool)
        {
            if (entry.Value.Contains(objectToFind)) return entry.Key;
        }
        return 0;
    }

}
