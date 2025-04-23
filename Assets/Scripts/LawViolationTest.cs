using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LawViolationTest : MonoBehaviour
{
    [SerializeField] private GameObject testOffender;
    [SerializeField] private float testPoints = 100f;
    [SerializeField] private float durationSeconds = 10f;

    private LawViolationList _violationList;

    [Inject]
    public void Construct(LawViolationList violationList)
    {
        _violationList = violationList;
    }

    private void Start()
    {
        RunTemporaryViolationTest().Forget();
    }

    private async UniTaskVoid RunTemporaryViolationTest()
    {
        Debug.Log("Добавляю временное нарушение...");

        await _violationList.AddTemporaryViolation(testOffender, testPoints, durationSeconds);

        Debug.Log($"Нарушение от {testOffender.name} завершено спустя {durationSeconds} сек.");
    }
}