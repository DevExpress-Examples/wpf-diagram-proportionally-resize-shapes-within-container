using DevExpress.Diagram.Core;
using DevExpress.Diagram.Core.Native;
using DevExpress.Utils;
using DevExpress.Xpf.Diagram;
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

namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            diagramControl1.Items.Add(CreateContainerShape1());
            diagramControl1.BeforeItemsResizing += DiagramControl1_BeforeItemsResizing;
            diagramControl1.ItemsResizing += DiagramControl1_ItemsResizing;

            this.Loaded += MainWindow_Loaded;
        }

        private void DiagramControl1_BeforeItemsResizing(object sender, DiagramBeforeItemsResizingEventArgs e) {
            var containers = e.Items.OfType<DiagramContainer>();
            foreach (var container in containers) {
                e.Items.Remove(container);
                foreach (var item in container.Items)
                    e.Items.Add(item);
            }
        }

        private void DiagramControl1_ItemsResizing(object sender, DiagramItemsResizingEventArgs e) {
            var groups = e.Items.GroupBy(x => x.Item.ParentItem);
            foreach (var group in groups) {
                if (group.Key is DiagramContainer container) {
                    var containingRect = container.Items.Select(x => x.RotatedDiagramBounds().BoundedRect()).Aggregate(Rect.Empty, Rect.Union);
                    container.Position = new Point(containingRect.X, containingRect.Y);
                    container.Width = (float)containingRect.Width;
                    container.Height = (float)containingRect.Height;
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            diagramControl1.FitToItems(diagramControl1.Items);
        }

        public DiagramContainer CreateContainerShape1() {
            var container = new DiagramContainer() {
                Width = 200,
                Height = 200,
                Position = new Point(100, 100),
                CanAddItems = false,
                ItemsCanChangeParent = false,
                ItemsCanCopyWithoutParent = false,
                ItemsCanDeleteWithoutParent = false,
                ItemsCanAttachConnectorBeginPoint = false,
                ItemsCanAttachConnectorEndPoint = false
            };

            container.StrokeThickness = 0;
            container.Background = Brushes.Transparent;

            var innerShape1 = new DiagramShape() {
                CanSelect = true,
                CanChangeParent = false,
                CanEdit = true,
                CanResize = false,
                CanCopyWithoutParent = false,
                CanDeleteWithoutParent = false,
                CanMove = false,
                Shape = BasicShapes.Trapezoid,
                Height = 50,
                Width = 200,

                Content = "Custom text"
            };

            var innerShape2 = new DiagramShape() {
                CanSelect = false,
                CanChangeParent = false,
                CanEdit = false,
                CanCopyWithoutParent = false,
                CanDeleteWithoutParent = false,
                CanMove = false,
                Shape = BasicShapes.Rectangle,
                Height = 150,
                Width = 200,
                Position = new Point(0, 50),
            };

            container.Items.Add(innerShape1);
            container.Items.Add(innerShape2);

            return container;
        }
    }
}
