using NSFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TCPData tcpData = new TCPData();
        tcpData.IP = "61.151.186.113";
        tcpData.Port = 8080;

        NetService.StartTCP(tcpData);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        NetService.StopTCP();
    }
}
