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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Calc calculator;
        public MainWindow()
        {
            InitializeComponent();
            calculator = new Calc();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button pressedButton = (Button)sender;
            string buttonTag = pressedButton.Tag.ToString();
            calculator.PerformOperation(buttonTag);
            outputTextBox.Text = calculator.OutputResult;
            this.currentStateBlock.Text = calculator.CurrentState;
        }
    }
}
