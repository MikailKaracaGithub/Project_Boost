using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 100f; // serializefield makes it public for the editor so its pretty handy
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip jingle;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem jingleParticles;

    enum State {  Alive, Dying, Transcending }
    State state = State.Alive;

    bool collisionAreEnabled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        state = State.Alive;
        
    }

   
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild) { 
        RespondToDebugKeys();
        }
    }
    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }else if (Input.GetKeyDown(KeyCode.C))
        {
            //toggle collision
            collisionAreEnabled = !collisionAreEnabled; // toggle
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionAreEnabled) { return; } //return leaves the function
        switch (collision.gameObject.tag)
        {
            case "Friendly":

                // do Nothing
                break;

            case "Finish":

                StartSuccess();
                break;

            default:

                StartDeath();
                break;


        }
    }
    private void StartDeath()
    {
        state = State.Dying;
        print("Dead");

        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();


        Invoke("ResetLevel", levelLoadDelay);
    }

    private void StartSuccess()
    {
        state = State.Transcending;
        print("Finished!");

        audioSource.Stop();
        audioSource.PlayOneShot(jingle);

        jingleParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void ResetLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToThrustInput()
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

    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
       
    }

    private void RespondToRotateInput()
    {
        rb.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);

        }
        rb.freezeRotation = false;
    }
}
