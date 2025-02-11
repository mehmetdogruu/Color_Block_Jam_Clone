using UnityEngine;

namespace TeamSystem
{
    public class MaterialSetter : MonoBehaviour
    {
        [SerializeField] private int[] indices;
        [SerializeField] private Renderer renderer;

        public void SetMaterial(Material mat)
        {
            var materials = renderer.sharedMaterials;
            foreach (var index in indices)
            {
                materials[index] = mat;
            }

            renderer.sharedMaterials = materials;
        }
    }
}

