using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    protected T[] mOriginArr;
    protected List<T>[] mPool;

    protected void PoolSetUp()
    {
        mPool = new List<T>[mOriginArr.Length];
        for(int i = 0; i < mPool.Length; i++)
        {
            mPool[i] = new List<T>();
        }
    }

    private void Start()
    {
        PoolSetUp();
    }

    public T GetFromPool(int id)
    {
        for(int i = 0; i < mPool[id].Count; i++)
        {
            if(!mPool[id][i].gameObject.activeInHierarchy)
            {
                mPool[id][i].gameObject.SetActive(true);
                return mPool[id][i];
            }
        }

        return MakeNewInstance(id);
    }

    protected virtual T MakeNewInstance(int id)
    {
        T newObj = Instantiate(mOriginArr[id]);
        mPool[id].Add(newObj);
        return newObj;
    }
}
