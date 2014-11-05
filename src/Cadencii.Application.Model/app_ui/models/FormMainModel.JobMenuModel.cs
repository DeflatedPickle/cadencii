using System;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class JobMenuModel
		{
			readonly FormMainModel parent;

			public JobMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}
		}
	}
}
