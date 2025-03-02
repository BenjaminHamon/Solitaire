using System;

namespace BenjaminHamon.Solitaire.UnityExtensions.Editor
{
	/// <summary>Attribute for a property to be exposed in the inspector.</summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class ExposePropertyAttribute : Attribute
	{ }
}
