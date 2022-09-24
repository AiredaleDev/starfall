using UnityEngine;


public class Kuze : APlayer
{

    //TEMPORARY, HACKY ANIMATION CONTROLLER FOR PITCH
    //TODO (ben): FIX THIS TRASH
    // TODO(mish question): This should be refactored? Maybe have an automatic animation
    // loader.
    public Animator anim;
    public GameObject rigRef;
    private Collider livingCollider;
    private Collider[] limbColliders;
    private Rigidbody[] limbPhysicsBodies;

    void Awake()
    {
        livingCollider = GetComponent<CapsuleCollider>();
        limbColliders = rigRef.GetComponentsInChildren<Collider>();
        limbPhysicsBodies = rigRef.GetComponentsInChildren<Rigidbody>();
        
        SetRagdollMode(false);
    }

    protected override void StartPlayer()
    {
        // TODO(ben): Will this be player specific or general for all the
        // players?
        anim = base.GetComponentInChildren<Animator>();
    }

    protected override void UpdatePlayer()
    {
        base.UpdatePlayer();
        
        // Implement Kuze specific update code here

    }

    protected override void HandlePlayerInputs()
    {
        HandleAnimationInputs();

        //Implement other Kuze specific inputs here        
    }

    // Other players could have different animations.
    private void HandleAnimationInputs()
    {
        if (anim != null)
        {
            float moveAxisRight = Input.GetAxisRaw(HorizontalInput);
            float moveAxisForward = Input.GetAxisRaw(VerticalInput);

            bool isMoving = moveAxisRight != 0 || moveAxisForward != 0;

            anim.SetBool("isMoving", isMoving);
            anim.SetBool("isFiring", Input.GetMouseButton(0));
        }
    }
    
    private void SetRagdollMode(bool isFlailing)
    {
        anim.enabled = !isFlailing;
        livingCollider.enabled = !isFlailing;
        
        foreach (var collider in limbColliders)
        {
            collider.enabled = isFlailing;
        }
        
        foreach (var rigidbody in limbPhysicsBodies)
        {
            rigidbody.isKinematic = !isFlailing;
        }
    }
    
    // NOTE(cameron): Have to put this here for now since animations are on the particular
    // character. Depending on how we change Animator, we might want to decouple this through
    // events (RagdollListener -- accepts an Animator and the limbs)
    public override void Kill() {
        base.Kill();
        
        // It's ragdolling time.
        SetRagdollMode(true);
    }
}

