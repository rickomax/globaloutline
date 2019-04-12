using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalOutline
{
    internal class OutlineEffect : MonoBehaviour
    {
        private List<Renderer> _renderers;
        private List<Graphic> _graphics;
        private List<Material> _originalGraphicMaterials;
        private List<Material> _instantiatedGraphicMaterials;
        private Canvas _canvas;
        private Camera _canvasCamera;
        private bool _hasToResetCamera;

        private void Start()
        {
            OutlineManager.Instance.Register(this);
            _renderers = new List<Renderer>();
            _graphics = new List<Graphic>();
            _originalGraphicMaterials = new List<Material>();
            _instantiatedGraphicMaterials = new List<Material>();
            CollectComponents();
        }

        private void OnDestroy()
        {
            OutlineManager.Instance.Unregister(this);
            DestroyInstantiatedMaterials();
        }

        private void OnTransformChildrenChanged()
        {
            CollectComponents();
        }

        private void CollectComponents()
        {
            _canvas = GetComponentInParent<Canvas>();
            GetComponentsInChildren(_renderers);
            GetComponentsInChildren(_graphics);
            _originalGraphicMaterials.Clear();
            DestroyInstantiatedMaterials();
            _instantiatedGraphicMaterials.Clear();
        }

        private void DestroyInstantiatedMaterials()
        {
            foreach (var material in _instantiatedGraphicMaterials)
            {
                Destroy(material);
            }
        }

        private void OverlayToCamera()
        {
            if (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                _canvasCamera = _canvas.worldCamera;
                _canvas.renderMode = RenderMode.ScreenSpaceCamera;
                _canvas.worldCamera = OutlineManager.Instance.EffectCamera;
                _hasToResetCamera = true;
            }
        }

        private void CameraToOverlay()
        {
            if (_hasToResetCamera)
            {
                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _canvas.worldCamera = _canvasCamera;
                _hasToResetCamera = false;
            }
        }

        public void BeginEffect()
        {
            if (!enabled)
            {
                return;
            }
            OverlayToCamera();
            for (var i = 0; i < _graphics.Count; i++)
            {
                Graphic graphic = _graphics[i];
                Material instantiatedGraphicMaterial;
                if (i >= _originalGraphicMaterials.Count || _graphics[i].material != _originalGraphicMaterials[i])
                {
                    instantiatedGraphicMaterial = Instantiate(graphic.material);
                    if (i >= _originalGraphicMaterials.Count)
                    {
                        _originalGraphicMaterials.Add(graphic.material);
                        _instantiatedGraphicMaterials.Add(instantiatedGraphicMaterial);
                    }
                    else
                    {
                        _originalGraphicMaterials[i] = graphic.material;
                        _instantiatedGraphicMaterials[i] = instantiatedGraphicMaterial;
                    }
                }
                else
                {
                    instantiatedGraphicMaterial = _instantiatedGraphicMaterials[i];
                }
                instantiatedGraphicMaterial.SetInt("_GlobalOutline", 1);
                graphic.material = instantiatedGraphicMaterial;
            }
            foreach (var renderer in _renderers)
            {
                renderer.material.SetInt("_GlobalOutline", 1);
            }
        }

        public void EndEffect()
        {
            if (!enabled)
            {
                return;
            }
            CameraToOverlay();
            for (var i = 0; i < _graphics.Count; i++)
            {
                _graphics[i].material = _originalGraphicMaterials[i];
            }
            foreach (var renderer in _renderers)
            {
                renderer.material.SetInt("_GlobalOutline", 0);
            }
        }
    }
}
