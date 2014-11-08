using System;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class OtherItemsModel
		{
			readonly FormMainModel parent;

			public OtherItemsModel (FormMainModel parent)
			{
				this.parent = parent;
			}
		}
	}
}
