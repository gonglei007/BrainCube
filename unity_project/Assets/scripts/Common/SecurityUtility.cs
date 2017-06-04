using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class SecurityUtility {
	private static SecurityUtility instance;
	private static string DES_KEY = "GOTECH78";
	private static string DES_IV = "WUJICIKE";
	private int offset;
	
	public static SecurityUtility GetInstance()
	{
		if (instance == null)
		{
			instance = new SecurityUtility();
			instance.offset = Random.Range(0, 100000);
		}
		return instance;
	}
	
	public int EncryptInt(int source)
	{
		return source ^ offset;
	}
	
	public int DecryptInt(int result)
	{
		return result ^ offset;
	}
	
	public float EncryptFloat(float source)
	{
		return source + offset;
	}
	
	public float DecryptFloat(float result)
	{
		return result - offset;
	}
	
	public double EncryptDouble(double source)
	{
		return source + offset;
	}
	
	public double DecryptDouble(double result)
	{
		return result - offset;
	}
	
	public string EncryptString(string stringToEncrypt)
	{
		using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
        {
			byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
	        des.Key = Encoding.UTF8.GetBytes(DES_KEY);
	        des.IV = Encoding.UTF8.GetBytes(DES_IV);
	        MemoryStream ms = new MemoryStream();
	        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
	        {
	            cs.Write(inputByteArray, 0, inputByteArray.Length);
	            cs.FlushFinalBlock();
	            cs.Close();
	        }
	        string str = System.Convert.ToBase64String(ms.ToArray());
	        ms.Close();
			return str;
		}
	}
	
	public string DecryptString(string stringToDecrypt)
	{
		using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
        {
			byte[] inputByteArray = System.Convert.FromBase64String(stringToDecrypt);
	        des.Key = Encoding.UTF8.GetBytes(DES_KEY);
	        des.IV = Encoding.UTF8.GetBytes(DES_IV);
	        MemoryStream ms = new MemoryStream();
	        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
	        {
	            cs.Write(inputByteArray, 0, inputByteArray.Length);
	            cs.FlushFinalBlock();
	            cs.Close();
	        }
	        string str = Encoding.UTF8.GetString(ms.ToArray());
	        ms.Close();
			return str;
		}
	}
}
