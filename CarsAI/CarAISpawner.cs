using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAISpawner : MonoBehaviour
{
    public GameObject[] AIPrefabs;
    public int AIToSpawn;
    public int Timer;


    void Start() {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        int count = 0;
        while(count < AIToSpawn) {
            int randomIndex = Random.Range(0, AIPrefabs.Length);
            GameObject obj = Instantiate(AIPrefabs[randomIndex]);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<CarWaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();

            obj.transform.position = child.position;
            yield return new WaitForSeconds(Timer);

            count++;
        }
    } 
}
