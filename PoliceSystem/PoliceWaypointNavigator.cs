using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceWaypointNavigator : MonoBehaviour
{
    [Header("AI character")]
    public PoliceOfficer character;
    public Waypoint currentWaypoint;
    int direction;

    void Awake() {
        character = GetComponent<PoliceOfficer>();
    }

    void Start() {
        direction = Mathf.RoundToInt(Random.Range(0, 1));
        character.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update() {
        if(character.destinationReached == true) {
            if(direction == 0) {
                if(currentWaypoint.nextWaypoint != null) {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
                else {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                    direction = 1;
                }
            }
            else if(direction == 1) {
                if(currentWaypoint.previousWaypoint != null) {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                }
                else {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                    direction = 0;
                }
            }
            character.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}
