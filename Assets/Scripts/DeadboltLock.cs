using UnityEngine;

public class DeadboltLock : Lock {
    // Fields
    public float boltLength = 5;
    public float knobRadius = 5;
    public Vector2 lockedToUnlockedAxis = Vector2.right;
 
    private bool knobIsHeld;

    private Transform knobTransform, boltTransform;

    void Start() {
        knobTransform = transform.GetChild(0).transform;
        boltTransform = transform.GetChild(1).transform;
    }

    public void Setup(Vector2 lockedToUnlockedAxis, float boltLength, Vector2 worldOrigin) {
        this.lockedToUnlockedAxis = lockedToUnlockedAxis.normalized;
        this.boltLength = boltLength;
        
    }

   
    void Update() {
        UpdateState();
        if (!isLocked()) {
            knobTransform.GetComponent<SpriteRenderer>().color = Color.blue;
            return;
        }

        Vector2 pointerInput = getPointerInput();
        knobIsHeld = (knobIsTouched(pointerInput) && Input.GetMouseButtonDown(0)) || (Input.GetMouseButton(0) && knobIsHeld);

        if (knobIsTouched(pointerInput)){
            knobTransform.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else knobTransform.GetComponent<SpriteRenderer>().color = Color.red;

        if (knobIsHeld) {
            knobTransform.GetComponent<SpriteRenderer>().color = Color.yellow;
            setKnobPosition(transform.InverseTransformPoint(pointerInput));
        }
    }

    void UpdateState() {
        lockedToUnlockedAxis.Normalize();
        Bounds knobBounds = knobTransform.GetComponent<SpriteRenderer>().sprite.bounds;
        Bounds boltBounds = boltTransform.GetComponent<SpriteRenderer>().sprite.bounds;

        knobTransform.localScale = new Vector2(knobRadius / knobBounds.size.x, knobRadius / knobBounds.size.y);
        boltTransform.localScale = new Vector2(boltLength / boltBounds.size.x, 1);
        boltTransform.localPosition = new Vector2(boltLength / 2, 0);
        
        transform.localRotation = Quaternion.FromToRotation(Vector2.right, lockedToUnlockedAxis);
            
    }
    
    override public bool isLocked() {
        // In the knob's local space, can simplify to a 1D line, where the unlocked position is equal to the length of the bolt.
        return !knobTransform.localPosition.x.Equals(boltLength);
    }

    public void setKnobPosition(Vector2 newPosition) {
        //Debug.Log("Local Point: " + newPosition);
        // First, need to project point onto this lock's axis:
        //Vector2 projectedPoint = Vector2.Dot(newPosition, knobUnlockPosition) / Vector2.Dot(knobUnlockPosition, knobUnlockPosition) * knobUnlockPosition;

        ////Debug.Log("Projected Point: " + projectedPoint);

        //// Then, need to bound point to the be between the start (0,0) and end points of this lock by clamping the magnitude of the vector to be in the range [0, boltLength]
        //Vector2 boundedPoint = projectedPoint.normalized * Mathf.Clamp(Mathf.Sign(Vector2.Dot(projectedPoint, knobUnlockPosition)) * projectedPoint.magnitude, 0, boltLength);

        //knobTransform.localPosition = boundedPoint;

        knobTransform.localPosition = new Vector2(Mathf.Clamp(newPosition.x, 0, boltLength), 0);

    }

    private bool knobIsTouched(Vector2 pointerLocation) {
        float distance = Vector2.Distance(knobTransform.position, pointerLocation);
        return distance <= knobRadius; 
    }

}
