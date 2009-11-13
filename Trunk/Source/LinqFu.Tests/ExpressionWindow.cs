using System.Linq.Expressions;
using System.Windows.Forms;
using ExpressionVisualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace LinqFu.Tests
{
    public sealed partial class ExpressionWindow : Form
    {
        private ExpressionWindow()
        {
            InitializeComponent();
        }

        public static void RenderExpression(Expression expression)
        {
            ExpressionWindow window = new ExpressionWindow();
            VisualizerDevelopmentHost host = new VisualizerDevelopmentHost(expression,
                                                 typeof(ExpressionTreeVisualizer),
                                                 typeof(ExpressionTreeVisualizerObjectSource));
            host.ShowVisualizer(window);
        }
    }
}
