using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class walk : MonoBehaviour
{

    private Rigidbody2D rigid;
    Animator anim;
    Animator coinpickupanim;

    [SerializeField]
    private float speed,jumpForce,hurtForce,fallForce,ExtraJumpForce;

    [SerializeField]
    public bool isGround,duck,İsRunning;

    [SerializeField]
    public Text coinCount;

    [SerializeField]
    private AudioClip[] sounds;

    [SerializeField]
    public int coin,health, maxhealth;

    private bool facingRight;

    [SerializeField]
    public GameObject[] healths;

    [SerializeField]
    public GameObject coinPickUpObject;

    void Start()
    {
        facingRight = true;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coinpickupanim = coinPickUpObject.GetComponent<Animator>();
        health = maxhealth;
        ExtraJumpForce = jumpForce + ExtraJumpForce;
        Debug.Log("walk.cs-Oyun Başladı");
        Debug.Log("walk.cs-rigid Tanımlandı");
        Debug.Log("walk.cs-anim Tanımlandı");

    }

    void Update()
        {

        if (Input.GetKeyDown(KeyCode.Space) && isGround && !duck)
            {
            rigid.AddForce(new Vector2(0, jumpForce));
            Debug.Log("walk.cs-Karakter Zıpladı");
            GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(5,7)]);
            }

        if (health <= 0)
        {
            Death();
        }

        coinCount.text = "" + coin;
        }
        void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            Movement(horizontal);
            flip(horizontal);

        if (anim.GetFloat("Speed") >= 0.01)
        {
            İsRunning = true;
        }
        if (anim.GetFloat("Speed") <= 0.01)
        {
            İsRunning = false;
        }
    }

        void Movement(float horizontal)
        {
        if (!duck)
        {
            rigid.velocity = new Vector2(horizontal * speed, rigid.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        }
        void flip(float horizontal)
        {
            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
            {
            if (!duck && isGround)
            {
                anim.SetBool("turn", true);
                Invoke("FlipFix", 0.03f);
            }
            facingRight = !facingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                Debug.Log("walk.cs-Karakter Çevrildi");
            }


        }

        void coinpickup(float horizontal) {

        }

        void OnTriggerEnter2D(Collider2D obje)
        {
            if(obje.gameObject.tag == "Coin")
            {
            coin++;
            GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(8,10)]);
            coinpickupanim.SetBool("isİdle", false);
            Invoke("CoinPickUpFix", 0.03f);
            coinPickUpObject.GetComponent<Transform>().position = obje.gameObject.GetComponent<Transform>().position;

            //coinPickUpObject.GetComponent<Transform>().position = new Vector2(11.78f,-5.28f);
            Destroy(obje.gameObject);
            Debug.Log("walk.cs-Karakter Para Aldı");
            }

        if (obje.gameObject.tag == "Health")
        {
        if (health != maxhealth)
            {
                Destroy(obje.gameObject);
                health++;
                HealthSystem();
                GetComponent<SpriteRenderer>().color = Color.green;
                Invoke("Fix", 0.1f);


            }
        }

        if (obje.gameObject.tag == "TutorialEnd")
        {
            SceneManager.LoadScene("Level 1");
        }
        if (obje.gameObject.tag == "Level1End")
        {
            SceneManager.LoadScene("Level 2");
        }
        if (obje.gameObject.tag == "Level2End")
        {
            SceneManager.LoadScene("Level 3");
        }
    }

    void Death()
    {
        string scenename = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scenename);
    }

    void HealthSystem()
    {
        for (int i = 0; i < maxhealth; i++) {
            healths[i].SetActive(false);
        }
        for (int i = 0; i < health; i++) {
            healths[i].SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D trip)
    {
        if (trip.gameObject.tag == "Trip") {
            health -= 1;
            rigid.AddForce (Vector2.up * hurtForce);
            GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(1, 5)]);
            GetComponent<SpriteRenderer>().color = Color.red;
            anim.SetBool("Hit", true);
#pragma warning disable UNT0016 // Unsafe way to get the method name
            Invoke("Fix", 0.1f);
            Debug.Log("walk.cs-Karakter Hasar Aldı");
            HealthSystem();
        }

        if (trip.gameObject.tag == "Fall Border")
        {
            health -= 1;
            rigid.AddForce(Vector2.up * fallForce);
            GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(1, 5)]);
            GetComponent<SpriteRenderer>().color = Color.red;
            anim.SetBool("Hit", true);
            Debug.Log("walk.cs-Karakter Hasar Aldı");
            Invoke("Fix", 0.1f);
            HealthSystem();
        }
   if (trip.gameObject.tag == "JumpPad")
        {
            rigid.AddForce(Vector2.up * ExtraJumpForce);
        }
    }

    void Fix() {
        GetComponent<SpriteRenderer>().color = Color.white;
        anim.SetBool("Hit", false);
    }

    void FlipFix()
    {
        anim.SetBool("turn", false);
    }

      void CoinPickUpFix()
    {
        coinpickupanim.SetBool("isİdle", true);
    }
}
