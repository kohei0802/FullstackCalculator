using System.Data;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClassLibrary1;

namespace SpecificLayout
{
    public partial class MainWindow : Window
    {
        //constant
        const string STRINGFORMATTER = "0.################";
        //int histoutcunt = 0;

        //core variables
        public CalculatorHelper helper = new CalculatorHelper();

        public MainWindow()
        {
            InitializeComponent();
            ResultText.Text = "0";
            ExpressionText.Text = "";

            RefreshHistoryList();
        }
        //public int HistoryCount { get { return histoutcunt; } , set{histoutcunt=value}}



        /*
         * HandleEventX only manipulate currentExpression and machineState. But no other operations. 
         * Optionally save results
         */

        private void RefreshHistoryList()
        {
            HistoryList.Items.Clear();
            List<MathExpression> historyList = helper.GetHistoryList();
            for (int i = 0; i < historyList.Count; i++)
            {
                var history = historyList[historyList.Count - 1 - i];

                Grid grid = new Grid();
                grid.Height = 60;
                RowDefinition row1 = new RowDefinition();
                row1.Height = new GridLength(20);
                RowDefinition row2 = new RowDefinition();
                row2.Height = new GridLength(40);
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                //<TextBox Name="ExpressionText" IsReadOnly="True" Background="Bisque" Padding="1,1,1,1" HorizontalContentAlignment="Right" VerticalContentAlignment="Center" Text="" FontFamily="Verdana" FontWeight="Bold" Grid.Row="0"/>
                TextBox text1 = new TextBox();
                text1.HorizontalAlignment = HorizontalAlignment.Right;
                text1.Text = $"{history.Left?.ToString(STRINGFORMATTER)}{history.Operation}{history.Right?.ToString(STRINGFORMATTER)}=";
                text1.Background = new SolidColorBrush(Color.FromRgb(255, 228, 196));
                text1.BorderBrush = Brushes.Transparent;
                text1.IsReadOnly = true;
                Grid.SetRow(text1, 0);

                TextBox text2 = new TextBox();
                text2.HorizontalAlignment = HorizontalAlignment.Right;
                text2.Text = history.Result?.ToString(STRINGFORMATTER);
                text2.Background = new SolidColorBrush(Color.FromRgb(255, 228, 196));
                text2.BorderBrush = Brushes.Transparent;
                text2.IsReadOnly = true;
                Grid.SetRow(text2, 1);

                grid.Children.Add(text1);
                grid.Children.Add(text2);

                HistoryList.Items.Add(grid);

            }
        }

        private void DisableCalculator(Exception ex)
        {
            ExpressionText.Text = "";
            ResultText.Text = ex.Message;
            helper.SetInputBuffer("");
            helper.SetCurrentExpression(0, null, null, null);
            PlusButton.IsEnabled = false;
            MinusButton.IsEnabled = false;
            MultiplyButton.IsEnabled = false;
            DivideButton.IsEnabled = false;
            EqualButton.IsEnabled = false;
        }

        private void ReEnableCalculator()
        {
            PlusButton.IsEnabled = true;
            MinusButton.IsEnabled = true;
            MultiplyButton.IsEnabled = true;
            DivideButton.IsEnabled = true;
            EqualButton.IsEnabled = true;
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            ReEnableCalculator();
            if (sender is Button button)
            {
                string input = button.Content.ToString();

                ResultText.Text = helper.HandleNumber(input);
            }

            RefreshHistoryList();
        }



        private async void Operation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button
                    && await helper.HandleOperation(button.Content.ToString()) is MathExpression expressionToPrint)
                {
                    ExpressionText.Text = $"{expressionToPrint.Left?.ToString(STRINGFORMATTER)}{expressionToPrint.Operation}{expressionToPrint.Right?.ToString(STRINGFORMATTER)}";
                    if (expressionToPrint.Result != null)
                    {
                        ResultText.Text = expressionToPrint.Result?.ToString(STRINGFORMATTER);
                    }

                }
            }
            catch (Exception ex)
            {
                DisableCalculator(ex);
            }
            finally
            {
                RefreshHistoryList();
            }
        }

        private async void Equal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && await helper.HandleEqual() is MathExpression expressionToPrint)
                {
                    ExpressionText.Text = $"{expressionToPrint.Left?.ToString(STRINGFORMATTER)}{expressionToPrint.Operation}{expressionToPrint.Right?.ToString(STRINGFORMATTER)}=";
                    ResultText.Text = expressionToPrint.Result?.ToString(STRINGFORMATTER);

                }
            }
            catch (Exception ex)
            {
                DisableCalculator(ex);
            }
            finally
            {
                RefreshHistoryList();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ReEnableCalculator();
            if (sender is Button button)
            {
                helper.HandleClear();
                ResultText.Text = "0";
                ExpressionText.Text = "";
            }

        }

        private void DeleteHistory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                helper.HandleDeleteHistory();
                RefreshHistoryList();
            }
        }

        private void ImportHistory_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                helper.HandleImportHistory();
                RefreshHistoryList();
            }
        }

        private void ExportHistory_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                helper.HandleExportHistory();
                RefreshHistoryList();
            }
        }

        /*
        private void SQLSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                helper.HandleSQLSaveAll();
                RefreshAllDisplay();
            }
        }*/

        private async void ClearDataSource_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                await helper.HandleDataSourceClear();
                RefreshHistoryList();
            }
        }

        private async void ReadDataSource_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                await helper.HandleDataSourceRead();
                RefreshHistoryList();
            }
        }
    }


}