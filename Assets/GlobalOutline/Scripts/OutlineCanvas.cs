using UnityEngine;
using UnityEngine.UI;

namespace GlobalOutline
{
    public class OutlineCanvas : MonoBehaviour
    {
        private Canvas _canvas;
        private RawImage _rawImage;
        private int _width;
        private int _height;

        private RenderTexture _tempRenderTexture;

        private void Awake()
        {
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 16960;
            _rawImage = gameObject.AddComponent<RawImage>();
            _rawImage.raycastTarget = false;
        }

        private void OnDestroy()
        {
            if (_tempRenderTexture != null)
            {
                RenderTexture.ReleaseTemporary(_tempRenderTexture);
            }
        }

        private void Update()
        {
            if (_width != Screen.width || _height != Screen.height)
            {
                if (_tempRenderTexture != null)
                {
                    RenderTexture.ReleaseTemporary(_tempRenderTexture);
                }
                _width = Screen.width;
                _height = Screen.height;
                _tempRenderTexture = RenderTexture.GetTemporary(_width, _height, 0);
                _rawImage.texture = _tempRenderTexture;
            }
            OutlineManager.Instance.FillTexture(_tempRenderTexture);
        }
    }
}
