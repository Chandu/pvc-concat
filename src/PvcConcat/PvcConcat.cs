using System.Collections.Generic;
using System.IO;
using System.Text;
using PvcCore;
using PvcPlugins;

namespace PvcConcat
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
			var streamContent = new StringBuilder();
			foreach (var inputStream in inputStreams)
			{
				streamContent.Append(inputStream.ToString());
			}
			var finalStream = PvcUtil.StringToStream(streamContent.ToString(), _resultName);
			finalStream.Position = 0;
			return new[]
			{
				finalStream
			};

		}
	}
}