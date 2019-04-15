using System.Collections.Generic;
using UnityEngine;

namespace GlobalOutline
{
    [RequireComponent(typeof(Camera))]
    [DefaultExecutionOrder(-1000)]
    public class OutlineManager : MonoBehaviour
    {
        public static OutlineManager Instance { get; private set; }

        private static Material _outlineMaterial;
        private static Material _blurMaterial;
        private static Color _transparentColor;
        private static Shader _outlineReplacementShader;

        private GameObject _effectGameObject;
        private Camera _camera;
        private RenderBuffer[] _renderBuffers = new RenderBuffer[2];
        private List<OutlineEffect> _registeredEffects = new List<OutlineEffect>();
        private GameObject _outlineCanvasGameObject;
        private OutlineCanvas _outlineCanvas;

        public float EffectSize = 5f;
        public Color EffectColor = Color.red;
        public int EffectBlurSteps = 5;

        public Camera EffectCamera { get; private set; }

        private void Start()
        {
            Instance = this;
            if (_outlineMaterial == null)
            {
                _outlineMaterial = new Material(Shader.Find("Hidden/GlobalOutline"));
            }
            if (_blurMaterial == null)
            {
                _blurMaterial = new Material(Shader.Find("Hidden/GlobalBlur"));
            }
            if (_transparentColor == null)
            {
                _transparentColor = new Color(0f, 0f, 0f, 0f);
            }
            if (_outlineReplacementShader == null)
            {
                _outlineReplacementShader = Shader.Find("Hidden/GlobalOutlineReplacement");
            }
            _camera = GetComponent<Camera>();
            _outlineCanvasGameObject = new GameObject("Outline Canvas");
            _outlineCanvas =  _outlineCanvasGameObject.AddComponent<OutlineCanvas>();
            _effectGameObject = new GameObject("Global Outline");
            _effectGameObject.transform.SetParent(transform, false);
            EffectCamera = _effectGameObject.AddComponent<Camera>();
            EffectCamera.enabled = false;
        }

        private void OnDestroy()
        {
            if (_outlineCanvasGameObject != null)
            {
                Destroy(_outlineCanvasGameObject);
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObject.AddComponent<OutlineEffect>();
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            var outlineEffect = gameObject.GetComponent<OutlineEffect>();
            if (outlineEffect != null)
            {
                Destroy(outlineEffect);
            }
        }

        internal void FillTexture(RenderTexture finalColorTexture)
        {
            foreach (var outlineEffect in _registeredEffects)
            {
                outlineEffect.BeginEffect();
            }
            EffectCamera.CopyFrom(_camera);
            EffectCamera.enabled = false;
            EffectCamera.clearFlags = CameraClearFlags.SolidColor;
            EffectCamera.backgroundColor = _transparentColor;
            EffectCamera.depthTextureMode = DepthTextureMode.Depth;
            EffectCamera.renderingPath = RenderingPath.Forward;
            var colorRenderTexture = GetTemporaryColorTexture();
            var depthRenderTexture = GetTemporaryDepthTexture();
            EffectCamera.SetTargetBuffers(colorRenderTexture.colorBuffer, depthRenderTexture.depthBuffer);
            EffectCamera.RenderWithShader(_outlineReplacementShader, null);
            var outlineRenderTexture = RenderTexture.GetTemporary(EffectCamera.pixelWidth, EffectCamera.pixelHeight, 0);
            _outlineMaterial.SetFloat("_OutlineSize", EffectSize);
            _outlineMaterial.SetColor("_OutlineColor", EffectColor);
            _outlineMaterial.mainTexture = colorRenderTexture;
            Graphics.Blit(colorRenderTexture, outlineRenderTexture, _outlineMaterial);
            RenderTexture.ReleaseTemporary(colorRenderTexture);
            RenderTexture.ReleaseTemporary(depthRenderTexture);
            foreach (var outlineEffect in _registeredEffects)
            {
                outlineEffect.EndEffect();
            }
            //var finalColorTexture = GetTemporaryColorTexture();
            var swapColorTexture = GetTemporaryColorTexture();
            Graphics.CopyTexture(outlineRenderTexture, finalColorTexture);
            RenderTexture.ReleaseTemporary(outlineRenderTexture);
            for (var i = 0; i < EffectBlurSteps; i++)
            {
                _blurMaterial.mainTexture = finalColorTexture;
                Graphics.Blit(finalColorTexture, swapColorTexture, _blurMaterial);
                Graphics.CopyTexture(swapColorTexture, finalColorTexture);
            }
            RenderTexture.ReleaseTemporary(swapColorTexture);
        }

        internal void Register(OutlineEffect outlineEffect)
        {
            _registeredEffects.Add(outlineEffect);
        }

        internal void Unregister(OutlineEffect outlineEffect)
        {
            _registeredEffects.Remove(outlineEffect);
        }

        private RenderTexture GetTemporaryColorTexture()
        {
            var renderTexture = RenderTexture.GetTemporary(EffectCamera.pixelWidth, EffectCamera.pixelHeight, 0);
            return renderTexture;
        }

        private RenderTexture GetTemporaryDepthTexture()
        {
            var renderTexture = RenderTexture.GetTemporary(EffectCamera.pixelWidth, EffectCamera.pixelHeight, 24, RenderTextureFormat.Depth);
            return renderTexture;
        }
    }
}
