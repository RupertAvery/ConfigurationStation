using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ConfigurationStation.WPF.Controls
{
    public class ScrollingTextBox : TextBox
    {

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            IsReadOnly = true;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            CaretIndex = Text.Length;
            ScrollToEnd();
        }

    }
}
