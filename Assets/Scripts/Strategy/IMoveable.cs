using System.Collections;
using System.Collections.Generic;
using UnityEngine;


﻿namespace Strategy{
    public interface IMoveable
    {
        float Speed { get; }
        void Move(Vector3 direction);
        void RotateTowards(Vector3 point);
    }
}