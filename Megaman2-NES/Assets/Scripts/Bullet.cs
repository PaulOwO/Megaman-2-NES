using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction_ = Vector2.zero;

    public Vector2 Direction
    {
        get => direction_;
        set => direction_ = value;
    }

    [SerializeField] private Rigidbody2D body_;
    private const float period = 1.0f;
    private const float bulletSpeed = 10.0f;
    private float timer_ = period;

    // Start is called before the first frame update
    void Start()
    {
        body_.velocity = direction_ * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        timer_ -= Time.deltaTime;
        if (timer_ <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
