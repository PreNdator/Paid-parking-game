using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PhotoAnalyzer : IPhotoAnalyzer
{
    private LawViolationList _violationList;

    public event Action<List<LawViolation>> OnViolationsDetected;

    [Inject]
    public void Construct(LawViolationList violationList)
    {
        _violationList = violationList;
    }

    public bool AnalyzePhoto(Camera camera)
    {
        if (camera == null || _violationList == null)
            return false;

        List<LawViolation> visibleViolations = GetVisibleViolations(camera);
        if (visibleViolations.Count == 0)
            return false;

        RemoveViolations(visibleViolations);
        OnViolationsDetected?.Invoke(visibleViolations);
        return true;
    }

    private List<LawViolation> GetVisibleViolations(Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        List<LawViolation> result = new List<LawViolation>();

        foreach (LawViolation violation in _violationList.AllViolations)
        {
            GameObject obj = violation.Offender;
            if (obj == null)
                continue;

            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null)
                continue;

            if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            {
                result.Add(violation);
            }
        }

        return result;
    }

    private void RemoveViolations(List<LawViolation> violations)
    {
        foreach (LawViolation violation in violations)
        {
            _violationList.RemoveViolation(violation);
        }
    }
}