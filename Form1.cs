using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCvSharp;


namespace LabelClear
{
    public partial class Form1 : Form
    {
        private JArray _labelData = [];
        private List<(List<OpenCvSharp.Point> Points, JObject Annotation)> _polygons = [];

        private List<OpenCvSharp.Point> _currentPolygonPoint = [];

        private JObject? _currentAnnotation = null;

        private Mat _originalImage = new Mat();

        private bool isEdit = false;

        private ContextMenuStrip _contextMenu = new();

        string _rootPath = string.Empty;

        private float scale = .0f;
        private float offsetX = .0f;
        private float offsetY = .0f;
        private Mat _tempImage;

        public Form1()
        {
            InitializeComponent();

            _rootPath = "F:\\m_work\\ocrSDK\\open-mantra-dataset\\";

            parseLabel(Path.Combine(_rootPath, "dataset.json"));

            _contextMenu.Items.Add("编辑", null, EditAnnotationMenuItem_Click);

        }

        private void openPathStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = folderBrowserDialog.SelectedPath;
                    pathTreeView.Nodes.Clear();
                    LoadDirectory(path, pathTreeView.Nodes);
                }
            }
        }

        private void LoadDirectory(string path, TreeNodeCollection parentNode)
        {
            string[] imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

            try
            {
                var directoryInfo = new DirectoryInfo(path);
                var directoryNode = new TreeNode(directoryInfo.Name) { Tag = directoryInfo.FullName };
                parentNode.Add(directoryNode);

                foreach (var dir in directoryInfo.GetDirectories())
                {
                    LoadDirectory(dir.FullName, directoryNode.Nodes);
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    if (imageExtensions.Contains(file.Extension.ToLower()))
                    {
                        var fileNode = new TreeNode(file.Name) { Tag = file.FullName };
                        directoryNode.Nodes.Add(fileNode);
                    }
                }

                directoryNode.Expand();
            }
            catch { }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {

            if (!isEdit || _currentPolygonPoint.Count == 0) return;
            var mouseEventArgs = e;
            if (mouseEventArgs == null) return;



            if (mouseEventArgs.X < offsetX || mouseEventArgs.X > offsetX + (pictureBox.Image.Size.Width * scale) ||
                mouseEventArgs.Y < offsetY || mouseEventArgs.Y > offsetY + (pictureBox.Image.Size.Height * scale))
            {
                return;
            }

            var imageX = (mouseEventArgs.X - offsetX) / scale;
            var imageY = (mouseEventArgs.Y - offsetY) / scale;

            Mat tempImage = _originalImage.Clone();
            // 绘制当前多边形
            if (_currentPolygonPoint.Count > 0)
            {
                Cv2.Polylines(tempImage, [[.. _currentPolygonPoint]], false, Scalar.Blue, 2);

                // 绘制从最后一个点到鼠标位置的线段
                var lastPoint = _currentPolygonPoint[^1];
                Cv2.Line(tempImage, lastPoint, new OpenCvSharp.Point((int)imageX, (int)imageY), Scalar.Green, 2);
            }

            // 更新显示
            pictureBox.Image?.Dispose();
            pictureBox.Image = Image.FromStream(tempImage.ToMemoryStream());
            tempImage.Dispose();
        }

        private (float scale, float offsetX, float offsetY) GetImageScaleAndOffset()
        {
            var pictureBoxSize = pictureBox.ClientSize;
            var imageSize = pictureBox.Image.Size;

            var scaleX = (float)pictureBoxSize.Width / imageSize.Width;
            var scaleY = (float)pictureBoxSize.Height / imageSize.Height;
            var scale = Math.Min(scaleX, scaleY);

            var imageDisplayWidth = imageSize.Width * scale;
            var imageDisplayHeight = imageSize.Height * scale;

            var offsetX = (pictureBoxSize.Width - imageDisplayWidth) / 2;
            var offsetY = (pictureBoxSize.Height - imageDisplayHeight) / 2;

            return (scale, offsetX, offsetY);
        }


        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null)
            {
                MessageBox.Show("No image loaded. Please select an image from the tree view.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e is not MouseEventArgs mouseEventArgs) return;

            if (!isEdit)
            {
                if (mouseEventArgs.Button == MouseButtons.Right)
                {
                    _contextMenu.Show(pictureBox, mouseEventArgs.Location);
                }
                return;
            }


            if (mouseEventArgs.X < offsetX || mouseEventArgs.X > offsetX + (pictureBox.Image.Size.Width * scale) ||
                mouseEventArgs.Y < offsetY || mouseEventArgs.Y > offsetY + (pictureBox.Image.Size.Height * scale))
            {
                return;
            }
            var imageX = (mouseEventArgs.X - offsetX) / scale;
            var imageY = (mouseEventArgs.Y - offsetY) / scale;

            if (mouseEventArgs.Button == MouseButtons.Right)
            {

                foreach (var (points, annotation) in _polygons)
                {
                    if (IsPointInPolygon(new OpenCvSharp.Point((int)imageX, (int)imageY), points))
                    {
                        DeleteAnnotation(annotation);
                        return;
                    }
                }
            }
            else if (mouseEventArgs.Button == MouseButtons.Left)
            {
                if (pictureBox.Image == null) return;

                _currentPolygonPoint.Add(new OpenCvSharp.Point((int)imageX, (int)imageY));
                Mat tempImage = _originalImage.Clone();
                if (_currentPolygonPoint.Count > 1)
                {
                    Cv2.Polylines(tempImage, [[.. _currentPolygonPoint]], false, Scalar.Blue, 2);
                }

                if (_currentPolygonPoint.Count >= 4 && distance(_currentPolygonPoint[0], _currentPolygonPoint[^1]) < 10)
                {
                    _currentPolygonPoint[^1] = _currentPolygonPoint[0];
                    JArray polygon = [];
                    foreach (var point in _currentPolygonPoint)
                    {
                        polygon.Add(point.X);
                        polygon.Add(point.Y);
                    }

                    JObject newAnnotation = new()
                    {
                        ["bbox"] = GetBoundingBox(polygon),
                        ["bbox_label"] = 1,
                        ["polygon"] = polygon,
                        ["ignore"] = false,
                        ["text"] = ""
                    };

                    if (_currentAnnotation != null && _currentAnnotation["instances"] is JArray instances)
                    {
                        instances.Add(newAnnotation);
                        RefreshImage(instances);


                    }
                    _currentPolygonPoint.Clear();
                }
                jsonPreviewBox.Clear();
                jsonPreviewBox.Text = _currentAnnotation?.ToString(Formatting.Indented);



                pictureBox.Image?.Dispose();
                pictureBox.Image = Image.FromStream(tempImage.ToMemoryStream());
                tempImage.Dispose();
            }

        }

        private JArray GetBoundingBox(JArray polygon)
        {
            if (polygon == null || polygon.Count < 2) return new JArray();

            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;

            for (int i = 0; i < polygon.Count; i += 2)
            {
                int x = polygon[i].ToObject<int>();
                int y = polygon[i + 1].ToObject<int>();

                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
            }

            int width = maxX - minX;
            int height = maxY - minY;

            return [minX, minY, width, height];
        }

        private float distance(OpenCvSharp.Point p1, OpenCvSharp.Point p2)
        {
            return (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private void EditAnnotationMenuItem_Click(object? sender, EventArgs e)
        {
            isEdit = true;
            this.cancelButton.Visible = true;
        }

        private void pathTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node == null || e.Node.Tag == null)
            {
                jsonPreviewBox.Clear();

                pictureBox.Image?.Dispose();
                pictureBox.Image = null;
                MessageBox.Show("No image selected or node is invalid.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (e.Node.Tag != null && File.Exists(e.Node.Tag as string))
            {
                try
                {


                    var filePath = e.Node.Tag as string;
                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        jsonPreviewBox.Clear();

                        pictureBox.Image?.Dispose();
                        pictureBox.Image = null;
                        return;
                    }
                    jsonPreviewBox.Clear();
                    pictureBox.Image?.Dispose();

                    _originalImage = new Mat();
                    string abs_img_path = string.Empty;
                    foreach (JObject item in _labelData)
                    {
                        abs_img_path = Path.Combine(_rootPath, item["img_path"]?.ToString() ?? string.Empty);
                        // MessageBox.Show($"compare: {abs_img_path} with {filePath}");
                        if (filePath == abs_img_path)
                        {
                            _currentAnnotation = item;

                            _originalImage = Cv2.ImRead(abs_img_path, ImreadModes.Color);
                            foreach (JObject ob in item["instances"] ?? new JArray())
                            {
                                if (ob["ignore"]?.ToObject<bool>() == true)
                                {
                                    continue;
                                }
                                var polygon = ob["polygon"]?.ToObject<int[]>();
                                var points = new List<OpenCvSharp.Point>();
                                for (int i = 0; i < polygon?.Length; i += 2)
                                {
                                    points.Add(new OpenCvSharp.Point(polygon[i], polygon[i + 1]));
                                }
                                Cv2.Polylines(_originalImage, new[] { points.ToArray() }, true, Scalar.Red, 2);

                                _polygons.Add((points, ob));

                            }
                            jsonPreviewBox.Text = item.ToString(Formatting.Indented);
                            break;
                        }
                    }
                    if (_originalImage.Empty())
                    {
                        MessageBox.Show($"Failed to load image. File Path: {abs_img_path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    pictureBox.Image = Image.FromStream(_originalImage.ToMemoryStream());

                    (scale, offsetX, offsetY) = GetImageScaleAndOffset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                jsonPreviewBox.Clear();
                pictureBox.Image?.Dispose();
                pictureBox.Image = null;
            }
        }

        private void parseLabel(string labelPath)
        {
            if (!File.Exists(labelPath))
            {
                MessageBox.Show("Label file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(labelPath);
                JObject jsonObject = JObject.Parse(jsonContent);
                var data_list = jsonObject["data_list"] as JArray;
                if (data_list == null)
                {
                    MessageBox.Show("The 'data_list' field is missing or not a valid JSON object.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _labelData = data_list;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading label file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool IsPointInPolygon(OpenCvSharp.Point point, List<OpenCvSharp.Point> polygon)
        {
            int n = polygon.Count;
            bool inside = false;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if ((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y) &&
                    point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private void DeleteAnnotation(JObject annotation)
        {
            foreach (var item in _labelData)
            {
                var instances = item["instances"] as JArray;
                if (instances != null && instances.Contains(annotation))
                {
                    instances.Remove(annotation);

                    jsonPreviewBox.Clear();
                    jsonPreviewBox.Text = item.ToString(Formatting.Indented);
                    RefreshImage(instances);
                    break;
                }
            }
        }

        private void RefreshImage(JArray? instances)
        {
            if (pictureBox.Image == null) return;
            var filePath = pathTreeView.SelectedNode?.Tag as string;
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;
            if (instances == null || instances.Count == 0)
            {
                foreach (JObject it in _labelData)
                {
                    // MessageBox.Show($"compare: {_rootPath + it["img_path"]?.ToString()} with {Path.GetFileName(filePath)}");
                    if ((_rootPath + it["img_path"]?.ToString()) == filePath)
                    {
                        instances = it["instances"] as JArray;
                        break;
                    }
                }
                if (instances == null || instances.Count == 0)
                {
                    MessageBox.Show("No annotations found for the selected image.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            _originalImage = Cv2.ImRead(filePath, ImreadModes.Color);
            _polygons.Clear();

            foreach (var ins in instances)
            {
                var polygon = ins["polygon"]?.ToObject<int[]>();
                if (polygon == null) continue;

                var points = new List<OpenCvSharp.Point>();
                for (int i = 0; i < polygon.Length; i += 2)
                {
                    points.Add(new OpenCvSharp.Point(polygon[i], polygon[i + 1]));
                }

                Cv2.Polylines(_originalImage, new[] { points.ToArray() }, true, Scalar.Red, 2);
                var annotation = ins as JObject;
                if (annotation != null)
                {
                    _polygons.Add((points, annotation));
                }
            }

            pictureBox.Image?.Dispose();
            pictureBox.Image = Image.FromStream(_originalImage.ToMemoryStream());
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (pathTreeView.SelectedNode?.Parent != null)
            {
                pathTreeView.SelectedNode = pathTreeView.SelectedNode.Parent;
            }
            else if (pathTreeView.Nodes.Count > 0)
            {
                pathTreeView.SelectedNode = pathTreeView.Nodes[0];
            }
            else
            {
                MessageBox.Show("No parent node available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (pathTreeView.SelectedNode != null)
            {
                pathTreeView_AfterSelect(this, new TreeViewEventArgs(pathTreeView.SelectedNode));
            }
            else
            {
                pictureBox.Image?.Dispose();
                pictureBox.Image = null;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string labelPath = Path.Combine(_rootPath, "test_dataset.json");
                JObject jsonObject = new JObject
                {
                    ["metainfo"] = new JObject
                    {
                        ["dataset_type"] = "OCRDataset",
                        ["task_name"] = "text_detection"
                    },
                    ["data_list"] = _labelData
                };
                File.WriteAllText(labelPath, jsonObject.ToString(Formatting.Indented));
                MessageBox.Show("Annotations saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving annotations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            isEdit = false;
            this.cancelButton.Visible = false;
            _currentPolygonPoint.Clear();
            RefreshImage(null);
        }

        private void annMergeButton_Click(object sender, EventArgs e)
        {
            if (_currentAnnotation == null)
            {
                MessageBox.Show("No annotation selected to merge.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_currentAnnotation?["instances"] is JArray instancesArray)
            {
                var newInstances = new JArray();

                for (int i = 0; i < instancesArray.Count; i++)
                {
                    var instance1 = instancesArray[i] as JObject;
                    var polygon1 = instance1?["polygon"]?.ToObject<int[]>();
                    if (polygon1 == null) continue;

                    bool merged = false;
                    for (int j = i + 1; j < instancesArray.Count; j++)
                    {
                        var instance2 = instancesArray[j] as JObject;
                        var polygon2 = instance2?["polygon"]?.ToObject<int[]>();
                        if (polygon2 == null) continue;

                        if (ArePolygonsClose(polygon1, polygon2))
                        {
                            var mergedPolygon = MergePolygons(polygon1, polygon2);
                            if (instance1 != null)
                            {
                                instance1["polygon"] = JArray.FromObject(mergedPolygon);
                                if (instance1["polygon"] is JArray polygon)
                                {
                                    instance1["bbox"] = GetBoundingBox(polygon);
                                }
                            }
                            if (instance2 != null)
                            {
                                instancesArray.RemoveAt(j);
                                j--; // Adjust index after removal
                            }
                            merged = true;
                        }
                    }
                    if (!merged)
                    {
                        // MessageBox.Show($"Polygon {i + 1} did not merge with any other polygons.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (instance1 != null)
                        {
                            newInstances.Add(instance1);
                        }
                    }
                }

                instancesArray = newInstances;
                RefreshImage(null);
                jsonPreviewBox.Clear();
                jsonPreviewBox.Text = _currentAnnotation.ToString(Formatting.Indented);
            }

        }

        private bool ArePolygonsClose(int[] polygon1, int[] polygon2, int threshold = 10)
        {
            var bbox1 = GetBoundingBox(polygon1);
            var bbox2 = GetBoundingBox(polygon2);
            int distanceX = Math.Max(0, Math.Max(bbox1[0] - (bbox2[0] + bbox2[2]), bbox2[0] - (bbox1[0] + bbox1[2])));
            int distanceY = Math.Max(0, Math.Max(bbox1[1] - (bbox2[1] + bbox2[3]), bbox2[1] - (bbox1[1] + bbox1[3])));
            return distanceX <= threshold && distanceY <= threshold;
        }

        private int[] MergePolygons(int[] polygon1, int[] polygon2)
        {
            // 将两个多边形的点合并
            var points = new List<OpenCvSharp.Point>();
            for (int i = 0; i < polygon1.Length; i += 2)
            {
                points.Add(new OpenCvSharp.Point(polygon1[i], polygon1[i + 1]));
            }
            for (int i = 0; i < polygon2.Length; i += 2)
            {
                points.Add(new OpenCvSharp.Point(polygon2[i], polygon2[i + 1]));
            }

            // 使用凸包算法生成新的多边形
            var hull = Cv2.ConvexHull(points);

            // 转换为 int[] 格式
            return hull.SelectMany(p => new[] { (int)p.X, (int)p.Y }).ToArray();
        }

        private int[] GetBoundingBox(int[] polygon)
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;

            for (int i = 0; i < polygon.Length; i += 2)
            {
                int x = polygon[i];
                int y = polygon[i + 1];

                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
            }

            return new[] { minX, minY, maxX - minX, maxY - minY }; // 返回 [x, y, width, height]
        }


    }
}
