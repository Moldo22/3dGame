#region Libraries
using UnityEngine;
using Unity.Netcode;
#endregion

public class Movement : NetworkBehaviour
{
    #region References
    [SerializeField] public Animator anim;
    #endregion

    #region Inputs
    private float offset = 0.3f;
    public float vitezaDeplasare = 5f;
    public float vitezaRotatie = 100f;
    public Rigidbody rb;
    private Vector3 teleportPOS;
    public GameObject prefabDeSpawn;
    private bool canJump = false;
    private Vector3 spawnPosition;
    private GameObject ultimulPlayer = null;
    #endregion

    #region Collision
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
    #endregion

    #region Init
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
    #endregion

    #region ServerSpawn
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
    #endregion

    #region Animation
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
    #endregion

    #region Handlers
    void Update()
    {
        Movement_Animation();
        if (Input.GetKeyDown(KeyCode.G)) SpawnPrefabServerRpc();
        Vector3 scale=transform.localScale;
        if (Input.GetKeyDown(KeyCode.O)) transform.localScale=new Vector3(scale.x+offset,scale.y+offset,scale.z+offset);
        if (Input.GetKeyDown(KeyCode.P)) transform.localScale = new Vector3(scale.x - offset, scale.y - offset, scale.z - offset);
    }

    void FixedUpdate()
    {
        anim.SetBool("isJumping", false);
    }
    #endregion

}
