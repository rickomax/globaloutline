using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalOutline
{
    internal class OutlineEffect : MonoBehaviour
    {
        private const int MaxCapacity = 9999;
        public List<Renderer> Renderers { get; private set; }
        public List<Graphic> Graphics { get; private set; }
        public List<Material> OriginalGraphicMaterials { get; private set; }
        public List<Material> InstantiatedGraphicMaterials { get; private set; }

        private void Start()
        {
            OutlineManager.Instance.Register(this);
            Renderers = new List<Renderer>(MaxCapacity);
            Graphics = new List<Graphic>(MaxCapacity);
            OriginalGraphicMaterials = new List<Material>(MaxCapacity);
            InstantiatedGraphicMaterials = new List<Material>(MaxCapacity);
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
            GetComponentsInChildren(Renderers);
            GetComponentsInChildren(Graphics);
            OriginalGraphicMaterials.Clear();
            DestroyInstantiatedMaterials();
            InstantiatedGraphicMaterials.Clear();
        }

        private void DestroyInstantiatedMaterials()
        {
            foreach (var material in InstantiatedGraphicMaterials)
            {
                Destroy(material);
            }
        }

        public void BeginEffect()
        {
            if (!enabled)
            {
                return;
            }
            for (var i = 0; i < Graphics.Count; i++)
            {
                Graphic graphic = Graphics[i];
                Material instantiatedGraphicMaterial;
                if (i >= OriginalGraphicMaterials.Count || Graphics[i].material != OriginalGraphicMaterials[i])
                {
                    instantiatedGraphicMaterial = Instantiate(graphic.material);
                    if (i >= OriginalGraphicMaterials.Count)
                    {
                        OriginalGraphicMaterials.Add(graphic.material);
                        InstantiatedGraphicMaterials.Add(instantiatedGraphicMaterial);
                    }
                    else
                    {
                        OriginalGraphicMaterials[i] = graphic.material;
                        InstantiatedGraphicMaterials[i] = instantiatedGraphicMaterial;
                    }
                }
                else
                {
                    instantiatedGraphicMaterial = InstantiatedGraphicMaterials[i];
                }
                instantiatedGraphicMaterial.SetInt("_GlobalOutline", 1);
                graphic.material = instantiatedGraphicMaterial;
            }
            foreach (var renderer in Renderers)
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
            for (var i = 0; i < Graphics.Count; i++)
            {
                Graphics[i].material = OriginalGraphicMaterials[i];
            }
            foreach (var renderer in Renderers)
            {
                renderer.material.SetInt("_GlobalOutline", 0);
            }
        }
    }
}
