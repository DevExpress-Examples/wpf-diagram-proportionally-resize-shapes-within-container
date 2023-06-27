Imports DevExpress.Diagram.Core
Imports DevExpress.Diagram.Core.Native
Imports DevExpress.Xpf.Diagram
Imports System
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media

Namespace WpfApp13

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
            Me.diagramControl1.Items.Add(CreateContainerShape1())
            AddHandler Me.diagramControl1.BeforeItemsResizing, AddressOf Me.DiagramControl1_BeforeItemsResizing
            AddHandler Me.diagramControl1.ItemsResizing, AddressOf Me.DiagramControl1_ItemsResizing
            AddHandler Loaded, AddressOf Me.MainWindow_Loaded
        End Sub

        Private Sub DiagramControl1_BeforeItemsResizing(ByVal sender As Object, ByVal e As DiagramBeforeItemsResizingEventArgs)
            Dim containers = e.Items.OfType(Of DiagramContainer)()
            For Each container In containers
                e.Items.Remove(container)
                For Each item In container.Items
                    e.Items.Add(item)
                Next
            Next
        End Sub

        Private Sub DiagramControl1_ItemsResizing(ByVal sender As Object, ByVal e As DiagramItemsResizingEventArgs)
            Dim groups = e.Items.GroupBy(Function(x) x.Item.ParentItem)
            For Each group In groups
                Dim container = CType(group.Key, DiagramContainer)
                Dim containingRect = container.Items.[Select](Function(x) x.RotatedDiagramBounds().BoundedRect()).Aggregate(Rect.Empty, New Func(Of Rect, Rect, Rect)(AddressOf Rect.Union))
                container.Position = New Point(containingRect.X, containingRect.Y)
                container.Width = CSng(containingRect.Width)
                container.Height = CSng(containingRect.Height)
            Next
        End Sub

        Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.diagramControl1.FitToItems(Me.diagramControl1.Items)
        End Sub

        Public Function CreateContainerShape1() As DiagramContainer
            Dim container = New DiagramContainer() With {.Width = 200, .Height = 200, .Position = New Point(100, 100)}
            container.StrokeThickness = 0
            container.Background = Brushes.Transparent
            Dim innerShape1 = New DiagramShape() With {.CanSelect = False, .CanChangeParent = False, .CanEdit = False, .CanCopyWithoutParent = False, .CanDeleteWithoutParent = False, .CanMove = False, .Shape = BasicShapes.Trapezoid, .Height = 50, .Width = 200, .Content = "Custom text"}
            Dim innerShape2 = New DiagramShape() With {.CanSelect = False, .CanChangeParent = False, .CanEdit = False, .CanCopyWithoutParent = False, .CanDeleteWithoutParent = False, .CanMove = False, .Shape = BasicShapes.Rectangle, .Height = 150, .Width = 200, .Position = New Point(0, 50)}
            container.Items.Add(innerShape1)
            container.Items.Add(innerShape2)
            Return container
        End Function
    End Class
End Namespace
