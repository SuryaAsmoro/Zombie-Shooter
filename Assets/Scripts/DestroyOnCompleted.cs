
using UnityEngine;

public class DestroyOnCompleted : MonoBehaviour
{
    public void DestroyParent()
    {
        Destroy(this.gameObject);
    }
}
