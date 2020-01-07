﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModelComparison
{
    public partial class FrmMain : Form
    {
        private VIZCore3D.NET.VIZCore3DControl vizcore1;
        private VIZCore3D.NET.VIZCore3DControl vizcore2;

        public FrmMain()
        {
            InitializeComponent();

            // Initialize VIZCore3D.NET
            VIZCore3D.NET.ModuleInitializer.Run();

            // Construction
            vizcore1 = new VIZCore3D.NET.VIZCore3DControl();
            vizcore1.Dock = DockStyle.Fill;
            splitContainer2.Panel1.Controls.Add(vizcore1);

            // Event
            vizcore1.OnInitializedVIZCore3D += VIZCore3D1_OnInitializedVIZCore3D;


            // Construction
            vizcore2 = new VIZCore3D.NET.VIZCore3DControl();
            vizcore2.Dock = DockStyle.Fill;
            splitContainer2.Panel2.Controls.Add(vizcore2);

            // Event
            vizcore2.OnInitializedVIZCore3D += VIZCore3D2_OnInitializedVIZCore3D;

            splitContainer2.SplitterDistance = this.Width / 2;
        }

        private void VIZCore3D1_OnInitializedVIZCore3D(object sender, EventArgs e)
        {
            //MessageBox.Show("OnInitializedVIZCore3D", "VIZCore3D.NET");

            // ================================================================
            // Example
            // ================================================================
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901);
            //vizcore3d.License.LicenseFile("C:\\Temp\\VIZCore3D.NET.lic");


            // ================================================================
            // TEST
            // ================================================================
            VIZCore3D.NET.Data.LicenseResults result = vizcore1.License.LicenseFile("C:\\Users\\Gjkim\\Documents\\LICENSE_GJKIM_TOTAL_DEVICE_NEW_V2.lic");
            if (result != VIZCore3D.NET.Data.LicenseResults.SUCCESS)
            {
                MessageBox.Show(string.Format("LICENSE CODE : {0}", result.ToString()), "VIZCore3D.NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            vizcore1.ToolbarNote.Visible = false;
            vizcore1.ToolbarMeasurement.Visible = false;
            vizcore1.ToolbarSection.Visible = false;
            vizcore1.ToolbarClash.Visible = false;
            vizcore1.ToolbarAnimation.Visible = false;
            vizcore1.ToolbarSimulation.Visible = false;

            vizcore1.View.EnablePreSelect = false;
        }

        private void VIZCore3D2_OnInitializedVIZCore3D(object sender, EventArgs e)
        {
            //MessageBox.Show("OnInitializedVIZCore3D", "VIZCore3D.NET");

            // ================================================================
            // Example
            // ================================================================
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901);
            //vizcore3d.License.LicenseFile("C:\\Temp\\VIZCore3D.NET.lic");


            // ================================================================
            // TEST
            // ================================================================
            VIZCore3D.NET.Data.LicenseResults result = vizcore2.License.LicenseFile("C:\\Users\\Gjkim\\Documents\\LICENSE_GJKIM_TOTAL_DEVICE_NEW_V2.lic");
            if (result != VIZCore3D.NET.Data.LicenseResults.SUCCESS)
            {
                MessageBox.Show(string.Format("LICENSE CODE : {0}", result.ToString()), "VIZCore3D.NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            vizcore2.ToolbarNote.Visible = false;
            vizcore2.ToolbarMeasurement.Visible = false;
            vizcore2.ToolbarSection.Visible = false;
            vizcore2.ToolbarClash.Visible = false;
            vizcore2.ToolbarAnimation.Visible = false;
            vizcore2.ToolbarSimulation.Visible = false;

            vizcore2.View.EnablePreSelect = false;
        }

        private void btnOpen1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = vizcore1.Model.OpenFilter;
            if (dlg.ShowDialog() != DialogResult.OK) return;

            txtModel1.Text = dlg.FileName;

            vizcore1.Model.Open(dlg.FileName);
        }

        private void btnOpen2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = vizcore2.Model.OpenFilter;
            if (dlg.ShowDialog() != DialogResult.OK) return;

            txtModel2.Text = dlg.FileName;

            vizcore2.Model.Open(dlg.FileName);
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            List<VIZCore3D.NET.Data.Node> node1 = vizcore1.Object3D.FromFilter(VIZCore3D.NET.Data.Object3dFilter.ALL, true);
            List<VIZCore3D.NET.Data.Node> node2 = vizcore2.Object3D.FromFilter(VIZCore3D.NET.Data.Object3dFilter.ALL, true);

            Dictionary<string, VIZCore3D.NET.Data.Node> map1 = vizcore1.Object3D.GetNodePathMap();
            Dictionary<string, VIZCore3D.NET.Data.Node> map2 = vizcore2.Object3D.GetNodePathMap();           

            List<ModelComparisonItem> result = new List<ModelComparisonItem>();
            foreach (KeyValuePair<string, VIZCore3D.NET.Data.Node> item in map1)
            {
                ModelComparisonItem model = new ModelComparisonItem();

                if (map2.ContainsKey(item.Key) == true)
                {
                    model.Node1 = item.Value;
                    model.Node2 = map2[item.Key];

                    model.BBox1 = vizcore1.Object3D.GeometryProperty.FromIndex(item.Value.Index, false).GetBoundBox();
                    model.BBox2 = vizcore2.Object3D.GeometryProperty.FromIndex(model.Node2.Index, false).GetBoundBox();

                    if (item.Value.NodeType == VIZCore3D.NET.Data.NodeTypes.PART)
                    {
                        model.MeshCount1 = vizcore1.Object3D.GetMeshCount(item.Value.Index);
                        model.MeshCount2 = vizcore2.Object3D.GetMeshCount(model.Node2.Index);
                    }

                    model.RESULT_EXIST_A = true;
                    model.RESULT_EXIST_B = true;
                    model.RESULT_EXIST_BOTH = true;

                    model.Compare();
                }
                else
                {
                    model.Node1 = item.Value;
                    //model.Node2 = map2[item.Key];

                    model.BBox1 = vizcore1.Object3D.GeometryProperty.FromIndex(item.Value.Index, false).GetBoundBox();
                    //model.BBox2 = vizcore2.Object3D.GeometryProperty.FromIndex(model.Node2.Index, false).GetBoundBox();

                    if (item.Value.NodeType == VIZCore3D.NET.Data.NodeTypes.PART)
                    {
                        model.MeshCount1 = vizcore1.Object3D.GetMeshCount(item.Value.Index);
                        //model.MeshCount2 = vizcore2.Object3D.GetMeshCount(model.Node2.Index);
                    }

                    model.RESULT_EXIST_A = true;
                    model.RESULT_EXIST_B = false;
                    model.RESULT_EXIST_BOTH = false;
                }

                result.Add(model);
            }

            foreach (KeyValuePair<string, VIZCore3D.NET.Data.Node> item in map2)
            {
                ModelComparisonItem model = new ModelComparisonItem();

                if (map1.ContainsKey(item.Key) == false)
                {
                    model.Node2 = item.Value;

                    //model.BBox1 = vizcore1.Object3D.GeometryProperty.FromIndex(item.Value.Index, false).GetBoundBox();
                    model.BBox2 = vizcore2.Object3D.GeometryProperty.FromIndex(model.Node2.Index, false).GetBoundBox();

                    if (item.Value.NodeType == VIZCore3D.NET.Data.NodeTypes.PART)
                    {
                        //model.MeshCount1 = vizcore1.Object3D.GetMeshCount(item.Value.Index);
                        model.MeshCount2 = vizcore2.Object3D.GetMeshCount(model.Node2.Index);
                    }

                    model.RESULT_EXIST_A = false;
                    model.RESULT_EXIST_B = true;
                    model.RESULT_EXIST_BOTH = false;

                    result.Add(model);
                }
            }

            lvResult.BeginUpdate();
            lvResult.Items.Clear();
            foreach (ModelComparisonItem item in result)
            {
                ListViewItem lvi = new ListViewItem(item.GetListViewItem());
                lvi.Tag = item;

                if (ckResultViewType.Checked == true)
                {
                    if(item.IsSameModel() == false)
                        lvResult.Items.Add(lvi);
                }
                else
                {
                    lvResult.Items.Add(lvi);
                }
            }
            lvResult.EndUpdate();
        }

        private void ResetView()
        {
            ResetViewFunc(vizcore1);
            ResetViewFunc(vizcore2);
        }

        private void ResetViewFunc(VIZCore3D.NET.VIZCore3DControl ctrl)
        {
            ctrl.View.XRay.Clear();
            ctrl.View.XRay.Enable = false;
            ctrl.View.ResetView();
        }

        private void HighlightModel(ModelComparisonItem model)
        {
            if (vizcore1.View.XRay.Enable == false)
                vizcore1.View.XRay.Enable = true;

            if (vizcore2.View.XRay.Enable == false)
                vizcore2.View.XRay.Enable = true;

            vizcore1.View.XRay.Clear();
            vizcore2.View.XRay.Clear();

            vizcore1.View.ResetView();
            vizcore2.View.ResetView();

            if (model.RESULT_EXIST_BOTH == true)
            {
                vizcore1.View.FlyToObject3d(new List<int>() { model.Node1.Index }, 1.0f);
                vizcore2.View.FlyToObject3d(new List<int>() { model.Node2.Index }, 1.0f);

                vizcore1.View.XRay.Select(new List<int>() { model.Node1.Index }, true);
                vizcore2.View.XRay.Select(new List<int>() { model.Node2.Index }, true);
            }
            else if(model.RESULT_EXIST_A == true)
            {
                vizcore1.View.FlyToObject3d(new List<int>() { model.Node1.Index }, 1.0f);
                //vizcore2.View.FlyToObject3d(new List<int>() { model.Node2.Index }, 1.0f);

                vizcore1.View.XRay.Select(new List<int>() { model.Node1.Index }, true);
                //vizcore2.View.XRay.Select(new List<int>() { model.Node2.Index }, true);
            }
            else
            {
                //vizcore1.View.FlyToObject3d(new List<int>() { model.Node1.Index }, 1.0f);
                vizcore2.View.FlyToObject3d(new List<int>() { model.Node2.Index }, 1.0f);

                //vizcore1.View.XRay.Select(new List<int>() { model.Node1.Index }, true);
                vizcore2.View.XRay.Select(new List<int>() { model.Node2.Index }, true);
            }
        }

        private void lvResult_DoubleClick(object sender, EventArgs e)
        {
            if (lvResult.SelectedItems.Count == 0) return;

            ListViewItem lvi = lvResult.SelectedItems[0];
            if (lvi == null)
            {
                ResetView();
                return;
            }
            if (lvi.Tag == null)
            {
                ResetView();
                return;
            }

            ModelComparisonItem model = (ModelComparisonItem)lvi.Tag;
            if (model == null)
            {
                ResetView();
                return;
            }

            HighlightModel(model);
        }
    }
}
