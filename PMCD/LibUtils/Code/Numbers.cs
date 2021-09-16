using System;
using System.Collections.Generic;
using System.Text;
namespace Lib.Utils
{
//Type 	Range 	Size
//sbyte 	-128 to 127 	Signed 8-bit integer
//byte 	0 to 255 	Unsigned 8-bit integer
//char 	U+0000 to U+ffff 	Unicode 16-bit character
//short 	-32,768 to 32,767 	Signed 16-bit integer
//ushort 	0 to 65,535 	Unsigned 16-bit integer
//int 	-2,147,483,648 to 2,147,483,647 	Signed 32-bit integer
//uint 	0 to 4,294,967,295 	Unsigned 32-bit integer
//long 	-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807 	Signed 64-bit integer
//ulong 	0 to 18,446,744,073,709,551,615 	Unsigned 64-bit integer
	public class Numbers
	{
		private int _NumberValue;
		private string _NumberSpell;
		//-----------------------------------------------------------------------
		public Numbers()
		{
		}
		//-----------------------------------------------------------------------
		public Numbers(int Num, string Spell)
		{
			NumberValue = Num;
			NumberSpell = Spell;
		}
		//-----------------------------------------------------------------------
		public Numbers(int Num)
		{
			NumberValue = Num;
			NumberSpell = Num.ToString();
		}
		//-----------------------------------------------------------------------
		~Numbers()
		{
		}
		//-----------------------------------------------------------------------
		public virtual void Dispose()
		{
		}
		//-----------------------------------------------------------------------
		public int NumberValue { get { return _NumberValue; } set { _NumberValue = value; } }
		public string NumberSpell { get { return _NumberSpell; } set { _NumberSpell = value; } }//---------------------------------------------------------------------------------------
		public List<Numbers> NumberToList(int NumTo)
		{
			int NumFrom = 0;
			return NumberToList(NumFrom, NumTo);
		}
		//---------------------------------------------------------------------------------------
		public List<Numbers> NumberToList(int NumFrom, int NumTo)
		{
			List<Numbers> RetVal = new List<Numbers>();
			if (NumTo >= NumFrom)
			{
				NumTo = NumTo + 1;
				for (int i = NumFrom; i < NumTo; i++)
				{
					RetVal.Add(new Numbers(i));
				}
			}
			return RetVal;
		}
	}
}
