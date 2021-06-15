using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GCore.StringHandling.Templating {
    /// <summary>
    /// Template Tag and Value
    /// </summary>
    public class TemplateTag {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="TemplateTag"/> instance.
        /// </summary>
        public TemplateTag() {
            _tag = string.Empty;
            _value = string.Empty;
        }

        /// <summary>
        /// Creates a new <see cref="TemplateTag"/> instance.
        /// </summary>
        /// <param name="Tag">Tag.</param>
        /// <param name="Value">Value.</param>
        public TemplateTag(string Tag, string Value) {
            _tag = "[%" + Tag + "%]";
            _value = Value;
        }
        #endregion
        #region Property - Tag
        public event EventHandler TagChanged;
        protected virtual void OnTagChanged() {
            if (TagChanged != null)
                TagChanged(this, EventArgs.Empty);
        }

        private string _tag;
        public string Tag {
            get { return _tag; }
            set {
                if (_tag != value) {
                    _tag = value;
                    OnTagChanged();
                }
            }
        }
        #endregion
        #region Property - Value
        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged() {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        private string _value;
        public string Value {
            get { return _value; }
            set {
                if (_value != value) {
                    _value = value;
                    OnValueChanged();
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Summary description for TemplateParser.
    /// </summary>
    public class TemplateParser {
        public TemplateParser() { }

        #region Template Tags - Adding, Updating, Removeing and Clearing Tags

        /// <summary>
        /// Adds the tag using a TemplateTag object.
        /// </summary>
        /// <param name="templateTag">Template tag.</param>
        public void AddTag(TemplateTag templateTag) {
            _templateTags[templateTag.Tag] = templateTag;
        }

        /// <summary>
        /// Adds the tag with empty string value.
        /// </summary>
        /// <param name="Tag">Tag.</param>
        public void AddTag(string Tag) {
            AddTag(new TemplateTag(Tag, string.Empty));
        }

        /// <summary>
        /// Adds the tag and the tag value.
        /// </summary>
        /// <param name="Tag">Tag.</param>
        /// <param name="Value">Value.</param>
        public void AddTag(string Tag, string Value) {
            AddTag(new TemplateTag(Tag, Value));
        }

        /// <summary>
        /// Removes the tag.
        /// </summary>
        /// <param name="Tag">Tag.</param>
        public void RemoveTag(string Tag) {
            _templateTags.Remove(Tag);
        }

        /// <summary>
        /// Clears the tags.
        /// </summary>
        public void ClearTags() {
            _templateTags.Clear();
        }
        #endregion

        #region Template Parsers

        /// <summary>
        /// Replaces the token Tag with the token Value.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns></returns>
        private string _replaceTagHandler(Match token) {
            if (_templateTags.Contains(token.Value))
                return ((TemplateTag)_templateTags[token.Value]).Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// Parses the template string.
        /// </summary>
        /// <param name="Template">Template.</param>
        /// <returns></returns>
        public string ParseTemplateString(string Template) {
            MatchEvaluator replaceCallback = new MatchEvaluator(_replaceTagHandler);
            string newString = Regex.Replace(Template, _matchPattern, replaceCallback);

            return newString;
        }

        /// <summary>
        /// Parses the template file.
        /// </summary>
        /// <param name="TemplateFilename">Template filename.</param>
        /// <returns></returns>
        public string ParseTemplateFile(string TemplateFilename) {
            string fileBuffer = _fileToBuffer(TemplateFilename);
            return ParseTemplateString(fileBuffer);
        }
        #endregion

        #region Find all Template Tags
        /// <summary>
        /// Finds the tags in template string.
        /// </summary>
        /// <param name="Template">Template.</param>
        public void FindTagsInTemplateString(string Template) {
            MatchCollection tags = Regex.Matches(Template, _matchPattern);

            foreach (Match tag in tags) AddTag(tag.ToString());
        }

        /// <summary>
        /// Finds the tags in template file.
        /// </summary>
        /// <param name="TemplateFilename">Template filename.</param>
        public void FindTagsInTemplateFile(string TemplateFilename) {
            string fileBuffer = _fileToBuffer(TemplateFilename);
            FindTagsInTemplateString(fileBuffer);
        }
        #endregion

        #region Read File into FileBuffer
        private string _fileToBuffer(string Filename) {
            if (!File.Exists(Filename)) throw new ArgumentNullException(Filename, "Template file does not exist");

            StreamReader reader = new StreamReader(Filename);
            string fileBuffer = reader.ReadToEnd();
            reader.Close();

            return fileBuffer;
        }
        #endregion

        #region Property - MatchPattern
        private string _matchPattern = @"(\[%\w+%\])";
        public string MatchPattern {
            get { return _matchPattern; }
            set { _matchPattern = value; }
        }
        #endregion
        #region Property - TemplateTags
        private Hashtable _templateTags = new Hashtable();
        public Hashtable TemplateTags {
            get { return _templateTags; }
        }
        #endregion
    }
}
