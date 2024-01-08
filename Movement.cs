using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    [SerializeField] public Animator anim;
    public float vitezaDeplasare = 5f;
    public float vitezaRotatie = 100f;
    public Rigidbody rb;
    private Vector3 teleportPOS;
    public GameObject prefabDeSpawn;
    private bool canJump = false;
    private Vector3 spawnPosition;
    private GameObject ultimulPlayer = null;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Teleport")) transform.position = teleportPOS;
    }

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }


    private void OnCollisionExit(Collision collision)
    {
        canJump = false;
    }

    void Start()
    {
        teleportPOS = new Vector3(-3.60f, 7.60f, -4.15f);
        rb = GetComponent<Rigidbody>();

        if (ultimulPlayer == null)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            if (playerObjects.Length > 0) ultimulPlayer = playerObjects[playerObjects.Length - 1];
        }       
    }

    [ServerRpc(RequireOwnership=false)]
    void SpawnPrefabServerRpc()
    {
        if (!IsOwner) return;
        if (prefabDeSpawn != null)
        {
            spawnPosition = ultimulPlayer.transform.position;
            spawnPosition.y += 1;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject spawnedPrefab = Instantiate(prefabDeSpawn, spawnPosition, spawnRotation);
            spawnedPrefab.GetComponent<NetworkObject>().Spawn();
            //if (networkObject!=null) NetworkManager.Singleton.Spawn(spawnedPrefab, networkObject.OwnerClientId);
        }
    }


void Update()
    {
        /* if (transform.position.y > 0)
         {
             transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, transform.rotation.eulerAngles.z);
         }*/

        
        Movement_Animation();
        if (Input.GetKeyDown(KeyCode.G)) SpawnPrefabServerRpc();
        

        
    }

    
    void Movement_Animation()
    {
        if (!IsOwner) return;
        

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            anim.SetBool("isJumping", true);
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(2 * Physics.gravity.magnitude * 4), rb.velocity.z);
        }

        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isMoving", true);
            transform.Translate(Vector3.forward * vitezaDeplasare * Time.deltaTime);

        }


        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isMoving", true);
            transform.Translate(Vector3.forward * -1 * vitezaDeplasare * Time.deltaTime);
        }


        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isMoving", true);
            transform.Rotate(Vector3.up, -vitezaRotatie * Time.deltaTime);
            transform.Translate(Vector3.forward * vitezaDeplasare * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoving", true);
            transform.Rotate(Vector3.up, vitezaRotatie * Time.deltaTime);
            transform.Translate(Vector3.forward * vitezaDeplasare * Time.deltaTime);
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) anim.SetBool("isMoving", false);
    }

    void FixedUpdate()
    {
        anim.SetBool("isJumping", false);
    }
    
}
