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

namespace Smy.Vst.UI
{
  /// <summary>
  /// Interaction logic for Wave.xaml
  /// </summary>
  public partial class Wave : UserControl
  {
    public Wave()
    {
      InitializeComponent();
      this.SizeChanged += Wave_SizeChanged;
      this.DataContextChanged += Wave_DataContextChanged;
    }

    private void Wave_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.UpdateGeometry();
    }

    private void UpdateGeometry()
    {
      if(this.ActualHeight <= 0  || this.ActualWidth <= 0) 
        return;

      if (this.viewModel == null)
        return;

      Line.Points = new PointCollection(this.viewModel.Select(p => new Point(p.X * this.ActualWidth, p.Y * this.ActualHeight)));
    }

    private void Wave_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.UpdateGeometry();
    }

    public IEnumerable<Point>? viewModel { get => this.DataContext as IEnumerable<Point>; }
  }
}