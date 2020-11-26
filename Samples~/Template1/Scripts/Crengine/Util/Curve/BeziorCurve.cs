using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crengine.Util.Curve
{

    
    public class Bezier
    {


        public static Vector3 CalculateBezierPoint(float _t, Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3)
        {
            float u = 1 - _t;
            float tt = _t * _t;
            float uu = u * u;
            float uuu = uu *u;
            float ttt = tt * _t;
            Vector3 p = uuu * _p0;
            p += 3 * uu * _t * _p1;
            p += 3 * u * tt *_p2;
            p += ttt * _p3;
            return p;
        }

        public static Vector3 CalculateBezierDirection(float _t, Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3)
        {
            Vector3 p1Point = CalculateBezierPoint(_t,_p0, _p1, _p2, _p3);

            Vector3 p2Point = CalculateBezierPoint(_t+0.01f, _p0, _p1, _p2, _p3);

            Vector3 result = p2Point - p1Point;
            result = result.normalized;

            return result;

        }


        
    }
}
