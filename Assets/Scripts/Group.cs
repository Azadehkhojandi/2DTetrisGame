using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Group : MonoBehaviour {

    // Time since last gravity tick
    float lastFall = 0;

    // Use this for initialization
    void Start () {
        
        // Default position not valid? Then it's game over
        if (!IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if valid
            if (IsValidGridPos())
                // It's valid. Update BlocksGrid.
                UpdateGrid();
            else
                // It's not valid. revert.
                transform.position += new Vector3(1, 0, 0);
        }

        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if valid
            if (IsValidGridPos())
                // It's valid. Update BlocksGrid.
                UpdateGrid();
            else
                // It's not valid. revert.
                transform.position += new Vector3(-1, 0, 0);
        }

        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            // See if valid
            if (IsValidGridPos())
                // It's valid. Update BlocksGrid.
                UpdateGrid();
            else
                // It's not valid. revert.
                transform.Rotate(0, 0, 90);
        }

        // Move Downwards and Fall
        else if (Input.GetKeyDown(KeyCode.DownArrow) ||
                 Time.time - lastFall >= 1)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (IsValidGridPos())
            {
                // It's valid. Update BlocksGrid.
                UpdateGrid();
            }
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                Grid.DeleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().SpawnNext();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
    }
    bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);

            // Not inside Border?
            if (!Grid.InsideBorder(v))
                return false;

            // Block in BlocksGrid cell (and not part of same group)?
            if (Grid.BlocksGrid[(int)v.x, (int)v.y] != null &&
                Grid.BlocksGrid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }
    void UpdateGrid()
    {
        // Remove old children from BlocksGrid
        for (int y = 0; y < Grid.Height; ++y)
            for (int x = 0; x < Grid.Width; ++x)
                if (Grid.BlocksGrid[x, y] != null)
                    if (Grid.BlocksGrid[x, y].parent == transform)
                        Grid.BlocksGrid[x, y] = null;

        // Add new children to BlocksGrid
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);
            Grid.BlocksGrid[(int)v.x, (int)v.y] = child;
        }
    }
}
