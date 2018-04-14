using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System;
public class WebService : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//调用方法
		StartCoroutine (IESendImgToServer("json参数1","参数1","你的webservice地址","参数1"));
	}
	//接收到的数据之后的回调方法
	public System.Action<string> getXmlCallBackEvent;
	void GetXmlCallBack(string _xml){

		if (getXmlCallBackEvent!=null) {
			getXmlCallBackEvent (_xml);
		}

	}
	//发送数据到服务器
	IEnumerator IESendImgToServer(string jsonStr ,string jkID,string url ,string jksqm  )
	{

		StringBuilder soap = new StringBuilder();
		  
		//25行的action是方法名
		soap.Append ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		soap.Append ("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
		soap.Append ("<soap:Body>"); 
		soap.Append("<ns2:action xmlns:ns2=\"http://impl.cxf.hstt.com/\">");//方法名，命名空间
		soap.Append("<arg0>" + jksqm + "</arg0>");
		soap.Append("<arg1>" + jkID + "</arg1>"); 
		soap.Append ("<arg2>" + jsonStr + "</arg2>");
		soap.Append("</ns2:action>");
		soap.Append("</soap:Body>");
		soap.Append("</soap:Envelope>");

		string _url = url;
		//		if (ServerInfo.instance.URL!=null) {
		//			url = ServerInfo.instance.URL;
		//		} 
		if (url==string.Empty) 
		{
			_url = "你的webservice地址"; 
		}
		WWW w = new WWW(_url, Encoding.UTF8.GetBytes(soap.ToString()));  

		yield return w;
		if (w.isDone) {
			if (w.error != null) 
			{
				Debug.Log (w.error );
			} 
			else
			{
				Debug.Log (w.text);
				string xmlMsg= ParsingXml (w.text); 
				GetXmlCallBack (xmlMsg); 
			}

		} 
	} 

	/// <summary>
	/// Parsings the xml.
	/// </summary>
	/// <param name="_xml">Xml.</param>
	/// <param name="_type">Type.</param>
	private string  ParsingXml(string _xml)
	{ 
		XmlDocument xmlDoc = new XmlDocument();
		try 
		{
			xmlDoc.LoadXml(_xml);
		} 
		catch (Exception ex) 
		{ 
			Debug.Log (ex.Data); 
		}


		XmlNode arrOfStr = xmlDoc.DocumentElement;
		XmlNodeList childNode = null;


		#region SOAP

		xmlDoc.LoadXml(arrOfStr.InnerXml);
		arrOfStr = xmlDoc.DocumentElement;
		xmlDoc.LoadXml(arrOfStr.InnerXml);
		arrOfStr = xmlDoc.DocumentElement;
		xmlDoc.LoadXml(arrOfStr.InnerXml);
		arrOfStr = xmlDoc.DocumentElement; 
		childNode = arrOfStr.ChildNodes;

		#endregion
 
		return arrOfStr.InnerXml;
	 

	}

}
