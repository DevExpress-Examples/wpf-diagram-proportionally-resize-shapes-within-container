using DevExpress.Diagram.Core;
using DevExpress.Diagram.Core.Native;
using DevExpress.Xpf.Diagram;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WpfApp13 {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            diagramControl1.Items.Add(CreateContainerShape1());
            diagramControl1.BeforeItemsResizing += DiagramControl1_BeforeItemsResizing;
            diagramControl1.ItemsResizing += DiagramControl1_ItemsResizing;

            DiagramControl.ItemTypeRegistrator.Register(typeof(CustomDiagramContainer));
            this.Loaded += MainWindow_Loaded;
        }

        private void DiagramControl1_BeforeItemsResizing(object sender, DiagramBeforeItemsResizingEventArgs e) {
            var containers = e.Items.OfType<CustomDiagramContainer>();
            foreach (var customContainer in containers) {
                e.Items.Remove(customContainer);
                foreach (var item in customContainer.Items)
                    e.Items.Add(item);
            }
        }

        private void DiagramControl1_ItemsResizing(object sender, DiagramItemsResizingEventArgs e) {
            var groups = e.Items.GroupBy(x => x.Item.ParentItem);
            foreach (var group in groups) {
                if (group.Key is CustomDiagramContainer) {
                    var customContainer = (CustomDiagramContainer)group.Key;
                    var containingRect = customContainer.Items.Select(x => x.RotatedDiagramBounds().BoundedRect()).Aggregate(Rect.Empty, Rect.Union);
                    customContainer.Position = new Point(containingRect.X, containingRect.Y);
                    customContainer.Width = (float)containingRect.Width;
                    customContainer.Height = (float)containingRect.Height;
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            diagramControl1.FitToItems(diagramControl1.Items);
        }

        public CustomDiagramContainer CreateContainerShape1() {
            var container = new CustomDiagramContainer() {
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

    public class CustomDiagramContainer : DiagramContainer { }
}
