using UnityEngine;

namespace Logic.Core
{
    public static class Helpers
    {
        
        public static bool LineSegmentIntersection (Vector2 line1point1, Vector2 line1point2, Vector2 line2point1, Vector2 line2point2) {
 
            Vector2 a = line1point2 - line1point1;
            Vector2 b = line2point1 - line2point2;
            Vector2 c = line1point1 - line2point1;
 
            float alphaNumerator = b.y * c.x - b.x * c.y;
            float betaNumerator  = a.x * c.y - a.y * c.x;
            float denominator    = a.y * b.x - a.x * b.y;
 
            if (denominator == 0) {
                return false;
            } else if (denominator > 0) {
                if (alphaNumerator < 0 || alphaNumerator > denominator || betaNumerator < 0 || betaNumerator > denominator) {
                    return false;
                }
            } else if (alphaNumerator > 0 || alphaNumerator < denominator || betaNumerator > 0 || betaNumerator < denominator) {
                return false;
            }
            return true;
        }

        
        public static float Diff(Vector2 a, Vector2 b)
        {
            var num1 = b.x - a.x;
            var num2 = a.y - b.y;
            return  num1 + num2;
        }
        
        public static Vector3 ClampVector(this Vector3 vector, Vector3 a, Vector3 b)
        {

            var min = Vector3.Min(a, b);
            var max = Vector3.Max(a, b);
            
            return new Vector3(
                Mathf.Clamp(vector.x, min.x, max.x),
                Mathf.Clamp(vector.y, min.y, max.y),
                Mathf.Clamp(vector.z, min.z, max.z));
        }

        public static Vector3 FinalMiddleVector(Vector3 a, Vector3 b)
        {
            return new Vector3((a.x + b.x) / 2f, (a.y + b.y) / 2f, (a.z + b.z) / 2f);
        }
    }
}