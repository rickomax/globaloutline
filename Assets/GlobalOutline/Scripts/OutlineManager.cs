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
        private Camera _effectCamera;
        private RenderBuffer[] _renderBuffers = new RenderBuffer[2];
        private List<OutlineEffect> _registeredEffects = new List<OutlineEffect>();

        public float EffectSize = 5f;
        public Color EffectColor = Color.red;
        public int EffectBlurSteps = 5;

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
            var outlineEffect = _camera.gameObject.AddComponent<OutlineCamera>();
            outlineEffect.OutlineManager = this;
            _effectGameObject = new GameObject("GlobalOutline");
            _effectGameObject.transform.SetParent(transform, false);
            _effectCamera = _effectGameObject.AddComponent<Camera>();
            _effectCamera.enabled = false;
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

        internal RenderTexture GetTexture()
        {
            foreach (var outlineEffect in _registeredEffects)
            {
                outlineEffect.BeginEffect();
            }
            _effectCamera.CopyFrom(_camera);
            _effectCamera.enabled = false;
            _effectCamera.clearFlags = CameraClearFlags.SolidColor;
            _effectCamera.backgroundColor = _transparentColor;
            _effectCamera.depthTextureMode = DepthTextureMode.Depth;
            var colorRenderTexture = GetTemporaryColorTexture();
            var depthRenderTexture = GetTemporaryDepthTexture();
            _effectCamera.SetTargetBuffers(colorRenderTexture.colorBuffer, depthRenderTexture.depthBuffer);
            _effectCamera.RenderWithShader(_outlineReplacementShader, null);
            var outlineRenderTexture = RenderTexture.GetTemporary(_effectCamera.pixelWidth, _effectCamera.pixelHeight, 0);
            _outlineMaterial.SetFloat("_OutlineSize", EffectSize);
            _outlineMaterial.SetColor("_OutlineColor", EffectColor);
            _outlineMaterial.SetTexture("_CanvasDepthTex", depthRenderTexture);
            _outlineMaterial.mainTexture = colorRenderTexture;
            Graphics.Blit(colorRenderTexture, outlineRenderTexture, _outlineMaterial);
            RenderTexture.ReleaseTemporary(colorRenderTexture);
            RenderTexture.ReleaseTemporary(depthRenderTexture);
            foreach (var outlineEffect in _registeredEffects)
            {
                outlineEffect.EndEffect();
            }
            var finalColorTexture = GetTemporaryColorTexture();
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
            return finalColorTexture;
        }

        internal void Register(OutlineEffect outlineEffect)
        {
            _registeredEffects.Add(outlineEffect);
        }

        internal void Unregister(OutlineEffect outlineEffect)
        {
            _registeredEffects.Remove(outlineEffect);
        }

        RenderTexture GetTemporaryColorTexture()
        {
            var renderTexture = RenderTexture.GetTemporary(_effectCamera.pixelWidth, _effectCamera.pixelHeight, 0);
            return renderTexture;
        }

        RenderTexture GetTemporaryDepthTexture()
        {
            var renderTexture = RenderTexture.GetTemporary(_effectCamera.pixelWidth, _effectCamera.pixelHeight, 24, RenderTextureFormat.Depth);
            return renderTexture;
        }
    }
}