using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Block block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    block.TryMove();
                }
            }
        }
    }
}
