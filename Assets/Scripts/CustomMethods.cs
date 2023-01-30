using UnityEngine;



public static class CustomMethods{
  public static bool isNaN_V2(Vector2 input){
    if (input.x != input.x || input.y != input.y){
        return true;
    }else{
        return false;
    }
}

public static bool isNaN_float(float input){
    if (input != input ){
        return true;
    }else{
        return false;
    }
}

public static Vector2 ScaledPos(Vector2 position){
    Vector2 output = new Vector2(0,0);
    output.x = (1f+ (position.x/boxdims.x))/2.0f;
    output.y = (1f+ (position.y/boxdims.y))/2.0f;
    return output;
}

public static Vector2 boxdims = new Vector2(1f,1f);
public static int[] gridDims = new int[2];

public static float ScaledSigmoid(float input,float slope){

    float output = 1.0f/(1+(Mathf.Pow(2f,-input*Mathf.Pow(2f,1f+slope)+Mathf.Pow(2f,slope))));
    return output;
}

}