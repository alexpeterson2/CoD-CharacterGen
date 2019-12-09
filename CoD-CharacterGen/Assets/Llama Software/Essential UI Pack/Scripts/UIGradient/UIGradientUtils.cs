using UnityEngine;

namespace LlamaSoftware.UI.Utility
{
    /* Inspired by https://github.com/azixMcAze/Unity-UIGradient
     * which has MIT License:
     * MIT License
        Copyright (c) 2017 azixMcAze

        Permission is hereby granted, free of charge, to any person obtaining a copy
        of this software and associated documentation files (the "Software"), to deal
        in the Software without restriction, including without limitation the rights
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the Software is
        furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all
        copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
        SOFTWARE.
     */

    public static class UIGradientUtils
    {
        public struct Matrix2x3
        {
            // Matrix coordinate indices
            private float m00, m01, m02, m10, m11, m12;
            public Matrix2x3(float m00, float m01, float m02, float m10, float m11, float m12)
            {
                this.m00 = m00;
                this.m01 = m01;
                this.m02 = m02;
                this.m10 = m10;
                this.m11 = m11;
                this.m12 = m12;
            }

            public static Vector2 operator *(Matrix2x3 m, Vector2 v)
            {
                float x = (m.m00 * v.x) - (m.m01 * v.y) + m.m02;
                float y = (m.m10 * v.x) + (m.m11 * v.y) + m.m12;
                return new Vector2(x, y);
            }
        }

        public static Matrix2x3 LocalPositionMatrix(Rect rect, Vector2 dir)
        {
            float cos = dir.x;
            float sin = dir.y;
            Vector2 rectMin = rect.min;
            Vector2 rectSize = rect.size;
            float c = 0.5f;
            float ax = rectMin.x / rectSize.x + c;
            float ay = rectMin.y / rectSize.y + c;
            float m00 = cos / rectSize.x;
            float m01 = sin / rectSize.y;
            float m02 = -(ax * cos - ay * sin - c);
            float m10 = sin / rectSize.x;
            float m11 = cos / rectSize.y;
            float m12 = -(ax * sin + ay * cos - c);
            return new Matrix2x3(m00, m01, m02, m10, m11, m12);
        }

        public static Vector2 RotationDirection(float angle)
        {
            float radianAngle = angle * Mathf.Deg2Rad;

            return new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));
        }

        public static Color Bilerp(Color a1, Color a2, Color b1, Color b2, Vector2 t)
        {
            return Color.Lerp(Color.Lerp(a1, a2, t.x), Color.Lerp(b1, b2, t.x), t.y);
        }

        public static float RotateX(float xPosition, float angle, float yPosition)
        {
            return xPosition * Mathf.Cos(Mathf.Deg2Rad * angle) 
                 - yPosition * Mathf.Sin(Mathf.Deg2Rad * angle);
        }
    }
}
