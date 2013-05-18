using System;

namespace java.lang.emul
{
	public class Number
	{

		protected bool IsComplexNumber(){
			return JCoeficient != 0;
		}

		public String ToComplexNumberString(){
			return RealPart.ToString () + "+j" + JCoeficient.ToString ();
		}

		public double RealPart {get;set;}
		public double JCoeficient { get; set; }
	}

}

