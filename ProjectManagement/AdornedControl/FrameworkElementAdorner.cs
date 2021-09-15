using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

// Source:
//  https://www.codeproject.com/Articles/54472/Defining-WPF-Adorners-in-XAML
// This code based on code available here:
//
//  http://www.codeproject.com/KB/WPF/WPFJoshSmith.aspx
namespace AdornedControl
{
    //
    // This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
    //
    public class FrameworkElementAdorner : Adorner
    {
        //
        // The framework element that is the adorner. 
        //
        private readonly FrameworkElement child;

        //
        // Placement of the child.
        //
        private readonly AdornerPlacement horizontalAdornerPlacement = AdornerPlacement.Inside;
        private readonly AdornerPlacement verticalAdornerPlacement = AdornerPlacement.Inside;

        //
        // Offset of the child.
        //
        private readonly double offsetX;
        private readonly double offsetY;

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
            : base(adornedElement)
        {
            child = adornerChildElement;

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                       AdornerPlacement horizontalAdornerPlacement, AdornerPlacement verticalAdornerPlacement,
                                       double offsetX, double offsetY)
            : base(adornedElement)
        {
            child = adornerChildElement;
            this.horizontalAdornerPlacement = horizontalAdornerPlacement;
            this.verticalAdornerPlacement = verticalAdornerPlacement;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            adornedElement.SizeChanged += new SizeChangedEventHandler(adornedElement_SizeChanged);

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        private void adornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateMeasure();
        }

        //
        // Position of the child (when not set to NaN).
        //
        public double PositionX { get; set; } = double.NaN;

        public double PositionY { get; set; } = double.NaN;

        protected override Size MeasureOverride(Size constraint)
        {
            child.Measure(constraint);
            return child.DesiredSize;
        }

        /// <summary>
        /// Determine the X coordinate of the child.
        /// </summary>
        private double DetermineX()
        {
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return horizontalAdornerPlacement == AdornerPlacement.Outside ? -child.DesiredSize.Width + offsetX : offsetX;
                case HorizontalAlignment.Right:
                    {
                        if (horizontalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            double adornedWidth = AdornedElement.ActualWidth;
                            return adornedWidth + offsetX;
                        }
                        else
                        {
                            double adornerWidth = child.DesiredSize.Width;
                            if (child is Canvas canvas)
                                for (int i = 0; i < canvas.Children.Count; i++)
                                {
                                    adornerWidth += canvas.Children[i].DesiredSize.Width;
                                }
                            double adornedWidth = AdornedElement.ActualWidth;
                            double x = adornedWidth - adornerWidth;
                            return x + offsetX;
                        }
                    }
                case HorizontalAlignment.Center:
                    {
                        double adornerWidth = child.DesiredSize.Width;
                        if (child is Canvas canvas)
                            for (int i = 0; i < canvas.Children.Count; i++)
                            {
                                adornerWidth += canvas.Children[i].DesiredSize.Width;
                            }
                        double adornedWidth = AdornedElement.ActualWidth;
                        double x = adornedWidth / 2 - adornerWidth / 2;
                        return x + offsetX;
                    }
                case HorizontalAlignment.Stretch:
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Determine the Y coordinate of the child.
        /// </summary>
        private double DetermineY()
        {
            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    return verticalAdornerPlacement == AdornerPlacement.Outside ? -child.DesiredSize.Height + offsetY : offsetY;
                case VerticalAlignment.Bottom:
                    {
                        if (verticalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            double adornedHeight = AdornedElement.ActualHeight;
                            return adornedHeight + offsetY;
                        }
                        else
                        {
                            double adornerHeight = child.DesiredSize.Height;
                            if (child is Canvas canvas)
                                for (int i = 0; i < canvas.Children.Count; i++)
                                {
                                    adornerHeight += canvas.Children[i].DesiredSize.Height;
                                }
                            double adornedHeight = AdornedElement.ActualHeight;
                            double x = adornedHeight - adornerHeight;
                            return x + offsetY;
                        }
                    }
                case VerticalAlignment.Center:
                    {
                        double adornerHeight = child.DesiredSize.Height;
                        if (child is Canvas canvas)
                            for (int i = 0; i < canvas.Children.Count; i++)
                            {
                                adornerHeight += canvas.Children[i].DesiredSize.Height;
                            }
                        double adornedHeight = AdornedElement.ActualHeight;
                        double x = adornedHeight / 2 - adornerHeight / 2;
                        return x + offsetY;
                    }
                case VerticalAlignment.Stretch:
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Determine the width of the child.
        /// </summary>
        private double DetermineWidth()
        {
            return !double.IsNaN(PositionX)
                ? child.DesiredSize.Width
                : child.HorizontalAlignment switch
                {
                    HorizontalAlignment.Left => child.DesiredSize.Width,
                    HorizontalAlignment.Right => child.DesiredSize.Width,
                    HorizontalAlignment.Center => child.DesiredSize.Width,
                    HorizontalAlignment.Stretch => AdornedElement.ActualWidth,
                    _ => 0.0,
                };
        }

        /// <summary>
        /// Determine the height of the child.
        /// </summary>
        private double DetermineHeight()
        {
            return !double.IsNaN(PositionY)
                ? child.DesiredSize.Height
                : child.VerticalAlignment switch
                {
                    VerticalAlignment.Top => child.DesiredSize.Height,
                    VerticalAlignment.Bottom => child.DesiredSize.Height,
                    VerticalAlignment.Center => child.DesiredSize.Height,
                    VerticalAlignment.Stretch => AdornedElement.ActualHeight,
                    _ => 0.0,
                };
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = PositionX;
            if (double.IsNaN(x))
            {
                x = DetermineX();
            }
            double y = PositionY;
            if (double.IsNaN(y))
            {
                y = DetermineY();
            }
            double adornerWidth = DetermineWidth();
            double adornerHeight = DetermineHeight();
            child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            return finalSize;
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            return child;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                ArrayList list = new()
                {
                    child
                };
                return list.GetEnumerator();
            }
        }

        /// <summary>
        /// Disconnect the child element from the visual tree so that it may be reused later.
        /// </summary>
        public void DisconnectChild()
        {
            RemoveLogicalChild(child);
            RemoveVisualChild(child);
        }

        /// <summary>
        /// Override AdornedElement from base class for less type-checking.
        /// </summary>
        public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;
    }
}
