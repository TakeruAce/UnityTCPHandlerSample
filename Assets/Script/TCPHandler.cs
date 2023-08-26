using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System;

public class TCPHandler : MonoBehaviour {

    public string ipAddress = "192.168.1.44";
    public int port = 8888;
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer;
    public float[] FloatMessage = {0};

    void Start () {
        client = new TcpClient(ipAddress, port);
        stream = client.GetStream();
        buffer = new byte[1024];
    }

    void Update () {
        if (stream.DataAvailable) {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Message received from ESP32: " + message);
            // SendMessageToESP("Message received: " + message);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            SendMessageToESP(FloatMessage);
        }
    }

    void SendMessageToESP(string message) {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Debug.Log("Message sent to ESP32: " + message);
    }

    void SendMessageToESP(float[] values) {
        byte[] data = new byte[values.Length * 4]; // 4 bytes per float
        for (int i = 0; i < values.Length; i++) {
            Debug.Log(values.Length);
            byte[] floatBytes = BitConverter.GetBytes(values[i]);
            for (int j = 0; j < 4; j++) {
                data[i * 4 + j] = floatBytes[j];
            }
        }
        Debug.Log(BitConverter.ToString(data));
        stream.Write(data, 0, data.Length);
        Debug.Log("Float array sent to ESP32");
    }


    void OnApplicationQuit() {
        stream.Close();
        client.Close();
    }
}

