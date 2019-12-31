using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimHash
{
    public static readonly int Move = Animator.StringToHash("IsMove");
    public static readonly int UIMove = Animator.StringToHash("Move");

    public delegate void TwoIntParaCallBack(int a, int b);

    public delegate void VoidCallBack();


    private const string JSON_PATH = "JsonFiles/";
    public const string COLLEAGUE_DATA_PATH = JSON_PATH + "Colleague";
    public const string PLAYER_DATA_PATH = JSON_PATH + "PlayerInfo";
}
