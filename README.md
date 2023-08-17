<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/659348498/17.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1174670)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# WPF - How to proportionally resize shapes whithin the parent container

This example demonstrates how to support proportional resizing for shapes when the parent container is resized. You can implement this additional logic if you create custom shapes based on containers, or use containers to support the grouping functionality.

To support this feature, handle `DiagramControl`'s [BeforeItemsResizing](https://docs.devexpress.com/WPF/DevExpress.Xpf.Diagram.DiagramControl.BeforeItemsResizing) event and pass a container's child items to the `e.Items` collection:

```csharp
private void DiagramControl1_BeforeItemsResizing(object sender, DiagramBeforeItemsResizingEventArgs e) {
    var containers = e.Items.OfType<CustomDiagramContainer>();
    foreach (var container in containers) {
        e.Items.Remove(container);
        foreach (var item in container.Items)
            e.Items.Add(item);
    }
}
```

In this case, `DiagramControl` will resize the inner items instead of the parent container.
After that, handle `DiagramControl`'s [ItemsResizing](https://docs.devexpress.com/WPF/DevExpress.Xpf.Diagram.DiagramControl.ItemsResizing) event and correct the container's position and size:

```csharp
private void DiagramControl1_ItemsResizing(object sender, DiagramItemsResizingEventArgs e) {
    var groups = e.Items.GroupBy(x => x.Item.ParentItem);
    foreach (var group in groups) {
        if (group.Key is CustomDiagramContainer container) {
            var containingRect = container.Items.Select(x => x.RotatedDiagramBounds().BoundedRect()).Aggregate(Rect.Empty, Rect.Union);
            container.Position = new Point(containingRect.X, containingRect.Y);
            container.Width = (float)containingRect.Width;
            container.Height = (float)containingRect.Height;
        }
    }
}
```

## Files to Review

- link.cs (VB: link.vb)
- link.js
- ...

## Documentation

- link
- link
- ...

## More Examples

- link
- link
- ...
