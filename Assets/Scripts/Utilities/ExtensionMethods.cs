//Reference: https://unity3d.com/learn/tutorials/topics/scripting/extension-methods

//static class that is meant to extend methods of classes.
using UnityEngine;

public static class ExtensionMethods
{
    //square and assign a float back to the value that called the function
    //float test = 5.0f;
    //test.Square();
    //print(test) -> 25.0f;
    public static void Square(this float lhs)
    {
        lhs = lhs * lhs;
    }

    //square and return a float back to the value that called the function
    //float test = 5.0f;
    //float testSquared = float.Square();
    //print(test) -> 5.0f;
    //print(testSquared) -> 25.0f;
    public static float Squared(this float lhs)
    {
        return lhs * lhs;
    }

    //square and assign an int back to the value that called the function
    //int test = 5;
    //test.Square();
    //print(test) -> 25;
    public static void Square(this int lhs)
    {
        lhs = lhs * lhs;
    }

    //square and return an int back to the value that called the function
    //int test = 5;
    //int testSquared = test.Square();
    //print(test) -> 5;
    //print(testSquared) -> 25;
    public static float Squared(this int lhs)
    {
        return lhs * lhs;
    }

    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        float tempX = vector.x;
        float tempY = vector.y;
        vector.x = (cos * tempX) - (sin * tempY);
        vector.y = (sin * tempX) + (cos * tempY);
        return vector;
    }
}
