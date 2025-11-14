// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        // Place your variables here:
        Vector2 playerPosition = new Vector2(380, 280); // Sets the initial player position
        float playerSpeed = 200f; // Sets the player speed
        float playerSize = 40f; // Sets the player size

        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            Window.SetTitle("Just Squares Jr."); // Sets the window title
            Window.SetSize(800, 600); // Sets the window size (currently maximum)
        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            // Set window background
            Window.ClearBackground(Color.Black);

            // Movement input
            float moveX = Input.GetAxis(KeyboardInput.Left, KeyboardInput.Right); // Left and Right keys move the player respectively
            float moveY = Input.GetAxis(KeyboardInput.Up, KeyboardInput.Down); // Up and Down keys move the player respectively

            Vector2 movement = new Vector2(moveX, moveY);
            playerPosition += movement * playerSpeed * Time.DeltaTime;

            // Player graphic
            Draw.FillColor = Color.Cyan;
            Draw.Square(playerPosition, playerSize);

        }

       
    }

}
