using System;
using System.Collections.Generic;
using System.Text;
namespace Lib.Utils
{
	public class NumberUtil
	{
		public static byte StandardMtLen = 160;
		public static byte StandardMtSplitLen = 153;
		//------------------------------------------------------------
		public static string RomanToLatin(string Digist)
		{
			string RetVal="";
			if (!string.IsNullOrEmpty(Digist))
			{
				Digist=Digist.ToUpper();
				switch (Digist)
				{
					case "I":
						{
							RetVal = "1";
							break;
						}
					case "II":
						{
							RetVal = "2";
							break;
						}
					case "III":
						{
							RetVal = "3";
							break;
						}
					case "IV":
						{
							RetVal = "4";
							break;
						}
					case "V":
						{
							RetVal = "5";
							break;
						}
					case "VI":
						{
							RetVal = "6";
							break;
						}
					case "VII":
						{
							RetVal = "7";
							break;
						}
					case "VIII":
						{
							RetVal = "8";
							break;
						}
					case "IX":
						{
							RetVal = "9";
							break;
						}
					case "X":
						{
							RetVal = "10";
							break;
						}
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------
		public static int Max(int Num1, int Num2)
		{
			return ((Num1 > Num2) ? Num1 : Num2); 
		}
		//---------------------------------------------------------------------------------------
		public static string IntToStr(int Num, byte Len)
		{
			string RetVal = Num.ToString();
			if (Len > RetVal.Length)
			{
				RetVal = Num.ToString("D" + Len);	
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------
		public static int CalculateSumMt(byte MsgTypeId, string MessageOut)
		{
			int RetVal = 1;
			int LenMessageOut;
			if (MsgTypeId == 0)
			{
				if (string.IsNullOrEmpty(MessageOut))
				{
					RetVal = 0;
				}
				else
				{
					LenMessageOut = MessageOut.Length;
					if (LenMessageOut > StandardMtLen)
					{
						RetVal = LenMessageOut / StandardMtSplitLen;
					}
					else
					{
						RetVal = 1;
					}
					if ((LenMessageOut % StandardMtSplitLen) > 0)
					{
						RetVal += 1;
					}
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------        
		public static int getRandom(int sides)
		{
			Random r = new Random();
			int i = 0;
			while (i == 0)
			{
				i = r.Next(sides + 1);
			}
			return i;
		}
		//---------------------------------------------------------------------------------------        
		public static int Static_GetRandom(int sides)
		{
			Random r = new Random();
			return r.Next(sides + 1);
		}
		//---------------------------------------------------------------------------------------
		public static List<Numbers> NumberToList(int NumTo)
		{
			int NumFrom = 0;
			return NumberToList(NumFrom, NumTo);
		}
		//---------------------------------------------------------------------------------------
		public static List<Numbers> NumberToList(int NumFrom, int NumTo, int Step)
		{
			List<Numbers> RetVal = new List<Numbers>();
			if (NumFrom == NumTo)
			{
				RetVal.Add(new Numbers(NumFrom));
			}
			else
			{
				if (Step == 0)
				{
					RetVal.Add(new Numbers(NumFrom));
					RetVal.Add(new Numbers(NumTo));
				}
				else
				{
					if (Step > 0)
					{
						if (NumFrom > NumTo)
						{
							while (NumTo <= NumFrom)
							{
								RetVal.Add(new Numbers(NumTo));
								NumTo += Step;
							}
						}
						else
						{
							while (NumFrom <= NumTo)
							{
								RetVal.Add(new Numbers(NumFrom));
								NumFrom += Step;
							}
						}
					}
					else
					{
						if (NumFrom > NumTo)
						{
							while (NumFrom <= NumTo)
							{
								RetVal.Add(new Numbers(NumFrom));
								NumFrom += Step;
							}
						}
						else
						{
							while (NumTo <= NumFrom)
							{
								RetVal.Add(new Numbers(NumTo));
								NumTo += Step;
							}
						}
					}
				}
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------
		public static List<Numbers> NumberToList(int NumFrom, int NumTo)
		{
			int Step = 1;
			return NumberToList(NumFrom,NumTo,Step);
		}
			
		//---------------------------------------------------------------------------------------
		public static List<Numbers> NumberToListEx(int NumFrom, int NumTo)
		{
			List<Numbers> retVal = new List<Numbers>();
			if (NumTo >= NumFrom)
			{
				for (int i = NumFrom; i < NumTo; i++)
				{
					if (i % 2 == 0)
					{
						retVal.Add(new Numbers(i));
					}
					else
					{
						retVal.Insert(0, new Numbers(i));
					}
				}
			}
			return retVal;
		}
		//---------------------------------------------------------------------------------------
		public static int RandNumber(int From, int To)
		{
			int RetVal = 0;
			try
			{
					Random rnd = new Random();
					RetVal = rnd.Next(From, To);
			}
			catch
			{
				RetVal = From;
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------
		public static Numbers RandNumber(List<Numbers> l_Numbers)
		{
			Numbers RetVal = new Numbers();
			if (l_Numbers.Count > 1)
			{
				int RandIndex = RandNumber(1, l_Numbers.Count - 1);
				RetVal = l_Numbers[RandIndex];
			}
			return RetVal;
		}
		//---------------------------------------------------------------------------------------
		public static List<Numbers> Reverse(List<Numbers> l_Numbers)
		{
			List<Numbers> retVal = new List<Numbers>();
			foreach(Numbers mNumbers in l_Numbers)
			{
				retVal.Insert(0, mNumbers);
			}
			return retVal;
		}
		//---------------------------------------------------------------------------------------
		public static bool IsMember(int Value, int[] IntVector)
		{
			bool retVal = false;
			if (IntVector!=null)
			{
				for (int i = 0; i < IntVector.Length; i++)
				{
					if (Value == IntVector[i])
					{
						retVal = true;
						break;
					}
				}
			}
			return retVal;
		}
	}
}
