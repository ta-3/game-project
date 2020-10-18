using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIgnitable {

    bool IsOnFire();
    void Ignite();
    void Extinguish();
}
