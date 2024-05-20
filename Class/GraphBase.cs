using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace Alarmlines.Class
{
    public enum DemoType
    {
        Financial,
        Line,
        General,
        Bar,
        Pie,
        Special,
        Tutorial
    }
    public abstract class GraphBase
    {
        protected string description;
        protected string title;
        protected ICollection types;

        public GraphBase(string description, string title, DemoType type)
        {
            ArrayList types = new ArrayList();
            types.Add(type);

            Init(description, title, types);
        }

        public GraphBase(string description, string title, DemoType type, DemoType type2)
        {
            ArrayList types = new ArrayList();
            types.Add(type);
            types.Add(type2);

            Init(description, title, types);
        }

        public GraphBase(string description, string title, ICollection types)
        {
            Init(description, title, types);
        }

        private void Init(string description, string title, ICollection types)
        {
            this.description = description;
            this.title = title;
            this.types = types;

            ZedGraphControl = new ZedGraphControl
            {
                IsAntiAlias = true,
                IsShowVScrollBar = true,
                IsShowHScrollBar = true,
                IsAutoScrollRange = true,

                //CrossHairType   = CrossHairType.MasterPane
            };
            //m_DistanceMeasurer = new DistanceMeasurer(ZedGraphControl, Color.DarkOrange, Color.Red, 9.0F, CoordType.AxisXYScale);
            GraphPane.IsBoundedRanges = true;
        }

        public virtual void Activate() { }
        public virtual void Deactivate() { }

        #region ZedGraphDemo Members

        //protected DistanceMeasurer m_DistanceMeasurer;

        /// <summary>
        /// The graph pane the chart is show in.
        /// </summary>
        public PaneBase Pane => ZedGraphControl.GraphPane;

        /// <summary>
        /// The graph pane the chart is show in.
        /// </summary>
        public MasterPane MasterPane => ZedGraphControl.MasterPane;

        /// <summary>
        /// The graph pane the chart is show in (same as .Pane).
        /// </summary>
        public GraphPane GraphPane => ZedGraphControl.GraphPane;

        public virtual string Description => description;

        public virtual string Title => title;

        public virtual ICollection Types => types;

        /// <summary>
        /// The control the graph pane is in.
        /// </summary>
        public ZedGraphControl ZedGraphControl { get; private set; }

        #endregion

    }
}
