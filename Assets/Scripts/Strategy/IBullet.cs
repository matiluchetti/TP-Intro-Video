using System.Collections;
using System.Collections.Generic;
using UnityEngine;


﻿namespace Strategy{
    public interface IBullet
    {
        int Damage { get; }
        float Speed { get; }
        float LifeTime { get; }

        void Travel();
        void OnCollisionEnter(Collision collision);
    }
}