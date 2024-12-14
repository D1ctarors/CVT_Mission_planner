/*

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using MissionPlanner;
using IronPython.Runtime.Operations;
using System.Linq;

namespace YourNamespace
{
    public class RoundedLabel : Label
    {
        public int CornerRadius { get; set; } = 15;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Создаём графический путь с закруглёнными углами
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90); // Левый верхний угол
                path.AddArc(Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90); // Правый верхний угол
                path.AddArc(Width - CornerRadius, Height - CornerRadius, CornerRadius, CornerRadius, 0, 90); // Правый нижний угол
                path.AddArc(0, Height - CornerRadius, CornerRadius, CornerRadius, 90, 90); // Левый нижний угол
                path.CloseAllFigures();

                // Устанавливаем сглаживание
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Заливаем фон
                using (Brush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Рисуем текст
                TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }

    public class FormEmpty : Form
    {
        private FlowLayoutPanel leftPanel, rightPanel;
        private Panel middlePanel;
        private Label lblTemp1, lblTemp2, lblPressure, lblCurrent, lblBatteryVoltage, lblOutputVoltage, lblTeVoltage, lblRuntime, lblFan, lblCounter;

        private void FormEmpty_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormEmpty
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "FormEmpty";
            this.Load += new System.EventHandler(this.FormEmpty_Load);
            this.ResumeLayout(false);

        }

        // Хранение статусов и ошибок как Label
        private Dictionary<string, Label> statusLabels = new Dictionary<string, Label>();
        private Dictionary<string, Label> errorLabels = new Dictionary<string, Label>();

        public FormEmpty()
        {
            this.Text = "Данные с водородной установки";
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 500);

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Black,
                Padding = new Padding(10)
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Левая колонка - 30% от ширины окна
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Центральная колонка - 36%
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Правая колонка - 30%
            this.Controls.Add(mainTable);

            // Левая панель - Статусы
            leftPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0, 0, 10, 0),
                AutoScroll = true
            };
            mainTable.Controls.Add(leftPanel, 0, 0);

            // Центральная панель - Данные
            middlePanel = new Panel
            {
                BackColor = Color.Black,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 10, 0)
            };
            mainTable.Controls.Add(middlePanel, 1, 0);

            // Правая панель - Ошибки
            rightPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0, 0, 0, 0),
                AutoScroll = true
            };
            mainTable.Controls.Add(rightPanel, 2, 0);

            // **Заполнение статусов**
            string[] statusTexts = { "Авария", "Запуск", "Работа", "Нагрузка ВКЛ", "Закоротка ТЭ", "Клапан подачи", "Клапан сброса" };
            foreach (var text in statusTexts)
            {
                Label lbl = CreateStatusLabel(text);
                statusLabels[text] = lbl;
                leftPanel.Controls.Add(lbl);
            }

            // **Заполнение ошибок**
            string[] errorTexts = { "Низкий ЭДС аккумулятора", "Давление по датчику", "Низкое давление по СК", "Утечка водорода по СК", "ЭДС ТЭ", "Перегрев ТЭ" };
            foreach (var etext in errorTexts)
            {
                Label lbl = CreateErrorLabel(etext);
                errorLabels[etext] = lbl;
                rightPanel.Controls.Add(lbl);
            }

            // Создаем элементы для отображения параметров установки
            lblTemp1 = CreateLabel("Температура 1:");
            lblTemp2 = CreateLabel("Температура 2:");
            lblPressure = CreateLabel("Давление:");
            lblCurrent = CreateLabel("Ток:");
            lblBatteryVoltage = CreateLabel("Напряжение аккумулятора:");
            lblOutputVoltage = CreateLabel("Выходное напряжение:");
            lblTeVoltage = CreateLabel("Напряжение ТЭ:");
            lblRuntime = CreateLabel("Время работы:");
            lblFan = CreateLabel("ШИМ вентилятора:");
            lblCounter = CreateLabel("Счетчик:");

            // Добавляем их в центральную панель middlePanel
            middlePanel.Controls.Add(lblTemp1);
            middlePanel.Controls.Add(lblTemp2);
            middlePanel.Controls.Add(lblPressure);
            middlePanel.Controls.Add(lblCurrent);
            middlePanel.Controls.Add(lblBatteryVoltage);
            middlePanel.Controls.Add(lblOutputVoltage);
            middlePanel.Controls.Add(lblTeVoltage);
            middlePanel.Controls.Add(lblRuntime);
            middlePanel.Controls.Add(lblFan);
            middlePanel.Controls.Add(lblCounter);

            // Устанавливаем их расположение
            lblTemp1.Location = new Point(0, 0);
            lblTemp2.Location = new Point(0, 30);
            lblPressure.Location = new Point(0, 60);
            lblCurrent.Location = new Point(0, 90);
            lblBatteryVoltage.Location = new Point(0, 120);
            lblOutputVoltage.Location = new Point(0, 150);
            lblTeVoltage.Location = new Point(0, 180);
            lblRuntime.Location = new Point(0, 210);
            lblFan.Location = new Point(0, 240);
            lblCounter.Location = new Point(0, 270);
        }

        // Создаем Label для статусов
        private Label CreateStatusLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 56, 208, 89), // Базовый цвет статуса
                Font = new Font("Arial", 10, FontStyle.Regular),
                AutoSize = false,
                Width = 230,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 15)
            };
        }

        // Создаем Label для ошибок
        private Label CreateErrorLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 208, 56, 56), // Базовый цвет ошибки
                Font = new Font("Arial", 10, FontStyle.Regular),
                AutoSize = false,
                Width = 230,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 15)
            };
        }

        /*
          // Создаем RoundedLabel для статусов
        private RoundedLabel CreateStatusLabel(string text)
        {
            return new RoundedLabel
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 56, 208, 89), // Базовый цвет статуса
                Font = new Font("Arial", 10, FontStyle.Regular),
                AutoSize = false,
                Width = 200,
                Height = 40,
                CornerRadius = 15,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 0)
            };
        }

        // Создаем RoundedLabel для ошибок
        private RoundedLabel CreateErrorLabel(string text)
        {
            return new RoundedLabel
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 208, 56, 56), // Базовый цвет ошибки
                Font = new Font("Arial", 10, FontStyle.Regular),
                AutoSize = false,
                Width = 200,
                Height = 40,
                CornerRadius = 15,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 0)
            };
        }
         

        // **Создаем Label для параметров**
        private Label CreateLabel(string text)
        {
            return new Label
            {
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Consolas", 10, FontStyle.Regular),
                AutoSize = true,
                Text = text
            };
        }

        // **Обновление данных установки**
        public void UpdatePlantData(CurrentState cs)
        {
            if (cs == null) return;

            // Проверяем, находимся ли мы в UI-потоке
            if (this.InvokeRequired)
            {
                // Переносим выполнение на основной поток
                this.Invoke(new Action(() => UpdatePlantData(cs)));
                return;
            }

            // **Обновляем значения параметров центральной панели**
            lblTemp1.Text = "Температура 1: " + cs.h_plant_temp1.ToString() + " °C";
            lblTemp2.Text = "Температура 2: " + cs.h_plant_temp2.ToString() + " °C";
            lblPressure.Text = "Давление: " + cs.h_plant_pressure.ToString() + " hPa";
            lblCurrent.Text = "Ток: " + cs.h_plant_current.ToString() + " A";
            lblBatteryVoltage.Text = "Напряжение аккумулятора: " + cs.h_plant_battery_voltage.ToString() + " V";
            lblOutputVoltage.Text = "Выходное напряжение: " + cs.h_plant_output_voltage.ToString() + " V";
            lblTeVoltage.Text = "Напряжение ТЭ: " + cs.h_plant_te_voltage.ToString() + " V";
            lblRuntime.Text = "Время работы: " + cs.h_plant_runtime.ToString() + " с";
            lblFan.Text = "ШИМ вентилятора: " + cs.h_plant_fan.ToString() + " %";
            lblCounter.Text = "Счетчик: " + cs.h_plant_counter.ToString();

            // **Обновляем цвета для статусов**
            var activeStatuses = cs.GetStatusDescription().Split(',');
            foreach (var status in statusLabels)
            {
                if (activeStatuses.Contains(status.Key.Trim()))
                {
                    status.Value.BackColor = Color.FromArgb(191, 56, 208, 89); // Активный статус
                }
                else
                {
                    status.Value.BackColor = Color.FromArgb(51, 56, 208, 89); // Базовый статус
                }
            }

            // **Обновляем цвета для ошибок**
            var activeErrors = cs.GetErrorsDescription().Split(',');
            foreach (var error in errorLabels)
            {
                if (activeErrors.Contains(error.Key.Trim()))
                {
                    error.Value.BackColor = Color.FromArgb(191, 208, 56, 56); // Активная ошибка
                }
                else
                {
                    error.Value.BackColor = Color.FromArgb(51, 208, 56, 56); // Базовая ошибка
                }
            }
        }

    }
}
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using MissionPlanner;
using IronPython.Runtime.Operations;
using System.Linq;

namespace YourNamespace
{
    public class RoundedLabel : Label
    {
        public int CornerRadius { get; set; } = 15;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
                path.AddArc(Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
                path.AddArc(Width - CornerRadius, Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                path.AddArc(0, Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                path.CloseAllFigures();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (Brush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }

    public class FormEmpty : Form
    {
        private FlowLayoutPanel leftPanel, rightPanel;
        private TableLayoutPanel middleTable;
        private Dictionary<string, Label> statusLabels = new Dictionary<string, Label>();
        private Dictionary<string, Label> errorLabels = new Dictionary<string, Label>();

        private Label lblTemp1, lblTemp2, lblPressure, lblCurrent, lblBatteryVoltage, lblOutputVoltage, lblTeVoltage, lblRuntime, lblFan, lblCounter;

        private Label CreateHeaderLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 8)
            };
        }

        public FormEmpty()
        {
            this.Text = "Данные с водородной установки";
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 500);

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                BackColor = Color.Black,
                Padding = new Padding(10)
            };
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.Controls.Add(mainTable);

            Label lblStatusHeader = CreateHeaderLabel("Статус");
            Label lblDataHeader = CreateHeaderLabel("Показания");
            Label lblErrorHeader = CreateHeaderLabel("Ошибки");

            mainTable.Controls.Add(lblStatusHeader, 0, 0); // Вставляем в первую колонку, первую строку
            mainTable.Controls.Add(lblDataHeader, 1, 0);   // Вставляем во вторую колонку, первую строку
            mainTable.Controls.Add(lblErrorHeader, 2, 0);  // Вставляем в третью колонку, первую строку


            // Левая панель - Статусы
            leftPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0, 0, 0, 0),
                AutoScroll = true
            };
            mainTable.Controls.Add(leftPanel, 0, 1);

            // Центральная таблица - Данные
            middleTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                AutoSize = true,
                Padding = new Padding(10, 0, 10, 0)
            };
            middleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60)); // Названия параметров
            middleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40)); // Значения параметров
            mainTable.Controls.Add(middleTable, 1, 1);

            // Правая панель - Ошибки
            rightPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0, 0, 0, 0),
                AutoScroll = true
            };
            mainTable.Controls.Add(rightPanel, 2, 1);

            // **Заполнение статусов**
            string[] statusTexts = { "Авария", "Запуск", "Работа", "Нагрузка ВКЛ", "Закоротка ТЭ", "Клапан подачи", "Клапан сброса" };
            foreach (var text in statusTexts)
            {
                Label lbl = CreateStatusLabel(text);
                statusLabels[text] = lbl;
                leftPanel.Controls.Add(lbl);
            }

            // **Заполнение ошибок**
            string[] errorTexts = { "Низкий ЭДС аккумулятора", "Давление по датчику", "Низкое давление по СК", "Утечка водорода по СК", "ЭДС ТЭ", "Перегрев ТЭ" };
            foreach (var etext in errorTexts)
            {
                Label lbl = CreateErrorLabel(etext);
                errorLabels[etext] = lbl;
                rightPanel.Controls.Add(lbl);
            }

            // **Добавление параметров**
            lblPressure = new Label();
            lblCurrent = new Label();
            lblTemp1 = new Label();
            lblTemp2 = new Label();
            lblBatteryVoltage = new Label();
            lblOutputVoltage = new Label();
            lblTeVoltage = new Label();
            lblRuntime = new Label();
            lblFan = new Label();
            lblCounter = new Label();

            AddParameterRow("Давление:", lblPressure, middleTable);
            AddParameterRow("Ток:", lblCurrent, middleTable);
            AddParameterRow("Температура 1:", lblTemp1, middleTable);
            AddParameterRow("Температура 2:", lblTemp2, middleTable);            
            AddParameterRow("U аккумулятора:", lblBatteryVoltage, middleTable);
            AddParameterRow("U выходное:", lblOutputVoltage, middleTable);
            AddParameterRow("U ТЭ:", lblTeVoltage, middleTable);
            AddParameterRow("Время работы:", lblRuntime, middleTable);
            AddParameterRow("ШИМ вентилятора:", lblFan, middleTable);
            AddParameterRow("Счетчик:", lblCounter, middleTable);

        }

        private void AddParameterRow(string parameterName, Label valueLabel, TableLayoutPanel table)
        {
            Label nameLabel = new Label
            {
                Text = parameterName,
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Consolas", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.TopLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 8)

            };

            valueLabel.ForeColor = Color.White;
            valueLabel.BackColor = Color.Black;
            valueLabel.Font = new Font("Consolas", 11, FontStyle.Regular);
            valueLabel.TextAlign = ContentAlignment.TopRight;
            valueLabel.Dock = DockStyle.Fill;

            table.Controls.Add(nameLabel);
            table.Controls.Add(valueLabel);
        }

        private Label CreateStatusLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 56, 208, 89),
                Font = new Font("Arial", 10, FontStyle.Regular),
                AutoSize = false,
                Width = 230,
                Height = 43,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 10)
            };
        }

        private Label CreateErrorLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(51, 208, 56, 56),
                Font = new Font("Arial", 10, FontStyle.Regular),
                AutoSize = false,
                Width = 230,
                Height = 43,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 10)
            };
        }

        public void UpdatePlantData(CurrentState cs)
        {
            if (cs == null) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdatePlantData(cs)));
                return;
            }

            // Обновляем центральную колонку

            lblPressure.Text = cs.h_plant_pressure.ToString() + " атм";
            lblCurrent.Text = cs.h_plant_current.ToString() + " А";
            lblBatteryVoltage.Text = cs.h_plant_battery_voltage.ToString() + " В";
            lblOutputVoltage.Text = cs.h_plant_output_voltage.ToString() + " В";
            lblTeVoltage.Text = cs.h_plant_te_voltage.ToString() + " В";
            lblTemp1.Text = cs.h_plant_temp1.ToString() + " °C";
            lblTemp2.Text = cs.h_plant_temp2.ToString() + " °C";
            lblFan.Text = cs.h_plant_fan.ToString() + " %";
            lblCounter.Text = cs.h_plant_counter.ToString();

            TimeSpan runtime = TimeSpan.FromSeconds(cs.h_plant_runtime);
            lblRuntime.Text = $"{runtime.Hours:D2}:{runtime.Minutes:D2}:{runtime.Seconds:D2}";




            // Обновляем цвета статусов
            var activeStatuses = cs.GetStatusDescription().Split(',');
            foreach (var status in statusLabels)
            {
                status.Value.BackColor = activeStatuses.Contains(status.Key.Trim())
                    ? Color.FromArgb(191, 56, 208, 89)
                    : Color.FromArgb(51, 56, 208, 89);
            }

            // Обновляем цвета ошибок
            var activeErrors = cs.GetErrorsDescription().Split(',');
            foreach (var error in errorLabels)
            {
                error.Value.BackColor = activeErrors.Contains(error.Key.Trim())
                    ? Color.FromArgb(191, 208, 56, 56)
                    : Color.FromArgb(51, 208, 56, 56);
            }
        }
    }
}
