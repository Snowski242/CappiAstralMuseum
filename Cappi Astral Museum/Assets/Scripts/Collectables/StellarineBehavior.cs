using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarineBehavior : MonoBehaviour
{
    [SerializeField]public int gemID;
    public bool gotten = false;
    public GameObject winScreen;
    public bool justSpawned = false;
    public bool anim;
    Animator animator;
    public AudioClip sfx;
    public AudioClip shakeSound;

    int bobUpTimer = 30;
    public int bobDownTimer = 30;
    int bobAmount = 0;
    int waitTimer = 40;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SaveManager.instance.gems.Count; i++)
        {
            if (SaveManager.instance.gems[i] == gemID)
            {
                gotten = true;

            }
        }

        if (justSpawned)
        {
            if (!anim)
            {
                animator.SetTrigger("W");
                
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !justSpawned)
        {
            for (int i = 0; i < SaveManager.instance.gems.Count; i++)
            {
                if (SaveManager.instance.gems[i] == gemID)
                {
                    gotten = true;

                }
            }
            if (!gotten)
            {
                Debug.Log("got star!");
                SaveManager.instance.gems.Insert(0, gemID);
            }
            else {
                Debug.Log("already got star..");
            }

            PlayerMovement player = FindAnyObjectByType<PlayerMovement>();

            var music = GameObject.Find("Music");

            music.GetComponent<AudioSource>().Stop();

            pauseMenu.canPause = false;
            player.state = "win";
            AudioSource.PlayClipAtPoint(sfx, transform.position);
            Instantiate(winScreen);
            SaveManager.instance.Save();
            transform.localScale = Vector3.zero;
        }
    }

    public void AnimCamFocus()
    {
        Time.timeScale = 0f;
        anim = true;
        PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
        AudioSource.PlayClipAtPoint(shakeSound, transform.position);
        player.state = "";
        CinemachineFreeLook cam = FindFirstObjectByType<CinemachineFreeLook>();
        if (cam != null)
        {
            cam.m_XAxis.m_MaxSpeed = 0f;
            cam.m_YAxis.m_MaxSpeed = 0f;

            cam.m_XAxis.m_MinValue = 0f;
            cam.m_YAxis.Value = 0f;

            cam.Follow = transform;
            cam.LookAt = transform;
        }
    }

    public void AnimCamEnd()
    {
        PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
        CinemachineFreeLook cam = FindFirstObjectByType<CinemachineFreeLook>();
        cam.Follow = player.transform;
        cam.LookAt = player.transform;

        cam.m_XAxis.m_MaxSpeed = 250f;
        cam.m_YAxis.m_MaxSpeed = 2f;

        cam.m_YAxis.Value = 0.4336098f;

        player.state = "idle";
        justSpawned = false;
        Time.timeScale = 1;
    }
}
