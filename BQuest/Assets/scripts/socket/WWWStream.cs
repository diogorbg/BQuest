using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

// Sockets.stream: http://answers.unity3d.com/questions/605087/streaming-audio-via-socket.html

public class WWWStream {/*

	private Socket socket;
	private NetworkStream stream;

	public WWWStream (string url, List<string> headers) {
		//socket = new Socket(
		//stream = new NetworkStream(socket, true);
		//conectar(url);
	}

	void update () {
		tmp = new float[6400 / 4]; 
		stream.Read (bytes, 0, 6400);
		Buffer.BlockCopy (bytes, 0, tmp, 0, 6400);
		if (!playing) {
			audio.loop = true;
			audio.clip = AudioClip.Create ("Stream", 16000, 1, 16000, false, true, onReaderCallback);

			audio.Play ();
			playing = true;
		}
	}

	void conectar (string url) {
		Debug.Log ("Connecting...");
		socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		socket.Connect (url, 80);
		Debug.Log ("* Connected");
		stream = new NetworkStream (socket);
	}

	void onReaderCallback (float[]data) {
		Debug.Log (data.Length+" "+ tmp.Length);
		for (var i=0; i<tmp.Length; i++) {
			data [i] = tmp [i];
		}
	}*/

}
