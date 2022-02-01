using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] float xboost;
    [SerializeField] GameEvent DashActivated;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(dashKey))
            xboost += 500 * Time.fixedDeltaTime;

        if (Input.GetKeyDown(dashKey))
            DashActivated?.Invoke(gameObject);

        if (Input.GetKeyUp(dashKey))
        {
            float xpos = transform.position.x;
            float ypos = transform.position.y;
            playerMove.MovePlayer(new Vector2(xpos + xboost, ypos));
            xboost = 0;
        }
    }
}