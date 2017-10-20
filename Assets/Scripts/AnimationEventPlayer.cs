using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPlayer : MonoBehaviour {
    public Direction dir;
    Player player;
    private void MoveRightFinished()
    {
        if (player == null)
            player = transform.parent.GetComponent<Player>();
        transform.parent.position = ToolKit.VectorSum(transform.position, dir);
        player.MoveFinished(true);
    }
}
