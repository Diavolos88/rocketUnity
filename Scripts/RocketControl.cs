using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RocketControl : MonoBehaviour
{
    [SerializeField] int energyTotal = 1000;

    [SerializeField] int energyApply = 10;
    
    float force = 1;
    public float rotSpeed = 100f;
    public float flySpeed = 10f;
    Rigidbody rigidBody;
    [SerializeField] TextMeshProUGUI  energyText;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] GameObject lamp;
    [SerializeField] AudioClip finishSound;
    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] Vector3 lampOffset;
    private bool lampColdown = true;
    private float lampDelay = 2f;
    private bool collisionOn = true;
    
    enum State {Playing, Dead, NextLevel};
    State state = State.Playing;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        energyText.text = energyTotal.ToString();
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Debug();
    }
    
    void FixedUpdate()
    {
        if(state == State.Playing && energyTotal > 5) {
            ProcessInput();
        } else {
            flyParticles.Stop();
            audioSource.Pause();
        }
    }

void Launch() {
        if(Input.GetKey(KeyCode.Space)) {
            energyTotal -= Mathf.RoundToInt(energyApply * Time.deltaTime);
            energyText.text = energyTotal.ToString();
            rigidBody.AddRelativeForce(Vector3.up * flySpeed);
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(flySound);
                flyParticles.Play();
        } else {
            flyParticles.Stop();
            audioSource.Pause();
        }
        if(Input.GetKey(KeyCode.F) && lampColdown) {
            Instantiate(lamp, transform.position, transform.rotation);
            ChangeLampColdown();
            Invoke("ChangeLampColdown", lampDelay);
        }
}

    void Rotation() {
        Vector3 rotation = rotSpeed * Vector3.forward * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if(Input.GetKey(KeyCode.A)) {
            transform.Rotate(rotation);
        } else if(Input.GetKey(KeyCode.D)){
            transform.Rotate(-rotation);
        }
        rigidBody.freezeRotation = false;
    }
    void ProcessInput() {
        Launch();
        Rotation();
    }

    void Debug() {
        if(Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        }
        if(Input.GetKeyDown(KeyCode.K)) {
            collisionOn = !collisionOn;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (state != State.Playing || !collisionOn) {
            return;
        }

        switch(other.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            case "Battery":
                GetEnergy(500, other.gameObject);
                break;
            case "Finish":
                Finish();
                break;
            default:
                Loos();
                break;
        }
    }

    void ChangeLampColdown() {
        lampColdown = !lampColdown;
    }
    void Finish() {
        flyParticles.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticles.Play();
        state = State.NextLevel;
        Invoke("LoadNextLevel", 2f);
    }

    void Loos() {
        flyParticles.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(boomSound);
        deathParticles.Play();
        state = State.Dead;
        Invoke("LoadFirstLevel", 2f);
    }

    void LoadNextLevel() {
        int current = SceneManager.GetActiveScene().buildIndex;
        int nextLevl = current + 1;
        if(nextLevl == SceneManager.sceneCountInBuildSettings) nextLevl = 0;
        SceneManager.LoadScene(nextLevl);
    }

    void GetEnergy (int add, GameObject battery) {
        battery.GetComponent<BoxCollider>().enabled = false;
        energyTotal += add;
        energyText.text = energyTotal.ToString();
        Destroy(battery);
    }

    void LoadFirstLevel() {
        SceneManager.LoadScene(1);
    }
}
