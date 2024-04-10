using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWaypointNavigator : MonoBehaviour
{
    [Header("AI Car")]
    public CarNavigatorScript Car;
    public Waypoint currentWaypoint;
    int direction;

    void Awake() {
        Car = GetComponent<CarNavigatorScript>();
    }

    void Start() {
        direction = Mathf.RoundToInt(Random.Range(0, 1));
        Car.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update() {
        if(Car.destinationReached == true) {
            if(currentWaypoint.previousWaypoint != null) {
                currentWaypoint = currentWaypoint.previousWaypoint;
            }
            Car.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}
