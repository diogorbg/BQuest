using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

//wireshark: (ip.src == 178.79.166.11) || (ip.dst == 178.79.166.11)

public class WebSocket : MonoBehaviour {

	public string server = "178.79.166.11";
	public int port = 80;

	// Create a socket connection with the specified server and port.
	private Socket s;

	void Start () {
		StartCoroutine( SocketSendReceive() );
	}

	Socket ConnectSocket(string server, int port) {
		Socket s = null;
		IPHostEntry hostEntry = null;

		// Get host related information.
		hostEntry = Dns.GetHostEntry(server);

		// Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
		// an exception that occurs when the host IP Address is not compatible with the address family
		// (typical in the IPv6 case).
		foreach(IPAddress address in hostEntry.AddressList) {
			IPEndPoint ipe = new IPEndPoint(address, port);
			Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			tempSocket.Connect(ipe);

			if(tempSocket.Connected) {
				s = tempSocket;
				break;
			} else {
				continue;
			}
		}
		return s;
	}

	// This method requests the home page content for the specified server.
	IEnumerator SocketSendReceive() {
		string readers =
			"GET / HTTP/1.1\r\n" +
			"Host: 178.79.166.11\r\n" +
			"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0\r\n" +
			"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\n" +
			"Accept-Language: pt-BR,pt;q=0.8,en-US;q=0.5,en;q=0.3\r\n" +
			"Accept-Encoding: gzip, deflate\r\n" +
			"DNT: 1\r\n" +
			"Sec-WebSocket-Version: 13\r\n" +
			"Origin: http://browserquest.mozilla.org\r\n" +
			"Sec-WebSocket-Extensions: permessage-deflate\r\n" +
			"Sec-WebSocket-Key: 4Pmo3NqrazBXHrMXmCwStQ==\r\n" +
			"Connection: keep-alive, Upgrade\r\n" +
			"Pragma: no-cache\r\n" +
			"Cache-Control: no-cache\r\n" +
			"Upgrade: websocket\r\n\r\n";

		// Receive the server home page content.
		int bytes = 0;
		byte[] bytesSent = Encoding.ASCII.GetBytes(readers);

		s = ConnectSocket(server, port);
		yield return null;

		if (s == null) {
			print("Connection failed");
			yield break;
		}

		// Send request to the server.
		s.Send(bytesSent, bytesSent.Length, SocketFlags.None);
		yield return null;

		s.ReceiveTimeout = 1;
		while (true) {
			try {
				bytes = s.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
				onReceive(bytes);
			} catch (SocketException) {
			}
			yield return null;
		}
	}

	//--------------------------------------------------------------------------------------------------------------------

	enum Est {
		header,
		go,
		numero,
		palavra,
		texto,
		textoEscape,
	}

	int i, nivel = 0;
	char c3='\0', c2='\0', c1='\0', c='\0';
	byte[] bytesReceived = new byte[1024];
	Est est = Est.header;
	string texto;
	List<string> lista = new List<string>();

	void onReceive (int bytes) {
		print( Encoding.ASCII.GetString(bytesReceived, 0, bytes) );
		for (i=0; i<bytes; i++) {
			c3 = c2; c2 = c1; c1 = c;
			c = (char) bytesReceived[i];
			switch (est) {
			case Est.header: //------------------------------------------
				if (c3==0x81 && c2==0x02 && c1=='g' && c=='o') { //< parei aki
					est = Est.go;
					sendChar();
				}
				break;

			case Est.go: //----------------------------------------------
				if (c=='"') {
					est = Est.texto;
					texto = "";
				} else if ( char.IsDigit(c) ) {
					est = Est.numero;
					texto = ""+c;
				} else if ( char.IsLetter(c) ) {
					est = Est.palavra;
					texto = ""+c;
				} else if (c==']' || c=='[')
					closePilha();
				break;

			case Est.numero: //------------------------------------------
				if ( char.IsNumber(c) )
					texto += c;
				else {
					est = Est.go;
					lista.Add( texto );
					if (c==']' || c=='[')
						closePilha();
				}
				break;

			case Est.palavra: //-----------------------------------------
				if ( char.IsLetterOrDigit(c) )
					texto += c;
				else {
					est = Est.go;
					lista.Add( texto );
					if (c==']' || c=='[')
						closePilha();
				}
				break;

			case Est.texto: //-------------------------------------------
				if (c=='"') {
					est = Est.go;
					lista.Add( texto );
				} else if (c=='\\') {
					est = Est.textoEscape;
				} else
					texto += c;
				break;

			case Est.textoEscape: //-------------------------------------
				texto += c;
				est = Est.texto;
				break;
			}
		}
	}

	void closePilha () {
		if (lista.Count==0)
			return;
		nivel--;
		string saida = "";
		foreach (var item in lista) {
			saida += item + ":";
		}
		lista.Clear();
		print(saida);
	}

	void sendChar () {
		Byte[] hello = Encoding.ASCII.GetBytes( "\x81\x0f[0,\"RBG\",21,61]" );
		hello[0] = 0x81;
		s.Send(hello, hello.Length, SocketFlags.None);
	}

}
