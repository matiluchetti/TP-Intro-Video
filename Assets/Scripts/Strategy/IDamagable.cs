using System.Collections;
using System.Collections.Generic;
using UnityEngine;


﻿namespace Strategy{
    public interface IDamagable
    {
        float Life { get; }
        float MaxLife { get; }
        void TakeDamage(int damage);
    }
}