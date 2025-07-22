using DG.Tweening;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 moveDirection;
    public bool isRemoved = false;

    public void TryMove()
    {
        if (CanMove())
        {
            Move();
        }
    }

    bool CanMove()
    {
        Ray ray = new Ray(transform.position, moveDirection);
        return !Physics.Raycast(ray, 1f); // Nếu không bị chặn
    }

    void Move()
    {
        isRemoved = true;
        transform.DOMove(transform.position + moveDirection * 10f, 0.5f)
            .OnComplete(() => Destroy(gameObject));
    }
}

