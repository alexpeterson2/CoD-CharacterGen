using UnityEngine;
using UnityEngine.UI;

namespace LlamaSoftware.UI.Utility
{
    /// <summary>
    /// Attach to an Image.
    /// Applies a different color to each corner of the Image and linearly interpolates between each
    /// </summary>
    [AddComponentMenu("UI/Effects/Corners Gradient")]
    public class CornersUIGradient : BaseMeshEffect
    {
        /// <summary>
        /// Color of the top left corner
        /// </summary>
        public Color TopLeftColor = Color.white;
        /// <summary>
        /// Color of the top right corner
        /// </summary>
        public Color TopRightColor = Color.white;
        /// <summary>
        /// Color of the bottom right corner
        /// </summary>
        public Color BottomRightColor = Color.white;
        /// <summary>
        /// Color of the bottom left corner
        /// </summary>
        public Color BottomLeftColor = Color.white;

        /// <summary>
        /// Do not call directly. Called by Unity
        /// </summary>
        /// <param name="vertexHelper">VertexHelper</param>
        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            if (enabled)
            {
                UIVertex vertex = default(UIVertex);

                for (int i = 0; i < vertexHelper.currentVertCount; i++)
                {
                    vertexHelper.PopulateUIVertex(ref vertex, i);
                    vertex.color *= UIGradientUtils.Bilerp(BottomLeftColor, BottomRightColor, TopLeftColor, TopRightColor, vertex.position);
                    vertexHelper.SetUIVertex(vertex, i);
                }
            }
        }
    }
}
