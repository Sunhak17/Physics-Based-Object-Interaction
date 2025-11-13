using UnityEngine;
using System.Collections;

public class FireworkController : MonoBehaviour
{
    public ParticleSystem fireworkLaunch;
    public ParticleSystem fireworkTrail;

    public float launchTime = 1.5f; // time before explosion
    public float repeatDelay = 2f; // delay between fireworks
    public bool isActive = false; // whether fireworks keep launching

    void Start()
    {
        // Stop all particle systems at the start
        if (fireworkLaunch != null) fireworkLaunch.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (fireworkTrail != null) fireworkTrail.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void LaunchFirework()
    {
        isActive = true; // Start repeating fireworks
        StartCoroutine(RepeatingFireworks());
    }

    public void StopFirework()
    {
        isActive = false; // Stop repeating
    }

    IEnumerator RepeatingFireworks()
    {
        while (isActive)
        {
            yield return StartCoroutine(FireworkSequence());
            yield return new WaitForSeconds(repeatDelay);
        }
    }

    IEnumerator FireworkSequence()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * 3f; // Reduced height to 3 units
        float t = 0f;

        // Step 1: Play the trail as it goes up
        if (fireworkTrail != null)
        {
            fireworkTrail.Play(true);
        }

        // Move firework upward
        while (t < launchTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t / launchTime);
            yield return null;
        }

        // Step 2: Stop trail and trigger explosion (fireworkLaunch with SubEmitters)
        if (fireworkTrail != null) 
        {
            fireworkTrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // Trigger the launch/explosion at the top position
        if (fireworkLaunch != null)
        {
            fireworkLaunch.transform.position = transform.position; // Move to explosion position
            fireworkLaunch.Play(true); // This triggers SubEmitters
        }

        // Wait for explosion to finish
        yield return new WaitForSeconds(2f);

        // Reset position for next firework
        transform.position = startPos;
        if (fireworkLaunch != null) fireworkLaunch.transform.position = startPos;
    }
}
