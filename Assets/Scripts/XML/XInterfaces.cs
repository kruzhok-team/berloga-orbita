

namespace XML
{
    /// <summary>
    /// Interface usage says that object have logic how to work with
    /// XML module and call needed functions by itself
    /// </summary>
    public interface IXHandler
    {
        public void CallBack();
    }

    /// <summary>
    /// Interface usage should guarantee that object encapsulates
    /// returning option, and can say about accessible of text
    /// </summary>
    public interface ITextGetter
    {
        public bool CanGetText();
        public string GetText();
    }
}
