using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float defaultSpeed = 5f;
    public float speed;
    
    private Vector3 movemnetInput;
    private Vector3 lastMovementDirection; // Zmienna do przechowywania ostatniego kierunku
    private Vector3 lastGhostDirection;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerAttack playerAttack;
    [SerializeField] private Animator ghostAnimator;

    private bool isMoving;
    private Vector3 targetPosition;

    void Start()
    {
        
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movemnetInput = context.ReadValue<Vector2>();
        if (movemnetInput != Vector3.zero)
        {
            lastMovementDirection = movemnetInput.normalized; // Zapisuje kierunek jako wektor jednostkowy
            lastMovementDirection = GetRoundedDirection(lastMovementDirection); // Zaokrągla kierunek do osi
        }

        animator.SetBool("isWalking", movemnetInput != Vector3.zero);
        
        animator.SetFloat("InputX", movemnetInput.x);
        animator.SetFloat("InputY", movemnetInput.y);
        
        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastMovementDirection.x);
            animator.SetFloat("LastInputY", lastMovementDirection.y);
        }
    }

    void Update()
    {
        rb.velocity = movemnetInput * speed;
        
        if (Input.GetMouseButtonDown(0) )
        {
            // Pobierz pozycję kliknięcia myszy w świecie gry
            Vector3 mouseScreenPosition = Input.mousePosition;
            targetPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            targetPosition.z = 0; // Ustaw z na 0 dla gry 2D

            // Oblicz kierunek ruchu i zapisz jako lastGhostDirection
            Vector3 direction = (targetPosition - transform.position).normalized;
            lastGhostDirection = GetRoundedDirection(direction); // Zaokrąglij kierunek do osi

            // Aktualizuj animacje
            ghostAnimator.SetFloat("LastInputX", lastGhostDirection.x);
            ghostAnimator.SetFloat("LastInputY", lastGhostDirection.y);
        }

        speed = StatsManager.instance.speed;
    }

    // Metoda do zaokrąglania kierunku do głównych osi
    private Vector3 GetRoundedDirection(Vector3 direction)
    {
        // Jeśli kierunek jest bliski zeru, zwróć wektor zerowy
        if (direction.magnitude < 0.1f)
            return Vector3.zero;

        // Zaokrąglanie na podstawie dominującej osi
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return new Vector3(Mathf.Sign(direction.x), 0, 0); // Wybierz oś X
        else
            return new Vector3(0, Mathf.Sign(direction.y), 0); // Wybierz oś Y
    }

    // Metoda do uzyskania ostatniego kierunku ruchu
    public Vector3 GetLastMovementDirection()
    {
        return lastMovementDirection;
    }
}
