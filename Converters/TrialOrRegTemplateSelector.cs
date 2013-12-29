using System.Windows;
using ScaleFinderWP7.Utility;


namespace ScaleFinderWP7.Converters
{
    public class TrialOrRegTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TrialScaleTemplate { get; set; }
        public DataTemplate FullScaleTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //http://www.windowsphonegeek.com/articles/Implementing-Windows-Phone-7-DataTemplateSelector-and-CustomDataTemplateSelector

            //String path = (string)item;
            //String ext = System.IO.Path.GetExtension(path);
            //if (System.IO.File.Exists(path) && ext == ".jpg")
            //    return ImageTemplate;
            //return StringTemplate;

            return null;
        }
    }
}