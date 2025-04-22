using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LawViolationList
{
    private LinkedList<LawViolation> _lawViolations = new LinkedList<LawViolation>();

    public IEnumerable<LawViolation> AllViolations => _lawViolations;

    public LawViolation AddLawViolation(GameObject obj, float score)
    {
        LawViolation violation = new LawViolation(obj, score);
        _lawViolations.AddLast(violation);
        return violation;
    }

    public async UniTask AddTemporaryViolation(GameObject obj, float score, float durationSeconds, CancellationToken cancellationToken = default)
    {
        LawViolation violation = new LawViolation(obj, score);
        _lawViolations.AddLast(violation);

        try
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(durationSeconds), cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Operation canceled");
        }

        _lawViolations.Remove(violation);
    }

    public List<LawViolation> GetViolationsForObject(GameObject obj)
    {
        List<LawViolation> result = new List<LawViolation>();
        foreach (LawViolation violation in _lawViolations)
        {
            if (violation.Offender == obj)
                result.Add(violation);
        }
        return result;
    }

    public void RemoveViolationsForObject(GameObject obj)
    {
        LinkedListNode<LawViolation> node = _lawViolations.First;
        while (node != null)
        {
            LinkedListNode<LawViolation> next = node.Next;
            if (node.Value.Offender == obj)
                _lawViolations.Remove(node);
            node = next;
        }
    }

    public void RemoveViolation(LawViolation violation)
    {
        _lawViolations.Remove(violation);
    }
}
