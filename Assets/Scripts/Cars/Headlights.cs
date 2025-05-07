using System.Collections.Generic;
using UnityEngine;

public class Headlights : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> _lights;

    public void TurnLights(bool enable)
    {
        if (_lights == null || _lights.Count == 0)
        {
            Debug.LogWarning("No lights assigned to Heaadlights.");
            return;
        }

        foreach (var light in _lights)
        {
            if (light != null)
            {
                light.SetActive(enable);
            }
        }
    }
}
