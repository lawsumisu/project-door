using UnityEngine;

public class ShutterLock : Lock {

    //Fields
    public float gravity, power;
    public Vector2 lockedToUnlockedAxis;
    public Vector2 dimensions;
    public float handleThickness;

    private float handleVelocity, handlePosition;
    private bool handleIsHeld;
    private Transform handleTransform, lockTransform;
    private Vector2 previousMousePosition = Vector2.zero;
    private Vector2 handleDimensions;
    private Vector2 handleOffset;


	// Use this for initialization
	void Start () {
        handleTransform = transform.GetChild(0).transform;
        lockTransform = transform.GetChild(1).transform;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateState();
        if (isLocked()) {
            UpdateKinematics();
        }
        
	}

    void UpdateState() {
        Bounds handleBounds = handleTransform.GetComponent<SpriteRenderer>().sprite.bounds;
        Bounds lockBounds = lockTransform.GetComponent<SpriteRenderer>().sprite.bounds;

        handleDimensions = new Vector2(dimensions.x, handleThickness);
        handleTransform.localScale = new Vector2(handleDimensions.x / handleBounds.size.x, handleDimensions.y / handleBounds.size.y);
        handleOffset = handleDimensions / 2;
        lockTransform.localScale = new Vector2(dimensions.x / lockBounds.size.x, dimensions.y / lockBounds.size.y);
        lockTransform.localPosition = dimensions / 2;
        transform.localRotation = Quaternion.FromToRotation(Vector2.up, lockedToUnlockedAxis);

        if (!isLocked()) {
            handleTransform.GetComponent<SpriteRenderer>().color = Color.blue;
            return;
        }

        if (!handleIsHeld && Input.GetMouseButtonDown(0) && handleIsTouched(getPointerInput())) {
            // If grabbing the handle...

            // Update state...
            handleIsHeld = true;
            previousMousePosition = getPointerInput();

            // Stop handle from moving
            handleVelocity = 0;
        }
        // If letting go of the handle...
        else if (!Input.GetMouseButton(0)) {
            // Update state
            handleIsHeld = false;
        }

        if (handleIsHeld) {
            handleTransform.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (handleIsTouched(getPointerInput())) {
            handleTransform.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else {
            handleTransform.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
    }

    void UpdateKinematics() {
        float dt = Time.deltaTime;

        handleVelocity -= gravity * dt;
        if (handleIsHeld) {
            handleVelocity = Mathf.Max(0, handleVelocity);
            float currentForce = Vector2.Dot(getPointerInput() - previousMousePosition, lockedToUnlockedAxis);
            if (currentForce < 0) {
                handlePosition += currentForce;
            }
            else {
                handleVelocity += Vector2.Dot(getPointerInput() - previousMousePosition, lockedToUnlockedAxis) * power;
            }   
            previousMousePosition = getPointerInput();
        }

        handlePosition = Mathf.Clamp(handlePosition + handleVelocity * dt, 0, dimensions.y-handleThickness);
        if (handlePosition == 0) {
            handleVelocity = 0;
        }

        handleTransform.localPosition = handleOffset + Vector2.up * handlePosition; 
    }

    override public bool isLocked() {
        // In the lock's local space, can simplify to a 1D line using the y-axis, where the unlocked position is equal to the length of the lock.
        return !handlePosition.Equals(dimensions.y - handleThickness);
    }

    private bool handleIsTouched(Vector2 pointerPosition) {
        Vector2 localSpacePointerPosition = transform.InverseTransformPoint(pointerPosition);
        Vector2 handlePosition = handleTransform.localPosition;
        Vector2 rectOrigin = handlePosition - handleDimensions / 2;
        Rect handleBounds = new Rect(rectOrigin, handleDimensions);
        return handleBounds.Contains(localSpacePointerPosition);
    }
}
