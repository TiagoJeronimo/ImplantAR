﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private Vector3 LastPosition;
    private Vector3 LastRelativePosition;
    private Quaternion LastRotation;
    public GameObject LoginText;
    public GameObject LoginCanvas;

    public void WorkOffline() {
        Debug.Log("work offline");
        LoginText.GetComponent<Text>().text = "Offline";
        LoginText.SetActive(true);
        LoginCanvas.SetActive(false);
    }

    public void ConnectToServer() {
        //if already connected ignore this function
        if (socketReady)
            return;

        //default host / port values
        string host = "127.0.0.1";
        int port = 6321;

        //Overwrite default host /port values if there is something in those boxes
        string h;
        int p;
        h = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (h != "")
            host = h;
        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
        if (p != 0)
            port = p;

        //create socket
        try {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e) {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void FixedUpdate() {
        if(socketReady) {
            LoginText.SetActive(true);
            LoginCanvas.SetActive(false);
            if (stream.DataAvailable) {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data); 
            }
            GameObject targetObject = GameObject.FindGameObjectWithTag("CImp");
            if (targetObject) {
                Transform targetTransform = targetObject.GetComponent<Transform>();

                /*Vector3 position = targetTransform.position; //Check the object tag if any error!!!!!

                if (LastPosition != position) {
                    LastPosition = position;
                    string message = position.ToString() + "1";
                    Send(message);
                }*/

                Vector3 relativePosition = FindRelativePosition.PositionRelativeToJaw; //Check the object tag if any error!!!!!

                if (LastRelativePosition != relativePosition) {
                    LastRelativePosition = relativePosition;
                    string message = relativePosition.ToString() + "1";
                    //Debug.Log("sentMessage: " + message);
                    Send(message);
                }

                Quaternion rotation = targetTransform.rotation;
                if (LastRotation != rotation) {
                    LastRotation = rotation;
                    string message = rotation.ToString() + "2";
                    Send(message);
                }
            }
        }
    }

    private void OnIncomingData(string data) {
        //recives data from server
        //Debug.Log("Server: " + data);
    }

    private void Send(string data) {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }

    private void CloseSocket() {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
    private void OnApplicationQuit() {
        CloseSocket();
    }
    private void OnDisable() {
        CloseSocket();
    }
}