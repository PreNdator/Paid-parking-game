using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PhotoTaker _photoTaker;
    [SerializeField]
    private Transform _inactivePos;
    [SerializeField]
    private Transform _activePos;

    [SerializeField]
    private float _changePosTime;
    [SerializeField]
    private float _jumpPower;

    private bool _canTakePhoto = false;

    private Tween _changePosTween;

    public void TryTakePhoto()
    {
        if (_canTakePhoto)
        {
            _photoTaker.TryTakePhoto();
        }
    }

    public void Aim(bool enable) {
        if (_changePosTween != null)
        {
            _changePosTween.Kill();
        }
        if (enable) {
            _changePosTween = transform
                .DOLocalJump(_activePos.localPosition, _jumpPower, 1, _changePosTime)
                .SetLink(gameObject)
                .OnComplete(() => _canTakePhoto = true);
        }
        else
        {
            _canTakePhoto = false;
            _changePosTween = transform
                .DOLocalJump(_inactivePos.localPosition, _jumpPower, 1, _changePosTime)
                .SetLink(gameObject);
        }
    
    }
   
}
