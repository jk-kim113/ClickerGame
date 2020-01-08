using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colleague : MonoBehaviour
{
    private Rigidbody2D mRB2D;

    [SerializeField]
    private float mSpeed;

    [SerializeField]
    private Transform mEffectPos;

    private Animator mAnim;

    private int mID;

    private void Awake()
    {
        mRB2D = GetComponent<Rigidbody2D>();
        mAnim = GetComponent<Animator>();
    }

    public void Init(int id, float period)
    {
        mID = id;

        StartCoroutine(Movement());
        StartCoroutine(Function(period));
    }

    private IEnumerator Movement()
    {
        WaitForSeconds moveTime = new WaitForSeconds(1);

        while(true)
        {
            int dir = Random.Range(0, 2);
            if(dir == 0)
            {
                transform.rotation = Quaternion.identity;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            int moveOrstay = Random.Range(0, 2);
            if(moveOrstay == 0)
            {
                mRB2D.velocity = Vector2.zero;
                mAnim.SetBool(AnimHash.Move, false);
            }
            else
            {
                mRB2D.velocity = transform.right * -mSpeed;
                mAnim.SetBool(AnimHash.Move, true);
            }

            yield return moveTime;
        }
    }

    public void ForcedJobFinish()
    {
        ColleagueController.Instance.JobFinish(mID, mEffectPos.position);
    }

    private IEnumerator Function(float time)
    {
        WaitForSeconds term = new WaitForSeconds(time);

        while(true)
        {
            yield return term;

            ColleagueController.Instance.JobFinish(mID, mEffectPos.position);
        }
    }
}
