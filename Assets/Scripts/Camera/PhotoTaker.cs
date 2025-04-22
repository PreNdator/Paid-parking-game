using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PhotoTaker : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private float _freezeTime = 1f;
    [SerializeField] private float _loadingTime = 2f;
    [SerializeField] private float _resultDisplayTime = 1f;
    [SerializeField] private Texture _loadingTexture;
    [SerializeField] private Texture _successTexture;
    [SerializeField] private Texture _failTexture;
    [SerializeField] private RenderTexture _targetRenderTexture;

    private bool _isBusy;
    private bool _lastAnalysisResult;
    private RenderTexture _originalRenderTexture;
    private IPhotoAnalyzer _photoAnalyzer;

    [Inject]
    public void Construct(IPhotoAnalyzer analyzer)
    {
        _photoAnalyzer = analyzer;
    }

    public void TryTakePhoto()
    {
        if (_isBusy || _cam == null || _targetRenderTexture == null || _loadingTexture == null || _photoAnalyzer == null)
            return;

        _isBusy = true;
        TakePhoto();
    }

    private void TakePhoto()
    {
        _originalRenderTexture = _cam.targetTexture;
        _lastAnalysisResult = _photoAnalyzer.AnalyzePhoto(_cam);
        _cam.enabled = false;
        Debug.Log("a");
        Invoke(nameof(ShowLoadingScreen), _freezeTime);
    }

    private void ShowLoadingScreen()
    {
        Graphics.Blit(_loadingTexture, _targetRenderTexture);
        Invoke(nameof(ShowResultTexture), _loadingTime);
    }

    private void ShowResultTexture()
    {
        Texture resultTexture = _lastAnalysisResult ? _successTexture : _failTexture;
        Graphics.Blit(resultTexture, _targetRenderTexture);
        Invoke(nameof(ResumeRendering), _resultDisplayTime);
    }

    private void ResumeRendering()
    {
        _cam.enabled = true;
        _isBusy = false;
    }
}