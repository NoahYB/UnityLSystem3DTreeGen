using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planter : MonoBehaviour
{
    public GameObject projectile;
    public State state;
    public float projectile_speed = 10;
    public float time_to_throw = 10;
    public float throw_distance;
    public GameObject cursor;
    public GameObject seed;
    public float dampening;
    Vector3 original_position;
    public LayerMask mask;
    public GameObject parentGun;
    public GameObject player;
    Vector3 Vo = new Vector3(0, 0, 0);

    public GameObject generator;
    public enum State
    {
        INHAND,
        MOVING_TOWARDS_TARGET,
        MOVING_TOWARDS_HAND

    }
    public void OnThrowCompletion()
    {
        GetComponent<Animator>().SetBool("Throw", false);
    }
    void Start()
    {
        original_position = transform.position;
    }
    bool travelling = false;
    bool arrived = false;
    void Update()
    {
        
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(camRay, out hit, 10000f,mask))
            {
                cursor.transform.position = hit.point + Vector3.up * 0.01f;
                Vector3.Lerp(cursor.transform.position, Input.mousePosition, Time.deltaTime * dampening);
                cursor.transform.up = hit.normal;
                cursor.SetActive(false);
                if (Input.GetMouseButtonDown(0))
                {
                    cursor.SetActive(false);
                    GameObject p = Instantiate(projectile);
                    p.transform.position = transform.position;
                    p.transform.rotation = transform.rotation;
                    StartCoroutine(SimulateProjectile(p.transform, cursor.transform.position, cursor.transform.rotation));
                }
            }

        }
    }

    IEnumerator SimulateProjectile(Transform projectile, Vector3 target, Quaternion rotation)
    {
        // Move projectile to the position of throwing object + add some offset if needed.

        // Calculate distance to target
        float target_Distance = Vector3.Distance(projectile.position, target);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * 5 * Mathf.Deg2Rad) / 9.8f);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(5 * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(5 * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
        projectile.rotation = Quaternion.LookRotation(target - projectile.position);
        // Rotate projectile to face the target.

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            projectile.Translate(0, (Vy - (9.8f * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
        
        generator.GetComponent<Generation>().grow_tree(Instantiate(seed, projectile.position, rotation));
        projectile.transform.position = new Vector3(0, -100, 0);
    }
}