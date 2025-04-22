using UnityEngine;

public class LawViolation
{
    private readonly GameObject _offender;
    private readonly float _points;

    public GameObject Offender => _offender;
    public float Points => _points;

    public LawViolation(GameObject offender, float points)
    {
        _offender = offender;
        _points = points;
    }
}