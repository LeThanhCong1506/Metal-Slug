using Imba.Utils;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : ManualSingletonMono<BulletManager>
{
    [SerializeField]
    private List<BulletController> bulletPool;
    [SerializeField]
    private BulletController objectToPool;
    [SerializeField]
    private int amountToPool;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        bulletPool = new();
        for (int i = 0; i < amountToPool; i++)
        {
            BulletController bullet = Instantiate(objectToPool, this.transform, true);
            bullet.DeActive();
            bulletPool.Add(bullet);
        }
    }

    public BulletController GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].IsActive)
            {
                return bulletPool[i];
            }
        }

        return null;
    }
}

