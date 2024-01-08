#region Libraries
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
#endregion

public class Multiplayer : MonoBehaviour
{
    #region References
    [SerializeField] private NetworkManager nm;
    [SerializeField] private UnityTransport ut;
    [SerializeField] public TextMeshProUGUI t;
    [SerializeField] public TextMeshProUGUI t2;
    [SerializeField] public InputField input;
    [SerializeField] private Button b1;
    [SerializeField] private Button b2;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Canvas cvs;
    #endregion
 
    #region IP
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return null;
    }
    #endregion

    #region Host&Client Connection
    public void CreateHost()
    {
        b1.gameObject.SetActive(false);
        b2.gameObject.SetActive(false);
        t2.gameObject.SetActive(false);
        input.gameObject.SetActive(false);
        Cursor.visible = false;
        t.text = "Host ip: " + GetLocalIPAddress();
        ut = nm.GetComponent<UnityTransport>();
        ut.ConnectionData.Address = GetLocalIPAddress();
        //NetworkManager.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(t.text);
        /* NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip);*/
        NetworkManager.Singleton.StartHost();    
                   
        
    }

    public void CreateClient()
    {

        b1.gameObject.SetActive(false);
        b2.gameObject.SetActive(false);
        t2.gameObject.SetActive(false);
        Cursor.visible = false;
        if (input!= null && !string.IsNullOrEmpty(input.text))
        {
            string textCitit = input.text;
            Debug.Log("Text citit: " + textCitit);
            ut = nm.GetComponent<UnityTransport>();
            ut.ConnectionData.Address = textCitit;
        }
        input.gameObject.SetActive(false);
        NetworkManager.Singleton.StartClient();
    }
    #endregion

    #region Handler
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) NetworkManager.Singleton.StartClient();
    }
    #endregion
}
