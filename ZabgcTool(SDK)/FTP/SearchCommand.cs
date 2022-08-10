using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZabgcTool_SDK_.FTP
{
    public static class SearchCommand
    {
        static SearchCommand()
        {
            // Можно прописать горячие клавиши по умолчанию
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F, ModifierKeys.Control, "Ctrl+F"));

            SomeCommand = new RoutedUICommand("Some", "SomeCommand", typeof(SearchCommand), inputs);
        }

        public static RoutedCommand SomeCommand { get; private set; }
    }
}
