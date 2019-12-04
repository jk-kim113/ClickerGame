using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    private static int mUImovehash = Animator.StringToHash("Move");

    [SerializeField]
    Animator[] mWindowAims;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveWindow(int id)
    {
        mWindowAims[id].SetTrigger(mUImovehash);
    }
}
