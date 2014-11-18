
namespace PariSharp
{
	#region Header
	/// <summary>
	/// Specifies a formatting style to be used when converting <see cref="PariObject"/>s to <c>string</c>
	/// representations.
	/// </summary>
	#endregion
	public enum TextStyle: int
	{
		//Values assigned here must correspond exactly to the values used by libpari.
		
		Raw = 0,
		PrettyMatrix = 1,
		Pretty = 3,
		Tex = 4
	}
}
