using UnityEngine;

public class CreateObject : MonoBehaviour
{
    [SerializeField] ObjectPool objectToSpawn;

    public void CreateNewObject(GameObject objLoc)
    {
        GameObject o = objectToSpawn.GetObject();
        if (o != null)
        {
            GameObject newObject = Instantiate(o, transform.position, Quaternion.Euler(0, 0, Random.Range(-180f, 180f)), objectToSpawn.transform);
            newObject.transform.position = objLoc.transform.position;
            newObject.SetActive(true);
        }
    }
}