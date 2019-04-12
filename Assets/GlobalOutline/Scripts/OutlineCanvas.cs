using UnityEngine;
using UnityEngine.UI;

namespace GlobalOutline
{
    public class OutlineCanvas : MonoBehaviour
    {
        private Canvas _canvas;
        private RawImage _rawImage;
        private RenderTexture _tempRenderTexture;

        private void Awake()
        {
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 1000000;
            _rawImage = gameObject.AddComponent<RawImage>();
            _rawImage.raycastTarget = false;
        }

        private void OnDestroy()
        {
            if (_tempRenderTexture != null)
            {
                _tempRenderTexture.Release();
            }
        }

        private void Update()
        {
            if (_tempRenderTexture != null)
            {
                _tempRenderTexture.Release();
            }
            _tempRenderTexture = OutlineManager.Instance.GetTexture();
            _rawImage.texture = _tempRenderTexture;
        }
    }
}
