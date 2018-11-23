
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
 


    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 15f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deadRocket;
    [SerializeField] AudioClip changeLevel;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;




    enum State {Alive, Dying, Transcending };
    State state = State.Alive;
    Scene scene;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
	{
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }

    }

   

    void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.PlayOneShot(changeLevel);
        successParticles.Play();
        Invoke("LoadNextScene", 1);
    }

    void StartDeathSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deadRocket);
        deathParticles.Play();
        state = State.Dying;
        Invoke("LoadDeathScene", 1);
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
            audioSource.Stop();
            mainEngineParticles.Stop();

        }
    }

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
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
