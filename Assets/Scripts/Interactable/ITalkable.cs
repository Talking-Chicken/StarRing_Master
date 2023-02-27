using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITalkable
{
    public abstract void SetProgress(string nodeBase);
    public abstract int GetProgress(string nodeBase);
}
