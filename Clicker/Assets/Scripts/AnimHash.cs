using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimHash
{
    public static readonly int Move = Animator.StringToHash("IsMove");

    public delegate void TwoIntParaCallBack(int a, int b);

    public delegate void VoidCallBack();
}
