/* Рабочий вариант без кнопок управления установки
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

        private Label lblTemp1, lblTemp2, lblPressure, lblCurrent, lblBatteryVoltage, lblOutputVoltage, lblTeVoltage, lblRuntime, lblFan, lblCounter, lblIdentifikator;

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
            lblIdentifikator = new Label();

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
            AddParameterRow("Идентификатор:", lblIdentifikator, middleTable);
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
            lblIdentifikator.Text = cs.h_plant_identifikator.ToString();

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
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using MissionPlanner;
using System.Linq;

using MissionPlanner.Comms;
using MissionPlanner.ArduPilot.Mavlink;
using static MAVLink.MAV_CMD;
using System.Threading.Tasks;



namespace HydrogenPlantSpace
{
    public class RoundedLabel : Label
    {
        public int CornerRadius { get; set; } = 15;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var path = new GraphicsPath())
            {
                path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
                path.AddArc(Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
                path.AddArc(Width - CornerRadius, Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                path.AddArc(0, Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                path.CloseAllFigures();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(BackColor))
                    e.Graphics.FillPath(brush, path);

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    ClientRectangle,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            }
        }
    }

    public class FormEmpty : Form
    {
        private FlowLayoutPanel leftPanel, rightPanel;
        private Panel middleContainer;
        private TableLayoutPanel middleTable;
        private Dictionary<string, Label> statusLabels = new Dictionary<string, Label>();
        private Dictionary<string, Label> errorLabels = new Dictionary<string, Label>();

        private byte hydrogenId = 88;

        // Метки для центральной части
        private Label lblPressure, lblCurrent, lblTemp1, lblTemp2,
                      lblBatteryVoltage, lblOutputVoltage, lblTeVoltage,
                      lblRuntime, lblFan, lblCounter, lblIdentifikator;

        // Кнопки
        private Button btnOn, btnOff;

        public FormEmpty()
        {
            // Настройка формы
            Text = "Данные с водородной установки";
            BackColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(800, 500);

            // Основная сетка: 2 строки (заголовки + контент) и 3 колонки
            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 3,
                BackColor = Color.Black,
                Padding = new Padding(10)
            };
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // заголовки
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // контент
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            Controls.Add(mainTable);

            // Заголовки
            mainTable.Controls.Add(CreateHeaderLabel("Статус"), 0, 0);
            mainTable.Controls.Add(CreateHeaderLabel("Показания"), 1, 0);
            mainTable.Controls.Add(CreateHeaderLabel("Ошибки"), 2, 0);

            // Левая панель (только статусы)
            leftPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0)
            };
            mainTable.Controls.Add(leftPanel, 0, 1);

            foreach (var text in new[]{
                "Авария",
                "Запуск",
                "Работа",
                "Нагрузка ВКЛ",
                "Закоротка ТЭ",
                "Клапан подачи",
                "Клапан сброса"
            })
            {
                var lbl = CreateStatusLabel(text);
                statusLabels[text] = lbl;
                leftPanel.Controls.Add(lbl);
            }

            // Правая панель (только ошибки)
            rightPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0)
            };
            mainTable.Controls.Add(rightPanel, 2, 1);

            foreach (var text in new[]{
                "Низкий ЭДС аккумулятора",
                "Давление по датчику",
                "Низкое давление по СК",
                "Утечка водорода по СК",
                "ЭДС ТЭ","Перегрев ТЭ"
            })
            {
                var lbl = CreateErrorLabel(text);
                errorLabels[text] = lbl;
                rightPanel.Controls.Add(lbl);
            }

            // Средний контейнер: показания + кнопки снизу
            middleContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0)
            };
            mainTable.Controls.Add(middleContainer, 1, 1);

            // Таблица показаний
            middleTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(10, 0, 10, 0),
                AutoSize = true
            };
            middleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            middleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            middleContainer.Controls.Add(middleTable);

            // Метки данных
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
            lblIdentifikator = new Label();

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
            AddParameterRow("Идентификатор:", lblIdentifikator, middleTable);

            var buttonsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.Black,
                Dock = DockStyle.Bottom,
                Margin = new Padding(0, 10, 0, 0)
            };

            /*
             btnOn.BackColor = Color.Lime;
             btnOff.BackColor = Color.DarkRed;

             btnOff.BackColor = Color.Red;
             btnOn.BackColor = Color.DarkGreen;
             */

            // Кнопка "Вкл" 
            btnOn = new Button
            {
                Text = "Вкл",
                Size = new Size(80, 28),
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false
            };
            btnOn.FlatAppearance.BorderSize = 0;
            btnOn.Click += (s, e) =>
            {
                byte sysid = (byte)MainV2.comPort.sysidcurrent;
                byte compid = (byte)MainV2.comPort.compidcurrent;

                btnOn.BackColor = Color.Lime;
                btnOff.BackColor = Color.DarkRed;

                MainV2.comPort.doCommandInt(
                    sysid,
                    compid,
                    DO_HYDROGEN_CONTROL,
                    hydrogenId,
                    1f,
                    0f,
                    0f,
                    0,
                    0,
                    0f,
                    true
                );
            };



            // Кнопка "Выкл" 
            btnOff = new Button
            {
                Text = "Выкл",
                Size = new Size(80, 28),
                BackColor = Color.DarkRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false
            };
            btnOff.FlatAppearance.BorderSize = 0;
            btnOff.Click += (s, e) =>
            {
                byte sysid = (byte)MainV2.comPort.sysidcurrent;
                byte compid = (byte)MainV2.comPort.compidcurrent;

                btnOff.BackColor = Color.Red;
                btnOn.BackColor = Color.DarkGreen;

                MainV2.comPort.doCommandInt(
                    sysid,
                    compid,
                    DO_HYDROGEN_CONTROL,
                    hydrogenId,
                    0f,
                    0f,
                    0f,
                    0,
                    0,
                    0f,
                    true
                );
            };

            buttonsPanel.Controls.Add(btnOn);
            buttonsPanel.Controls.Add(btnOff);
            middleContainer.Controls.Add(buttonsPanel);
        }

        private Label CreateHeaderLabel(string txt) => new Label
        {
            Text = txt,
            ForeColor = Color.White,
            BackColor = Color.Black,
            Font = new Font("Arial", 10, FontStyle.Regular),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 0, 4)
        };

        private Label CreateStatusLabel(string txt) => new Label
        {
            Text = txt,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(51, 56, 208, 89),
            Font = new Font("Arial", 9, FontStyle.Regular),
            Size = new Size(220, 36),
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(0, 0, 0, 6)
        };

        private Label CreateErrorLabel(string txt) => new Label
        {
            Text = txt,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(51, 208, 56, 56),
            Font = new Font("Arial", 9, FontStyle.Regular),
            Size = new Size(220, 36),
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(0, 0, 0, 6)
        };

        private void AddParameterRow(string name, Label val, TableLayoutPanel tbl)
        {
            var lblName = new Label
            {
                Text = name,
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Consolas", 9, FontStyle.Regular),
                TextAlign = ContentAlignment.TopLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 6)
            };
            val.ForeColor = Color.White;
            val.BackColor = Color.Black;
            val.Font = new Font("Consolas", 10, FontStyle.Regular);
            val.TextAlign = ContentAlignment.TopRight;
            val.Dock = DockStyle.Fill;

            tbl.Controls.Add(lblName);
            tbl.Controls.Add(val);
        }

        public void UpdatePlantData(CurrentState cs)
        {
            if (cs == null) return;
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdatePlantData(cs)));
                return;
            }

            // Обновляем данные
            lblPressure.Text = $"{cs.h_plant_pressure} атм";
            lblCurrent.Text = $"{cs.h_plant_current} А";
            lblBatteryVoltage.Text = $"{cs.h_plant_battery_voltage} В";
            lblOutputVoltage.Text = $"{cs.h_plant_output_voltage} В";
            lblTeVoltage.Text = $"{cs.h_plant_te_voltage} В";
            lblTemp1.Text = $"{cs.h_plant_temp1} °C";
            lblTemp2.Text = $"{cs.h_plant_temp2} °C";
            lblFan.Text = $"{cs.h_plant_fan} %";
            lblCounter.Text = cs.h_plant_counter.ToString();
            lblIdentifikator.Text = cs.h_plant_identifikator.ToString();

            var rt = TimeSpan.FromSeconds(cs.h_plant_runtime);
            lblRuntime.Text = $"{rt.Hours:D2}:{rt.Minutes:D2}:{rt.Seconds:D2}";

            // Статусы
            var activeSt = cs.GetStatusDescription().Split(',');
            foreach (var kv in statusLabels)
                kv.Value.BackColor = activeSt.Contains(kv.Key.Trim())
                    ? Color.FromArgb(191, 56, 208, 89)
                    : Color.FromArgb(51, 56, 208, 89);

            // Ошибки
            var activeErr = cs.GetErrorsDescription().Split(',');
            foreach (var kv in errorLabels)
                kv.Value.BackColor = activeErr.Contains(kv.Key.Trim())
                    ? Color.FromArgb(191, 208, 56, 56)
                    : Color.FromArgb(51, 208, 56, 56);
        }
    }
}
