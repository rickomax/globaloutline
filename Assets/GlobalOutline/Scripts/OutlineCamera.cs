using UnityEngine;

namespace GlobalOutline
{
    internal class OutlineCamera : MonoBehaviour
    {
        internal OutlineManager OutlineManager;

        private static Material _blendMaterial;
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            if (_blendMaterial == null)
            {
                _blendMaterial = new Material(Shader.Find("Hidden/GlobalBlend"));
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var texture = OutlineManager.GetTexture();
            _blendMaterial.mainTexture = source;
            _blendMaterial.SetTexture("_OutlineTex", texture);
            Graphics.Blit(source, destination, _blendMaterial);
            RenderTexture.ReleaseTemporary(texture);
        }
    }
}
