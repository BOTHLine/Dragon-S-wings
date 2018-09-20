using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    [SerializeField]
    private EntityGeneralSettings entityGeneralSettings;

    private EntityInput entityInput;
    private EntityMovement entityMovement;
    private EntityDash entityDash;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        entityInput = entityGeneralSettings.IsPlayer ? new PlayerInput() : null;

        entityMovement = new EntityMovement(entityInput, rigidbody2D, entityGeneralSettings.EntityMovementSettings);
        entityDash = new EntityDash(entityInput, rigidbody2D, entityGeneralSettings.EntityDashSettings);
    }

    private void Update()
    {
        entityInput.ReadInput();
    }
}