using System.Collections.Generic;
using System.IO;
using System.Linq;
using PvcCore;

namespace PvcPlugins
{
	public class PvcConcat : PvcPlugin
	{
		private readonly string _resultName;

		public PvcConcat(string resultName)
		{
			_resultName = resultName;
		}

		public override IEnumerable<PvcStream> Execute(IEnumerable<PvcStream> inputStreams)
		{
			//Not using .Any() to avoid multiple enumeration and to avoid converting the enumerable to list. Using good old bool set check.
			var hasAnyStream = false;
				
			var copyToStream = new MemoryStream();

			foreach (var inputStream in inputStreams)
			{
				hasAnyStream = true;
				inputStream.Position = 0;
				inputStream.CopyTo(copyToStream);
			}
			if (!hasAnyStream)
			{
				return Enumerable.Empty<PvcStream>();
			}
			var returnStream = new PvcStream(() => copyToStream)
			{
				Position = 0
			};

			return new[]
			{
				returnStream.As(_resultName)
			};
		}
	}
}