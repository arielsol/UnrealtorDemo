using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class QuadUtility 
{

    public static Vector3 GetMidpoint(QuadSO quad)
    {
        return (quad.topVertex + quad.bottomVertex) / 2;
    }

    // This needs to account for how the left player's position will be displaced left but will be looking forward
    // Same for right
    // I think it's a static angle no matter how far you are 
    public static Vector3 GetQuadNormal(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 position)
    {
        // Return the normal function in the direction of the player
        if (Vector3.Dot(Normal1(pointA, pointB, pointC), position) < 0) 
        {
            return Normal1(pointA, pointB, pointC);
        }
        else
        {
            return Normal2(pointA, pointB, pointC);
        }
    }
    private static Vector3 Normal1(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        // Calculate two edges of the quad
        Vector3 edgeAB = pointB - pointA;
        Vector3 edgeBC = pointC - pointB;

        // Calculate the normal using the cross product
        Vector3 normal = Vector3.Cross(edgeAB, edgeBC).normalized;
        return normal;
    }

    private static Vector3 Normal2(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        // Calculate two edges of the quad
        Vector3 edgeAB = pointB - pointA;
        Vector3 edgeBC = pointC - pointB;

        // Calculate the normal using the cross product
        Vector3 normal = Vector3.Cross(edgeBC, edgeAB).normalized;
        return normal;
    }
}
