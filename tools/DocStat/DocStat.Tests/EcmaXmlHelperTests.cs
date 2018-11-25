using NUnit.Framework;
using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using DocStat;

namespace DocStat.Tests
{
	[TestFixture]
	public class EcmaXmlHelperTests
	{
		[Test]
		public void NewElementsYieldsAddedElements()
		{
			XDocument xmlNew = XDocument.Load("TestData/currentxml/t1.xml");
			XDocument xmlOld = XDocument.Load("TestData/oldxml/t1.xml");

			IEnumerable<XElement> newMembers = EcmaXmlHelper.NewMembers(xmlNew, xmlOld);

			XElement e1 =
				xmlNew.Element("Type").Element("Members").Elements()
					  .FirstOrDefault((XElement arg) => arg.Attribute("MemberName").Value == "WeakDelegate");

			XElement e2 =
				xmlNew.Element("Type").Element("Members").Elements()
					  .FirstOrDefault((XElement arg) => arg.Attribute("MemberName").Value == "WeakDataSource");

			Assert.AreEqual(2, newMembers.Count());
			Assert.True(newMembers.Contains(e1));
			Assert.True(newMembers.Contains(e2));
		}

		[Test]
		public void MembersReturnsMembers()
		{
			XDocument xmlOld = XDocument.Load("TestData/oldxml/t1.xml");
			IEnumerable<XElement> members = EcmaXmlHelper.Members(xmlOld);
			Assert.AreEqual(13, members.Count());

			members = EcmaXmlHelper.Members(XDocument.Load("TestData/currentxml/t1.xml"));

			Assert.AreEqual(15, members.Count());
		}

        [Test]
        public void MembersHandlesEmptyMembersList()
        {
            XDocument x = XDocument.Load("TestData/currentxml/AVAssetImageGeneratorCompletionHandler.xml");


                IEnumerable<XElement> elements = EcmaXmlHelper.Members(x);
                Assert.IsEmpty(elements);

        }

         
        [Test]
		public void NewMembersHandlesEmptyMemberList()
		{
			XDocument o = XDocument.Load("TestData/oldxml/AVAssetImageGeneratorCompletionHandler.xml");
			XDocument n = XDocument.Load("TestData/currentxml/AVAssetImageGeneratorCompletionHandler.xml");

            Assert.IsEmpty(EcmaXmlHelper.NewMembers(n, o));
		}
	}
}
