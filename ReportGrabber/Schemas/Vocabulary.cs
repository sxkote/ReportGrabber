using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGrabber.Schemas
{
    /// <summary>
    /// Vocabulary Item
    /// </summary>
    public class VocabularyItem
    {
        private string _text;
        private string _value;

        public string Text
        { get { return _text; } }

        public string Value
        { get { return _value; } }

        public VocabularyItem(string text, string value)
        {
            _text = text;
            _value = value;
        }
    }

    /// <summary>
    /// Vocabulary of replacements to be applied while grabbing
    /// </summary>
    public class Vocabulary
    {
        private List<VocabularyItem> _items;

        public IList<VocabularyItem> Items
        { get { return _items.AsReadOnly(); } }

        public Vocabulary(params VocabularyItem[] items)
        {
            _items = items == null ? new List<VocabularyItem>() : items.ToList();
        }

        public Vocabulary(IEnumerable<VocabularyItem> items)
        {
            _items = items == null ? new List<VocabularyItem>() : items.ToList();
        }
    }
}
