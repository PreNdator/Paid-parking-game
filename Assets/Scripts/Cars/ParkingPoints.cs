using System.Collections.Generic;
using UnityEngine;

public class ParkingPoints : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _violationPoints;

    public List<Transform> ViolationPoints => _violationPoints;
}
