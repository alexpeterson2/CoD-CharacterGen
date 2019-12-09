using UnityEngine;
using UnityEngine.UI;

namespace LlamaSoftware.UI.Utility
{
    /// <summary>
    /// Attach to an Image - updates the color of the image to be a linear gradient from StartColor to EndColor
    /// </summary>
    [AddComponentMenu("UI/Effects/Rotatable 2 Color Linear Gradient")]
    public class RotatableLinearUIGradient : BaseMeshEffect
    {
        /// <summary>
        /// The starting color of the Gradient
        /// </summary>
        public Color StartColor = Color.white;
        /// <summary>
        /// The ending color of the Gradient
        /// </summary>
        public Color EndColor = Color.white;
        /// <summary>
        /// Extra weight added to the starting color.
        /// 1 = only Start Color (if WeightEndColor = 0)
        /// 0 = 50% Start Color, 50% End Color, if WeightEndColor = 0
        /// </summary>
        [Range(0, 1)]
        public float WeightStartColor = 0f;
        /// <summary>
        /// Extra weight added to the starting color.
        /// 1 = only End Color (if WeightStartColor = 0)
        /// 0 = 50% End Color, 50% Start Color, if WeightStartColor = 0
        /// </summary>
        [Range(0, 1)]
        public float WeightEndColor = 0f;
        /// <summary>
        /// Angle of Rotation for the Gradient.
        /// 0 = Horizontal
        /// +-90 = Vertical
        /// +-180 = Reverse Horizontal
        /// </summary>
        [Range(-360f, 360f)]
        public float Angle = 0f;

        private UIGradientUtils.Matrix2x3 LocalPositionMatrix;

        /// <summary>
        /// Called by Unity - do not call directly
        /// </summary>
        /// <param name="vertexHelper">Vertex Helper</param>
        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            if (enabled)
            {
                LocalPositionMatrix = UIGradientUtils.LocalPositionMatrix(graphic.rectTransform.rect, UIGradientUtils.RotationDirection(Angle));

                UIVertex vertex = default(UIVertex);
                for (int i = 0; i < vertexHelper.currentVertCount; i++)
                {
                    vertexHelper.PopulateUIVertex(ref vertex, i);
                    Vector2 localPosition = LocalPositionMatrix * vertex.position;
                    vertex.color *= Color.Lerp(EndColor, StartColor, localPosition.y + WeightStartColor - WeightEndColor);
                    vertexHelper.SetUIVertex(vertex, i);
                }
            }
        }
    }
}
