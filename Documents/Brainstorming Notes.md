#### 17.09.04
* Shutter Door Implementation
    * There are many ways to go about replicating the physics for this door, and while some might be more realistic than others, others might be more intuitive or just give a better experience:
        * Door whose "weight" not only makes it harder to pull up, but also makes it fall faster (more intuitive?)
        * Door whose "weight" only opposes the upward motion, but falls at the same speed regardless of weight (more realistic)
#### 17.08.02
* Mobile phone game about opening doors
    * Goal is to open as many doors as possible
    * Different doors require different methods for opening
        * Deadbolt doors: Swipe in a direction to unlock
            * Come in horizontal and vertical variants
            * 1 door may have many locks
        * Knocker doors: Tap screen a number of times to open them
        * Dial-locked door: Swipe numeric dials to enter code to unlock door
        * Shutter door: Swipe in a direction across entire screen
            * Door will reset to original position if in an intermediate position