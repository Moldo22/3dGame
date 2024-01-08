#region Libraries
using System.Collections;
using UnityEngine;
using Unity.Netcode;
#endregion

public class Grenade : NetworkBehaviour
{
    #region References
    [SerializeField] public ParticleSystem sistemParticule;
    [SerializeField] private AudioSource audioSource;
    #endregion

    #region Variables
    public float durataSistemParticule = 3f;
    private float timer = 7f;
    private bool sistemDejaActivat = false;
    #endregion

    #region Handlers
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ActivareSistemParticule();
        }
        else
        {
            float t = Mathf.PingPong(timer, 0.4f) / 1f;
            Color culoare = Color.Lerp(Color.red, Color.yellow, t);
            GetComponent<Renderer>().material.color = culoare;
        }
    }
    #endregion

    #region Functions
    void ActivareSistemParticule()
    {
        if (!sistemDejaActivat && sistemParticule != null)
        {
            AscundeObiect();
            sistemParticule.Play();
            audioSource.Play();
            Invoke("OpresteSunet", 2f);
            sistemDejaActivat = true;
            StartCoroutine(DistrugereDupaDurataSistem());
        }
    }

    void OpresteSunet()
    {
        audioSource.Stop();
    }

    void AscundeObiect()
    {
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
    }

    IEnumerator DistrugereDupaDurataSistem()
    {
        yield return new WaitForSeconds(durataSistemParticule);
        Destroy(gameObject);
    }
    #endregion
}
