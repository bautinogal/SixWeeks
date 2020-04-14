using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    static int propsLayer = LayerMask.GetMask("Props");

    public static int PropsLayer()
    {
        return propsLayer;
    }
}
