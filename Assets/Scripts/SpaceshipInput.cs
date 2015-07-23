using UnityEngine;
using System.Collections;

public class SpaceshipInput : MonoBehaviour {

	//Speeds
	public Vector2 speed = new Vector2( 0f, 10.0F);
    public float rotationSpeed = 200.0f;
	
	//Wrapping
	private Renderer[] renderers;
	private bool isWrappingX = false;
	private bool isWrappingY = false;

	// Use this for initialization
	void Start()
	{
    	renderers = GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
    void Update() {
		//Deal with rotation first
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
		transform.Rotate(0, 0 , -rotation);
        
		//Translation is an special case, only occurs if the player wants to go up!
		if( Input.GetAxis("Vertical") == 1 ){
			Vector2 translation = Input.GetAxis("Vertical") * speed;
			translation *= Time.deltaTime;
			GetComponent<Rigidbody2D>().AddRelativeForce(translation, ForceMode2D.Impulse);
		}
		
		//Do Screenwarpping if necessary
		ScreenWrap();
    }
	
	bool CheckRenderers()
	{
	    foreach(Renderer renderer in renderers)
	    {
	        // If at least one render is visible, return true
	        if(renderer.isVisible)
	        {
	            return true;
	        }
	    }
	 
	    // Otherwise, the object is invisible
	    return false;
	}
	
	void ScreenWrap()
	{
    	bool isVisible = CheckRenderers();
 
    	if(isVisible)
	    {
	        isWrappingX = false;
	        isWrappingY = false;
	        return;
	    }
	 
	    if(isWrappingX && isWrappingY) {
	        return;
	    }
	 
	    Camera cam = Camera.main;
	    Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);
	    Vector3 newPosition = transform.position;
	 
	    if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
	    {
	        newPosition.x = -newPosition.x;
	 
	        isWrappingX = true;
	    }
	 
	    if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
	    {
	        newPosition.y = -newPosition.y;
	 
	        isWrappingY = true;
	    }
	 
	    transform.position = newPosition;
	}
}
