using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle
{
    Vector3 position;
    Vector3 heading;
    bool drawn;
    ArrayList drawing_vectors;
    float h = 0;

    float pitch_delta;
    float yaw_delta;
    float roll_delta;
    public Turtle(Vector3 position, Vector3 heading, bool drawn, ref ArrayList drawing_vectors, float h)
    {
        this.drawing_vectors = drawing_vectors;
        this.position = position;
        drawing_vectors.Add(position);
        this.heading = heading;
        this.drawn = drawn;
        this.h = h;
        this.pitch_delta = 0;
        this.yaw_delta = 0;
        this.roll_delta = 0;
    }

    public void move(float distance)
    {
        Vector3 new_position = Vector3.zero;
        new_position[0] = position[0] + distance * (Mathf.Sin(Mathf.Deg2Rad * h));
        new_position[1] = position[1] + distance * (Mathf.Cos(Mathf.Deg2Rad * h));
        position = new_position;
        drawing_vectors.Add(new_position);
    }

    public void turn(float delta)
    {
        this.h += delta;
    }
    public void pitch(float delta)
    {
        float x = this.position.x * Mathf.Cos(delta) + this.position.z * Mathf.Sin(delta);
        float z = this.position.x * -Mathf.Sin(delta) + this.position.z * Mathf.Cos(delta);
        this.position = new Vector3(x, this.heading.y, z);
    }
    public void roll(float delta)
    {
        float x = this.position.x * Mathf.Cos(delta) + this.position.y * -Mathf.Sin(delta);
        float y = this.position.x * Mathf.Sin(delta) + this.position.y * Mathf.Cos(delta);
        this.position = new Vector3(x, y, this.position.z);
    }

    public void turn_around()
    {

    }
    public Vector3 get_pos()
    {
        return position;
    }
    public float get_heading()
    { 
        return h;
    }
    public Turtle deep_copy()
    {
        Turtle turtle_copy = new Turtle(this.position, this.heading, this.drawn, ref this.drawing_vectors, this.h);
        return turtle_copy;
    }
    public float get_pitch()
    {
        return this.pitch_delta;
    }
    public float get_yaw()
    {
        return this.yaw_delta;
    }
    public float get_roll()
    {
        return this.roll_delta;
    }
}