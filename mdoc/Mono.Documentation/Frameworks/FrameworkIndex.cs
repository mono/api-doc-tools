using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Mono.Documentation
{
	public class FrameworkTypeEntry
	{
		List<string> members = new List<string> ();

		public string Name { get; set; }

		public IList<string> Members
		{
			get
			{
				return this.members;
			}
		}

		public void ProcessMember (IEnumerable<string> signatures) {

		}
	}

	public class FrameworkEntry
	{
		List<FrameworkTypeEntry> types = new List<FrameworkTypeEntry> ();

		public string Name { get; set; }

		public IList<FrameworkTypeEntry> Types {
			get {
				return this.types;
			}
		}

		public FrameworkTypeEntry ProcessType (TypeDefinition type)
		{
			throw new NotImplementedException ();
		}

}

	public class FrameworkIndex
	{
		List<FrameworkEntry> frameworks = new List<FrameworkEntry> ();

		public FrameworkIndex ()
		{
		}

		public IList<FrameworkEntry> Frameworks {
			get {
				return this.frameworks;
			}
		}

		public FrameworkEntry StartProcessingAssembly (AssemblyDefinition assembly)
		{
			throw new NotImplementedException ();
		}

		public void Write ()
		{
			throw new NotImplementedException ();
		}

}
}
