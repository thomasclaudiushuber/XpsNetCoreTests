using NUnit.Framework;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace XpsDocumentWriterTests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class XpsDocumentWriterTests
    {
        string _fileName;

        [SetUp]
        public void Setup()
        {
            _fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xps");
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
        }

        [Test]
        public void ShouldCreateDocument()
        {
            CreateASimpleDocument();
            Assert.IsTrue(File.Exists(_fileName));
        }

        private void CreateASimpleDocument()
        {
            var fSeq = new FixedDocumentSequence();
            var fDoc = new FixedDocument();

            // Add DocumentReference with FixedDoc to FixedDocSeq
            var docRef = new DocumentReference();
            docRef.SetDocument(fDoc);
            fSeq.References.Add(docRef);

            // Create Fixed Page
            var fPage = new FixedPage
            {
                Width = 816,
                Height = 1056
            };

            var txt = new TextBlock
            {
                Text = "Hello XPS"
            };
            fPage.Children.Add(txt);

            // Add FixedPage to PageContent
            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fPage);

            // Add PageContent to FixedDoc
            fDoc.Pages.Add(pageContent);

            // Save as XPS
            if (File.Exists(_fileName))
                File.Delete(_fileName);

            XpsDocument doc = new XpsDocument(_fileName, FileAccess.Write);
            try
            {
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                writer.Write(fSeq);
            }
            finally
            {
                doc.Close();
            }
        }
    }
}
