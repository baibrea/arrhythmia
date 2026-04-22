using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float startOffset = 0f;

    void Start()
    {
        if (TryGetComponent(out Animator anim))
        {
            anim.speed = Random.Range(0.9f, 1.1f); // Slight random speed variation to prevent uniformity

            // Sets the 'Offset' parameter in the Animator
            anim.SetFloat("Offset", startOffset);

            // Forces the Animator to update immediately so it doesn't flicker at frame 0 before jumping to the offset.
            anim.Update(0f);
        }
    }
}