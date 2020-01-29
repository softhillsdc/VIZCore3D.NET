﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VIZCore3D.NET.Structure
{
    public partial class FrmMain : Form
    {
        // ================================================
        // Attribute & Property
        // ================================================
        /// <summary>
        /// VIZCore3D.NET
        /// </summary>
        private VIZCore3D.NET.VIZCore3DControl vizcore3d;

        /// <summary>
        /// 모델 구조(Structure) 처리기
        /// </summary>
        public VIZCore3D.NET.Data.ShStructure StructureManager { get; set; }

        public FrmMain()
        {
            InitializeComponent();

            // Initialize VIZCore3D.NET
            VIZCore3D.NET.ModuleInitializer.Run();

            // Construction
            vizcore3d = new VIZCore3DControl();
            vizcore3d.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(vizcore3d);

            // Event
            vizcore3d.OnInitializedVIZCore3D += VIZCore3D_OnInitializedVIZCore3D;
        }

        // ================================================
        // Event - VIZCore3D.NET
        // ================================================
        private void VIZCore3D_OnInitializedVIZCore3D(object sender, EventArgs e)
        {
            //MessageBox.Show("OnInitializedVIZCore3D", "VIZCore3D.NET");

            // ================================================================
            // Example
            // ================================================================
            //vizcore3d.License.LicenseFile("C:\\Temp\\VIZCore3D.NET.lic");                         // 라이선스 파일
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901);                                   // 라이선스 서버 - 제품 오토 선택
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901, Data.Products.AUTO);               // 라이선스 서버 - 제품 오토 선택
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901, Data.Products.VIZZARD_MANAGER);    // 라이선스 서버 - 지정된 제품으로 인증
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901, Data.Products.VIZZARD_STANDARD);   // 라이선스 서버 - 지정된 제품으로 인증
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901, Data.Products.VIZCORE3D_MANAGER);  // 라이선스 서버 - 지정된 제품으로 인증
            //vizcore3d.License.LicenseServer("127.0.0.1", 8901, Data.Products.VIZCORE3D_STANDARD); // 라이선스 서버 - 지정된 제품으로 인증


            // ================================================================
            // TEST
            // ================================================================
            VIZCore3D.NET.Data.LicenseResults result = vizcore3d.License.LicenseFile("C:\\License\\VIZCore3D.NET.lic");
            //VIZCore3D.NET.Data.LicenseResults result = vizcore3d.License.LicenseServer("127.0.0.1", 8901);
            //VIZCore3D.NET.Data.LicenseResults result = vizcore3d.License.LicenseServer("192.168.0.215", 8901);
            if (result != Data.LicenseResults.SUCCESS)
            {
                MessageBox.Show(string.Format("LICENSE CODE : {0}", result.ToString()), "VIZCore3D.NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Init. VIZCore3D.NET
            InitializeVIZCore3D();
            InitializeVIZCore3DEvent();
        }

        // ================================================
        // Function - VIZCore3D.NET : Initialize
        // ================================================
        private void InitializeVIZCore3D()
        {
            // ================================================================
            // 모델 열기 시, 3D 화면 Rendering 차단
            // ================================================================
            vizcore3d.View.BeginUpdate();


            // ================================================================
            // 설정 - 기본
            // ================================================================
            #region 설정 - 기본
            // 모델 자동 언로드 (파일 노드 언체크 시, 언로드)
            vizcore3d.Model.UncheckToUnload = true;

            // 모델 열기 시, Edge 정보 로드 활성화
            vizcore3d.Model.LoadEdge = true;

            // 모델 열기 시, Edge 정보 변환 활성화
            vizcore3d.Model.ConvertEdge = true;

            // 모델 조회 시, 하드웨어 가속
            vizcore3d.View.EnableHardwareAcceleration = true;

            // 모델 열기 시, 스트럭처 병합 설정
            vizcore3d.Model.OpenMergeStructureMode = Data.MergeStructureModes.NONE;

            // 모델 저장 시, 스트럭처 병합 설정
            vizcore3d.Model.SaveMergeStructureMode = Data.MergeStructureModes.NONE;

            // 실린더 원형 품질 개수 : Nomal(12~36), Small(6~36)
            vizcore3d.Model.ReadNormalCylinderSide = 12;
            vizcore3d.Model.ReadSmallCylinderSide = 6;

            // 보이는 모델만 저장

            // VIZXML to VIZ 옵션
            vizcore3d.Model.VIZXMLtoVIZOption = Data.ExportVIZXMLToVIZOptions.LOAD_UNLOADED_NODE;

            // 선택 가능 개체 : 전체, 불투명한 개체
            vizcore3d.View.SelectionObject3DType = Data.SelectionObject3DTypes.ALL;

            // 개체 선택 유형 : 색상, 경계로 선택 (개체), 경계로 선택 (전체)
            vizcore3d.View.SelectionMode = Data.Object3DSelectionOptions.HIGHLIGHT_COLOR;

            // 개체 선택 색상
            vizcore3d.View.SelectionColor = Color.Red;

            // 모델 조회 시, Pre-Select 설정
            vizcore3d.View.EnablePreSelect = false;

            // 모델 조회 시, Pre-Select 색상 설정
            vizcore3d.View.PreSelectColor = Color.Lime;
            #endregion


            // ================================================================
            // 설정 - 보기
            // ================================================================
            #region 설정 - 보기
            // 자동 애니메이션 : 박스줌, 개체로 비행 등 기능에서 애니메이션 활성화/비활성화
            vizcore3d.View.EnableAnimation = true;

            // 자동화면맞춤
            vizcore3d.View.EnableAutoFit = false;

            // 연속회전모드
            vizcore3d.View.EnableInertiaRotate = false;

            // 확대/축소 비율 : 5.0f ~ 50.0f
            vizcore3d.View.ZoomRatio = 30.0f;

            // 회전각도
            vizcore3d.View.RotationAngle = 90.0f;

            // 회전 축
            vizcore3d.View.RotationAxis = Data.Axis.X;
            #endregion

            // ================================================================
            // 설정 - 탐색
            // ================================================================
            #region 설정 - 탐색
            // Z축 고정
            vizcore3d.View.Walkthrough.LockZAxis = true;
            // 선속도 : m/s
            vizcore3d.View.Walkthrough.Speed = 2.0f;
            // 각속도
            vizcore3d.View.Walkthrough.AngularSpeed = 30.0f;
            // 높이
            vizcore3d.View.Walkthrough.AvatarHeight = 1800.0f;
            // 반지름
            vizcore3d.View.Walkthrough.AvatarCollisionRadius = 400.0f;
            // 숙임높이
            vizcore3d.View.Walkthrough.AvatarBowWalkHeight = 1300.0f;
            // 충돌
            vizcore3d.View.Walkthrough.UseAvatarCollision = false;
            // 중력
            vizcore3d.View.Walkthrough.UseAvatarGravity = false;
            // 숙임
            vizcore3d.View.Walkthrough.UseAvatarBowWalk = false;
            // 모델
            vizcore3d.View.Walkthrough.AvatarModel = (int)Data.AvatarModels.MAN1;
            // 자동줌
            vizcore3d.View.Walkthrough.EnableAvatarAutoZoom = false;
            // 충돌상자보기
            vizcore3d.View.Walkthrough.ShowAvatarCollisionCylinder = false;
            #endregion


            // ================================================================
            // 설정 - 조작
            // ================================================================
            #region 설정 - 조작
            // 시야각
            vizcore3d.View.FOV = 60.0f;
            // 광원 세기
            vizcore3d.View.SpecularGamma = 100.0f;
            // 모서리 굵기
            vizcore3d.View.EdgeWidthRatio = 0.0f;
            // X-Ray 모델 조회 시, 개체 색상 - 선택색상, 모델색상
            vizcore3d.View.XRay.ColorType = Data.XRayColorTypes.SELECTION_COLOR;
            // 배경유형
            //vizcore3d.View.BackgroundMode = Data.BackgroundModes.COLOR_ONE;
            // 배경색1
            //vizcore3d.View.BackgroundColor1 = Color.Gray;
            // 배경색2
            //vizcore3d.View.BackgroundColor2 = Color.Gray; 
            #endregion

            // ================================================================
            // 설정 - 노트
            // ================================================================


            // ================================================================
            // 설정 - 측정
            // ================================================================



            // ================================================================
            // 설정 - 단면
            // ================================================================
            // 단면 좌표간격으로 이동
            vizcore3d.Section.MoveSectionByFrameGrid = true;


            // ================================================================
            // 설정 - 간섭검사
            // ================================================================
            // 다중간섭검사
            vizcore3d.Clash.EnableMultiThread = true;


            // ================================================================
            // 설정 - 프레임(SHIP GRID)
            // ================================================================
            #region 설정 - 프레임
            // 프레임 평면 설정
            vizcore3d.View.Frame.XYPlane = true;
            vizcore3d.View.Frame.YZPlane = true;
            vizcore3d.View.Frame.ZXPlane = true;
            vizcore3d.View.Frame.PlanLine = true;
            vizcore3d.View.Frame.SectionLine = true;
            vizcore3d.View.Frame.ElevationLine = true;

            // 좌표값 표기
            vizcore3d.View.Frame.ShowNumber = true;

            // 모델 앞에 표기
            vizcore3d.View.Frame.BringToFront = false;

            // Frame(좌표계, SHIP GRID) 색상
            vizcore3d.View.Frame.ForeColor = Color.Black;

            // 홀수번째 표시
            vizcore3d.View.Frame.ShowOddNumber = true;
            // 짝수번째 표시
            vizcore3d.View.Frame.ShowEvenNumber = true;
            // 단면상자에 자동 맞춤
            vizcore3d.View.Frame.AutoFitSectionBox = true;
            #endregion



            // ================================================================
            // 설정 - 툴바
            // ================================================================
            #region 설정 - 툴바
            vizcore3d.ToolbarNote.Visible = false;
            vizcore3d.ToolbarMeasurement.Visible = false;
            vizcore3d.ToolbarSection.Visible = false;
            vizcore3d.ToolbarClash.Visible = false;
            vizcore3d.ToolbarAnimation.Visible = false;
            vizcore3d.ToolbarSimulation.Visible = false;
            #endregion

            // ================================================================
            // 설정 - 상태바
            // ================================================================
            vizcore3d.Statusbar.Visible = true;


            // ================================================================
            // 모델 열기 시, 3D 화면 Rendering 재시작
            // ================================================================
            vizcore3d.View.EndUpdate();
        }

        /// <summary>
        /// 이벤트 등록
        /// </summary>
        private void InitializeVIZCore3DEvent()
        {
        }

        private void btnVIZ_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "VIZ (*.viz)|*.viz";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            this.Cursor = Cursors.WaitCursor;
            ShowStructure(dlg.FileName);
            this.Cursor = Cursors.Default;
        }

        private void ShowStructure(string path)
        {
            txtVIZ.Text = path;

            // 모델 로드
            StructureManager = new Data.ShStructure(path, false);

            MakeModelTree();
        }

        private void MakeModelTree()
        {
            tvStructure.BeginUpdate();
            tvStructure.Nodes.Clear();

            // 처리된 모델 구조 불러오기
            List<VIZCore3D.NET.Data.ShModelTreeNode> roots = StructureManager.Roots;

            // 최상위는 파일 노드이므로, 파일이름으로 생성
            TreeNode rootNode = tvStructure.Nodes.Add(System.IO.Path.GetFileNameWithoutExtension(txtVIZ.Text));
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;

            // 최상위 루트 기준
            for (int i = 0; i < roots.Count; i++)
            {
                // 파일 노드 하위의 최상위 노드
                VIZCore3D.NET.Data.ShModelTreeNode node = roots[i];

                TreeNode tnNode = rootNode.Nodes.Add(node.NodeName);
                tnNode.Tag = node;

                if (node.NodeType == VIZCore3D.NET.Data.ShModelTreeNodeType.ASSEMBLY)
                {
                    tnNode.ImageIndex = 1;
                    tnNode.SelectedImageIndex = 1;
                }
                else if (node.NodeType == VIZCore3D.NET.Data.ShModelTreeNodeType.PART)
                {
                    tnNode.ImageIndex = 2;
                    tnNode.SelectedImageIndex = 2;
                }
                else if (node.NodeType == VIZCore3D.NET.Data.ShModelTreeNodeType.BODY)
                {
                    tnNode.ImageIndex = 3;
                    tnNode.SelectedImageIndex = 3;
                }

                // 하위 노드가 있는 경우, 재귀 호출
                if (node.Nodes.Count != 0)
                {
                    BindNode(tnNode, node);
                }
            }

            // 최상위 노드 펼치기
            rootNode.Expand();

            tvStructure.EndUpdate();
        }

        /// <summary>
        /// 하위 노드를 트리 컨트롤에 추가
        /// </summary>
        /// <param name="tnNode"></param>
        /// <param name="mtNode"></param>
        private void BindNode(TreeNode tnNode, VIZCore3D.NET.Data.ShModelTreeNode mtNode)
        {
            // 부모 노드의 하위 자식 목록
            List<VIZCore3D.NET.Data.ShModelTreeNode> children = mtNode.Nodes;

            for (int i = 0; i < children.Count; i++)
            {
                // 노드 정보
                VIZCore3D.NET.Data.ShModelTreeNode node = children[i];

                // 트리 노드 생성
                TreeNode childNode = tnNode.Nodes.Add(node.NodeName);
                childNode.Tag = node;

                if (node.NodeType == VIZCore3D.NET.Data.ShModelTreeNodeType.ASSEMBLY)
                {
                    childNode.ImageIndex = 1;
                    childNode.SelectedImageIndex = 1;
                }
                else if (node.NodeType == VIZCore3D.NET.Data.ShModelTreeNodeType.PART)
                {
                    childNode.ImageIndex = 2;
                    childNode.SelectedImageIndex = 2;
                }
                else if (node.NodeType == VIZCore3D.NET.Data.ShModelTreeNodeType.BODY)
                {
                    childNode.ImageIndex = 3;
                    childNode.SelectedImageIndex = 3;
                }

                // 하위 노드가 있는 경우, 재귀 호출
                if (node.Nodes.Count != 0)
                {
                    BindNode(childNode, node);
                }
            }
        }

        private void tvStructure_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null) return;

            VIZCore3D.NET.Data.ShModelTreeNode node = (VIZCore3D.NET.Data.ShModelTreeNode)e.Node.Tag;

            // 선택된 노드의 속성정보 불러오기
            // Example 1
            List<VIZCore3D.NET.Data.ShNodeAttribute> AttrItems1 = StructureManager.GetProperty(Convert.ToInt32(node.EntityId));
            // Example 2
            List<VIZCore3D.NET.Data.ShNodeAttribute> AttrItems2 = node.NodePropertyList;

            ShowProperty(AttrItems2);
        }

        private void ShowProperty(List<VIZCore3D.NET.Data.ShNodeAttribute> items)
        {
            lvProperty.BeginUpdate();
            lvProperty.Items.Clear();
            foreach (VIZCore3D.NET.Data.ShNodeAttribute item in items)
            {
                ListViewItem lvi = new ListViewItem(new string[] { item.Key, item.Value });
                lvProperty.Items.Add(lvi);
            }
            lvProperty.EndUpdate();
        }
    }


    public class ModelNodeVO
    {
        public long NodeId { get; set; }
        public string NodeName { get; set; }
        public string NodeType { get; set; }
        public string ParentNodeName { get; set; }
        public int ChildrenCount { get; set; }
        public int Depth { get; set; }
        public string NodePath { get; set; }
    }
}