using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ArduinoStandWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Arduino ard;
        public Sensors sens;
        public int ArdX = 560;
        public int ArdY = 220;
        public Dictionary<Port, Port> connections;
        private double currX = -1;
        private double currY = -1;
        private string directory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private Port prevPort;
        private List<Line> lines = new List<Line>();
        private List<int> prevIndex = new List<int>();
        private List<int> comands = new List<int>();
        private Brush brush = Brushes.Green;
        private Point? movePoint;
        private Point targetPoint;
        private Line previousLine;
        private string pName;
        private string fPort;
        private string sPort;
        private int lineCount = 0;
        private int sensorsCount = 0;
        private struct port
        {
            public Image portImage;
            public int coordX;
            public int coordY;
            public Line currLine;
            public string firstPort;

            public port(Image portImage, int coordX, int coordY)
            {
                this.portImage = portImage;
                this.coordX = coordX;
                this.coordY = coordY;
                this.firstPort = "";
                this.currLine = null;

            }
            public void SetLineUid(Line currLine)
            {
                this.currLine = currLine;
            }
            public void SetFirstPort(string firstPort)
            {
                this.firstPort = firstPort;
            }
        }
        private List<Image> canvasPorts = new List<Image>();
        private List<Image> changedPorts = new List<Image>();
        private Dictionary<string, List<port>> canvasElements = new Dictionary<string, List<port>>();

        public MainWindow()
        {
            InitializeComponent();
            Load_Canvas();
            prevIndex.Add(0);

            CommandBinding binding = new CommandBinding(ApplicationCommands.Undo);
            binding.Executed += new ExecutedRoutedEventHandler(Binding_Executed);
            binding.CanExecute += new CanExecuteRoutedEventHandler(Binding_CanExecute);
            this.CommandBindings.Add(binding);
        }

        private void Binding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            if (lineCount > 0 && connections.Count > 0)
            {
                connections.Remove(connections.Keys.Last());
                canvas.Children.Remove(lines[lines.Count - 1]);
                lines.RemoveAt(lines.Count - 1);
                lineCount--;
                for (int it = changedPorts.Count - 1; it != changedPorts.Count - 3; it--)
                {
                    if (changedPorts[it].Uid.StartsWith("portPressed"))
                    {
                        Image i = changedPorts[it];
                        Port p;
                        p = sens.GetPort(i.Name);
                        if (p == null)
                        {
                            p = ard.GetPort(i.Name);
                            p.GetNextPort().Clear();
                            i.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed.png"), UriKind.Absolute));
                        }
                        else
                        {
                            p.GetNextPort().Clear();
                            i.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed_2.png"), UriKind.Absolute));
                        }
                    }
                }
                changedPorts.RemoveAt(changedPorts.Count - 1);
                changedPorts.RemoveAt(changedPorts.Count - 1);
            }
        }

        private void Binding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ArduinoRendering()
        {
            Image img = new Image();
            List<port> canvasPorts = new List<port>();
            canvasElements.Add("Arduino_UNO", canvasPorts);
            img.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory, ard.GetIconPath()), UriKind.Absolute));  //добавить относительный путь
            img.Name = "Arduino_UNO";
            img.Uid = "Arduino_UNO";
            img.MouseDown += ElementOnMouseDown;
            img.MouseUp += ElementOnMouseUp;
            img.MouseMove += ElementOnMouseMove;
            canvas.Children.Add(img);
            Canvas.SetLeft(img, ArdX);
            Canvas.SetTop(img, ArdY);

            foreach (var p in ard.ports)
            {
                Image p1 = new Image();
                canvasPorts.Add(new port(p1, p.Value.GetCoordX(), p.Value.GetCoordY()));
                p1.Uid = "portA";
                p1.Name = p.Value.GetName();
                p1.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed.png"), UriKind.Absolute));
                p1.MouseDown += Port_MouseDown;
                p1.MouseDown += Port_MouseDown;
                p1.MouseEnter += Port_MouseOver;
                p1.MouseLeave += PortA_MouseLeave;
                canvas.Children.Add(p1);
                Canvas.SetLeft(p1, ArdX + p.Value.GetCoordX());
                Canvas.SetTop(p1, ArdY + p.Value.GetCoordY());
                ToolTip t = new ToolTip();
                t.Content = p.Value.GetName();
                p1.ToolTip = t;
            }
        }

        private void SensorsRendering()
        {
            if (sens.sensors.Count <= 10)
            {
                foreach (var item in sens.sensors)
                {
                    Button btn = new Button();
                    btn.Uid = item.Value.GetName();
                    btn.Margin = new Thickness(5, 5, 0, 0);
                    btn.Width = 90;
                    btn.Height = 90;
                    btn.Background = Brushes.Transparent;
                    var brush2 = new ImageBrush();
                    brush2.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(directory, item.Value.GetIconPath()), UriKind.Absolute));

                    if (brush2.ImageSource.Width > btn.Width || brush2.ImageSource.Height > btn.Height)
                        brush2.Stretch = Stretch.Uniform;
                    else
                        brush2.Stretch = Stretch.None;
                    btn.Background = brush2;
                    btn.VerticalAlignment = VerticalAlignment.Top;
                    btn.HorizontalAlignment = HorizontalAlignment.Right;
                    btn.Click += AddSensor;
                    components.Children.Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Load_Canvas()
        {
            ard = new Arduino();
            sens = new Sensors();
            connections = new Dictionary<Port, Port>();
            ArduinoRendering();
            SensorsRendering();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                currX = -1;
                currY = -1;
                prevPort = null;
            }
        }

        private void Change_Color(object sender, SelectionChangedEventArgs e)
        {
            prevIndex.Add(color_changing.SelectedIndex);
            switch (color_changing.SelectedIndex)
            {
                case 0:
                    brush = Brushes.Green;
                    break;
                case 1:
                    brush = Brushes.Red;
                    break;
                case 2:
                    brush = Brushes.Blue;
                    break;
                case 3:
                    brush = Brushes.Yellow;
                    break;
                case 4:
                    brush = Brushes.Purple;
                    break;
                case 5:
                    brush = Brushes.Black;
                    break;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            List<UIElement> itemStoreMove = new List<UIElement>();
            List<string> elementStoreMove = new List<string>();
            foreach (UIElement ui in canvas.Children)
            {
                if (ui.Uid.StartsWith("portPressed"))
                {
                    //ui.Uid = "portUnpressed";
                    Image i = ui as Image;
                    Port p;

                    p = sens.GetPort(i.Name);
                    if (p == null)
                    {
                        p = ard.GetPort(i.Name);
                        p.GetNextPort().Clear();
                        i.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed.png"), UriKind.Absolute));
                    }
                    else
                    {
                        p.GetNextPort().Clear();
                        i.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed_2.png"), UriKind.Absolute));
                    }
                }
            }
            foreach (UIElement ui in canvas.Children)
            {
                if (!ui.Uid.StartsWith("Arduino") && !ui.Uid.StartsWith("portA") && !ui.Uid.StartsWith("portPressedA") && !ui.Uid.StartsWith("portUnpressed"))
                {
                    
                    itemStoreMove.Add(ui);
                }
            }

            foreach (UIElement ui in itemStoreMove)
            {
                canvas.Children.Remove(ui);
            }

            foreach (var el in canvasElements)
            {
                if (el.Key != "Arduino_UNO")
                {
                    elementStoreMove.Add(el.Key);
                }
            }

            foreach (var el in elementStoreMove)
            {
                canvasElements.Remove(el);
            }
            //canvasElements.Clear();
            connections.Clear();
            changedPorts.Clear();
        }

        private void Port_MouseOver(object sender, MouseEventArgs e)
        {
            var portImage = sender as Image;
            if (sens.GetPort(portImage.Name) == null)
            {
                portImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_pressed.png"), UriKind.Absolute));
            }
            else
                portImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_pressed_2.png"), UriKind.Absolute));

        }

        private void PortS_MouseLeave(object sender, MouseEventArgs e)
        {
            var portImage = sender as Image;
            Port port = sens.GetPort(portImage.Name);
            if (port == null)
            {
                port = ard.GetPort(portImage.Name);
            }
            if (port != null && port.GetNextPort().Count == 0)
            {
                portImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed_2.png"), UriKind.Absolute));
            }
        }

        private void PortA_MouseLeave(object sender, MouseEventArgs e)
        {
            var portImage = sender as Image;
            Port port = sens.GetPort(portImage.Name);
            if (port == null)
            {
                port = ard.GetPort(portImage.Name);
            }
            if (port != null && port.GetNextPort().Count == 0)
            {
                portImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed.png"), UriKind.Absolute));
            }
        }

        private void Port_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var i = sender as Image;

            i.Uid = "portPressed";
            var portType = 0;
            Port p = sens.GetPort(i.Name);
            if (p == null)
            {
                i.Uid = "portPressedA";
                portType = 1;
                p = ard.GetPort(i.Name);
            }
            else
            {
                pName = i.Name;
            }
            
            if (e.LeftButton == MouseButtonState.Pressed && p.GetNextPort().Count == 0)
            {
                lineCount++;
                if (portType == 0)
                    i.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_pressed_2.png"), UriKind.Absolute));
                else
                    i.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_pressed.png"), UriKind.Absolute));
                p.GetNextPort().Add("1");

                if (prevPort == null)
                {
                    if (portType == 0)
                    {
                        currX = Canvas.GetLeft(sender as Image) + 3;
                        currY = Canvas.GetTop(sender as Image) + 3;
                    }
                    else
                    {
                        currX = Canvas.GetLeft(sender as Image) + 5;
                        currY = Canvas.GetTop(sender as Image) + 5;
                    }
                    prevPort = p;
                    fPort = i.Name;
                    changedPorts.Add(i);
                }
                else
                {
                    Line line1 = new Line();
                    line1.StrokeThickness = 2.5;
                    line1.Stroke = brush;
                    line1.X1 = currX;
                    line1.Y1 = currY;
                    if (portType == 0)
                    {
                        line1.X2 = Canvas.GetLeft(sender as Image) + 3;
                        line1.Y2 = Canvas.GetTop(sender as Image) + 3;
                        currX = Canvas.GetLeft(sender as Image) + 3;
                        currY = Canvas.GetTop(sender as Image) + 3;
                    }
                    else
                    {
                        line1.X2 = Canvas.GetLeft(sender as Image) + 5;
                        line1.Y2 = Canvas.GetTop(sender as Image) + 5;
                        currX = Canvas.GetLeft(sender as Image) + 5;
                        currY = Canvas.GetTop(sender as Image) + 5;
                    }
                    line1.Uid = "Line" + lineCount;
                    lines.Add(line1);
                    canvas.Children.Add(line1);
                    if (portType == 0)
                    {
                        connections.Add(p, prevPort);
                    }
                    else
                    {
                        double x1 = line1.X1;
                        double y1 = line1.Y1;
                        line1.X1 = line1.X2;
                        line1.Y1 = line1.Y2;
                        line1.X2 = x1;
                        line1.Y2 = y1;
                        connections.Add(prevPort, p);
                    }
                    prevPort = null;
                    sPort = i.Name;
                    changedPorts.Add(i);

                    foreach (var c in canvasElements)
                    {
                        for (int t = 0; t < c.Value.Count; t++)
                        {
                            if (fPort == c.Value[t].portImage.Name)
                            {
                                Trace.WriteLine("fPort = " + fPort);
                                port temp = c.Value[t];
                                temp.SetLineUid(line1);
                                if(portType == 0)
                                    temp.SetFirstPort(fPort); // there
                                else
                                    temp.SetFirstPort(sPort);
                                c.Value[t] = temp;
                            }
                            if (sPort == c.Value[t].portImage.Name)
                            {
                                Trace.WriteLine("sPort = " + sPort);
                                port temp = c.Value[t];
                                temp.SetLineUid(line1);
                                if (portType == 0)
                                    temp.SetFirstPort(fPort);
                                else
                                    temp.SetFirstPort(sPort); // there
                                c.Value[t] = temp;
                            }
                        }
                    }
                    fPort = "";
                    sPort = "";
                }
                
            }
        }

        private void Conf_Click(object sender, RoutedEventArgs e)
        {
            TopFileGenerator();
            PinAssignmentsFileGenerator();
            PythonScriptExecution();
        }

        private void TopFileGenerator()
        {
            StreamWriter sw = new StreamWriter(directory + "/../../../project_855/top.sv");

            List<string> ls = new List<string>();

            sw.WriteLine("module top(");
            int i = 1;
            foreach (var item in connections)
            {
                if (i != connections.Count)
                {
                    if (item.Key.GetPortType() == "in")
                    {
                        sw.WriteLine("    output " + item.Key.GetAddress() + ",");
                        sw.WriteLine("    input " + item.Value.GetAddress() + ",");
                        ls.Add("assign " + item.Key.GetAddress() + " = " + item.Value.GetAddress() + ";");
                    }
                    if (item.Key.GetPortType() == "out")
                    {
                        sw.WriteLine("    input " + item.Key.GetAddress() + ",");
                        sw.WriteLine("    output " + item.Value.GetAddress() + ",");
                        ls.Add("assign " + item.Value.GetAddress() + " = " + item.Key.GetAddress() + ";");
                    }
                    i++;
                }
                else
                {
                    if (item.Key.GetPortType() == "in")
                    {
                        sw.WriteLine("    output " + item.Key.GetAddress() + ",");
                        sw.WriteLine("    input " + item.Value.GetAddress());
                        ls.Add("assign " + item.Key.GetAddress() + " = " + item.Value.GetAddress() + ";");
                    }
                    if (item.Key.GetPortType() == "out")
                    {
                        sw.WriteLine("    input " + item.Key.GetAddress() + ",");
                        sw.WriteLine("    output " + item.Value.GetAddress());
                        ls.Add("assign " + item.Value.GetAddress() + " = " + item.Key.GetAddress() + ";");
                    }
                    i++;
                }
            }

            sw.WriteLine(");");

            foreach (var item in ls)
            {
                sw.WriteLine(item);
            }

            sw.WriteLine("endmodule");

            sw.Close();
        }

        private void PinAssignmentsFileGenerator()
        {
            StreamWriter sw = new StreamWriter(directory + "/../../../project_855/project_855.qsf");

            sw.WriteLine("set_global_assignment -name FAMILY \"MAX 10\"");
            sw.WriteLine("set_global_assignment -name DEVICE 10M50DAF484C7G");
            sw.WriteLine("set_global_assignment -name TOP_LEVEL_ENTITY top");
            sw.WriteLine("set_global_assignment -name ORIGINAL_QUARTUS_VERSION 18.1.0");
            sw.WriteLine("set_global_assignment -name PROJECT_CREATION_TIME_DATE \"01:45:41  MARCH 18, 2022\"");
            sw.WriteLine("set_global_assignment -name LAST_QUARTUS_VERSION \"18.1.0 Lite Edition\"");
            sw.WriteLine("set_global_assignment -name PROJECT_OUTPUT_DIRECTORY output_files");
            sw.WriteLine("set_global_assignment -name MIN_CORE_JUNCTION_TEMP 0");
            sw.WriteLine("set_global_assignment -name MAX_CORE_JUNCTION_TEMP 85");
            sw.WriteLine("set_global_assignment -name ERROR_CHECK_FREQUENCY_DIVISOR 256");
            sw.WriteLine("set_global_assignment -name EDA_SIMULATION_TOOL \"ModelSim -Altera (Verilog)\"");
            sw.WriteLine("set_global_assignment -name SYSTEMVERILOG_FILE top.sv");

            foreach (var item in connections)
            {
                sw.WriteLine("set_location_assignment " + item.Key.GetAddress() + " -to " + item.Key.GetAddress());
                sw.WriteLine("set_location_assignment " + item.Value.GetAddress() + " -to " + item.Value.GetAddress());
            }
            sw.WriteLine("set_instance_assignment -name PARTITION_HIERARCHY root_partition -to | -section_id Top");
            sw.Close();
        }

        private void PythonScriptExecution()
        {

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"..\..\..\project_855\SOF_TO_FPGA_4.py";
            Process.Start(psi);

        }

        private void AddSensor(object sender, RoutedEventArgs e)
        {
            string ui = (sender as Button).Uid;
            foreach (var item in sens.sensors)
            {
                if (item.Value.GetName() == ui && !canvasElements.ContainsKey(item.Value.GetName()))
                {
                    Image s = new Image();
                    List<port> canvasPorts = new List<port>();
                    canvasElements.Add(item.Value.GetName(), canvasPorts);
                    s.Uid = ui;
                    s.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory, item.Value.GetIconPath()), UriKind.Absolute));
                    canvas.Children.Add(s);
                    s.MouseDown += ElementOnMouseDown;
                    s.MouseUp += ElementOnMouseUp;
                    s.MouseMove += ElementOnMouseMove;
                    Canvas.SetLeft(s, 450);
                    Canvas.SetTop(s, 100);
                    ToolTip st = new ToolTip();
                    st.Content = item.Value.GetName();
                    s.ToolTip = st;

                    foreach (var p in item.Value.ports)
                    {
                        Image p1 = new Image();
                        canvasPorts.Add(new port(p1, p.Value.GetCoordX(), p.Value.GetCoordY()));
                        p1.Name = p.Value.GetName();
                        p1.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directory + "/../../../Resourses/port_unpressed_2.png"), UriKind.Absolute));
                        p1.MouseDown += Port_MouseDown;
                        p1.MouseEnter += Port_MouseOver;
                        p1.MouseLeave += PortS_MouseLeave;
                        p1.MouseMove += ElementOnMouseMove;
                        canvas.Children.Add(p1);
                        Canvas.SetLeft(p1, Canvas.GetLeft(s) + p.Value.GetCoordX());     //// 1) Сделать относительно датчика, чтобы прописывались координаты относительно датчика
                        Canvas.SetTop(p1, Canvas.GetTop(s) + p.Value.GetCoordY());
                        ToolTip t = new ToolTip();
                        t.Content = p.Value.GetName();
                        p1.ToolTip = t;
                    }
                    sensorsCount++;
                }
            }
        }

        private void ElementOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var targetElement = e.Source as IInputElement;
            if (targetElement != null)
            {
                targetPoint = e.GetPosition(targetElement);
                targetElement.CaptureMouse();
            }
        }

        private void ElementOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void ElementOnMouseMove(object sender, MouseEventArgs e)
        {
            var targetElement = Mouse.Captured as UIElement;
            
            if (e.LeftButton == MouseButtonState.Pressed && targetElement != null)
            {
                Image sensor = sender as Image;
                foreach (var s in canvasElements)
                {
                    if (s.Key == sensor.Uid)
                    {
                        foreach (var p in s.Value)
                        {
                            Trace.WriteLine("targetElement.Uid = " + targetElement.Uid);
                            Trace.WriteLine("first port = " + p.firstPort);
                            if (p.currLine != null && p.firstPort.StartsWith(targetElement.Uid))
                            {
                                Trace.WriteLine("first el");
                                p.currLine.X1 = Canvas.GetLeft(targetElement) + p.coordX + 3;
                                p.currLine.Y1 = Canvas.GetTop(targetElement) + p.coordY + 3;
                            }
                            else if (p.currLine != null)
                            {
                                Trace.WriteLine("second el");
                                p.currLine.X2 = Canvas.GetLeft(targetElement) + p.coordX + 3;
                                p.currLine.Y2 = Canvas.GetTop(targetElement) + p.coordY + 3;
                            }
                            Canvas.SetLeft(p.portImage, Canvas.GetLeft(targetElement) + p.coordX);
                            Canvas.SetTop(p.portImage, Canvas.GetTop(targetElement) + p.coordY);
                        }
                    }
                }
                var pCanvas = e.GetPosition(canvas);
                Canvas.SetLeft(targetElement, pCanvas.X - targetPoint.X);
                Canvas.SetTop(targetElement, pCanvas.Y - targetPoint.Y);
            }


        }
    }
}
