// (c) Vasian Cepa 2005
// Version 2

using System.Collections;

// required for NumericComparer : IComparer only

namespace _Scripts.SharedOverall.Utility
{

	public class NumericComparer : IComparer
	{
		public NumericComparer()
		{}
		
		public int Compare(object x, object y)
		{
			if((x is string) && (y is string))
			{
				return StringLogicalComparer.Compare((string)x, (string)y);
			}
			return -1;
		}
	}//EOC

}