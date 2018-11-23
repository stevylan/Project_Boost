
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource RocketThruster;
    AudioSource RocketDead;
    AudioSource LevelChange;


    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 15f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deadRocket;
    [SerializeField] AudioClip changeLevel;


    enum State {Alive, Dying, Transcending };
    State state = State.Alive;
    Scene scene;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        RocketThruster = GetComponent<AudioSource>();
        RocketDead = GetComponent<AudioSource>();
        LevelChange = GetComponent<AudioSource>();



    }

    // Update is called once per frame
    void Update ()
	{
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (state == State.Dying)
        {
            RocketThruster.Stop();
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if (state != State.Alive) { return; }
       
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                state = State.Transcending;
                LevelChange.PlayOneShot(changeLevel);
                Invoke("LoadNextScene", 1);
                break;
            default:
                print("Dead!");
                RocketDead.PlayOneShot(deadRocket);
                state = State.Dying;
                Invoke("LoadDeathScene", 1);
                break;
        }

    }



    void LoadNextScene()
    {
        scene = SceneManager.GetActiveScene();

        if (scene.name == "Level 1")
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    void LoadDeathScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Level 1")
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            RocketThruster.Stop();
        }
    }

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!RocketThruster.isPlaying)
        {
            RocketThruster.PlayOneShot(mainEngine);
        }
    }

    void RespondToRotateInput()
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
