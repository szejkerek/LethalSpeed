using UnityEngine;

public class DebugWeapon : MonoBehaviour
{
    [SerializeField] float startingOffset = 1.10f;
    [SerializeField] Vector3 startingVector = Vector3.up;
    private void OnDrawGizmos()
    {
        Debug.DrawLine((transform.position) + startingVector * startingOffset, (transform.position + -transform.up * 50) + startingVector * startingOffset);
    }
}
