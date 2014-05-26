using System.Collections.Generic;
using System.IO;
using System.Linq;
using Machine.Specifications;
using PvcCore;

namespace PvcConcat.Specs
{
	[Subject(typeof (PvcConcat), ".Execute")]
	internal class PvcConcatSpecs
	{
		public class When_the_input_streams_have_content
		{
			private static PvcConcat _pvcConcat;
			private static IEnumerable<PvcStream> _inputStreams;
			private static IEnumerable<PvcStream> _outputStreams;

			private Establish context = () =>
			{
				_pvcConcat = new PvcConcat("somefile.txt");
				_inputStreams = new[]
				{
					PvcUtil.StringToStream("File1Content", "File1", "C:\\File1.txt"),
					PvcUtil.StringToStream("File2Content", "File2", "D:\\File2.txt"),
				};
			};

			private Because of = () =>
				_outputStreams = _pvcConcat.Execute(_inputStreams);

			private It should_return_streams_with_one_entry = () =>
				_outputStreams.Count().ShouldEqual(1);

			private It should_merge_input_streams_to_one_stream = () =>
			{
				var output = _outputStreams.First();
				new StreamReader(output).ReadToEnd().ShouldEqual("File1ContentFile2Content");
			};
				
		}
	}
}