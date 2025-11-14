// Include the namespaces (code libraries) you need below.
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        // Variables:

        // Player Variables
        Vector2 playerPosition = new Vector2(380, 280); // Sets the initial player position
        float playerSpeed = 200f; // Set the player speed
        float playerSize = 40f; // Set the player size

        // Enemy Variables
        Enemy[] enemies = new Enemy[100]; // Set a maximum of 100 enemies on-screen at a time
        int enemyCount = 0; // Count the number of currently active enemies
        float enemySpeed = 150f; // Set the enemies' speed
        float enemySize = 40f; // Set the enemies' size
        float enemyCooldown = 1f; // How long before a new enemy can spawn
        float enemyTimer = 0f; // When a new enemy will spawn (Works in tandem with enemyCooldown)

        // Game over check
        bool gameOver = false;

        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            Window.SetTitle("Just Squares Jr."); // Set the window title
            Window.SetSize(800, 600); // Set the window size (currently maximum)
        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            // Set window background
            Window.ClearBackground(Color.Black);

            if (!gameOver) // Make the game only play if the Game is not Over
            {
                // Movement input
                float moveX = Input.GetAxis(KeyboardInput.Left, KeyboardInput.Right); // Left and Right keys move the player respectively
                float moveY = Input.GetAxis(KeyboardInput.Up, KeyboardInput.Down); // Up and Down keys move the player respectively

                Vector2 movement = new Vector2(moveX, moveY); // Create a vector for player movement
                playerPosition += movement * playerSpeed * Time.DeltaTime; // Update the player's position based on input

                // Player collision - Prevent the player from moving off the...
                if (playerPosition.X < 0) playerPosition.X = 0; // ...left edge of the window
                if (playerPosition.X > 800 - playerSize) playerPosition.X = 800 - playerSize; // ...right edge of the window
                if (playerPosition.Y < 0) playerPosition.Y = 0; // ...top edge of the window
                if (playerPosition.Y > 600 - playerSize) playerPosition.Y = 600 - playerSize; // ...bottom edge of the window

                // Draw player
                Draw.FillColor = Color.Cyan; // Make the player cyan
                Draw.Square(playerPosition, playerSize); // Make the player a square according to their position and size

                // Draw enemies
                enemyTimer -= Time.DeltaTime; // Decrease the enemy timer per frame
                if (enemyTimer <= 0f) // When the timer reaches 0, spawn a new enemy
                {
                    SpawnEnemy(); // Call function to spawn an enemy
                    enemyTimer = enemyCooldown; // Reset the enemy spawn timer
                }

                for (int i = 0; i < enemyCount; i++) // Update each enemy that spawns
                {
                    enemies[i].Update(); // Draw and move each enemy

                    float distanceX = Math.Abs(playerPosition.X - enemies[i].Position.X); // Calculate horizontal distance between the player and enemies
                    float distanceY = Math.Abs(playerPosition.Y - enemies[i].Position.Y); // Calculate vertical distance between the player and enemies
                    float combinedSize = (playerSize + enemies[i].Size) / 2; // Calculate distance when a player-enemy collision occurs

                    if (distanceX < combinedSize && distanceY < combinedSize) // If the player is touching an enemy, cause a Game Over
                    {
                        gameOver = true;
                    }
                }


            }

            // Game Over screen
            if (gameOver)
            {
                Text.Color = Color.Magenta; // Make "GAME OVER" text magenta
                Text.Size = 112; // Set "GAME OVER" text size
                Text.Draw("GAME OVER", 150f, 250f); // Write "GAME OVER" and set text position
            }
        }

        public class Enemy // Enemy class
        {
            public Vector2 Position; // Current position of an enemy
            public Vector2 Velocity; // Current velocity of an enemy
            public float Size; // Size of all enemies

            public Enemy(Vector2 position, Vector2 velocity, float size) // Initialize an enemy
            {
                Position = position;
                Velocity = velocity;
                Size = size;
            }

            public void Update() // Enemy specific updates
            {
                Position += Velocity * Time.DeltaTime; // Move enemies according to their velocity

                // Enemy collision - Make enemies bounce off the window edges
                if (Position.X - Size / 2 < 0) // Bounce enemies horizontally
                {
                    Velocity.X *= -1; // Cause horizontal bounce
                }

                if (Position.Y - Size / 2 < 0 || Position.Y + Size / 2 > 600) // Bounces enemies vertically
                {
                    Velocity.Y *= -1; // Cause vertical bounce
                }

                Draw.FillColor = Color.Magenta; // Make enemies magenta
                Draw.Square(Position, Size); // Make enemies squares according to their position and size

            }

        }

        void SpawnEnemy() // Enemy spawning mechanic
        {
            if (enemyCount >= enemies.Length) return; // Check if the Enemy array is full so they stop spawning

            int windowEdge = Random.Integer(0, 4); // Randomly picks between the four sides of the screen to spawn enemies
            Vector2 spawnPoint = new Vector2(0, 0); // Initialize enemy spawn point

            if (windowEdge == 0) spawnPoint = new Vector2(Random.Float(0, 800), enemySize); // Spawn enemies at the top
            if (windowEdge == 1) spawnPoint = new Vector2(Random.Float(0, 800), 600 - enemySize); // Spawn enemies at the bottom
            if (windowEdge == 2) spawnPoint = new Vector2(enemySize, Random.Float(0 + enemySize, 600)); // Spawn enemies from the left
            if (windowEdge == 3) spawnPoint = new Vector2(800 - enemySize, Random.Float(0 + enemySize, 600)); // Spawn enemies from the right

            Vector2 enemyDirection = Random.Direction(); // Make enemies move in a random direction

            enemies[enemyCount] = new Enemy(spawnPoint, enemyDirection * enemySpeed, enemySize); // Create a new enemy and add it to the array
            enemyCount++; // Increase the count of active enemies
        }
    }

}
