using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] ScreenBounds screenBounds;
    [SerializeField] Vector2Variable moveControl;
    [SerializeField] float moveForce;
    [SerializeField] float reflectForce;
    [SerializeField] float topOffset, bottomOffset;
    [Header("Event")]
    [SerializeField] GameEvent OnHitWall;
    [SerializeField] GameEvent PassedThrough;
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if (screenBounds == null)
            screenBounds = Camera.main.GetComponent<ScreenBounds>();
    }

    void OnEnable() => moveControl.OnValueChanged += MovePlayer;

    void OnDisable() => moveControl.OnValueChanged -= MovePlayer;


    void Update()
    {
        if (moveControl == null | screenBounds == null) return;
        Vector2 moveValue = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));;

        if (transform.position.x < screenBounds.ScreenWidth() * .5f)
        {
            moveValue.x = moveValue.x < 0 ? 0 : moveValue.x;
            Vector2 veloc = GetVelocityAndHalt();
            Reflect(new Vector2(1, veloc.y));                   
        }
        if (transform.position.y < (screenBounds.ScreenHeight() * .5f) + bottomOffset)
        {
            moveValue.y = moveValue.y < 0 ? 0 : moveValue.y;
            Vector2 veloc = GetVelocityAndHalt();
            Reflect(new Vector2(veloc.x, 1));
        }
        if (transform.position.y > Mathf.Abs(screenBounds.ScreenHeight() * .5f) + topOffset)
        {
            moveValue.y = moveValue.y > 0 ? 0 : moveValue.y;
            Vector2 veloc = GetVelocityAndHalt();
            Reflect(new Vector2(veloc.x, -1));
        }
        if (transform.position.x > Mathf.Abs(screenBounds.ScreenWidth() * .5f))
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(screenBounds.ScreenWidth() * .5f, pos.y, pos.z);
            PassedThrough?.Invoke(gameObject);
        }
        moveControl.Value = moveValue;
    }


    public void MovePlayer(Vector2 value)
    {
        if (rigid == null) return;
        rigid.AddForce(moveForce * Time.fixedDeltaTime * value, ForceMode.VelocityChange);
    }

    Vector2 GetVelocityAndHalt()
    {
        Vector2 velocity = rigid.velocity;
        rigid.velocity = Vector2.zero;
        return velocity;
    }

    void Reflect(Vector2 outDirection)
    {
        if (rigid == null) return;
        if (OnHitWall != null)
            OnHitWall?.Invoke(gameObject);
        rigid.AddForce(reflectForce * Time.fixedDeltaTime * outDirection, ForceMode.Impulse);
    }
}