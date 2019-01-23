using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper{


    public static decimal Map(this decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }



}
