using System.Collections.Generic;
using UnityEngine;

public class RoadPath : MonoBehaviour, IPathProvider
{
    [SerializeField]
    private List<Transform> _waypoints;

    [SerializeField]
    private int _pointsBetween = 5;

    private List<Vector3> _interpolatedPoints;

    private void Awake()
    {
        RecalculatePath();
    }

    public void RecalculatePath()
    {
        _interpolatedPoints = new List<Vector3>();

        if (_waypoints == null || _waypoints.Count < 2)
        {
            return;
        }

        for (int i = 0; i < _waypoints.Count - 1; i++)
        {
            Vector3 start = _waypoints[i].position;
            Vector3 end = _waypoints[i + 1].position;

            for (int j = 0; j <= _pointsBetween; j++)
            {
                float t = (float)j / (_pointsBetween + 1);
                _interpolatedPoints.Add(Vector3.Lerp(start, end, t));
            }
        }

        _interpolatedPoints.Add(_waypoints[_waypoints.Count - 1].position);
    }

    public IEnumerable<Vector3> GetPoints()
    {
        return _interpolatedPoints;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_waypoints == null || _waypoints.Count < 2) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < _waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }

        if (_interpolatedPoints != null && _interpolatedPoints.Count > 0)
        {
            Gizmos.color = Color.cyan;
            foreach (var point in _interpolatedPoints)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
#endif
}
