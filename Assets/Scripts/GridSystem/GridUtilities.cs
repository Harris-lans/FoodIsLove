using System;

[System.Serializable]
public struct GridPosition
{
	public byte X;
	public byte Z; 

	public GridPosition(byte x, byte z)
	{
		X = x;
		Z = z;
	}

	public static bool operator== (GridPosition a, GridPosition b)
	{
		return (a.X == b.X) && (a.Z == b.Z);
	}

	public static bool operator!= (GridPosition a, GridPosition b)
	{
		return (a.X != b.X) || (a.Z != b.Z);
	}

	public override bool Equals(object obj)
	{	
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		
		return base.Equals (obj);
	}
	
	// override object.GetHashCode
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
