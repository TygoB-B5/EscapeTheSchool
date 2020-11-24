using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public Rigidbody player;
    private GameObject enemy;
    private Camera cam;

    public float sens;

    private float Speed = 0.04f;
    private float speedController;

    private float jumpStrength = 10;
    private float bobbingAmount = 0.1f;
    private float bobbingSpeed = 0.2f;
    private float midpoint = 0.75f;
    private float UD;
    private float HR;
    private bool isGrounded;
    private float timer;
    private float LR;
    private float FB;
    private bool isSprinting;

    private float flashlightEnergy = 1;
    public Light flashLight;
    private float energy = 1;
    private bool drainEnergy;
    private bool canMove = true;


    private Uihandler uiHandler;
    private Playerstats playerStats;
    private AudioHandler audioHandler;
    private SceneHandler sceneManager;
    private Vault vault;

    void Start()
    {
        flashLight.enabled = !flashLight.enabled;
        bobbingSpeed = 0.1f;
        speedController = Speed;
        player = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        uiHandler = FindObjectOfType<Uihandler>();
        uiHandler.EnableDieUi(false);
        playerStats = FindObjectOfType<Playerstats>();
        audioHandler = FindObjectOfType<AudioHandler>();
        sceneManager = FindObjectOfType<SceneHandler>();
        vault = FindObjectOfType<Vault>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if(!canMove)
        { return; }
        LR = Input.GetAxis("Horizontal");
        FB = Input.GetAxis("Vertical");

        player.transform.Translate(Vector3.forward * speedController * FB);
        player.transform.Translate(Vector3.left * speedController * -LR);
    }

    void Update()
    {
        Movement();
        TestForObjectPickup();
        Flashlight();
        TestIfTouchedByEnemy();
    }

    void Flashlight()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            flashLight.enabled = !flashLight.enabled;
        }

        if(flashLight.enabled == true && flashlightEnergy > 0)
        {
            flashlightEnergy -= 0.1f * Time.deltaTime;
            flashLight.intensity = flashlightEnergy * 2 * 1;
        }
        else
        {
            flashlightEnergy += 0.005f * Time.deltaTime;
            flashLight.enabled = false;
        }
    }

    void TestIfTouchedByEnemy()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (Vector3.Distance(player.transform.position, enemy.transform.position) < 2.5f)
        {
            Die();
        }
    }

    void Die()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        audioHandler.StopSound();
        audioHandler.PlaySound("Scream");
        uiHandler.EnableDieUi(true);
        player.transform.position = new Vector3(0, 0, 0);
        StartCoroutine(GameOver());
        canMove = false;
    }

    //pickup
    #region
    void TestForObjectPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 7))
        {
            if (hit.transform.CompareTag("Ground") || hit.transform.CompareTag("Untagged") || hit.transform.CompareTag("Enemy"))
            {
                uiHandler.EnablePressEui(false);
                vault.CheckVaultStatus(false);
            }
            else
            {
                if(!hit.transform.CompareTag("Vault") && playerStats.Keys() != 5)
                uiHandler.EnablePressEui(true);

                Pickup(hit);
            }
        }
        else
        {
            uiHandler.EnablePressEui(false);
            vault.CheckVaultStatus(false);
        }
    }

    void Pickup(RaycastHit hit)
    {
        if(hit.transform.tag == "Vault")
        {
            vault.CheckVaultStatus(true);
        }
        else
        {
            vault.CheckVaultStatus(false);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(hit.transform.tag == "Key")
            {
                playerStats.AddKey();
            } else if (hit.transform.tag == "Batterypack")
            {
                flashlightEnergy = 1;
            }

            if (hit.transform.tag == "Vault")
            {
                vault.VaultOpen();
                return;
            }

            Destroy(hit.transform.gameObject);
        }
    }
    #endregion

    //movement
    #region

    void Movement()
    {
        MouseRotation();
        Headbob();
        Sprintinput();
    }

    void MouseRotation()
    {
        if (!canMove)
        { return; }

        UD = (UD += -Input.GetAxisRaw("Mouse Y") * sens);
        UD = Mathf.Clamp(UD, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(UD, cam.transform.localRotation.y, cam.transform.localRotation.z);

        HR = (HR += -Input.GetAxisRaw("Mouse X") * sens);
        player.transform.localRotation = Quaternion.Euler(player.transform.localRotation.x, -HR, player.transform.localRotation.z);
    }

    void Sprintinput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            drainEnergy = true;
            speedController = Speed * 1.5f;
            bobbingSpeed = bobbingSpeed * 1.5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            drainEnergy = false;
            speedController = Speed;
            bobbingSpeed = 0.1f;
        }
        else if(energy < 0)
        {
            drainEnergy = false;
            speedController = Speed;
            bobbingSpeed = 0.1f;
        }

        ControlEnergy();
    }

    void ControlEnergy()
    {
        if (energy > 0 && drainEnergy == true)
        {
            energy -= 0.2f * Time.deltaTime;
        }

        if(energy < 1 && drainEnergy == false)
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                energy += 0.2f * Time.deltaTime;
            }
            else
            {
                energy += 0.05f * Time.deltaTime;
            }
            
        }
    }

    void Headbob()
    {
        float waveslice = 0.0f;
        if (Mathf.Abs(LR) == 0 && Mathf.Abs(FB) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
                audioHandler.PlaySound("Step");
            }
        }

        Vector3 v3T = cam.transform.localPosition;
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(LR) + Mathf.Abs(FB);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            v3T.y = midpoint + translateChange;
        }
        else
        {
            v3T.y = midpoint;
        }
        cam.transform.localPosition = v3T;
    }
    #endregion

    //get
    #region
    public float Energy()
    {
        return energy;
    }

    public float FlashlightEnergy()
    {
        return flashlightEnergy;
    }
    #endregion


    //go to Gameover scene
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.5f);
        sceneManager.load("GameOver");
        yield return null;
    }
}

