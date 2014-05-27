using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Machine.Specifications;
using PvcCore;

// ReSharper disable CheckNamespace
namespace PvcPlugins.Concat.Specs
// ReSharper restore CheckNamespace
{
	[Subject(typeof(PvcConcat), ".Execute")]
	internal class PvcConcatSpecs
	{
		public class When_input_streams_are_empty
		{
			private static PvcConcat _pvcConcat;
			private static IEnumerable<PvcStream> _inputStreams;
			private static IEnumerable<PvcStream> _outputStreams;
			private static Exception _exception;

			private Establish context = () =>
			{
				_pvcConcat = new PvcConcat("somefile.txt");
				_inputStreams = Enumerable.Empty<PvcStream>();
			};

			private Because of = () =>
				_exception = Catch.Exception(() => _outputStreams = _pvcConcat.Execute(_inputStreams));

			private It should_not_throw = () =>
				_exception.ShouldBeNull();

			private It should_return_empty_output_streams = () =>
				_outputStreams.ShouldBeEmpty();
		}

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

			private It should_assign_stream_name = () =>
				_outputStreams.First().StreamName.ShouldEqual("somefile.txt");

			private It should_merge_input_streams_to_one_stream = () =>
			{
				var output = _outputStreams.First();
				new StreamReader(output).ReadToEnd().ShouldEqual("File1ContentFile2Content");
			};
		}
	}
}