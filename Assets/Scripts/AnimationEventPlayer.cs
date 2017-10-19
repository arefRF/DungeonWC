using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPlayer : MonoBehaviour {
    public Direction dir;
    private void MoveRightFinished()
    {
        transform.parent.position = ToolKit.VectorSum(transform.position, dir);
    }
}
