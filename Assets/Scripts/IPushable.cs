using UnityEngine;
public interface IPushable
{
    void Push(Vector3 force);
    bool CanBePushed();
}
