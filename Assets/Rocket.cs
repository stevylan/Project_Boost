
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
    AudioSource RocketThruster;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 15f;


    // Use this for initialization
    void Start () {

		rigidBody = GetComponent<Rigidbody>();
        RocketThruster = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        Thrust();
        Rotate();
    }

    void OnCollisionEnter (Collision collision)
    {
        Scene scene = SceneManager.GetActiveScenene();
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                if(scene == 0)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }       
                break;
            default:
                print("Dead!");
                if (scene == 0)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    SceneManager.LoadScene(1);
                }
                break;
        }

    }
    void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up*mainThrust);
            if (!RocketThruster.isPlaying)
            {
                RocketThruster.Play();
            }
        }
        else
        {
            RocketThruster.Stop();
        }
    }

    void Rotate()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward*rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

}
