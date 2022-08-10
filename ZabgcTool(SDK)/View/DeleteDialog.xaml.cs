using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZabgcTool_SDK_.View
{

    public partial class DeleteDialog : Window
    {
        readonly Action _deleteAction;
        public DeleteDialog(Action DeleteAction)
        {
            InitializeComponent();

            MouseLeftButtonDown += (s, e) =>
             {
                 DragMove();
             };
            Yes.Click += (s,e) => {

                DeleteAction();
                Close();
            };
            No.Click += (s, e) =>
            {
                Close();
            };
            butn.Click += (s, e) =>
            {
                Close();
            };
        }


    }
}
