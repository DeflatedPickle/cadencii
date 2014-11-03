using System;
using System.Collections;
using cadencii.java.awt;

namespace cadencii
{
	public interface RebarBandCollection : IList
	{
		//int NextID ();
		Rebar Rebar { get; }
		RebarBand this [int i] { get; }
		RebarBand this [string name] { get; }
	}
}
