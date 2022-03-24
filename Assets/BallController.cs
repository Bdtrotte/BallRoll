using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Display = FlighterUnity.Display;
using Flighter.Core;
using FlighterUnity;
using Color = Flighter.Core.Color;
using Flighter;

public class BallController : MonoBehaviour
{
    public float speed;
    // Info for the font in the UI
    public TextStyleInfo textStyle;

    [Tooltip("How many coins one jump costs")]
    public int coinsPerJump = 1;
    [Tooltip("How much force to apply on jump")]
    public float jumpForce = 100;

    // This represents the physics of the object.
    Rigidbody body;     
    // This is just a regular int, wrapped in some junk to notify the UI system
    // when it changes.
    ValueChangeNotifier<int> coins = new ValueChangeNotifier<int>();

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

        // This displays a user interface, showing coin count and if jump is enabled.
        // It is using a custom UI engine, Flighter. 
        Display.OnScreen(new ValueChangeBuilder<int>(
            notifier: coins,
            builder: (c, _) => new Align(
                alignment: Alignment.TopCenter,
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.Start,
                    children: new List<Flighter.Widget>
                    {
                        new Padding(
                            edgeInsets: new EdgeInsets(top: 50),
                            child: new Text(
                                    data: $"Coins: {c} | Jump: ",
                                    style: textStyle.ToTextStyle()
                                )),
                        new Padding(
                            edgeInsets: new EdgeInsets(50),
                            child: new BoxConstrained(
                                constraints: BoxConstraints.Tight(50, 40),
                                child: new ColoredBox(c >= coinsPerJump
                                    ? new Color(0, 1, 0)
                                    : new Color(1, 0, 0))))
                    }))));
    }

    void Jump()
    {
        if (coins.Value < coinsPerJump)
            return;

        coins.Value -= coinsPerJump;

        // If this is going down, kill the y velocity before adding the jump force.
        var v = body.velocity;
        if (v.y < 0)
        {
            v.y = 0;
            body.velocity = v;
        }

        // Applys an immediate one time fore up.
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // This is called by Unity every PHYSICS frame. These occure on fixed intervals,
    // and should be used for physics stuff.
    void FixedUpdate()
    {
        // We get the forward direction vector.
        var forward = Camera.main.transform.forward;
        // Flaten it
        forward.y = 0;
        forward.Normalize();

        // We get the right direction vector.
        var right = Camera.main.transform.right;
        // Flaten it
        right.y = 0;
        right.Normalize();

        var xInput = Input.GetAxis("Horizontal");
        var yInput = Input.GetAxis("Vertical");

        var dir = forward * yInput + right * xInput;
        dir.Normalize();

        body.AddForce(dir * speed);
    }

    // This is called by Unity every frame.
    void Update()
    {
        // When the space key is pressed, we trigger the jump method which 
        // checks the coin count.
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    // This is called by Unity when this object collides with a TriggerCollider
    void OnTriggerEnter(Collider other) 
    {
        // Makes sure the thing this hit is a coin.
        if (other.tag != "Coin")
            return;

        // Deletes the coin.
        Destroy(other.gameObject);
        coins.Value++;
    }
}
