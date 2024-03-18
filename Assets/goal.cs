using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    // Reference to the particle system placed at the goal or as a child of the goal.
    // Assign this via the Unity Editor by dragging the Particle System you want to activate into this field in the script component.
    public ParticleSystem goalExplosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "ball".
        if (other.CompareTag("ball"))
        {
            // Check if the particle system is assigned and not already playing.
            if (goalExplosionEffect != null && !goalExplosionEffect.isPlaying)
            {
                // Play the particle explosion effect at the goal.
                goalExplosionEffect.Play();
                // Optional: Debug log to confirm the method is called.
                Debug.Log("Particle explosion triggered at the goal.");

                // Here you can add additional logic as needed.
            }
        }
    }
}
