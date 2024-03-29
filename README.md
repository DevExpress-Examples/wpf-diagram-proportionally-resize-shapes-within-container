<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/659348498/17.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1174670)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# WPF DiagramControl - Proportionally Resize Shapes Within the Parent Container

This example resizes inner shapes when the associated parent container is resized. You can introduce the logic herein if you [create custom shapes based on containers](https://github.com/DevExpress-Examples/wpf-diagram-create-custom-shapes-based-on-diagram-containers) or use containers to group shapes.

![](./img/Proportionally_Resize_Shape_Containers.gif) 

## Implementation Details

1. Create a [DiagramContainer](https://docs.devexpress.com/WPF/DevExpress.Xpf.Diagram.DiagramContainer) class descendant to retain the behavior of standard containers:

   ```cs
   public class CustomDiagramContainer : DiagramContainer { }
   ```

2. Handle the [DiagramControl.BeforeItemsResizing](https://docs.devexpress.com/WPF/DevExpress.Xpf.Diagram.DiagramControl.BeforeItemsResizing) event and pass container child items to the `e.Items` collection:

   ```csharp
   private void DiagramControl1_BeforeItemsResizing(object sender, DiagramBeforeItemsResizingEventArgs e) {
       var containers = e.Items.OfType<CustomDiagramContainer>();
       foreach (var customContainer in containers) {
           e.Items.Remove(customContainer);
           foreach (var item in customContainer.Items)
               e.Items.Add(item);
       }
   }
   ```

   In this instance, the `DiagramControl` resizes these inner items instead of the parent container.

3. Handle the [DiagramControl.ItemsResizing](https://docs.devexpress.com/WPF/DevExpress.Xpf.Diagram.DiagramControl.ItemsResizing) event and correct container position and size:

   ```csharp
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
   ```

## Files to Review

- [MainWindow.xaml.cs](./CS/WpfApp13/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/WpfApp13/MainWindow.xaml.vb))

## Documentation

- [Containers and Lists](https://docs.devexpress.com/WPF/117205/controls-and-libraries/diagram-control/diagram-items/containers)
- [Create Custom Diagram Items](https://docs.devexpress.com/WPF/404589/controls-and-libraries/diagram-control/diagram-items/create-custom-diagram-items)
- [DiagramControl.BeforeItemsResizing](https://docs.devexpress.com/WPF/DevExpress.Xpf.Diagram.DiagramControl.BeforeItemsResizing)
- [DiagramControl.ItemsResizing](https://docs.devexpress.com/WPF/DevExpress.Xpf.Diagram.DiagramControl.ItemsResizing)

## More Examples

- [WPF DiagramControl - Create Custom Shapes Based on Diagram Containers](https://github.com/DevExpress-Examples/wpf-diagram-create-custom-shapes-based-on-diagram-containers)
- [WPF DiagramControl - Create Rotatable Containers with Shapes](https://github.com/DevExpress-Examples/wpf-diagram-create-rotatable-containers-with-shapes)
