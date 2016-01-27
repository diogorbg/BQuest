using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Sockets;

//wireshark: (ip.src == 178.79.166.11) || (ip.dst == 178.79.166.11)

public class Conectar : MonoBehaviour {

	//NetworkStream stream = new NetworkStream(

	public Text log;

	private string url = "http://178.79.166.11";

	void Start () {
		print("start 0");
		StartCoroutine( conectar() );
		print("start 1");
	}

	void Update () {
		//print("update..");
	}

	IEnumerator conectar () {
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0");
		headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
		headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.5,en;q=0.3");
		headers.Add("Accept-Encoding", "gzip, deflate");
		headers.Add("DNT", "1");
		headers.Add("Sec-WebSocket-Version", "13");
		headers.Add("Origin", "http://browserquest.mozilla.org");
		headers.Add("Sec-WebSocket-Extensions", "permessage-deflate");
		headers.Add("Sec-WebSocket-Key", "4Pmo3NqrazBXHrMXmCwStQ==");
		headers.Add("Connection", "keep-alive, Upgrade");
		headers.Add("Pragma", "no-cache");
		headers.Add("Cache-Control", "no-cache");
		headers.Add("Upgrade", "websocket");
		WWW www = new WWW(url, null, headers);
		print("sim 0");
		yield return null;
		print("sim 1");
		while (!www.isDone) {
			print("* notDone");
			yield return null;
			log.text = getString(www.bytes);
		}
		log.text = www.text;

		print("sim 2");
		www = new WWW(url, getBytes("[0,\"RBG\",21,61]"));
		yield return www;
		print("sim 3");
		log.text = www.text;
	}

	static byte[] getBytes(string str) {
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	static string getString(byte[] bytes) {
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

}
